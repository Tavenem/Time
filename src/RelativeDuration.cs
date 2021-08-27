using System.Text.Json.Serialization;
using Tavenem.Time.Converters;

namespace Tavenem.Time;

/// <summary>
/// <para>
/// Represents a duration which can be either absolute (<seealso
/// cref="Duration"/>), or relative to the duration of a local year or day (i.e.
/// the revolution or rotation period of a planet), when the actual durations of the local year
/// and/or day are unknown. For example, "one quarter of one day" can be represented in a
/// uniform, consistent way even when the length of a day might vary from place to place.
/// Capable of accurately representing extremely large and small time intervals, suitable for
/// time scales such as the age of a universe, with precision down to Planck time.
/// </para>
/// <para>
/// Also able to represent an infinite (perpetual) duration.
/// </para>
/// </summary>
[JsonConverter(typeof(RelativeDurationConverter))]
public readonly partial struct RelativeDuration :
    IDivisionOperators<RelativeDuration, double, RelativeDuration>,
    IEqualityOperators<RelativeDuration, RelativeDuration>,
    IEqualityOperators<RelativeDuration, Duration>,
    IEqualityOperators<RelativeDuration, DateTime>,
    IEqualityOperators<RelativeDuration, TimeSpan>,
    IMultiplyOperators<RelativeDuration, double, RelativeDuration>,
    ISpanFormattable,
    ISpanParseable<RelativeDuration>
{
    private const string DayString = "Dx";
    private const string YearString = "Yx";

    /// <summary>
    /// A duration representing an infinite amount of time into the past. Read-only.
    /// </summary>
    public static readonly RelativeDuration NegativeInfinity = new(Duration.NegativeInfinity);

    /// <summary>
    /// A duration representing an infinite amount of time. Read-only.
    /// </summary>
    public static readonly RelativeDuration PositiveInfinity = new(Duration.PositiveInfinity);

    /// <summary>
    /// A duration representing no time. Read-only.
    /// </summary>
    public static readonly RelativeDuration Zero = new(Duration.Zero);

    /// <summary>
    /// The absolute duration represented by this instance. Will be <see cref="Duration.Zero"/>
    /// if <see cref="Relativity"/> is not <see cref="RelativeDurationType.Absolute"/>.
    /// </summary>
    public Duration Duration { get; }

    /// <summary>
    /// Indicates that this value represents an infinite amount of time.
    /// </summary>
    [JsonIgnore]
    public bool IsPerpetual => Relativity == RelativeDurationType.Absolute && Duration.IsPerpetual;

    /// <summary>
    /// Whether this value represents zero time.
    /// </summary>
    [JsonIgnore]
    public bool IsZero => Relativity == RelativeDurationType.Absolute ? Duration.IsZero : Proportion == 0;

    /// <summary>
    /// The proportion of a local day or year represented by this instance. Will be 0 if <see
    /// cref="Relativity"/> is <see cref="RelativeDurationType.Absolute"/>.
    /// </summary>
    public double Proportion { get; }

    /// <summary>
    /// <para>
    /// Indicates the type of time measurement used by this instance: absolute, relative to a
    /// local day, or relative to a local year.
    /// </para>
    /// <para>
    /// Determines whether <see cref="Duration"/> or <see cref="Proportion"/> is used to
    /// determine the duration represented by this instance.
    /// </para>
    /// </summary>
    public RelativeDurationType Relativity { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="RelativeDuration"/> with the given absolute
    /// <see cref="Duration"/>.
    /// </summary>
    /// <param name="duration">A <see cref="Duration"/> to set as the absolute
    /// value of this instance.</param>
    public RelativeDuration(Duration duration)
    {
        Duration = duration;
        Proportion = 0;
        Relativity = RelativeDurationType.Absolute;
    }

    /// <summary>
    /// Initializes a new instance of <see cref="RelativeDuration"/> with the given absolute
    /// <see cref="Duration"/>.
    /// </summary>
    /// <param name="proportion">
    /// The proportion of a local day or year represented by this instance. Will be 0 if
    /// <paramref name="relativity"/> is <see cref="RelativeDurationType.Absolute"/>.
    /// </param>
    /// <param name="relativity">
    /// <para>
    /// Indicates the type of time measurement used by this instance: absolute, relative to a
    /// local day, or relative to a local year.
    /// </para>
    /// <para>
    /// Determines whether <see cref="Duration"/> or <see cref="Proportion"/> is used to
    /// determine the duration represented by this instance.
    /// </para>
    /// </param>
    public RelativeDuration(decimal proportion, RelativeDurationType relativity)
    {
        Duration = Duration.Zero;
        Proportion = (double)Math.Max(0, proportion);
        Relativity = relativity;
    }

    /// <summary>
    /// Initializes a new instance of <see cref="RelativeDuration"/> with the given absolute
    /// <see cref="Duration"/>.
    /// </summary>
    /// <param name="proportion">
    /// The proportion of a local day or year represented by this instance. Will be 0 if
    /// <paramref name="relativity"/> is <see cref="RelativeDurationType.Absolute"/>.
    /// </param>
    /// <param name="relativity">
    /// <para>
    /// Indicates the type of time measurement used by this instance: absolute, relative to a
    /// local day, or relative to a local year.
    /// </para>
    /// <para>
    /// Determines whether <see cref="Duration"/> or <see cref="Proportion"/> is used to
    /// determine the duration represented by this instance.
    /// </para>
    /// </param>
    public RelativeDuration(double proportion, RelativeDurationType relativity)
    {
        Duration = Duration.Zero;
        Proportion = Math.Max(0, proportion);
        Relativity = relativity;
    }

    /// <summary>
    /// Initializes a new instance of <see cref="RelativeDuration"/> with the given absolute
    /// <see cref="Duration"/>.
    /// </summary>
    /// <param name="duration">A <see cref="Duration"/> to set as the absolute
    /// value of this instance.</param>
    /// <param name="proportion">
    /// The proportion of a local day or year represented by this instance. Will be 0 if
    /// <paramref name="relativity"/> is <see cref="RelativeDurationType.Absolute"/>.
    /// </param>
    /// <param name="relativity">
    /// <para>
    /// Indicates the type of time measurement used by this instance: absolute, relative to a
    /// local day, or relative to a local year.
    /// </para>
    /// <para>
    /// Determines whether <see cref="Duration"/> or <see cref="Proportion"/> is used to
    /// determine the duration represented by this instance.
    /// </para>
    /// </param>
    [JsonConstructor]
    public RelativeDuration(Duration duration, double proportion, RelativeDurationType relativity)
    {
        Duration = relativity != RelativeDurationType.Absolute ? Duration.Zero : duration;
        Proportion = relativity == RelativeDurationType.Absolute ? 0 : Math.Max(0, proportion);
        Relativity = relativity;
    }

    /// <summary>
    /// Gets the maximum of two <see cref="RelativeDuration"/> instances.
    /// </summary>
    /// <param name="first">The first instance.</param>
    /// <param name="localYearFirst">The duration of the local year for <paramref
    /// name="first"/>.</param>
    /// <param name="localDayFirst">The duration of the local day for <paramref name="first"/>.</param>
    /// <param name="second">The second instance.</param>
    /// <param name="localYearSecond">The duration of the local year for <paramref
    /// name="second"/>.</param>
    /// <param name="localDaySecond">The duration of the local day for <paramref name="second"/>.</param>
    /// <returns>The maximum of two <see cref="RelativeDuration"/> instances.</returns>
    public static RelativeDuration Max(
        RelativeDuration first, Duration localYearFirst, Duration localDayFirst,
        RelativeDuration second, Duration localYearSecond, Duration localDaySecond)
        => first.ToUniversalDuration(localYearFirst, localDayFirst)
        .CompareTo(second.ToUniversalDuration(localYearSecond, localDaySecond)) >= 0 ? first : second;

    /// <summary>
    /// Gets the minimum of two <see cref="RelativeDuration"/> instances.
    /// </summary>
    /// <param name="first">The first instance.</param>
    /// <param name="localYearFirst">The duration of the local year for <paramref
    /// name="first"/>.</param>
    /// <param name="localDayFirst">The duration of the local day for <paramref name="first"/>.</param>
    /// <param name="second">The second instance.</param>
    /// <param name="localYearSecond">The duration of the local year for <paramref
    /// name="second"/>.</param>
    /// <param name="localDaySecond">The duration of the local day for <paramref name="second"/>.</param>
    /// <returns>The minimum of two <see cref="RelativeDuration"/> instances.</returns>
    public static RelativeDuration Min(
        RelativeDuration first, Duration localYearFirst, Duration localDayFirst,
        RelativeDuration second, Duration localYearSecond, Duration localDaySecond)
        => first.ToUniversalDuration(localYearFirst, localDayFirst)
        .CompareTo(second.ToUniversalDuration(localYearSecond, localDaySecond)) <= 0 ? first : second;

    /// <summary>
    /// <para>
    /// Divides this instance by the given amount.
    /// </para>
    /// <para>
    /// Results in <see cref="Zero"/> if this <see cref="IsZero"/> is <see langword="true"/>
    /// for this instance and <paramref name="divisor"/> is zero.
    /// </para>
    /// <para>
    /// Results in <see cref="PositiveInfinity"/> if <see cref="IsPerpetual"/> is <see langword="true"/>
    /// for this instance, or <paramref name="divisor"/> is zero.
    /// </para>
    /// <para>
    /// Results in <see cref="Zero"/> if <paramref name="divisor"/> is negative or <see
    /// cref="double.PositiveInfinity"/>.
    /// </para>
    /// </summary>
    /// <param name="divisor">An amount by which to divide this instance.</param>
    /// <returns>A new <see cref="RelativeDuration"/> instance.</returns>
    public RelativeDuration Divide(double divisor)
    {
        if (IsZero)
        {
            return Zero;
        }
        if (IsPerpetual || divisor == 0)
        {
            return PositiveInfinity;
        }
        if (divisor < 0 || double.IsInfinity(divisor))
        {
            return Zero;
        }

        return Multiply(1 / divisor);
    }

    /// <summary>
    /// <para>
    /// Multiplies this instance by the given amount.
    /// </para>
    /// <para>
    /// Results in <see cref="Zero"/> if <paramref name="factor"/> is negative.
    /// </para>
    /// </summary>
    /// <param name="factor">An amount by which to multiply this instance.</param>
    /// <returns>A new <see cref="RelativeDuration"/> instance.</returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="factor"/> is <see cref="double.NaN"/>.
    /// </exception>
    public RelativeDuration Multiply(double factor)
    {
        if (double.IsNaN(factor))
        {
            throw new ArgumentException($"{nameof(factor)} was NaN.");
        }
        if (factor <= 0)
        {
            return Zero;
        }
        if (factor == 0)
        {
            return Zero;
        }
        if (IsPerpetual || double.IsPositiveInfinity(factor))
        {
            return PositiveInfinity;
        }

        return Relativity switch
        {
            RelativeDurationType.Absolute => new RelativeDuration(Duration * factor),
            RelativeDurationType.ProportionOfDay => new RelativeDuration(Proportion * factor, RelativeDurationType.ProportionOfDay),
            RelativeDurationType.ProportionOfYear => new RelativeDuration(Proportion * factor, RelativeDurationType.ProportionOfYear),
            _ => Zero,
        };
    }

    /// <summary>
    /// Indicates whether this <see cref="RelativeDuration"/> instance and another are equal.
    /// </summary>
    /// <param name="other">The <see cref="RelativeDuration"/> instance to compare with this
    /// one.</param>
    /// <returns>
    /// <see langword="true"/> if the <see cref="RelativeDuration"/> instance and this one are
    /// equal; otherwise <see langword="false"/>.
    /// </returns>
    public bool Equals(RelativeDuration other)
        => Duration == other.Duration
        && Proportion == other.Proportion
        && Relativity == other.Relativity;

    /// <summary>
    /// Indicates whether this instance and a specified object are equal.
    /// </summary>
    /// <param name="obj">The object to compare with the current instance.</param>
    /// <returns>
    /// <see langword="true"/> if the object and the current instance are equal; otherwise <see
    /// langword="false"/>.
    /// </returns>
    public override bool Equals(object? obj) => obj is RelativeDuration other && Equals(other);

    /// <summary>
    /// Indicates whether this <see cref="RelativeDuration"/> instance and a <see
    /// cref="Duration"/> instance are equal.
    /// </summary>
    /// <param name="other">The <see cref="Duration"/> instance to compare with this
    /// <see cref="RelativeDuration"/> instance.</param>
    /// <returns>
    /// <see langword="true"/> if the <see cref="Duration"/> instance and this <see
    /// cref="RelativeDuration"/> instance are equal; otherwise <see langword="false"/>.
    /// </returns>
    public bool Equals(Duration other)
        => Relativity == RelativeDurationType.Absolute && other.Equals(Duration);

    /// <summary>
    /// Indicates whether this <see cref="RelativeDuration"/> instance and a <see
    /// cref="DateTime"/> instance are equal.
    /// </summary>
    /// <param name="other">The <see cref="DateTime"/> instance to compare with this
    /// <see cref="RelativeDuration"/> instance.</param>
    /// <returns>
    /// <see langword="true"/> if the <see cref="DateTime"/> instance and this <see
    /// cref="RelativeDuration"/> instance are equal; otherwise <see langword="false"/>.
    /// </returns>
    public bool Equals(DateTime other)
        => Relativity == RelativeDurationType.Absolute && Duration.Equals(other);

    /// <summary>
    /// Indicates whether this <see cref="RelativeDuration"/> instance and a <see
    /// cref="DateTimeOffset"/> instance are equal.
    /// </summary>
    /// <param name="other">The <see cref="DateTimeOffset"/> instance to compare with this
    /// <see cref="RelativeDuration"/> instance.</param>
    /// <returns>
    /// <see langword="true"/> if the <see cref="DateTimeOffset"/> instance and this <see
    /// cref="RelativeDuration"/> instance are equal; otherwise <see langword="false"/>.
    /// </returns>
    public bool Equals(DateTimeOffset other)
        => Relativity == RelativeDurationType.Absolute && Duration.Equals(other);

    /// <summary>
    /// Indicates whether this <see cref="RelativeDuration"/> instance and a <see
    /// cref="TimeSpan"/> instance are equal.
    /// </summary>
    /// <param name="other">The <see cref="TimeSpan"/> instance to compare with this
    /// <see cref="RelativeDuration"/> instance.</param>
    /// <returns>
    /// <see langword="true"/> if the <see cref="TimeSpan"/> instance and this <see
    /// cref="RelativeDuration"/> instance are equal; otherwise <see langword="false"/>.
    /// </returns>
    public bool Equals(TimeSpan other)
        => Relativity == RelativeDurationType.Absolute && Duration.Equals(other);

    /// <summary>
    /// Returns the hash code for this instance.
    /// </summary>
    /// <returns>The hash code for this instance.</returns>
    public override int GetHashCode() => HashCode.Combine(Duration, Proportion, Relativity);
}
