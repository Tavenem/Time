using System.Numerics;
using System.Text.Json.Serialization;

namespace Tavenem.Time;

/// <summary>
/// The timeline of a universe, including the present time.
/// </summary>
/// <remarks>
/// <para>
/// Can accurately track ages up to the year roughly equal to 1e1816. That is greater than some
/// estimates for the age of the universe when it reaches a theorized "heat death," which should be
/// sufficient for most purposes.
/// </para>
/// <para>
/// Time is modeled as a series of epochs (<seealso cref="Epochs"/>), starting at the beginning of
/// time in the universe. The current epoch is not included in the collection (since it has no fixed
/// duration, as it is ongoing). Instead its name may be recorded for reference (<seealso
/// cref="CurrentEpoch"/>). The present moment (<seealso cref="Now"/>) is an <see cref="Instant"/>
/// offset from the start of the current epoch.
/// </para>
/// <para>
/// Note that adding an epoch to the end of the colleciton does not affect the value of any existing
/// <see cref="Instant"/> instances: they will still reference their original epoch, and should
/// continue to refer to the same moment. Removing an epoch, inserting one anywhere but at the end
/// of the collection, or even replacing an epoch with one that has a different duration, will all
/// render incorrect any <see cref="Instant"/> instances which refer to that epoch or any that
/// follow.
/// </para>
/// <para>
/// The present time cannot be modified directly. Instead, the <see cref="AddTime(Duration)"/> and
/// <see cref="SubtractTime(Duration)"/> methods can be used to modify the value of the present
/// time. These methods guarantee that the present cannot become an illegal value (negative or
/// infinite).
/// </para>
/// </remarks>
public class CosmicTime : IEquatable<CosmicTime?>
{
    private const string DefaultCurrentEpoch = "Anthropocene Epoch";

    // 15 July 1945 23:59:59.999 UTC, USEnglish Gregorian Calendar
    private const long StartOfCurrentEpochTicks = 613636127999990000;

    private static readonly Duration _DateTimeEpoch = new(years: 1944, nanoseconds: 156383999990000);
    private static readonly DateTime _StartOfCurrentEpoch = new(StartOfCurrentEpochTicks);
    private static readonly Duration _StartOfDateTimeInPreviousEpoch = new(years: 9631, nanoseconds: 15919200001000000);

    /// <summary>
    /// The name of the current epoch. May be <see langword="null"/>.
    /// </summary>
    public string? CurrentEpoch { get; set; } = DefaultCurrentEpoch;

    /// <summary>
    /// <para>
    /// A collection of epochs which constitute the timeline of the universe.
    /// </para>
    /// <para>
    /// The present time (<seealso cref="Now"/>) is considered to be measured from the end of
    /// the last epoch.
    /// </para>
    /// </summary>
    public List<Epoch> Epochs { get; set; } = Epoch.DefaultEpochs.ToList();

#pragma warning disable IDE0032 // Use auto property; requires init for deserialization, and private setter
    private Instant _now;
#pragma warning restore IDE0032 // Use auto property
    /// <summary>
    /// <para>
    /// The present moment.
    /// </para>
    /// <para>
    /// Read-only. Adjust with <see cref="AddTime(Duration)"/> or <see
    /// cref="SubtractTime(Duration)"/>.
    /// </para>
    /// </summary>
    public Instant Now
    {
        get => _now;
        init => _now = value;
    }

    /// <summary>
    /// Initializes a new instance of <see cref="CosmicTime"/> based on the current time.
    /// </summary>
    public CosmicTime() : this(Duration.FromDateTime(DateTime.UtcNow)) { }

    /// <summary>
    /// Initializes a new instance of <see cref="CosmicTime"/> with the given initial value.
    /// </summary>
    /// <param name="now">The present moment.</param>
    /// <param name="epochs">A collection of epochs which constitute the timeline of the universe.</param>
    /// <param name="currentEpoch">The name of the current epoch. May be <see langword="null"/>.</param>
    [JsonConstructor]
    public CosmicTime(Instant now, List<Epoch>? epochs = null, string? currentEpoch = null)
    {
        Now = now;
        Epochs = epochs ?? new();
        CurrentEpoch = currentEpoch;
    }

    /// <summary>
    /// Initializes a new instance of <see cref="CosmicTime"/> with the given initial value.
    /// </summary>
    /// <param name="now">The elapsed time since the beginning of the current epoch.</param>
    /// <param name="epochs">A collection of epochs which constitute the timeline of the universe.</param>
    public CosmicTime(Duration now, params Epoch[] epochs)
    {
        Now = new(now);
        Epochs = epochs.Length == 0 ? new() : epochs.ToList();
    }

    /// <summary>
    /// Initializes a new instance of <see cref="CosmicTime"/> with the given initial value.
    /// </summary>
    /// <param name="now">The elapsed time since the beginning of the current epoch.</param>
    /// <param name="currentEpoch">The name of the current epoch. May be <see langword="null"/>.</param>
    /// <param name="epochs">A collection of epochs which constitute the timeline of the universe.</param>
    public CosmicTime(Duration now, string? currentEpoch, params Epoch[] epochs)
    {
        Now = new(now);
        CurrentEpoch = currentEpoch;
        Epochs = epochs.Length == 0 ? new() : epochs.ToList();
    }

    /// <summary>
    /// Converts the given <see cref="DateTime"/> value to a <see cref="CosmicTime"/> instance.
    /// </summary>
    /// <param name="dateTime">A <see cref="DateTime"/> value to convert.</param>
    /// <returns>A <see cref="CosmicTime"/> instance with the value of <see cref="Now"/> set to an
    /// equivalent of <paramref name="dateTime"/>.</returns>
    /// <remarks>
    /// <para>
    /// The default epoch set is used. The final epoch is assumed to end on 15 July 1945
    /// 23:59:59.999 (the apparent most likely candidate for the proposed start of the
    /// Anthropocene is 16 July 1945). If the given <paramref name="dateTime"/> represents a
    /// value prior to that time, the resulting <see cref="CosmicTime"/> will have one less epoch than
    /// the default set (i.e. it will be set during the Holocene).
    /// </para>
    /// <para>
    /// The <see cref="DateTime.ToUniversalTime"/> method is used to ensure that time zone
    /// information is stripped prior to conversion. This means that converting various <see
    /// cref="DateTime"/> instances with differing time zone information should result in uniform
    /// <see cref="Duration"/> representations. Note, however, that <see cref="DateTime"/>
    /// instances with <see cref="DateTime.Kind"/> set to <see cref="DateTimeKind.Unspecified"/>
    /// may behave in unexpected ways when converted.
    /// </para>
    /// </remarks>
    public static CosmicTime FromDateTime(DateTime dateTime)
    {
        var currentEpoch = (string?)DefaultCurrentEpoch;
        var epochs = Epoch.DefaultEpochs.ToList();
        var offset = Duration.Zero;
        if (dateTime < _StartOfCurrentEpoch)
        {
            currentEpoch = epochs[^1].Name;
            epochs.RemoveAt(epochs.Count - 1);
            offset = _StartOfDateTimeInPreviousEpoch;
        }
        var ticks = dateTime.ToUniversalTime().Ticks;
        var years = (uint)(ticks / (TimeSpan.TicksPerSecond * Duration.SecondsPerDay));
        ticks %= TimeSpan.TicksPerSecond * Duration.SecondsPerDay;
        return new CosmicTime(new(offset + new Duration(years: years, nanoseconds: (ulong)ticks * Duration.NanosecondsPerTick)), epochs, currentEpoch);
    }

