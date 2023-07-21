using System.Numerics;
using System.Text.Json.Serialization;
using Tavenem.Time.Converters;

namespace Tavenem.Time;

/// <summary>
/// <para>
/// Represents an absolute duration. Capable of accurately representing extremely large and
/// small time intervals, suitable for time scales such as the age of a universe, with precision
/// down to Planck time.
/// </para>
/// <para>
/// Uses SI days (86,400 seconds exactly) and astronomical (Julian) years (365.25 days, or
/// 31,557,600s exactly), rather than days or years based on the solar cycles of Earth (or any
/// other celestial movements). This avoids complications such as time zones and leap years (or
/// leap seconds, or leap anything else).
/// </para>
/// <para>
/// Also able to represent an infinite (perpetual) duration.
/// </para>
/// </summary>
[JsonConverter(typeof(DurationConverter))]
public readonly partial struct Duration :
    IAdditiveIdentity<Duration, Duration>,
    IAdditionOperators<Duration, Duration, Duration>,
    IAdditionOperators<Duration, TimeSpan, Duration>,
    IDivisionOperators<Duration, Duration, double>,
    IDivisionOperators<Duration, decimal, Duration>,
    IDivisionOperators<Duration, double, Duration>,
    IEqualityOperators<Duration, RelativeDuration, bool>,
    IComparisonOperators<Duration, Duration, bool>,
    IComparisonOperators<Duration, DateTime, bool>,
    IComparisonOperators<Duration, DateTimeOffset, bool>,
    IComparisonOperators<Duration, TimeSpan, bool>,
    IModulusOperators<Duration, Duration, Duration>,
    IMultiplyOperators<Duration, decimal, Duration>,
    IMultiplyOperators<Duration, double, Duration>,
    IFormattable,
    ISpanFormattable,
    ISpanParsable<Duration>,
    ISubtractionOperators<Duration, Duration, Duration>,
    ISubtractionOperators<Duration, TimeSpan, Duration>,
    IUnaryNegationOperators<Duration, Duration>,
    IUnaryPlusOperators<Duration, Duration>
{
    /// <summary>
    /// A duration representing an infinite amount of time into the past. Read-only.
    /// </summary>
    public static Duration NegativeInfinity { get; } = new(
        true,
        true,
        null,
        0,
        0,
        0,
        null);

    /// <summary>
    /// A duration representing an infinite amount of time. Read-only.
    /// </summary>
    public static Duration PositiveInfinity { get; } = new(
        false,
        true,
        null,
        0,
        0,
        0,
        null);

    /// <summary>
    /// A duration representing no time. Read-only.
    /// </summary>
    public static Duration Zero { get; } = new();
    /// <summary>
    /// A duration representing no time. Read-only.
    /// </summary>
    public static Duration AdditiveIdentity => Zero;

    private static readonly BigInteger _DefaultAeons = 13799;

    /// <summary>
    /// <para>
    /// The number of astronomical aeons represented by this <see cref="Duration"/>.
    /// </para>
    /// <para>
    /// An astronomical aeon (AE) is 10e9 years.
    /// </para>
    /// </summary>
    [JsonConverter(typeof(BigIntegerConverter))]
    public BigInteger? Aeons { get; }

    /// <summary>
    /// The number of whole attoseconds represented by this <see cref="Duration"/> beyond its
    /// <see cref="Femtoseconds"/>. This is a calculated property which reflects the value
    /// of <see cref="TotalYoctoseconds"/>.
    /// </summary>
    [JsonIgnore]
    public uint Attoseconds => (uint)(TotalYoctoseconds / YoctosecondsPerAttosecond % AttosecondsPerFemtosecond);

    /// <summary>
    /// <para>
    /// The number of whole days represented by this <see cref="Duration"/> beyond its
    /// <see cref="Years"/>.
    /// </para>
    /// <para>
    /// This is a calculated property which reflects the value of <see cref="TotalNanoseconds"/>.
    /// </para>
    /// <para>
    /// An SI day is exactly 86400 seconds long.
    /// </para>
    /// </summary>
    [JsonIgnore]
    public uint Days => (uint)(TotalNanoseconds / NanosecondsPerDay);

    /// <summary>
    /// The number of whole femtoseconds represented by this <see cref="Duration"/> beyond its
    /// <see cref="Picoseconds"/>. This is a calculated property which reflects the value
    /// of <see cref="TotalYoctoseconds"/>.
    /// </summary>
    [JsonIgnore]
    public uint Femtoseconds => (uint)(TotalYoctoseconds / YoctosecondsPerFemtosecond % FemtosecondsPerPicosecond);

    /// <summary>
    /// <para>
    /// The number of whole hours represented by this <see cref="Duration"/> beyond its
    /// <see cref="Days"/>.
    /// </para>
    /// <para>
    /// This is a calculated property which reflects the value of <see cref="TotalNanoseconds"/>.
    /// </para>
    /// </summary>
    [JsonIgnore]
    public uint Hours => (uint)(TotalNanoseconds / NanosecondsPerHour % HoursPerDay);

    /// <summary>
    /// Indicates that this <see cref="Duration"/> represents a negative duration.
    /// </summary>
    public bool IsNegative { get; }

    /// <summary>
    /// Indicates that this <see cref="Duration"/> represents an infinite amount of
    /// time (positive or negative).
    /// </summary>
    public bool IsPerpetual { get; }

    /// <summary>
    /// Indicates that this <see cref="Duration"/> represents an infinite amount of
    /// time in the positive direction.
    /// </summary>
    [JsonIgnore]
    public bool IsPositiveInfinity => IsPerpetual && !IsNegative;

    /// <summary>
    /// Indicates that this <see cref="Duration"/> represents an infinite amount of
    /// time in the negative direction.
    /// </summary>
    [JsonIgnore]
    public bool IsNegativeInfinity => IsPerpetual && IsNegative;

    /// <summary>
    /// Whether this <see cref="Duration"/> represents zero time.
    /// </summary>
    [JsonIgnore]
    public bool IsZero => !IsPerpetual
        && Years == 0
        && TotalNanoseconds == 0
        && TotalYoctoseconds == 0
        && PlanckTime?.IsZero != false
        && Aeons?.IsZero != false;

    /// <summary>
    /// The number of whole microseconds represented by this <see cref="Duration"/> beyond its
    /// <see cref="Milliseconds"/>. This is a calculated property which reflects the value of <see
    /// cref="TotalNanoseconds"/>.
    /// </summary>
    [JsonIgnore]
    public uint Microseconds => (uint)(TotalNanoseconds / NanosecondsPerMicrosecond % MicrosecondsPerMillisecond);

    /// <summary>
    /// The number of whole milliseconds represented by this <see cref="Duration"/> beyond its <see
    /// cref="Seconds"/>. This is a calculated property which reflects the value of <see
    /// cref="TotalNanoseconds"/>.
    /// </summary>
    [JsonIgnore]
    public uint Milliseconds => (uint)(TotalNanoseconds / NanosecondsPerMillisecond % MillisecondsPerSecond);

    /// <summary>
    /// <para>
    /// The number of whole hours represented by this <see cref="Duration"/> beyond its
    /// <see cref="Hours"/>.
    /// </para>
    /// <para>
    /// This is a calculated property which reflects the value of <see cref="TotalNanoseconds"/>.
    /// </para>
    /// </summary>
    [JsonIgnore]
    public uint Minutes => (uint)(TotalNanoseconds / NanosecondsPerMinute % MinutesPerHour);

    /// <summary>
    /// <para>
    /// The number of whole nanoseconds represented by this <see cref="Duration"/> beyond its
    /// <see cref="Microseconds"/>.
    /// </para>
    /// <para>
    /// This is a calculated property which reflects the value of <see
    /// cref="TotalNanoseconds"/>.
    /// </para>
    /// </summary>
    /// <remarks>
    /// The property <see cref="TotalNanoseconds"/> records all nanoseconds in excess of <see
    /// cref="Years"/>, rather than only those since the last microsecond. This calculated
    /// property is provided as a convenience for obtaining the more intuitive number of
    /// nanoseconds.
    /// </remarks>
    [JsonIgnore]
    public uint Nanoseconds => (uint)(TotalNanoseconds % NanosecondsPerMicrosecond);

    /// <summary>
    /// The number of whole picoseconds represented by this <see cref="Duration"/> beyond its
    /// <see cref="Yoctoseconds"/>. This is a calculated property which reflects the value
    /// of <see cref="TotalYoctoseconds"/>.
    /// </summary>
    [JsonIgnore]
    public uint Picoseconds => (uint)(TotalYoctoseconds / YoctosecondsPerPicosecond);

    /// <summary>
    /// <para>
    /// The amount of Planck time represented by this <see cref="Duration"/> beyond its
    /// <see cref="Yoctoseconds"/>.
    /// </para>
    /// <para>
    /// Cannot be negative; only the <see cref="Duration"/> as a whole can be negative.
    /// </para>
    /// </summary>
    /// <remarks>
    /// Although Planck time does not divide evenly into a yoctosecond, this property is treated as
    /// though it did. When the value exceeds <see cref="PlanckTimePerYoctosecond"/> it will
    /// rollover to one yoctosecond.
    /// </remarks>
    [JsonConverter(typeof(BigIntegerConverter))]
    public BigInteger? PlanckTime { get; }

    /// <summary>
    /// <para>
    /// The number of whole seconds represented by this <see cref="Duration"/> beyond its
    /// <see cref="Minutes"/>.
    /// </para>
    /// <para>
    /// This is a calculated property which reflects the value of <see cref="TotalNanoseconds"/>.
    /// </para>
    /// </summary>
    [JsonIgnore]
    public uint Seconds => (uint)(TotalNanoseconds / NanosecondsPerSecond % SecondsPerMinute);

    /// <summary>
    /// A number that indicates the sign (negative, positive, or zero) of this instance.
    /// </summary>
    [JsonIgnore]
    public int Sign
    {
        get
        {
            if (IsNegative)
            {
                return -1;
            }
            else if (IsZero)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }
    }

    /// <summary>
    /// The total number of nanoseconds represented by this <see cref="Duration"/> beyond its <see
    /// cref="Years"/>.
    /// </summary>
    /// <remarks>
    /// Note carefully that this is not nanoseconds in excess of <see cref="Microseconds"/>. <see
    /// cref="Milliseconds"/>, <see cref="Microseconds"/>, and <see cref="Nanoseconds"/> are all
    /// calculated properties based on <see cref="TotalNanoseconds"/>.
    /// </remarks>
    public ulong TotalNanoseconds { get; }

    /// <summary>
    /// The total number of yoctoseconds represented by this <see cref="Duration"/> beyond its <see
    /// cref="TotalNanoseconds"/>.
    /// </summary>
    /// <remarks>
    /// Note carefully that this is not yoctoseconds in excess of <see cref="Zeptoseconds"/>. <see
    /// cref="Picoseconds"/>, <see cref="Femtoseconds"/>, <see cref="Attoseconds"/>, <see
    /// cref="Zeptoseconds"/>, and <see cref="Yoctoseconds"/> are all calculated properties based on
    /// <see cref="TotalYoctoseconds"/>.
    /// </remarks>
    public ulong TotalYoctoseconds { get; }

    /// <summary>
    /// <para>
    /// The number of astronomical years represented by this <see cref="Duration"/> beyond its <see
    /// cref="Aeons"/>.
    /// </para>
    /// <para>
    /// An astronomical (Julian) year is exactly 31557600 seconds long (365.25 * 86400).
    /// </para>
    /// </summary>
    public uint Years { get; }

    /// <summary>
    /// <para>
    /// The number of whole yoctoseconds represented by this <see cref="Duration"/> beyond its <see
    /// cref="Zeptoseconds"/>.
    /// </para>
    /// <para>
    /// This is a calculated property which reflects the value of <see cref="TotalYoctoseconds"/>.
    /// </para>
    /// </summary>
    /// <remarks>
    /// The property <see cref="TotalYoctoseconds"/> records all seconds in excess of <see
    /// cref="TotalNanoseconds"/>, rather than only those since the last zeptosecond. This
    /// calculated property is provided as a convenience for obtaining the more intuitive number of
    /// yoctoseconds.
    /// </remarks>
    [JsonIgnore]
    public uint Yoctoseconds => (uint)(TotalYoctoseconds % YoctosecondsPerZeptosecond);

    /// <summary>
    /// The number of whole zeptoseconds represented by this <see cref="Duration"/> beyond its <see
    /// cref="Attoseconds"/>. This is a calculated property which reflects the value of <see
    /// cref="TotalYoctoseconds"/>.
    /// </summary>
    [JsonIgnore]
    public uint Zeptoseconds => (uint)(TotalYoctoseconds / YoctosecondsPerZeptosecond % ZeptosecondsPerAttosecond);

    /// <summary>
    /// Initializes a new instance of <see cref="Duration"/>
    /// </summary>
    /// <param name="isNegative">Whether the instance will represent a negative
    /// duration.</param>
    /// <param name="aeons">
    /// <para>
    /// The number of astronomical aeons.
    /// </para>
    /// <para>
    /// An astronomical aeon (AE) is 10e9 years.
    /// </para>
    /// <para>
    /// Cannot be negative; only the <see cref="Duration"/> as a whole can be negative. Values
    /// less than zero will be converted to 0.
    /// </para>
    /// </param>
    /// <param name="years">
    /// <para>
    /// The number of astronomical years.
    /// </para>
    /// <para>
    /// An astronomical (Julian) year is exactly 31557600 seconds long (365.25 * 86400).
    /// </para>
    /// </param>
    /// <param name="days">
    /// <para>
    /// The number of days.
    /// </para>
    /// <para>
    /// An SI day is exactly 86400 seconds long.
    /// </para>
    /// </param>
    /// <param name="hours">The number of hours.</param>
    /// <param name="minutes">The number of minutes.</param>
    /// <param name="seconds">The number of seconds.</param>
    /// <param name="milliseconds">The number of milliseconds.</param>
    /// <param name="microseconds">The number of microseconds.</param>
    /// <param name="nanoseconds">The number of nanoseconds.</param>
    /// <param name="picoseconds">The number of picoseconds.</param>
    /// <param name="femtoseconds">The number of femtoseconds.</param>
    /// <param name="attoseconds">The number of attoseconds.</param>
    /// <param name="zeptoseconds">The number of zeptoseconds.</param>
    /// <param name="yoctoseconds">The number of yoctoseconds.</param>
    /// <param name="planckTime">
    /// <para>
    /// The amount of Planck time.
    /// </para>
    /// <para>
    /// Cannot be negative; only the <see cref="Duration"/> as a whole can be negative. Values
    /// less than zero will be converted to 0.
    /// </para>
    /// </param>
    public Duration(
        bool isNegative = false,
        BigInteger? aeons = null,
        uint years = 0,
        uint days = 0,
        uint hours = 0,
        uint minutes = 0,
        uint seconds = 0,
        uint milliseconds = 0,
        uint microseconds = 0,
        ulong nanoseconds = 0,
        uint picoseconds = 0,
        uint femtoseconds = 0,
        uint attoseconds = 0,
        uint zeptoseconds = 0,
        ulong yoctoseconds = 0,
        BigInteger? planckTime = null)
    {
        IsNegative = isNegative;
        IsPerpetual = false;

        var ys = 0ul;
        var ns = 0ul;
        var y = 0u;
        BigInteger? aes = null;

        PlanckTime = planckTime <= BigInteger.Zero ? null : planckTime;
        if (PlanckTime >= PlanckTimePerYoctosecond)
        {
            var pys = PlanckTime.Value / PlanckTimePerYoctosecond;
            PlanckTime = PlanckTime.Value % PlanckTimePerYoctosecond;

            if (pys > YoctosecondsPerNanosecond)
            {
                var pns = pys / YoctosecondsPerNanosecond;
                pys %= YoctosecondsPerNanosecond;

                if (pns > NanosecondsPerYear)
                {
                    var py = pns / NanosecondsPerYear;
                    pns %= NanosecondsPerYear;

                    if (py > YearsPerAeon)
                    {
                        aes = py / YearsPerAeon;
                        py %= YearsPerAeon;
                    }

                    y = (uint)py;
                }

                ns = (uint)pns;
            }

            ys = (ulong)pys;
        }

        ys += zeptoseconds * (ulong)YoctosecondsPerZeptosecond;
        if (ys >= YoctosecondsPerNanosecond)
        {
            ns += (uint)(ys / YoctosecondsPerNanosecond);
            ys %= YoctosecondsPerNanosecond;
        }

        ys += attoseconds * (ulong)YoctosecondsPerAttosecond;
        if (ys >= YoctosecondsPerNanosecond)
        {
            ns += (uint)(ys / YoctosecondsPerNanosecond);
            ys %= YoctosecondsPerNanosecond;
        }

        ys += femtoseconds * (ulong)YoctosecondsPerFemtosecond;
        if (ys >= YoctosecondsPerNanosecond)
        {
            ns += (uint)(ys / YoctosecondsPerNanosecond);
            ys %= YoctosecondsPerNanosecond;
        }

        ys += picoseconds * (ulong)YoctosecondsPerPicosecond;
        if (ys >= YoctosecondsPerNanosecond)
        {
            ns += (uint)(ys / YoctosecondsPerNanosecond);
            ys %= YoctosecondsPerNanosecond;
        }

        ys += yoctoseconds;
        if (ys >= YoctosecondsPerNanosecond)
        {
            ns += (uint)(ys / YoctosecondsPerNanosecond);
            ys %= YoctosecondsPerNanosecond;
        }
        TotalYoctoseconds = ys;

        if (ns >= NanosecondsPerYear)
        {
            y += (uint)(ns / NanosecondsPerYear);
            ns %= NanosecondsPerYear;
        }

        ns += microseconds * (ulong)NanosecondsPerMicrosecond;
        if (ns >= NanosecondsPerYear)
        {
            y += (uint)(ns / NanosecondsPerYear);
            ns %= NanosecondsPerYear;
        }

        ns += milliseconds * (ulong)NanosecondsPerMillisecond;
        if (ns >= NanosecondsPerYear)
        {
            y += (uint)(ns / NanosecondsPerYear);
            ns %= NanosecondsPerYear;
        }

        ns += nanoseconds;
        if (ns >= NanosecondsPerYear)
        {
            y += (uint)(ns / NanosecondsPerYear);
            ns %= NanosecondsPerYear;
        }

        ns += seconds * (ulong)NanosecondsPerSecond;
        if (ns >= NanosecondsPerYear)
        {
            y += (uint)(ns / NanosecondsPerYear);
            ns %= NanosecondsPerYear;
        }

        ns += minutes * (ulong)NanosecondsPerMinute;
        if (ns >= NanosecondsPerYear)
        {
            y += (uint)(ns / NanosecondsPerYear);
            ns %= NanosecondsPerYear;
        }

        ns += hours * (ulong)NanosecondsPerHour;
        if (ns >= NanosecondsPerYear)
        {
            y += (uint)(ns / NanosecondsPerYear);
            ns %= NanosecondsPerYear;
        }

        var extraYears = (uint)(days * (ulong)NanosecondsPerDay / NanosecondsPerYear);
        y += extraYears;
        ns += (days * (ulong)NanosecondsPerDay) - (extraYears * (ulong)NanosecondsPerYear);
        if (ns >= NanosecondsPerYear)
        {
            y += (uint)(ns / NanosecondsPerYear);
            ns %= NanosecondsPerYear;
        }
        TotalNanoseconds = ns;

        if (y >= YearsPerAeon)
        {
            aes = (aes ?? BigInteger.Zero) + (y / YearsPerAeon);
            y %= YearsPerAeon;
        }

        y += years;
        if (y >= YearsPerAeon)
        {
            aes = (aes ?? BigInteger.Zero) + (y / YearsPerAeon);
            y %= YearsPerAeon;
        }
        Years = y;

        if (aeons.HasValue && aeons > BigInteger.Zero)
        {
            aes = (aes ?? BigInteger.Zero) + aeons.Value;
        }
        Aeons = aes;
    }

    /// <summary>
    /// Initializes a new instance of <see cref="Duration"/>
    /// </summary>
    /// <param name="isNegative">
    /// Whether the instance will represent a negative duration.
    /// </param>
    /// <param name="isPerpetual">
    /// Indicates that this <see cref="Duration"/> will represent an infinite amount of time
    /// (positive or negative).
    /// </param>
    /// <param name="planckTime">
    /// <para>
    /// The amount of Planck time.
    /// </para>
    /// <para>
    /// Cannot be negative; only the <see cref="Duration"/> as a whole can be negative. Values
    /// less than zero will be converted to 0.
    /// </para>
    /// </param>
    /// <param name="totalYoctoseconds">
    /// The number of yoctoseconds beyond <paramref name="totalNanoseconds"/>.
    /// </param>
    /// <param name="totalNanoseconds">
    /// The number of nanoseconds beyond <paramref name="years"/>.
    /// </param>
    /// <param name="years">
    /// <para>
    /// The number of astronomical years beyond <paramref name="aeons"/>.
    /// </para>
    /// <para>
    /// An astronomical (Julian) year is exactly 31557600 seconds long (365.25 * 86400).
    /// </para>
    /// </param>
    /// <param name="aeons">
    /// <para>
    /// The number of astronomical aeons.
    /// </para>
    /// <para>
    /// An astronomical aeon (AE) is 10e9 years.
    /// </para>
    /// <para>
    /// Cannot be negative; only the <see cref="Duration"/> as a whole can be negative. Values
    /// less than zero will be converted to 0.
    /// </para>
    /// </param>
    [JsonConstructor]
    public Duration(
        bool isNegative,
        bool isPerpetual,
        BigInteger? planckTime,
        ulong totalYoctoseconds,
        ulong totalNanoseconds,
        uint years,
        BigInteger? aeons)
    {
        IsNegative = isNegative;
        IsPerpetual = isPerpetual;
        if (isPerpetual)
        {
            Aeons = null;
            PlanckTime = null;
            TotalNanoseconds = 0;
            TotalYoctoseconds = 0;
            Years = 0;
        }
        else
        {
            Aeons = aeons <= BigInteger.Zero ? null : aeons;
            PlanckTime = planckTime <= BigInteger.Zero ? null : planckTime;
            TotalNanoseconds = totalNanoseconds;
            TotalYoctoseconds = totalYoctoseconds;
            Years = years;
        }
    }

    /// <summary>
    /// Indicates whether this <see cref="Duration"/> instance and another are equal.
    /// </summary>
    /// <param name="other">The <see cref="Duration"/> instance to compare with this
    /// one.</param>
    /// <returns>
    /// <see langword="true"/> if the <see cref="Duration"/> instance and this one are
    /// equal; otherwise <see langword="false"/>.
    /// </returns>
    public bool Equals(Duration other)
        => IsNegative == other.IsNegative
        && IsPerpetual == other.IsPerpetual
        && PlanckTime == other.PlanckTime
        && TotalYoctoseconds == other.TotalYoctoseconds
        && TotalNanoseconds == other.TotalNanoseconds
        && Years == other.Years
        && Aeons == other.Aeons;

    /// <summary>
    /// Indicates whether this <see cref="Duration"/> instance and another are equal.
    /// </summary>
    /// <param name="other">The <see cref="Duration"/> instance to compare with this
    /// one.</param>
    /// <returns>
    /// <see langword="true"/> if the <see cref="Duration"/> instance and this one are
    /// equal; otherwise <see langword="false"/>.
    /// </returns>
    public bool Equals(Duration? other)
        => other.HasValue && Equals(other.Value);

    /// <summary>
    /// Indicates whether this <see cref="Duration"/> instance and a <see
    /// cref="RelativeDuration"/> instance are equal.
    /// </summary>
    /// <param name="other">The <see cref="RelativeDuration"/> instance to compare with this
    /// <see cref="Duration"/> instance.</param>
    /// <returns>
    /// <see langword="true"/> if the <see cref="RelativeDuration"/> instance and this <see
    /// cref="Duration"/> instance are equal; otherwise <see langword="false"/>.
    /// </returns>
    public bool Equals(RelativeDuration other)
        => other.Relativity == RelativeDurationType.Absolute && Equals(other.Duration);

    /// <summary>
    /// Indicates whether this <see cref="Duration"/> instance and a <see
    /// cref="RelativeDuration"/> instance are equal.
    /// </summary>
    /// <param name="other">The <see cref="RelativeDuration"/> instance to compare with this
    /// <see cref="Duration"/> instance.</param>
    /// <returns>
    /// <see langword="true"/> if the <see cref="RelativeDuration"/> instance and this <see
    /// cref="Duration"/> instance are equal; otherwise <see langword="false"/>.
    /// </returns>
    public bool Equals(RelativeDuration? other)
        => other.HasValue && Equals(other.Value);

    /// <summary>
    /// Indicates whether this <see cref="Duration"/> instance and a <see cref="DateTime"/>
    /// instance are equal.
    /// </summary>
    /// <param name="other">The <see cref="DateTime"/> instance to compare with this
    /// <see cref="Duration"/> instance.</param>
    /// <returns>
    /// <see langword="true"/> if the <see cref="DateTime"/> instance and this <see
    /// cref="Duration"/> instance are equal; otherwise <see langword="false"/>.
    /// </returns>
    public bool Equals(DateTime other) => Equals(FromDateTime(other));

    /// <summary>
    /// Indicates whether this <see cref="Duration"/> instance and a <see cref="DateTime"/>
    /// instance are equal.
    /// </summary>
    /// <param name="other">The <see cref="DateTime"/> instance to compare with this
    /// <see cref="Duration"/> instance.</param>
    /// <returns>
    /// <see langword="true"/> if the <see cref="DateTime"/> instance and this <see
    /// cref="Duration"/> instance are equal; otherwise <see langword="false"/>.
    /// </returns>
    public bool Equals(DateTime? other)
        => other.HasValue && Equals(other.Value);

    /// <summary>
    /// Indicates whether this <see cref="Duration"/> instance and a <see cref="DateOnly"/>
    /// instance are equal.
    /// </summary>
    /// <param name="other">The <see cref="DateOnly"/> instance to compare with this
    /// <see cref="Duration"/> instance.</param>
    /// <returns>
    /// <see langword="true"/> if the <see cref="DateOnly"/> instance and this <see
    /// cref="Duration"/> instance are equal; otherwise <see langword="false"/>.
    /// </returns>
    public bool Equals(DateOnly other) => Equals(FromDateOnly(other));

    /// <summary>
    /// Indicates whether this <see cref="Duration"/> instance and a <see cref="DateOnly"/>
    /// instance are equal.
    /// </summary>
    /// <param name="other">The <see cref="DateOnly"/> instance to compare with this
    /// <see cref="Duration"/> instance.</param>
    /// <returns>
    /// <see langword="true"/> if the <see cref="DateOnly"/> instance and this <see
    /// cref="Duration"/> instance are equal; otherwise <see langword="false"/>.
    /// </returns>
    public bool Equals(DateOnly? other)
        => other.HasValue && Equals(other.Value);

    /// <summary>
    /// Indicates whether this <see cref="Duration"/> instance and a <see
    /// cref="DateTimeOffset"/> instance are equal.
    /// </summary>
    /// <param name="other">The <see cref="DateTimeOffset"/> instance to compare with this
    /// <see cref="Duration"/> instance.</param>
    /// <returns>
    /// <see langword="true"/> if the <see cref="DateTimeOffset"/> instance and this <see
    /// cref="Duration"/> instance are equal; otherwise <see langword="false"/>.
    /// </returns>
    public bool Equals(DateTimeOffset other) => Equals(FromDateTimeOffset(other));

    /// <summary>
    /// Indicates whether this <see cref="Duration"/> instance and a <see
    /// cref="DateTimeOffset"/> instance are equal.
    /// </summary>
    /// <param name="other">The <see cref="DateTimeOffset"/> instance to compare with this
    /// <see cref="Duration"/> instance.</param>
    /// <returns>
    /// <see langword="true"/> if the <see cref="DateTimeOffset"/> instance and this <see
    /// cref="Duration"/> instance are equal; otherwise <see langword="false"/>.
    /// </returns>
    public bool Equals(DateTimeOffset? other)
        => other.HasValue && Equals(other.Value);

    /// <summary>
    /// Indicates whether this <see cref="Duration"/> instance and a <see cref="TimeOnly"/>
    /// instance are equal.
    /// </summary>
    /// <param name="other">The <see cref="TimeOnly"/> instance to compare with this
    /// <see cref="Duration"/> instance.</param>
    /// <returns>
    /// <see langword="true"/> if the <see cref="TimeOnly"/> instance and this <see
    /// cref="Duration"/> instance are equal; otherwise <see langword="false"/>.
    /// </returns>
    public bool Equals(TimeOnly other) => Equals(FromTimeOnly(other));

    /// <summary>
    /// Indicates whether this <see cref="Duration"/> instance and a <see cref="TimeOnly"/>
    /// instance are equal.
    /// </summary>
    /// <param name="other">The <see cref="TimeOnly"/> instance to compare with this
    /// <see cref="Duration"/> instance.</param>
    /// <returns>
    /// <see langword="true"/> if the <see cref="TimeOnly"/> instance and this <see
    /// cref="Duration"/> instance are equal; otherwise <see langword="false"/>.
    /// </returns>
    public bool Equals(TimeOnly? other)
        => other.HasValue && Equals(other.Value);

    /// <summary>
    /// Indicates whether this <see cref="Duration"/> instance and a <see cref="TimeSpan"/>
    /// instance are equal.
    /// </summary>
    /// <param name="other">The <see cref="TimeSpan"/> instance to compare with this
    /// <see cref="Duration"/> instance.</param>
    /// <returns>
    /// <see langword="true"/> if the <see cref="TimeSpan"/> instance and this <see
    /// cref="Duration"/> instance are equal; otherwise <see langword="false"/>.
    /// </returns>
    public bool Equals(TimeSpan other) => Equals(FromTimeSpan(other));

    /// <summary>
    /// Indicates whether this <see cref="Duration"/> instance and a <see cref="TimeSpan"/>
    /// instance are equal.
    /// </summary>
    /// <param name="other">The <see cref="TimeSpan"/> instance to compare with this
    /// <see cref="Duration"/> instance.</param>
    /// <returns>
    /// <see langword="true"/> if the <see cref="TimeSpan"/> instance and this <see
    /// cref="Duration"/> instance are equal; otherwise <see langword="false"/>.
    /// </returns>
    public bool Equals(TimeSpan? other)
        => other.HasValue && Equals(other.Value);

    /// <summary>
    /// Indicates whether this instance and a specified object are equal.
    /// </summary>
    /// <param name="obj">The object to compare with the current instance.</param>
    /// <returns>
    /// <see langword="true"/> if the object and the current instance are equal; otherwise <see
    /// langword="false"/>.
    /// </returns>
    public override bool Equals(object? obj) => obj switch
    {
        null => false,
        Duration other => Equals(other),
        RelativeDuration relativeDuration => Equals(relativeDuration),
        DateTime dateTime => Equals(dateTime),
        DateOnly dateOnly => Equals(dateOnly),
        DateTimeOffset dateTimeOffset => Equals(dateTimeOffset),
        TimeOnly timeOnly => Equals(timeOnly),
        TimeSpan timeSpan => Equals(timeSpan),
        _ => false,
    };

    /// <summary>
    /// Returns the hash code for this instance.
    /// </summary>
    /// <returns>The hash code for this instance.</returns>
    public override int GetHashCode() => HashCode.Combine(
        IsNegative,
        IsPerpetual,
        PlanckTime,
        TotalNanoseconds,
        Years,
        Aeons);

    /// <summary>
    /// Returns a value which indicates whether this instance is less than, equal to, or greater
    /// than the given <see cref="Duration"/> instance.
    /// </summary>
    /// <param name="other">A <see cref="Duration"/> to compare.</param>
    /// <returns>-1 if this instance is less than the given instance; 0 if they are equal, and 1
    /// if this instance is greater.</returns>
    public int CompareTo(Duration other)
    {
        if (IsNegative && !other.IsNegative)
        {
            return -1;
        }
        if (IsPerpetual)
        {
            return other.IsPerpetual ? 0 : 1 * Sign;
        }
        else if (other.IsPerpetual)
        {
            return -1 * Sign;
        }

        var comparison = (Aeons ?? BigInteger.Zero)
            .CompareTo(other.Aeons ?? BigInteger.Zero);
        if (comparison != 0)
        {
            return comparison * Sign;
        }

        comparison = Years.CompareTo(other.Years);
        if (comparison != 0)
        {
            return comparison * Sign;
        }

        comparison = TotalNanoseconds.CompareTo(other.TotalNanoseconds);
        if (comparison != 0)
        {
            return comparison * Sign;
        }

        comparison = TotalYoctoseconds.CompareTo(other.TotalYoctoseconds);
        if (comparison != 0)
        {
            return comparison * Sign;
        }

        return (PlanckTime ?? BigInteger.Zero)
            .CompareTo(other.PlanckTime ?? BigInteger.Zero) * Sign;
    }

    /// <summary>
    /// Returns a value which indicates whether this instance is less than, equal to, or greater
    /// than the given object.
    /// </summary>
    /// <param name="obj">An object to compare.</param>
    /// <returns>-1 if this instance is less than the given instance; 0 if they are equal, and 1
    /// if this instance is greater.</returns>
    public int CompareTo(object? obj) => obj is not Duration other ? 1 : CompareTo(other);

    /// <summary>
    /// Returns a value which indicates whether this instance is less than, equal to, or greater
    /// than the given <see cref="DateTime"/> instance.
    /// </summary>
    /// <param name="other">A <see cref="DateTime"/> to compare.</param>
    /// <returns>-1 if this instance is less than the given instance; 0 if they are equal, and 1
    /// if this instance is greater.</returns>
    public int CompareTo(DateTime other) => CompareTo(FromDateTime(other));

    /// <summary>
    /// Returns a value which indicates whether this instance is less than, equal to, or greater
    /// than the given <see cref="DateTimeOffset"/> instance.
    /// </summary>
    /// <param name="other">A <see cref="DateTimeOffset"/> to compare.</param>
    /// <returns>-1 if this instance is less than the given instance; 0 if they are equal, and 1
    /// if this instance is greater.</returns>
    public int CompareTo(DateTimeOffset other) => CompareTo(FromDateTimeOffset(other));

    /// <summary>
    /// Returns a value which indicates whether this instance is less than, equal to, or greater
    /// than the given <see cref="TimeSpan"/> instance.
    /// </summary>
    /// <param name="other">A <see cref="TimeSpan"/> to compare.</param>
    /// <returns>-1 if this instance is less than the given instance; 0 if they are equal, and 1
    /// if this instance is greater.</returns>
    public int CompareTo(TimeSpan other) => CompareTo(FromTimeSpan(other));
}