    /// <summary>
    /// Converts the given <see cref="DateOnly"/> value to a <see cref="CosmicTime"/> instance.
    /// </summary>
    /// <param name="dateOnly">A <see cref="DateOnly"/> value to convert.</param>
    /// <returns>A <see cref="CosmicTime"/> instance with the value of <see cref="Now"/> set to an
    /// equivalent of <paramref name="dateOnly"/>.</returns>
    /// <remarks>
    /// <para>
    /// The default epoch set is used. The final epoch is assumed to end on 15 July 1945
    /// 23:59:59.999 (the apparent most likely candidate for the proposed start of the
    /// Anthropocene is 16 July 1945). If the given <paramref name="dateOnly"/> represents a
    /// value prior to that time, the resulting <see cref="CosmicTime"/> will have one less epoch than
    /// the default set (i.e. it will be set during the Holocene).
    /// </para>
    /// </remarks>
    public static CosmicTime FromDateOnly(DateOnly dateOnly)
    {
        var currentEpoch = (string?)DefaultCurrentEpoch;
        var epochs = Epoch.DefaultEpochs.ToList();
        var offset = Duration.Zero;
        var dateTime = dateOnly.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);
        if (dateTime < _StartOfCurrentEpoch)
        {
            currentEpoch = epochs[^1].Name;
            epochs.RemoveAt(epochs.Count - 1);
            offset = _StartOfDateTimeInPreviousEpoch;
        }
        var ticks = dateTime.Ticks;
        var years = (uint)(ticks / (TimeSpan.TicksPerSecond * Duration.SecondsPerDay));
        ticks %= TimeSpan.TicksPerSecond * Duration.SecondsPerDay;
        return new CosmicTime(new(offset + new Duration(years: years, nanoseconds: (ulong)ticks * Duration.NanosecondsPerTick)), epochs, currentEpoch);
    }

    /// <summary>
    /// Converts the given <see cref="DateTimeOffset"/> value to a <see cref="CosmicTime"/> instance.
    /// </summary>
    /// <param name="dateTime">A <see cref="DateTimeOffset"/> value to convert.</param>
    /// <returns>A <see cref="CosmicTime"/> instance with the value of <see cref="Now"/> set to an
    /// equivalent of <paramref name="dateTime"/>.</returns>
    /// <remarks>
    /// <para>
    /// The default epoch set is used. The final epoch is assumed to end on 15 July 1945
    /// 23:59:59.999 (the apparent most likely candidate for the proposed start of the
    /// Anthropocene is 16 July 1945). If the given <paramref name="dateTime"/> represents a
    /// value prior to that time, the resulting <see cref="CosmicTime"/> will have one less epoch than
    /// the default set (i.e. it will be set during the Holocene).
    /// </para>
    /// <para>
    /// The <see cref="DateTimeOffset.ToUniversalTime"/> method is used to ensure that time zone
    /// information is stripped prior to conversion. This means that converting various <see
    /// cref="DateTimeOffset"/> instances with differing time zone information should result in uniform
    /// <see cref="Duration"/> representations.
    /// </para>
    /// </remarks>
    public static CosmicTime FromDateTimeOffset(DateTimeOffset dateTime)
    {
        var currentEpoch = (string?)DefaultCurrentEpoch;
        var epochs = Epoch.DefaultEpochs.ToList();
        var offset = Duration.Zero;
        if (dateTime < _StartOfCurrentEpoch)
        {
            currentEpoch = epochs[^1].Name;
            epochs.RemoveAt(epochs.Count - 1);
            offset = _StartOfDateTimeInPreviousEpoch;
        }
        var ticks = dateTime.ToUniversalTime().Ticks;
        var years = (uint)(ticks / (TimeSpan.TicksPerSecond * Duration.SecondsPerDay));
        ticks %= TimeSpan.TicksPerSecond * Duration.SecondsPerDay;
        return new CosmicTime(new(offset + new Duration(years: years, nanoseconds: (ulong)ticks * Duration.NanosecondsPerTick)), epochs, currentEpoch);
    }

    /// <summary>
    /// Converts the value of <see cref="DateTime.Now"/> to a <see cref="CosmicTime"/> instance.
    /// </summary>
    /// <returns>A <see cref="CosmicTime"/> instance with the value of <see cref="Now"/> set to an
    /// equivalent of <see cref="DateTime.Now"/>.</returns>
    /// <remarks>
    /// <para>
    /// The default epoch set is used. The final epoch is assumed to end on 15 July 1945
    /// 23:59:59.999 (the apparent most likely candidate for the proposed start of the
    /// Anthropocene is 16 July 1945).
    /// </para>
    /// <para>
    /// The <see cref="DateTime.ToUniversalTime"/> method is used to ensure that time zone
    /// information is stripped prior to conversion. This means that converting various <see
    /// cref="DateTime"/> instances with differing time zone information should result in uniform
    /// <see cref="Duration"/> representations.
    /// </para>
    /// </remarks>
    public static CosmicTime FromDateTimeNow() => FromDateTime(DateTime.UtcNow);

    /// <summary>
    /// Converts the given <paramref name="duration"/> to a <see cref="DateTime"/> value.
    /// </summary>
    /// <param name="duration">
    /// A <see cref="Duration"/> to convert, which is presumed to be offset from the beginning
    /// of the current epoch.
    /// </param>
    /// <returns>A <see cref="DateTime"/> value equivalent to the given <paramref
    /// name="duration"/>.</returns>
    /// <remarks>
    /// <para>
    /// The default epoch set is used. The final epoch is assumed to end on 15 July 1945
    /// 23:59:59.999 (the apparent most likely candidate for the proposed start of the
    /// Anthropocene is 16 July 1945).
    /// </para>
    /// <para>
    /// The returned <see cref="DateTime"/> will have a <see cref="DateTime.Kind"/> of <see
    /// cref="DateTimeKind.Utc"/>.
    /// </para>
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException"/>
    public static DateTime ToDateTime(Duration duration)
    {
        duration += Epoch.DefaultEpochs[^1].Duration - _StartOfDateTimeInPreviousEpoch;
        if (duration.IsNegative)
        {
            throw new ArgumentOutOfRangeException(nameof(duration));
        }
        if (duration.IsPerpetual
            || duration.Aeons > BigInteger.Zero
            || duration.Years > 9998)
        {
            throw new ArgumentOutOfRangeException(nameof(duration));
        }
        var ticks = ((duration.Years * (ulong)Duration.NanosecondsPerYear)
            + duration.TotalNanoseconds
            + (duration.TotalYoctoseconds / Duration.YoctosecondsPerNanosecond))
            * Duration.NanosecondsPerTick;
        if (ticks > (ulong)DateTime.MaxValue.Ticks)
        {
            throw new ArgumentOutOfRangeException(nameof(duration));
        }
        return new DateTime((long)ticks, DateTimeKind.Utc);
    }

    /// <summary>
    /// Converts the given <paramref name="duration"/> to a <see cref="DateTimeOffset"/> value.
    /// </summary>
    /// <param name="duration">
    /// A <see cref="Duration"/> to convert, which is presumed to be offset from the beginning
    /// of the current epoch.
    /// </param>
    /// <returns>A <see cref="DateTimeOffset"/> value equivalent to the given <paramref
    /// name="duration"/>.</returns>
    /// <remarks>
    /// <para>
    /// The default epoch set is used. The final epoch is assumed to end on 15 July 1945
    /// 23:59:59.999 (the apparent most likely candidate for the proposed start of the
    /// Anthropocene is 16 July 1945).
    /// </para>
    /// <para>
    /// The returned <see cref="DateTimeOffset"/> will have an <see
    /// cref="DateTimeOffset.Offset"/> of <see cref="TimeSpan.Zero"/>.
    /// </para>
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException"/>
    public static DateTimeOffset ToDateTimeOffset(Duration duration)
    {
        duration += Epoch.DefaultEpochs[^1].Duration - _StartOfDateTimeInPreviousEpoch;
        if (duration.IsNegative)
        {
            throw new ArgumentOutOfRangeException(nameof(duration));
        }
        if (duration.IsPerpetual
            || duration.Aeons > BigInteger.Zero
            || duration.Years > 9998)
        {
            throw new ArgumentOutOfRangeException(nameof(duration));
        }
        var ticks = ((duration.Years * (ulong)Duration.NanosecondsPerYear)
            + duration.TotalNanoseconds
            + (duration.TotalYoctoseconds / Duration.YoctosecondsPerNanosecond))
            * Duration.NanosecondsPerTick;
        if (ticks > (ulong)DateTimeOffset.MaxValue.Ticks)
        {
            throw new ArgumentOutOfRangeException(nameof(duration));
        }
        return new DateTimeOffset((long)ticks, TimeSpan.Zero);
    }

    /// <summary>
    /// Converts the given <see cref="DateTime"/> value to a <see cref="Duration"/>.
    /// </summary>
    /// <param name="dateTime">A <see cref="DateTime"/> value to convert.</param>
    /// <returns>A <see cref="Duration"/> instance equivalent to <paramref
    /// name="dateTime"/>.</returns>
    /// <remarks>
    /// <para>
    /// The default epoch set is used. The final epoch is assumed to end on 15 July 1945
    /// 23:59:59.999 (the apparent most likely candidate for the proposed start of the
    /// Anthropocene is 16 July 1945). If the given <paramref name="dateTime"/> represents a
    /// value prior to that time, the resulting <see cref="Duration"/> will be negative.
    /// </para>
    /// <para>
    /// The <see cref="DateTime.ToUniversalTime"/> method is used to ensure that time zone
    /// information is stripped prior to conversion. This means that converting various <see
    /// cref="DateTime"/> instances with differing time zone information should result in uniform
    /// <see cref="Duration"/> representations. Note, however, that <see cref="DateTime"/>
    /// instances with <see cref="DateTime.Kind"/> set to <see cref="DateTimeKind.Unspecified"/>
    /// may behave in unexpected ways when converted.
    /// </para>
    /// </remarks>
    public static Duration ToDuration(DateTime dateTime) => Duration.FromTimeSpan(dateTime - _StartOfCurrentEpoch);

    /// <summary>
    /// Converts the given <see cref="DateTimeOffset"/> value to a <see cref="Duration"/>.
    /// </summary>
    /// <param name="dateTime">A <see cref="DateTimeOffset"/> value to convert.</param>
    /// <returns>A <see cref="Duration"/> instance equivalent to <paramref
    /// name="dateTime"/>.</returns>
    /// <remarks>
    /// <para>
    /// The default epoch set is used. The final epoch is assumed to end on 15 July 1945
    /// 23:59:59.999 (the apparent most likely candidate for the proposed start of the
    /// Anthropocene is 16 July 1945). If the given <paramref name="dateTime"/> represents a
    /// value prior to that time, the resulting <see cref="Duration"/> will be negative.
    /// </para>
    /// <para>
    /// The <see cref="DateTimeOffset.ToUniversalTime"/> method is used to ensure that time zone
    /// information is stripped prior to conversion. This means that converting various <see
    /// cref="DateTimeOffset"/> instances with differing time zone information should result in uniform
    /// <see cref="Duration"/> representations
    /// </para>
    /// </remarks>
    public static Duration ToDuration(DateTimeOffset dateTime) => Duration.FromTimeSpan(dateTime - _StartOfCurrentEpoch);

    /// <summary>
    /// Gets an <see cref="Instant"/> which represents the result of adding the given <paramref
    /// name="duration"/> to the given <paramref name="instant"/>.
    /// </summary>
    /// <param name="instant">An <see cref="Instant"/>.</param>
    /// <param name="duration">A <see cref="Duration"/>. May be negative or infinite.</param>
    /// <returns>
    /// The result of adding <paramref name="duration"/> to <paramref name="instant"/>. An
    /// instant representing an infinite time in the future is possible. Operations which would
    /// result in a time before the beginning of the timeline represented by this <see
    /// cref="CosmicTime"/> instance will instead become zero.
    /// </returns>
    public Instant Add(Instant instant, Duration duration)
    {
        var epoch = instant.Epoch;
        if (epoch > 0 && Epochs.Count == 0)
        {
            epoch = -1;
        }
        var offset = instant.Offset + duration;
        if (Epochs.Count > 0)
        {
            while (offset.IsNegative)
            {
                if (epoch == 0)
                {
                    break;
                }
                if (epoch < 0)
                {
                    epoch = Epochs.Count - 1;
                }
                else
                {
                    epoch--;
                }
                duration += Epochs[epoch].Duration;
                offset = instant.Offset + duration;
            }
            while (offset.IsPositiveInfinity)
            {
                if (epoch < 0)
                {
                    break;
                }
                if (epoch == Epochs.Count - 1)
                {
                    duration -= Epochs[epoch].Duration;
                    epoch = -1;
                }
                else
                {
                    duration -= Epochs[epoch++].Duration;
                }
                offset = instant.Offset + duration;
            }
        }
        return offset.IsNegative
            ? new Instant(Duration.Zero, Epochs.Count == 0 ? -1 : 0)
            : new Instant(offset, epoch);
    }

    /// <summary>
    /// <para>
    /// Advances time by the given duration.
    /// </para>
    /// <para>
    /// Negative values can be added to reverse time.
    /// </para>
    /// </summary>
    /// <param name="duration">
    /// <para>
    /// An amount of time to add.
    /// </para>
    /// <para>
    /// Adding a negative amount which would cause <see cref="Now"/> to become negative results
    /// in <see cref="Now"/> being set to <see cref="Duration.Zero"/> instead.
    /// </para>
    /// <para>
    /// Adding infinite time is treated as adding none.
    /// </para>
    /// </param>
    public void AddTime(Duration duration)
    {
        if (Now.IsPerpetual
            || duration.IsPerpetual
            || duration.IsZero)
        {
            return;
        }

        var t = Now;
        while (t.Epoch >= 0)
        {
            var d = t.Offset.Add(duration);

            if (d.IsNegative)
            {
                if (t.Epoch == 0
                    || (t.Epoch < 0
                    && Epochs.Count == 0)
                    || t.Epoch >= Epochs.Count)
                {
                    t = new(Duration.Zero, Epochs.Count == 0 ? -1 : 0);
                    break;
                }

                duration += t.Offset;
                t = new(Epochs[t.Epoch - 1].Duration, t.Epoch - 1);
                continue;
            }

            if (t.Epoch >= 0
                && Epochs[t.Epoch].Duration < d)
            {
                t = new(
                    d - Epochs[t.Epoch].Duration,
                    t.Epoch >= Epochs.Count - 1
                        ? -1
                        : t.Epoch + 1);
            }
            else
            {
                t = new(d, t.Epoch);
            }
            break;
        }

        _now = t;
    }

    /// <inheritdoc />
    public bool Equals(CosmicTime? other)
        => other is not null
        && CurrentEpoch == other.CurrentEpoch
        && Now.Equals(other.Now)
        && Epochs.SequenceEqual(other.Epochs);

    /// <inheritdoc />
    public override bool Equals(object? obj) => Equals(obj as CosmicTime);

    /// <summary>
    /// Gets an <see cref="Instant"/> which represents the result of adding the given <paramref
    /// name="duration"/> to <see cref="Now"/>.
    /// </summary>
    /// <param name="duration">A <see cref="Duration"/>. May be negative or infinite.</param>
    /// <returns>
    /// The result of adding <paramref name="duration"/> to <see cref="Now"/>. An
    /// instant representing an infinite time in the future is possible. Operations which would
    /// result in a time before the beginning of the timeline represented by this <see
    /// cref="CosmicTime"/> instance will instead become zero.
    /// </returns>
    public Instant FromNow(Duration duration) => Add(Now, duration);

    /// <summary>
    /// Calculates the difference between the given <see cref="Instant"/> instances.
    /// </summary>
    /// <param name="first">The first <see cref="Instant"/>.</param>
    /// <param name="second">The second <see cref="Instant"/>.</param>
    /// <returns>
    /// The result of subtracting <paramref name="first"/> from <paramref name="second"/>; or
    /// <see cref="Duration.NegativeInfinity"/> or <see cref="Duration.PositiveInfinity"/> if
    /// the result cannot be represented as a finite <see cref="Duration"/>.
    /// </returns>
    public Duration GetDifference(Instant first, Instant second)
    {
        var epoch = first.Epoch;
        if (epoch > 0 && Epochs.Count == 0)
        {
            epoch = -1;
        }
        var secondEpoch = second.Epoch;
        if (secondEpoch > 0 && Epochs.Count == 0)
        {
            secondEpoch = -1;
        }
        var duration = first.Offset;
        if (epoch >= 0 && epoch < secondEpoch && epoch < Epochs.Count)
        {
            duration -= Epochs[epoch].Duration;
            epoch++;
        }
        while (epoch != secondEpoch && epoch < Epochs.Count)
        {
            if (epoch < 0 || epoch > secondEpoch)
            {
                duration += Epochs[epoch].Duration;
                epoch = epoch < 0 ? Epochs.Count - 1 : epoch - 1;
            }
            else
            {
                duration -= Epochs[epoch++].Duration;
            }
        }
        if (epoch != secondEpoch)
        {
            if (epoch < 0 || epoch > secondEpoch)
            {
                return Duration.NegativeInfinity;
            }
            else
            {
                return Duration.PositiveInfinity;
            }
        }
        else
        {
            return second.Offset - duration;
        }
    }

    /// <summary>
    /// Calculates the difference between the given <see cref="Instant"/> instance and <see
    /// cref="Now"/>.
    /// </summary>
    /// <param name="instant">The first <see cref="Instant"/>.</param>
    /// <returns>
    /// The result of subtracting <see cref="Now"/> from <paramref name="instant"/>; or <see
    /// cref="Duration.NegativeInfinity"/> or <see cref="Duration.PositiveInfinity"/> if the
    /// result cannot be represented as a finite <see cref="Duration"/>.
    /// </returns>
    public Duration GetDifferenceFromNow(Instant instant) => GetDifference(Now, instant);

    /// <summary>
    /// Gets the index of the epoch indicated by the given duration from the beginning of the
    /// universe.
    /// </summary>
    /// <param name="age">The age of the universe.</param>
    /// <returns>
    /// The index of the epoch indicated by the given duration from the beginning of the
    /// universe; or -1 if no epochs are defined, or if the given <paramref name="age"/> is
    /// within the present epoch, or if the given duration is negative or infinite.
    /// </returns>
    public int GetEpoch(Duration age)
    {
        if (Epochs.Count == 0
            || age.IsPerpetual
            || age.IsNegative)
        {
            return -1;
        }

        for (var i = 0; i < Epochs.Count; i++)
        {
            age -= Epochs[i].Duration;
            if (age.IsNegative)
            {
                return i;
            }
        }
        return -1;
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        hashCode.Add(CurrentEpoch?.GetHashCode() ?? 0);
        hashCode.Add(Now.GetHashCode());
        hashCode.Add(GetEpochsHashCode());
        return hashCode.ToHashCode();
    }

    /// <summary>
    /// <para>
    /// Regresses time by the given duration.
    /// </para>
    /// <para>
    /// Negative values can be subtracted to progress time.
    /// </para>
    /// </summary>
    /// <param name="duration">
    /// <para>
    /// An amount of time to subtract.
    /// </para>
    /// <para>
    /// Subtracting an amount which would cause <see cref="Now"/> to become negative results in
    /// <see cref="Now"/> being set to <see cref="Duration.Zero"/> instead.
    /// </para>
    /// <para>
    /// Subtracting infinite time is treated as subtracting none. Subtracting an amount which
    /// would cause the result to overflow to infinity causes the result to be set to the
    /// maximum allowed value instead.
    /// </para>
    /// </param>
    public void SubtractTime(Duration duration) => AddTime(duration.Negate());

    /// <summary>
    /// Converts this instance to a <see cref="DateTime"/> value.
    /// </summary>
    /// <returns>A <see cref="DateTime"/> value equivalent to the value of <see
    /// cref="Now"/>.</returns>
    /// <remarks>
    /// <para>
    /// The current epoch is assumed to begin on 15 July 1945 23:59:59.999 (the apparent most
    /// likely candidate for the proposed start of the Anthropocene is 16 July 1945).
    /// </para>
    /// <para>
    /// The returned <see cref="DateTime"/> will have a <see cref="DateTime.Kind"/> of <see
    /// cref="DateTimeKind.Utc"/>.
    /// </para>
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException"/>
    public DateTime ToDateTime()
    {
        var duration = Now.Offset + _DateTimeEpoch;
        if (duration.IsPerpetual
            || duration.Aeons > BigInteger.Zero
            || duration.Years > 9998)
        {
            throw new OverflowException();
        }
        var ticks = ((duration.Years * (ulong)Duration.NanosecondsPerYear)
            + duration.TotalNanoseconds
            + (duration.TotalYoctoseconds / Duration.YoctosecondsPerNanosecond))
            * Duration.NanosecondsPerTick;
        if (ticks > (ulong)DateTime.MaxValue.Ticks)
        {
            throw new OverflowException();
        }
        return new DateTime((long)ticks, DateTimeKind.Utc);
    }

    /// <summary>
    /// Converts this instance to a <see cref="DateTimeOffset"/> value.
    /// </summary>
    /// <returns>A <see cref="DateTimeOffset"/> value equivalent to the value of <see
    /// cref="Now"/>.</returns>
    /// <remarks>
    /// <para>
    /// The current epoch is assumed to begin on 15 July 1945 23:59:59.999 (the apparent most
    /// likely candidate for the proposed start of the Anthropocene is 16 July 1945).
    /// </para>
    /// <para>
    /// The returned <see cref="DateTimeOffset"/> will have an <see
    /// cref="DateTimeOffset.Offset"/> of <see cref="TimeSpan.Zero"/>.
    /// </para>
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException"/>
    public DateTimeOffset ToDateTimeOffset()
    {
        var duration = Now.Offset + _DateTimeEpoch;
        if (duration.IsPerpetual
            || duration.Aeons > BigInteger.Zero
            || duration.Years > 9998)
        {
            throw new OverflowException();
        }
        var ticks = ((duration.Years * (ulong)Duration.NanosecondsPerYear)
            + duration.TotalNanoseconds
            + (duration.TotalYoctoseconds / Duration.YoctosecondsPerNanosecond))
            * Duration.NanosecondsPerTick;
        if (ticks > (ulong)DateTimeOffset.MaxValue.Ticks)
        {
            throw new OverflowException();
        }
        return new DateTimeOffset((long)ticks, TimeSpan.Zero);
    }

    /// <summary>
    /// Converts the value of the given <see cref="Instant"/> object to its equivalent string
    /// representation using the specified <paramref name="format"/> and culture-specific format
    /// information. In addition to the usual representation of the <see
    /// cref="Instant.Offset"/>, prefixes the epoch name or number.
    /// </summary>
    /// <param name="instant">The instant whose value is to be converted.</param>
    /// <param name="format">
    /// <para>
    /// A standard or custom date and time format string.
    /// </para>
    /// <para>
    /// Not all <see cref="DateTime"/> format strings are recognized. See Remarks for more info.
    /// </para>
    /// </param>
    /// <param name="formatProvider">An object that supplies culture-specific formatting
    /// information.</param>
    /// <returns>A string representation of value of <paramref name="instant"/>, as specified by
    /// <paramref name="format"/> and <paramref name="formatProvider"/>.</returns>
    /// <remarks>
    /// <para>
    /// The <paramref name="format"/> parameter can contain any of these single-character
    /// standard format strings:
    /// </para>
    /// <list type="table">
    /// <listheader>
    /// <term>Format specifier</term>
    /// <term>Description</term>
    /// </listheader>
    /// <item>
    /// <term>"d"</term>
    /// <description>Short date format. Corresponds to the "y d" custom format
    /// string.</description>
    /// </item>
    /// <item>
    /// <term>"D"</term>
    /// <description>Long date format. Corresponds to the "a d" custom format
    /// string.</description>
    /// </item>
    /// <item>
    /// <term>"E"</term>
    /// <description>General extended format. Corresponds to the "y d
    /// HH:mm:ss:nn:YY:ppppppppppppppppp" custom format string.</description>
    /// </item>
    /// <item>
    /// <term>"f"</term>
    /// <description>Full date short time format. Corresponds to the Long date and short time,
    /// separated by a space.</description>
    /// </item>
    /// <item>
    /// <term>"F"</term>
    /// <description>Full date long time format. Corresponds to the Long date and short time,
    /// separated by a space.</description>
    /// </item>
    /// <item>
    /// <term>"g"</term>
    /// <description>General short format. Corresponds to the "y d HH:mm" custom format
    /// string.</description>
    /// </item>
    /// <item>
    /// <term>"G"</term>
    /// <description>General long format. The default if none is selected. Corresponds to the "y
    /// d HH:mm:ss" custom format string.</description>
    /// </item>
    /// <item>
    /// <term>"O", "o"</term>
    /// <description>Round trip format. Corresponds to the
    /// "a'-'d'T'HH':'mm':'ss':'nn':'YY':'ppppppppppppppppp" custom format string. Because the
    /// length of certain segments varies (unavoidably, to prevent strings from being longer
    /// than <see cref="int.MaxValue"/> to account for <see cref="Duration"/> instances with the
    /// maximum number of aeons), this format is unsuitable for sorting.</description>
    /// </item>
    /// <item>
    /// <term>"t"</term>
    /// <description>Short time format. Corresponds to the "HH:mm" custom format
    /// string.</description>
    /// </item>
    /// <item>
    /// <term>"T"</term>
    /// <description>Long time format. Corresponds to the "HH:mm:ss" custom format
    /// string.</description>
    /// </item>
    /// </list>
    /// <para>
    /// The <paramref name="format"/> parameter can also or a custom format pattern with any of
    /// the following format strings:
    /// </para>
    /// <list type="table">
    /// <listheader>
    /// <term>Format specifier</term>
    /// <term>Description</term>
    /// </listheader>
    /// <item>
    /// <term>"a", "aa", "aaa", etc.</term>
    /// <para>
    /// The number of years, <i>including</i> those represented by aeons. Always displayed in G
    /// format, with precision equal to the number of "a" characters, except when only 1 "a" is
    /// present, which indicates "G29" format. Successful round-trip operations are not possible
    /// for values larger than can be accurately represented with the chosen specification, and
    /// since aeons may contain a number of significant digits greater than can be recorded in a
    /// <see cref="string"/> without causing memory overflow, loss of precision may be
    /// unavoidable.
    /// </para>
    /// <para>
    /// Since the length of this element can vary independently of the length of the format
    /// string, custom formats which do not provide a separator between this element and the
    /// next (e.g. "aaaddd") may result in a string representation which cannot be parsed, even
    /// with an exact format string parameter. Therefore a separator (even a space: " ") is
    /// recommended.
    /// </para>
    /// </item>
    /// <item>
    /// <term>"d", "dd", "ddd"</term>
    /// <description>
    /// <para>
    /// The number of days, from 0 to 365. Always displayed using as many digits as required,
    /// regardless of the number of "d" characters in the format string, but will not be
    /// displayed with <i>fewer</i> digits than there are "d" characters in the string; leading
    /// zeros will be added as needed.
    /// </para>
    /// <para>
    /// Since the length of this element can vary independently of the length of the format
    /// string, custom formats which do not provide a separator between this element and the
    /// next (e.g. "aaaddd") may result in a string representation which cannot be parsed, even
    /// with an exact format string parameter. Therefore a separator (even a space: " ") is
    /// recommended.
    /// </para>
    /// </description>
    /// </item>
    /// <item>
    /// <term>"dddd"</term>
    /// <description>Not recognized; <see cref="Duration"/> does not count weeks.</description>
    /// </item>
    /// <item>
    /// <term>"f", "F", "ff", "FF", "fff", etc.</term>
    /// <description>
    /// <para>
    /// Displays fractions of a second to as many significant digits as there are "f"
    /// characters. Unlike <see cref="DateTime"/> format strings, case is disregarded: the exact
    /// number of "f" characters is always displayed, regardless of whether the digits are
    /// significant.
    /// </para>
    /// <para>
    /// Note that if fractional seconds and a unit of time smaller than seconds are both
    /// displayed, aside from causing confusion this will result in failure to round-trip (the
    /// values of the fractional seconds and the smaller units each be counted on parse, causing
    /// the value to be larger than expected).
    /// </para>
    /// </description>
    /// </item>
    /// <item>
    /// <term>"g", "gg"</term>
    /// <description>Not recognized; <see cref="Duration"/> does not count periods or
    /// eras.</description>
    /// </item>
    /// <item>
    /// <term>"h", "H"</term>
    /// <description>The number of hours, from 0 to 23. 12-hour time is not recognized, since a
    /// <see cref="Duration"/> represents an amount of time, not a time of day.</description>
    /// </item>
    /// <item>
    /// <term>"hh", "HH", "hH", "Hh"</term>
    /// <description>The number of hours, from 00 to 23. Always displays two digits, even for
    /// single-digit values. 12-hour time is not recognized, since a <see cref="Duration"/>
    /// represents an amount of time, not a time of day.</description>
    /// </item>
    /// <item>
    /// <term>"K"</term>
    /// <description>Not recognized; <see cref="Duration"/> does not count time
    /// zones.</description>
    /// </item>
    /// <item>
    /// <term>"m"</term>
    /// <description>The number of minutes, from 0 to 59.</description>
    /// </item>
    /// <item>
    /// <term>"mm"</term>
    /// <description>The number of minutes, from 00 to 59. Always displays two digits, even for
    /// single-digit values.</description>
    /// </item>
    /// <item>
    /// <term>"M", "MM", etc.</term>
    /// <description>Not recognized; <see cref="Duration"/> does not count months.</description>
    /// </item>
    /// <item>
    /// <term>"n", "nn", "nnn", etc.</term>
    /// <description>
    /// <para>
    /// The number of nanoseconds. Always displayed using as many digits as required, regardless
    /// of the number of "n" characters in the format string, but will not be displayed with
    /// <i>fewer</i> digits than there are "n" characters in the string; leading zeros will be
    /// added as needed.
    /// </para>
    /// <para>
    /// Since the length of this element can vary independently of the length of the format
    /// string, custom formats which do not provide a separator between this element and the
    /// next (e.g. "aaaddd") may result in a string representation which cannot be parsed, even
    /// with an exact format string parameter. Therefore a separator (even a space: " ") is
    /// recommended.
    /// </para>
    /// </description>
    /// </item>
    /// <item>
    /// <term>"p", "pp", "ppp", etc.</term>
    /// <description>
    /// <para>
    /// The amount of Planck time. Always displayed in G format, with precision equal to the
    /// number of "p" characters, except when only 1 "p" is present, which indicates "G" format.
    /// 17 should be used to round-trip.
    /// </para>
    /// <para>
    /// Since the length of this element can vary independently of the length of the format
    /// string, custom formats which do not provide a separator between this element and the
    /// next (e.g. "aaaddd") may result in a string representation which cannot be parsed, even
    /// with an exact format string parameter. Therefore a separator (even a space: " ") is
    /// recommended.
    /// </para>
    /// </description>
    /// </item>
    /// <item>
    /// <term>"s"</term>
    /// <description>The number of seconds, from 0 to 59.</description>
    /// </item>
    /// <item>
    /// <term>"ss"</term>
    /// <description>The number of seconds, from 00 to 59. Always displays two digits, even for
    /// single-digit values.</description>
    /// </item>
    /// <item>
    /// <term>"t", "tt"</term>
    /// <description>Not recognized; <see cref="Duration"/> represents an amount of time, not a
    /// time of day.</description>
    /// </item>
    /// <item>
    /// <term>"y", "yy", "yyy", etc.</term>
    /// <description>
    /// <para>
    /// The number of years, not counting those represented by aeons. Always displayed using as
    /// many digits as required, regardless of the number of "y" characters in the format
    /// string, but will not be displayed with <i>fewer</i> digits than there are "y" characters
    /// in the string; leading zeros will be added as needed.
    /// </para>
    /// <para>
    /// Since the length of this element can vary independently of the length of the format
    /// string, custom formats which do not provide a separator between this element and the
    /// next (e.g. "aaaddd") may result in a string representation which cannot be parsed, even
    /// with an exact format string parameter. Therefore a separator (even a space: " ") is
    /// recommended.
    /// </para>
    /// </description>
    /// </item>
    /// <item>
    /// <term>"Y", "YY", "YYY", etc.</term>
    /// <description>
    /// <para>
    /// The number of yoctoseconds. Always displayed using as many digits as required,
    /// regardless of the number of "Y" characters in the format string, but will not be
    /// displayed with <i>fewer</i> digits than there are "Y" characters in the string; leading
    /// zeros will be added as needed.
    /// </para>
    /// <para>
    /// Since the length of this element can vary independently of the length of the format
    /// string, custom formats which do not provide a separator between this element and the
    /// next (e.g. "aaaddd") may result in a string representation which cannot be parsed, even
    /// with an exact format string parameter. Therefore a separator (even a space: " ") is
    /// recommended.
    /// </para>
    /// </description>
    /// </item>
    /// <item>
    /// <term>"z", "zz", "zzz"</term>
    /// <description>Not recognized; <see cref="Duration"/> does not count time
    /// zones.</description>
    /// </item>
    /// <item>
    /// <term>":"</term>
    /// <description>The time separator.</description>
    /// </item>
    /// <item>
    /// <term>"/"</term>
    /// <description>The date separator.</description>
    /// </item>
    /// <item>
    /// <term>"<i>string</i>", '<i>string</i>'</term>
    /// <description>Literal string delimiters.</description>
    /// </item>
    /// <item>
    /// <term>"%"</term>
    /// <description>Defines the following character as a custom format specifier.</description>
    /// </item>
    /// <item>
    /// <term>"\"</term>
    /// <description>The escape character.</description>
    /// </item>
    /// <item>
    /// <term>Any other character</term>
    /// <description>The character is copied to the result string unchanged.</description>
    /// </item>
    /// </list>
    /// <para>
    /// If <paramref name="format"/> is empty, or an unrecognized single character, the standard
    /// format specifier ("G") is used.
    /// </para>
    /// <para>
    /// A perpetual <see cref="Duration"/> is represented with the infinity symbol ("∞")
    /// regardless of format.
    /// </para>
    /// </remarks>
    public string ToString(Instant instant, string format, IFormatProvider formatProvider)
    {
        var epochName = instant.Epoch < 0 || instant.Epoch >= Epochs.Count
            ? CurrentEpoch ?? Epochs.Count.ToString(formatProvider)
            : Epochs[instant.Epoch].Name ?? instant.Epoch.ToString(formatProvider);
        return $"{epochName} {instant.ToString(format, formatProvider)}";
    }

    /// <summary>
    /// Converts the value of the given <see cref="Instant"/> object to its equivalent string
    /// representation using the specified culture-specific format information. In addition to
    /// the usual representation of the <see cref="Instant.Offset"/>, prefixes the epoch name or
    /// number.
    /// </summary>
    /// <param name="instant">The instant whose value is to be converted.</param>
    /// <param name="formatProvider">An object that supplies culture-specific formatting
    /// information.</param>
    /// <returns>A string representation of value of <paramref name="instant"/>, offset from the
    /// beginning of its epoch (if any), as specified by <paramref
    /// name="formatProvider"/>.</returns>
    /// <exception cref="FormatException">
    /// <paramref name="formatProvider"/> is not a valid provider for <see cref="Duration"/>.
    /// </exception>
    public string ToString(Instant instant, IFormatProvider formatProvider)
    {
        var epochName = instant.Epoch < 0 || instant.Epoch >= Epochs.Count
            ? CurrentEpoch ?? Epochs.Count.ToString(formatProvider)
            : Epochs[instant.Epoch].Name ?? instant.Epoch.ToString(formatProvider);
        return $"{epochName} {instant.ToString(formatProvider)}";
    }

    /// <summary>
    /// Converts the value of the given <see cref="Instant"/> object to its equivalent string
    /// representation using the specified <paramref name="format"/> information. In addition to
    /// the usual representation of the <see cref="Instant.Offset"/>, prefixes the epoch name or
    /// number.
    /// </summary>
    /// <param name="instant">The instant whose value is to be converted.</param>
    /// <param name="format">
    /// <para>
    /// A standard or custom date and time format string.
    /// </para>
    /// <para>
    /// Not all <see cref="DateTime"/> format strings are recognized. See Remarks for more info.
    /// </para>
    /// </param>
    /// <returns>A string representation of value of <paramref name="instant"/>, offset from the
    /// beginning of its epoch (if any), as specified by <paramref name="format"/>.</returns>
    /// <remarks>
    /// <para>
    /// The <paramref name="format"/> parameter can contain any of these single-character
    /// standard format strings:
    /// </para>
    /// <list type="table">
    /// <listheader>
    /// <term>Format specifier</term>
    /// <term>Description</term>
    /// </listheader>
    /// <item>
    /// <term>"d"</term>
    /// <description>Short date format. Corresponds to the "y d" custom format
    /// string.</description>
    /// </item>
    /// <item>
    /// <term>"D"</term>
    /// <description>Long date format. Corresponds to the "e d" custom format
    /// string.</description>
    /// </item>
    /// <item>
    /// <term>"E"</term>
    /// <description>General extended format. Corresponds to the "y d
    /// HH:mm:ss:MMM:uuu:nnn:ppp:fff:aaa:zzz:YYY:PPPPPPPPPPPPPPPPP" custom format
    /// string.</description>
    /// </item>
    /// <item>
    /// <term>"f"</term>
    /// <description>Full date short time format. Corresponds to the Long date and short time,
    /// separated by a space.</description>
    /// </item>
    /// <item>
    /// <term>"F"</term>
    /// <description>Full date long time format. Corresponds to the Long date and short time,
    /// separated by a space.</description>
    /// </item>
    /// <item>
    /// <term>"g"</term>
    /// <description>General short format. Corresponds to the "y d HH:mm" custom format
    /// string.</description>
    /// </item>
    /// <item>
    /// <term>"G"</term>
    /// <description>General long format. The default if none is selected. Corresponds to the "y
    /// d HH:mm:ss" custom format string.</description>
    /// </item>
    /// <item>
    /// <term>"O", "o"</term>
    /// <description>Round trip format. Corresponds to the
    /// "e'-'d'T'HH':'mm':'ss':'MMM':'uuu':'nnn':'ppp':'fff':'aaa':'zzz':'YYY':'PPPPPPPPPPPPPPPPP"
    /// custom format string. Because the length of certain segments varies (unavoidably, to
    /// prevent strings from being longer than <see cref="int.MaxValue"/> to account for <see
    /// cref="Duration"/> instances with the maximum number of aeons), this format is unsuitable
    /// for sorting.</description>
    /// </item>
    /// <item>
    /// <term>"t"</term>
    /// <description>Short time format. Corresponds to the "HH:mm" custom format
    /// string.</description>
    /// </item>
    /// <item>
    /// <term>"T"</term>
    /// <description>Long time format. Corresponds to the "HH:mm:ss" custom format
    /// string.</description>
    /// </item>
    /// <item>
    /// <term>"X"</term>
    /// <description>
    /// <para>
    /// Extensible format. Corresponds to the "e' a 'd' d 'H' h 'm' min 's' s 'M' ms 'u' μs 'n'
    /// ns 'p' ps 'f' fs 'a' as 'z' zs 'Y' ys 'P' tP" custom format string, except that each
    /// unit is displayed only if it is non-zero.
    /// </para>
    /// <para>
    /// The symbols denoting each unit are in SI notation, and are not culture-specific. In
    /// particular it should be noted that the symbol "a" is the SI symbol for years, which may
    /// confuse English readers on first glance.
    /// </para>
    /// </description>
    /// </item>
    /// </list>
    /// <para>
    /// The <paramref name="format"/> parameter can also or a custom format pattern with any of
    /// the following format strings:
    /// </para>
    /// <list type="table">
    /// <listheader>
    /// <term>Format specifier</term>
    /// <term>Description</term>
    /// </listheader>
    /// <item>
    /// <term>"a", "aa", "aaa"</term>
    /// <description>
    /// The number of attoseconds, from 0 to 999. The number of "a" characters in the format
    /// string represents the minimum number of significant digits; leading zeros will be added
    /// as needed.
    /// </description>
    /// </item>
    /// <item>
    /// <term>"d", "dd", "ddd"</term>
    /// <description>
    /// The number of days, from 0 to 365. Always displayed using as many digits as required,
    /// regardless of the number of "d" characters in the format string, but will not be
    /// displayed with <i>fewer</i> digits than there are "d" characters in the string; leading
    /// zeros will be added as needed.
    /// </description>
    /// </item>
    /// <item>
    /// <term>"dddd"</term>
    /// <description>Not recognized; <see cref="Duration"/> does not count weeks.</description>
    /// </item>
    /// <item>
    /// <term>"e", "ee", "eee", etc.</term>
    /// The number of years, <i>including</i> those represented by aeons. Always displayed in G
    /// format, with precision equal to the number of "e" characters, except when only 1 "e" is
    /// present, which indicates "G29" format. Successful round-trip operations are not possible
    /// for values larger than can be accurately represented with the chosen specification, and
    /// since aeons may contain a number of significant digits greater than can be recorded in a
    /// <see cref="string"/> without causing memory overflow, loss of precision may be
    /// unavoidable.
    /// </item>
    /// <item>
    /// <term>"f", "ff", "fff"</term>
    /// <description>
    /// The number of femtoseconds, from 0 to 999. The number of "f" characters in the format
    /// string represents the minimum number of significant digits; leading zeros will be added
    /// as needed.
    /// </description>
    /// </item>
    /// <item>
    /// <term>"F", "FF", "FFF", etc.</term>
    /// <description>
    /// <para>
    /// Displays fractions of a second to as many significant digits as there are "F"
    /// characters. Note that unlike <see cref="DateTime"/> format strings, the lower case "f"
    /// is not used to display a variable number of fractional seconds. The "f" is reserved in
    /// <see cref="Duration"/> format strings for femtoseconds.
    /// </para>
    /// <para>
    /// Note that if fractional seconds and a unit of time smaller than seconds are both
    /// displayed, aside from causing confusion this will result in failure to round-trip (the
    /// values of the fractional seconds and the smaller units each be counted on parse, causing
    /// the value to be larger than expected).
    /// </para>
    /// </description>
    /// </item>
    /// <item>
    /// <term>"g", "gg"</term>
    /// <description>Not recognized; <see cref="Duration"/> does not count periods or
    /// eras.</description>
    /// </item>
    /// <item>
    /// <term>"h", "H"</term>
    /// <description>The number of hours, from 0 to 23. 12-hour time is not recognized, since a
    /// <see cref="Duration"/> represents an amount of time, not a time of day.</description>
    /// </item>
    /// <item>
    /// <term>"hh", "HH", "hH", "Hh"</term>
    /// <description>The number of hours, from 00 to 23. Always displays two digits, even for
    /// single-digit values. 12-hour time is not recognized, since a <see cref="Duration"/>
    /// represents an amount of time, not a time of day.</description>
    /// </item>
    /// <item>
    /// <term>"K"</term>
    /// <description>Not recognized; <see cref="Duration"/> does not count time
    /// zones.</description>
    /// </item>
    /// <item>
    /// <term>"m", "mm"</term>
    /// <description>
    /// The number of minutes, from 0 to 59. The number of "m" characters in the format string
    /// represents the minimum number of significant digits; leading zeros will be added as
    /// needed.
    /// </description>
    /// </item>
    /// <item>
    /// <term>"M", "MM", "MMM"</term>
    /// <description>
    /// <para>
    /// The number of milliseconds, from 0 to 999. The number of "M" characters in the format
    /// string represents the minimum number of significant digits; leading zeros will be added
    /// as needed.
    /// </para>
    /// <para>
    /// Note carefully that this is in contrast to <see cref="DateTime"/> format strings, which
    /// uses "M" to denote months.
    /// </para>
    /// </description>
    /// </item>
    /// <item>
    /// <term>"n", "nn", "nnn"</term>
    /// <description>
    /// The number of nanoseconds, from 0 to 999. The number of "n" characters in the format
    /// string represents the minimum number of significant digits; leading zeros will be added
    /// as needed.
    /// </description>
    /// </item>
    /// <item>
    /// <term>"p", "pp", "ppp"</term>
    /// <description>
    /// The number of picoseconds, from 0 to 999. The number of "p" characters in the format
    /// string represents the minimum number of significant digits; leading zeros will be added
    /// as needed.
    /// </description>
    /// </item>
    /// <item>
    /// <term>"P", "PP", "PPP", etc.</term>
    /// <description>
    /// The amount of Planck time. Always displayed in G format, with precision equal to the
    /// number of "p" characters, except when only 1 "p" is present, which indicates "G" format.
    /// 17 should be used to round-trip.
    /// </description>
    /// </item>
    /// <item>
    /// <term>"s", "ss"</term>
    /// <description>
    /// The number of seconds, from 0 to 59. The number of "s" characters in the format string
    /// represents the minimum number of significant digits; leading zeros will be added as
    /// needed.
    /// </description>
    /// </item>
    /// <item>
    /// <term>"t", "tt"</term>
    /// <description>Not recognized; <see cref="Duration"/> represents an amount of time, not a
    /// time of day.</description>
    /// </item>
    /// <item>
    /// <term>"u", "uu", "uuu"</term>
    /// <description>
    /// The number of microseconds, from 0 to 999. The number of "U" characters represents the
    /// minimum number of significant digits displayed.
    /// </description>
    /// </item>
    /// <item>
    /// <term>"y", "yy", "yyy", etc.</term>
    /// <description>
    /// The number of years, not counting those represented by aeons. The number of "y"
    /// characters in the format string represents the minimum number of significant digits;
    /// leading zeros will be added as needed.
    /// </description>
    /// </item>
    /// <item>
    /// <term>"Y", "YY", "YYY"</term>
    /// <description>
    /// The number of yoctoseconds, from 0 to 999. The number of "Y" characters in the format
    /// string represents the minimum number of significant digits; leading zeros will be added
    /// as needed.
    /// </description>
    /// </item>
    /// <item>
    /// <term>"z", "zz", "zzz"</term>
    /// <description>
    /// <para>
    /// The number of zeptoseconds, from 0 to 999. The number of "z" characters in the format
    /// string represents the minimum number of significant digits; leading zeros will be added
    /// as needed.
    /// </para>
    /// <para>
    /// Note carefully that this is in contrast to <see cref="DateTime"/> format strings, which
    /// uses "z" to denote time zone.
    /// </para>
    /// </description>
    /// </item>
    /// <item>
    /// <term>":"</term>
    /// <description>The time separator.</description>
    /// </item>
    /// <item>
    /// <term>"/"</term>
    /// <description>The date separator.</description>
    /// </item>
    /// <item>
    /// <term>"<i>string</i>", '<i>string</i>'</term>
    /// <description>Literal string delimiters.</description>
    /// </item>
    /// <item>
    /// <term>"%"</term>
    /// <description>Defines the following character as a custom format specifier.</description>
    /// </item>
    /// <item>
    /// <term>"\"</term>
    /// <description>The escape character.</description>
    /// </item>
    /// <item>
    /// <term>Any other character</term>
    /// <description>The character is copied to the result string unchanged.</description>
    /// </item>
    /// </list>
    /// <para>
    /// Since the length of various format elements can vary independently of the length of the
    /// format string, custom formats which do not provide a separator between these elements
    /// and the next (e.g. "aaaddd") may result in a string representation which cannot be
    /// parsed, even with an exact format string parameter. Therefore a separator is always
    /// recommended (even whitespace is sufficient: " ").
    /// </para>
    /// <para>
    /// If <paramref name="format"/> is empty, or an unrecognized single character, the standard
    /// format specifier ("G") is used.
    /// </para>
    /// <para>
    /// A perpetual <see cref="Duration"/> is represented with the infinity symbol ("∞")
    /// regardless of format.
    /// </para>
    /// </remarks>
    public string ToString(Instant instant, string format) => ToString(instant, format, DurationFormatProvider.Instance);

    /// <summary>
    /// Converts the value of the given <see cref="Instant"/> object to its equivalent string
    /// representation. In addition to the usual representation of the <see
    /// cref="Instant.Offset"/>, prefixes the epoch name or number.
    /// </summary>
    /// <param name="instant">The instant whose value is to be converted.</param>
    /// <returns>A string representation of value of <paramref name="instant"/>, offset from the
    /// beginning of its epoch (if any).</returns>
    public string ToString(Instant instant) => ToString(instant, DurationFormatProvider.Instance);

    /// <summary>
    /// Converts the value of <see cref="Now"/> to its equivalent string representation using
    /// the specified <paramref name="format"/> and culture-specific format information.
    /// </summary>
    /// <param name="format">
    /// <para>
    /// A standard or custom date and time format string.
    /// </para>
    /// <para>
    /// Not all <see cref="DateTime"/> format strings are recognized. See Remarks for more info.
    /// </para>
    /// </param>
    /// <param name="formatProvider">An object that supplies culture-specific formatting
    /// information.</param>
    /// <returns>A string representation of value of <see cref="Now"/>
    /// as specified by <paramref name="format"/> and <paramref
    /// name="formatProvider"/>.</returns>
    /// <remarks>
    /// <para>
    /// The <paramref name="format"/> parameter can contain any of these single-character
    /// standard format strings:
    /// </para>
    /// <list type="table">
    /// <listheader>
    /// <term>Format specifier</term>
    /// <term>Description</term>
    /// </listheader>
    /// <item>
    /// <term>"d"</term>
    /// <description>Short date format. Corresponds to the "y d" custom format
    /// string.</description>
    /// </item>
    /// <item>
    /// <term>"D"</term>
    /// <description>Long date format. Corresponds to the "e d" custom format
    /// string.</description>
    /// </item>
    /// <item>
    /// <term>"E"</term>
    /// <description>General extended format. Corresponds to the "y d
    /// HH:mm:ss:MMM:uuu:nnn:ppp:fff:aaa:zzz:YYY:PPPPPPPPPPPPPPPPP" custom format
    /// string.</description>
    /// </item>
    /// <item>
    /// <term>"f"</term>
    /// <description>Full date short time format. Corresponds to the Long date and short time,
    /// separated by a space.</description>
    /// </item>
    /// <item>
    /// <term>"F"</term>
    /// <description>Full date long time format. Corresponds to the Long date and short time,
    /// separated by a space.</description>
    /// </item>
    /// <item>
    /// <term>"g"</term>
    /// <description>General short format. Corresponds to the "y d HH:mm" custom format
    /// string.</description>
    /// </item>
    /// <item>
    /// <term>"G"</term>
    /// <description>General long format. The default if none is selected. Corresponds to the "y
    /// d HH:mm:ss" custom format string.</description>
    /// </item>
    /// <item>
    /// <term>"O", "o"</term>
    /// <description>Round trip format. Corresponds to the
    /// "e'-'d'T'HH':'mm':'ss':'MMM':'uuu':'nnn':'ppp':'fff':'aaa':'zzz':'YYY':'PPPPPPPPPPPPPPPPP"
    /// custom format string. Because the length of certain segments varies (unavoidably, to
    /// prevent strings from being longer than <see cref="int.MaxValue"/> to account for <see
    /// cref="Duration"/> instances with the maximum number of aeons), this format is unsuitable
    /// for sorting.</description>
    /// </item>
    /// <item>
    /// <term>"t"</term>
    /// <description>Short time format. Corresponds to the "HH:mm" custom format
    /// string.</description>
    /// </item>
    /// <item>
    /// <term>"T"</term>
    /// <description>Long time format. Corresponds to the "HH:mm:ss" custom format
    /// string.</description>
    /// </item>
    /// <item>
    /// <term>"X"</term>
    /// <description>
    /// <para>
    /// Extensible format. Corresponds to the "e' a 'd' d 'H' h 'm' min 's' s 'M' ms 'u' μs 'n'
    /// ns 'p' ps 'f' fs 'a' as 'z' zs 'Y' ys 'P' tP" custom format string, except that each
    /// unit is displayed only if it is non-zero.
    /// </para>
    /// <para>
    /// The symbols denoting each unit are in SI notation, and are not culture-specific. In
    /// particular it should be noted that the symbol "a" is the SI symbol for years, which may
    /// confuse English readers on first glance.
    /// </para>
    /// </description>
    /// </item>
    /// </list>
    /// <para>
    /// The <paramref name="format"/> parameter can also or a custom format pattern with any of
    /// the following format strings:
    /// </para>
    /// <list type="table">
    /// <listheader>
    /// <term>Format specifier</term>
    /// <term>Description</term>
    /// </listheader>
    /// <item>
    /// <term>"a", "aa", "aaa"</term>
    /// <description>
    /// The number of attoseconds, from 0 to 999. The number of "a" characters in the format
    /// string represents the minimum number of significant digits; leading zeros will be added
    /// as needed.
    /// </description>
    /// </item>
    /// <item>
    /// <term>"d", "dd", "ddd"</term>
    /// <description>
    /// The number of days, from 0 to 365. Always displayed using as many digits as required,
    /// regardless of the number of "d" characters in the format string, but will not be
    /// displayed with <i>fewer</i> digits than there are "d" characters in the string; leading
    /// zeros will be added as needed.
    /// </description>
    /// </item>
    /// <item>
    /// <term>"dddd"</term>
    /// <description>Not recognized; <see cref="Duration"/> does not count weeks.</description>
    /// </item>
    /// <item>
    /// <term>"e", "ee", "eee", etc.</term>
    /// The number of years, <i>including</i> those represented by aeons. Always displayed in G
    /// format, with precision equal to the number of "e" characters, except when only 1 "e" is
    /// present, which indicates "G29" format. Successful round-trip operations are not possible
    /// for values larger than can be accurately represented with the chosen specification, and
    /// since aeons may contain a number of significant digits greater than can be recorded in a
    /// <see cref="string"/> without causing memory overflow, loss of precision may be
    /// unavoidable.
    /// </item>
    /// <item>
    /// <term>"f", "ff", "fff"</term>
    /// <description>
    /// The number of femtoseconds, from 0 to 999. The number of "f" characters in the format
    /// string represents the minimum number of significant digits; leading zeros will be added
    /// as needed.
    /// </description>
    /// </item>
    /// <item>
    /// <term>"F", "FF", "FFF", etc.</term>
    /// <description>
    /// <para>
    /// Displays fractions of a second to as many significant digits as there are "F"
    /// characters. Note that unlike <see cref="DateTime"/> format strings, the lower case "f"
    /// is not used to display a variable number of fractional seconds. The "f" is reserved in
    /// <see cref="Duration"/> format strings for femtoseconds.
    /// </para>
    /// <para>
    /// Note that if fractional seconds and a unit of time smaller than seconds are both
    /// displayed, aside from causing confusion this will result in failure to round-trip (the
    /// values of the fractional seconds and the smaller units each be counted on parse, causing
    /// the value to be larger than expected).
    /// </para>
    /// </description>
    /// </item>
    /// <item>
    /// <term>"g", "gg"</term>
    /// <description>Not recognized; <see cref="Duration"/> does not count periods or
    /// eras.</description>
    /// </item>
    /// <item>
    /// <term>"h", "H"</term>
    /// <description>The number of hours, from 0 to 23. 12-hour time is not recognized, since a
    /// <see cref="Duration"/> represents an amount of time, not a time of day.</description>
    /// </item>
    /// <item>
    /// <term>"hh", "HH", "hH", "Hh"</term>
    /// <description>The number of hours, from 00 to 23. Always displays two digits, even for
    /// single-digit values. 12-hour time is not recognized, since a <see cref="Duration"/>
    /// represents an amount of time, not a time of day.</description>
    /// </item>
    /// <item>
    /// <term>"K"</term>
    /// <description>Not recognized; <see cref="Duration"/> does not count time
    /// zones.</description>
    /// </item>
    /// <item>
    /// <term>"m", "mm"</term>
    /// <description>
    /// The number of minutes, from 0 to 59. The number of "m" characters in the format string
    /// represents the minimum number of significant digits; leading zeros will be added as
    /// needed.
    /// </description>
    /// </item>
    /// <item>
    /// <term>"M", "MM", "MMM"</term>
    /// <description>
    /// <para>
    /// The number of milliseconds, from 0 to 999. The number of "M" characters in the format
    /// string represents the minimum number of significant digits; leading zeros will be added
    /// as needed.
    /// </para>
    /// <para>
    /// Note carefully that this is in contrast to <see cref="DateTime"/> format strings, which
    /// uses "M" to denote months.
    /// </para>
    /// </description>
    /// </item>
    /// <item>
    /// <term>"n", "nn", "nnn"</term>
    /// <description>
    /// The number of nanoseconds, from 0 to 999. The number of "n" characters in the format
    /// string represents the minimum number of significant digits; leading zeros will be added
    /// as needed.
    /// </description>
    /// </item>
    /// <item>
    /// <term>"p", "pp", "ppp"</term>
    /// <description>
    /// The number of picoseconds, from 0 to 999. The number of "p" characters in the format
    /// string represents the minimum number of significant digits; leading zeros will be added
    /// as needed.
    /// </description>
    /// </item>
    /// <item>
    /// <term>"P", "PP", "PPP", etc.</term>
    /// <description>
    /// The amount of Planck time. Always displayed in G format, with precision equal to the
    /// number of "p" characters, except when only 1 "p" is present, which indicates "G" format.
    /// 17 should be used to round-trip.
    /// </description>
    /// </item>
    /// <item>
    /// <term>"s", "ss"</term>
    /// <description>
    /// The number of seconds, from 0 to 59. The number of "s" characters in the format string
    /// represents the minimum number of significant digits; leading zeros will be added as
    /// needed.
    /// </description>
    /// </item>
    /// <item>
    /// <term>"t", "tt"</term>
    /// <description>Not recognized; <see cref="Duration"/> represents an amount of time, not a
    /// time of day.</description>
    /// </item>
    /// <item>
    /// <term>"u", "uu", "uuu"</term>
    /// <description>
    /// The number of microseconds, from 0 to 999. The number of "U" characters represents the
    /// minimum number of significant digits displayed.
    /// </description>
    /// </item>
    /// <item>
    /// <term>"y", "yy", "yyy", etc.</term>
    /// <description>
    /// The number of years, not counting those represented by aeons. The number of "y"
    /// characters in the format string represents the minimum number of significant digits;
    /// leading zeros will be added as needed.
    /// </description>
    /// </item>
    /// <item>
    /// <term>"Y", "YY", "YYY"</term>
    /// <description>
    /// The number of yoctoseconds, from 0 to 999. The number of "Y" characters in the format
    /// string represents the minimum number of significant digits; leading zeros will be added
    /// as needed.
    /// </description>
    /// </item>
    /// <item>
    /// <term>"z", "zz", "zzz"</term>
    /// <description>
    /// <para>
    /// The number of zeptoseconds, from 0 to 999. The number of "z" characters in the format
    /// string represents the minimum number of significant digits; leading zeros will be added
    /// as needed.
    /// </para>
    /// <para>
    /// Note carefully that this is in contrast to <see cref="DateTime"/> format strings, which
    /// uses "z" to denote time zone.
    /// </para>
    /// </description>
    /// </item>
    /// <item>
    /// <term>":"</term>
    /// <description>The time separator.</description>
    /// </item>
    /// <item>
    /// <term>"/"</term>
    /// <description>The date separator.</description>
    /// </item>
    /// <item>
    /// <term>"<i>string</i>", '<i>string</i>'</term>
    /// <description>Literal string delimiters.</description>
    /// </item>
    /// <item>
    /// <term>"%"</term>
    /// <description>Defines the following character as a custom format specifier.</description>
    /// </item>
    /// <item>
    /// <term>"\"</term>
    /// <description>The escape character.</description>
    /// </item>
    /// <item>
    /// <term>Any other character</term>
    /// <description>The character is copied to the result string unchanged.</description>
    /// </item>
    /// </list>
    /// <para>
    /// Since the length of various format elements can vary independently of the length of the
    /// format string, custom formats which do not provide a separator between these elements
    /// and the next (e.g. "aaaddd") may result in a string representation which cannot be
    /// parsed, even with an exact format string parameter. Therefore a separator is always
    /// recommended (even whitespace is sufficient: " ").
    /// </para>
    /// <para>
    /// If <paramref name="format"/> is empty, or an unrecognized single character, the standard
    /// format specifier ("G") is used.
    /// </para>
    /// <para>
    /// A perpetual <see cref="Duration"/> is represented with the infinity symbol ("∞")
    /// regardless of format.
    /// </para>
    /// </remarks>
    public string ToString(string format, IFormatProvider formatProvider) => Now.ToString(format, formatProvider);

    /// <summary>
    /// Converts the value of <see cref="Now"/> to its equivalent string representation using
    /// the specified culture-specific format information.
    /// </summary>
    /// <param name="formatProvider">An object that supplies culture-specific formatting
    /// information.</param>
    /// <returns>A string representation of value of <see cref="Now"/>
    /// as specified by <paramref name="formatProvider"/>.</returns>
    /// <exception cref="FormatException">
    /// <paramref name="formatProvider"/> is not a valid provider for <see cref="Duration"/>.
    /// </exception>
    public string ToString(IFormatProvider formatProvider) => Now.ToString(formatProvider);

    /// <summary>
    /// Converts the value of <see cref="Now"/> to its equivalent string representation using
    /// the specified <paramref name="format"/> and culture-specific format information.
    /// </summary>
    /// <param name="format">
    /// <para>
    /// A standard or custom date and time format string.
    /// </para>
    /// <para>
    /// Not all <see cref="DateTime"/> format strings are recognized. See Remarks for more info.
    /// </para>
    /// </param>
    /// <returns>A string representation of value of <see cref="Now"/>
    /// as specified by <paramref name="format"/>.</returns>
    /// <remarks>
    /// <para>
    /// The <paramref name="format"/> parameter can contain any of these single-character
    /// standard format strings:
    /// </para>
    /// <list type="table">
    /// <listheader>
    /// <term>Format specifier</term>
    /// <term>Description</term>
    /// </listheader>
    /// <item>
    /// <term>"d"</term>
    /// <description>Short date format. Corresponds to the "y d" custom format
    /// string.</description>
    /// </item>
    /// <item>
    /// <term>"D"</term>
    /// <description>Long date format. Corresponds to the "e d" custom format
    /// string.</description>
    /// </item>
    /// <item>
    /// <term>"E"</term>
    /// <description>General extended format. Corresponds to the "y d
    /// HH:mm:ss:MMM:uuu:nnn:ppp:fff:aaa:zzz:YYY:PPPPPPPPPPPPPPPPP" custom format
    /// string.</description>
    /// </item>
    /// <item>
    /// <term>"f"</term>
    /// <description>Full date short time format. Corresponds to the Long date and short time,
    /// separated by a space.</description>
    /// </item>
    /// <item>
    /// <term>"F"</term>
    /// <description>Full date long time format. Corresponds to the Long date and short time,
    /// separated by a space.</description>
    /// </item>
    /// <item>
    /// <term>"g"</term>
    /// <description>General short format. Corresponds to the "y d HH:mm" custom format
    /// string.</description>
    /// </item>
    /// <item>
    /// <term>"G"</term>
    /// <description>General long format. The default if none is selected. Corresponds to the "y
    /// d HH:mm:ss" custom format string.</description>
    /// </item>
    /// <item>
    /// <term>"O", "o"</term>
    /// <description>Round trip format. Corresponds to the
    /// "e'-'d'T'HH':'mm':'ss':'MMM':'uuu':'nnn':'ppp':'fff':'aaa':'zzz':'YYY':'PPPPPPPPPPPPPPPPP"
    /// custom format string. Because the length of certain segments varies (unavoidably, to
    /// prevent strings from being longer than <see cref="int.MaxValue"/> to account for <see
    /// cref="Duration"/> instances with the maximum number of aeons), this format is unsuitable
    /// for sorting.</description>
    /// </item>
    /// <item>
    /// <term>"t"</term>
    /// <description>Short time format. Corresponds to the "HH:mm" custom format
    /// string.</description>
    /// </item>
    /// <item>
    /// <term>"T"</term>
    /// <description>Long time format. Corresponds to the "HH:mm:ss" custom format
    /// string.</description>
    /// </item>
    /// <item>
    /// <term>"X"</term>
    /// <description>
    /// <para>
    /// Extensible format. Corresponds to the "e' a 'd' d 'H' h 'm' min 's' s 'M' ms 'u' μs 'n'
    /// ns 'p' ps 'f' fs 'a' as 'z' zs 'Y' ys 'P' tP" custom format string, except that each
    /// unit is displayed only if it is non-zero.
    /// </para>
    /// <para>
    /// The symbols denoting each unit are in SI notation, and are not culture-specific. In
    /// particular it should be noted that the symbol "a" is the SI symbol for years, which may
    /// confuse English readers on first glance.
    /// </para>
    /// </description>
    /// </item>
    /// </list>
    /// <para>
    /// The <paramref name="format"/> parameter can also or a custom format pattern with any of
    /// the following format strings:
    /// </para>
    /// <list type="table">
    /// <listheader>
    /// <term>Format specifier</term>
    /// <term>Description</term>
    /// </listheader>
    /// <item>
    /// <term>"a", "aa", "aaa"</term>
    /// <description>
    /// The number of attoseconds, from 0 to 999. The number of "a" characters in the format
    /// string represents the minimum number of significant digits; leading zeros will be added
    /// as needed.
    /// </description>
    /// </item>
    /// <item>
    /// <term>"d", "dd", "ddd"</term>
    /// <description>
    /// The number of days, from 0 to 365. Always displayed using as many digits as required,
    /// regardless of the number of "d" characters in the format string, but will not be
    /// displayed with <i>fewer</i> digits than there are "d" characters in the string; leading
    /// zeros will be added as needed.
    /// </description>
    /// </item>
    /// <item>
    /// <term>"dddd"</term>
    /// <description>Not recognized; <see cref="Duration"/> does not count weeks.</description>
    /// </item>
    /// <item>
    /// <term>"e", "ee", "eee", etc.</term>
    /// The number of years, <i>including</i> those represented by aeons. Always displayed in G
    /// format, with precision equal to the number of "e" characters, except when only 1 "e" is
    /// present, which indicates "G29" format. Successful round-trip operations are not possible
    /// for values larger than can be accurately represented with the chosen specification, and
    /// since aeons may contain a number of significant digits greater than can be recorded in a
    /// <see cref="string"/> without causing memory overflow, loss of precision may be
    /// unavoidable.
    /// </item>
    /// <item>
    /// <term>"f", "ff", "fff"</term>
    /// <description>
    /// The number of femtoseconds, from 0 to 999. The number of "f" characters in the format
    /// string represents the minimum number of significant digits; leading zeros will be added
    /// as needed.
    /// </description>
    /// </item>
    /// <item>
    /// <term>"F", "FF", "FFF", etc.</term>
    /// <description>
    /// <para>
    /// Displays fractions of a second to as many significant digits as there are "F"
    /// characters. Note that unlike <see cref="DateTime"/> format strings, the lower case "f"
    /// is not used to display a variable number of fractional seconds. The "f" is reserved in
    /// <see cref="Duration"/> format strings for femtoseconds.
    /// </para>
    /// <para>
    /// Note that if fractional seconds and a unit of time smaller than seconds are both
    /// displayed, aside from causing confusion this will result in failure to round-trip (the
    /// values of the fractional seconds and the smaller units each be counted on parse, causing
    /// the value to be larger than expected).
    /// </para>
    /// </description>
    /// </item>
    /// <item>
    /// <term>"g", "gg"</term>
    /// <description>Not recognized; <see cref="Duration"/> does not count periods or
    /// eras.</description>
    /// </item>
    /// <item>
    /// <term>"h", "H"</term>
    /// <description>The number of hours, from 0 to 23. 12-hour time is not recognized, since a
    /// <see cref="Duration"/> represents an amount of time, not a time of day.</description>
    /// </item>
    /// <item>
    /// <term>"hh", "HH", "hH", "Hh"</term>
    /// <description>The number of hours, from 00 to 23. Always displays two digits, even for
    /// single-digit values. 12-hour time is not recognized, since a <see cref="Duration"/>
    /// represents an amount of time, not a time of day.</description>
    /// </item>
    /// <item>
    /// <term>"K"</term>
    /// <description>Not recognized; <see cref="Duration"/> does not count time
    /// zones.</description>
    /// </item>
    /// <item>
    /// <term>"m", "mm"</term>
    /// <description>
    /// The number of minutes, from 0 to 59. The number of "m" characters in the format string
    /// represents the minimum number of significant digits; leading zeros will be added as
    /// needed.
    /// </description>
    /// </item>
    /// <item>
    /// <term>"M", "MM", "MMM"</term>
    /// <description>
    /// <para>
    /// The number of milliseconds, from 0 to 999. The number of "M" characters in the format
    /// string represents the minimum number of significant digits; leading zeros will be added
    /// as needed.
    /// </para>
    /// <para>
    /// Note carefully that this is in contrast to <see cref="DateTime"/> format strings, which
    /// uses "M" to denote months.
    /// </para>
    /// </description>
    /// </item>
    /// <item>
    /// <term>"n", "nn", "nnn"</term>
    /// <description>
    /// The number of nanoseconds, from 0 to 999. The number of "n" characters in the format
    /// string represents the minimum number of significant digits; leading zeros will be added
    /// as needed.
    /// </description>
    /// </item>
    /// <item>
    /// <term>"p", "pp", "ppp"</term>
    /// <description>
    /// The number of picoseconds, from 0 to 999. The number of "p" characters in the format
    /// string represents the minimum number of significant digits; leading zeros will be added
    /// as needed.
    /// </description>
    /// </item>
    /// <item>
    /// <term>"P", "PP", "PPP", etc.</term>
    /// <description>
    /// The amount of Planck time. Always displayed in G format, with precision equal to the
    /// number of "p" characters, except when only 1 "p" is present, which indicates "G" format.
    /// 17 should be used to round-trip.
    /// </description>
    /// </item>
    /// <item>
    /// <term>"s", "ss"</term>
    /// <description>
    /// The number of seconds, from 0 to 59. The number of "s" characters in the format string
    /// represents the minimum number of significant digits; leading zeros will be added as
    /// needed.
    /// </description>
    /// </item>
    /// <item>
    /// <term>"t", "tt"</term>
    /// <description>Not recognized; <see cref="Duration"/> represents an amount of time, not a
    /// time of day.</description>
    /// </item>
    /// <item>
    /// <term>"u", "uu", "uuu"</term>
    /// <description>
    /// The number of microseconds, from 0 to 999. The number of "U" characters represents the
    /// minimum number of significant digits displayed.
    /// </description>
    /// </item>
    /// <item>
    /// <term>"y", "yy", "yyy", etc.</term>
    /// <description>
    /// The number of years, not counting those represented by aeons. The number of "y"
    /// characters in the format string represents the minimum number of significant digits;
    /// leading zeros will be added as needed.
    /// </description>
    /// </item>
    /// <item>
    /// <term>"Y", "YY", "YYY"</term>
    /// <description>
    /// The number of yoctoseconds, from 0 to 999. The number of "Y" characters in the format
    /// string represents the minimum number of significant digits; leading zeros will be added
    /// as needed.
    /// </description>
    /// </item>
    /// <item>
    /// <term>"z", "zz", "zzz"</term>
    /// <description>
    /// <para>
    /// The number of zeptoseconds, from 0 to 999. The number of "z" characters in the format
    /// string represents the minimum number of significant digits; leading zeros will be added
    /// as needed.
    /// </para>
    /// <para>
    /// Note carefully that this is in contrast to <see cref="DateTime"/> format strings, which
    /// uses "z" to denote time zone.
    /// </para>
    /// </description>
    /// </item>
    /// <item>
    /// <term>":"</term>
    /// <description>The time separator.</description>
    /// </item>
    /// <item>
    /// <term>"/"</term>
    /// <description>The date separator.</description>
    /// </item>
    /// <item>
    /// <term>"<i>string</i>", '<i>string</i>'</term>
    /// <description>Literal string delimiters.</description>
    /// </item>
    /// <item>
    /// <term>"%"</term>
    /// <description>Defines the following character as a custom format specifier.</description>
    /// </item>
    /// <item>
    /// <term>"\"</term>
    /// <description>The escape character.</description>
    /// </item>
    /// <item>
    /// <term>Any other character</term>
    /// <description>The character is copied to the result string unchanged.</description>
    /// </item>
    /// </list>
    /// <para>
    /// Since the length of various format elements can vary independently of the length of the
    /// format string, custom formats which do not provide a separator between these elements
    /// and the next (e.g. "aaaddd") may result in a string representation which cannot be
    /// parsed, even with an exact format string parameter. Therefore a separator is always
    /// recommended (even whitespace is sufficient: " ").
    /// </para>
    /// <para>
    /// If <paramref name="format"/> is empty, or an unrecognized single character, the standard
    /// format specifier ("G") is used.
    /// </para>
    /// <para>
    /// A perpetual <see cref="Duration"/> is represented with the infinity symbol ("∞")
    /// regardless of format.
    /// </para>
    /// </remarks>
    public string ToString(string format) => Now.ToString(format);

    /// <summary>
    /// <para>
    /// Converts the value of <see cref="Now"/> to its equivalent string representation using
    /// the default format and current culture.
    /// </para>
    /// <para>
    /// A perpetual <see cref="Duration"/> is represented with the infinity symbol ("∞")
    /// regardless of format.
    /// </para>
    /// </summary>
    public override string ToString() => Now.ToString();

    private int GetEpochsHashCode()
    {
        unchecked
        {
            return 367 * Epochs
                .Aggregate(0, (a, c) => (a * 397) ^ c.GetHashCode());
        }
    }

#pragma warning disable CS1591

    public static explicit operator CosmicTime(DateTime value) => FromDateTime(value);

    public static explicit operator DateTime(CosmicTime value) => value.ToDateTime();

    public static explicit operator CosmicTime(DateTimeOffset value) => FromDateTimeOffset(value);

    public static explicit operator DateTimeOffset(CosmicTime value) => value.ToDateTimeOffset();

    public static bool operator ==(CosmicTime? left, CosmicTime? right) => EqualityComparer<CosmicTime>.Default.Equals(left, right);
    public static bool operator !=(CosmicTime? left, CosmicTime? right) => !(left == right);

#pragma warning restore CS1591
}
