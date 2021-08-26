using System.Numerics;

namespace Tavenem.Time;

public partial struct Duration
{
    /// <summary>
    /// The number of attoseconds in a femtosecond.
    /// </summary>
    public const int AttosecondsPerFemtosecond = 1000;

    /// <summary>
    /// The number of attoseconds in a microsecond.
    /// </summary>
    public const long AttosecondsPerMicrosecond = 1000000000000;

    /// <summary>
    /// The number of attoseconds in a millisecond.
    /// </summary>
    public const long AttosecondsPerMillisecond = 1000000000000000;

    /// <summary>
    /// The number of attoseconds in a nanosecond.
    /// </summary>
    public const int AttosecondsPerNanosecond = 1000000000;

    /// <summary>
    /// The number of attoseconds in a picosecond.
    /// </summary>
    public const int AttosecondsPerPicosecond = 1000000;

    /// <summary>
    /// The number of attoseconds in a second.
    /// </summary>
    public const long AttosecondsPerSecond = 1000000000000000000;

    /// <summary>
    /// The number of SI days in an astronomical aeon (1000000 years).
    /// </summary>
    public const int DaysPerAeon = 365250000;

    /// <summary>
    /// The number of SI days in an astronomical (Julian) year.
    /// </summary>
    public const double DaysPerYear = 365.25;

    /// <summary>
    /// The number of femtoseconds in an hour.
    /// </summary>
    public const long FemtosecondsPerHour = 3600000000000000000;

    /// <summary>
    /// The number of femtoseconds in a microsecond.
    /// </summary>
    public const int FemtosecondsPerMicrosecond = 1000000000;

    /// <summary>
    /// The number of femtoseconds in a millisecond.
    /// </summary>
    public const long FemtosecondsPerMillisecond = 1000000000000;

    /// <summary>
    /// The number of femtoseconds in a minute.
    /// </summary>
    public const long FemtosecondsPerMinute = 60000000000000000;

    /// <summary>
    /// The number of femtoseconds in a nanosecond.
    /// </summary>
    public const int FemtosecondsPerNanosecond = 1000000;

    /// <summary>
    /// The number of femtoseconds in a picosecond.
    /// </summary>
    public const int FemtosecondsPerPicosecond = 1000;

    /// <summary>
    /// The number of femtoseconds in a second.
    /// </summary>
    public const long FemtosecondsPerSecond = 1000000000000000;

    /// <summary>
    /// The number of hours in an astronomical aeon (1000000 years).
    /// </summary>
    public const long HoursPerAeon = 8766000000;

    /// <summary>
    /// The number of hours in an SI day.
    /// </summary>
    public const int HoursPerDay = 24;

    /// <summary>
    /// The number of hours in an astronomical (Julian) year.
    /// </summary>
    public const int HoursPerYear = 8766;

    /// <summary>
    /// The number of microseconds in an SI day.
    /// </summary>
    public const long MicrosecondsPerDay = 86400000000;

    /// <summary>
    /// The number of microseconds in an hour.
    /// </summary>
    public const long MicrosecondsPerHour = 3600000000;

    /// <summary>
    /// The number of microseconds in a millisecond.
    /// </summary>
    public const int MicrosecondsPerMillisecond = 1000;

    /// <summary>
    /// The number of microseconds in a minute.
    /// </summary>
    public const int MicrosecondsPerMinute = 60000000;

    /// <summary>
    /// The number of microseconds in a second.
    /// </summary>
    public const int MicrosecondsPerSecond = 1000000;

    /// <summary>
    /// The number of microseconds in an astronomical (Julian) year.
    /// </summary>
    public const long MicrosecondsPerYear = 31557600000000;

    /// <summary>
    /// The number of milliseconds in an astronomical aeon (1000000 years).
    /// </summary>
    public const long MillisecondsPerAeon = 31557600000000000;

    /// <summary>
    /// The number of milliseconds in an SI day.
    /// </summary>
    public const long MillisecondsPerDay = 86400000;

    /// <summary>
    /// The number of milliseconds in an hour.
    /// </summary>
    public const long MillisecondsPerHour = 3600000;

    /// <summary>
    /// The number of milliseconds in a minute.
    /// </summary>
    public const int MillisecondsPerMinute = 60000;

    /// <summary>
    /// The number of milliseconds in a second.
    /// </summary>
    public const int MillisecondsPerSecond = 1000;

    /// <summary>
    /// The number of milliseconds in an astronomical (Julian) year.
    /// </summary>
    public const long MillisecondsPerYear = 31557600000;

    /// <summary>
    /// The number of minutes in an astronomical aeon (1000000 years).
    /// </summary>
    public const long MinutesPerAeon = 525960000000;

    /// <summary>
    /// The number of minutes in an SI day.
    /// </summary>
    public const int MinutesPerDay = 1440;

    /// <summary>
    /// The number of minutes in an hour.
    /// </summary>
    public const int MinutesPerHour = 60;

    /// <summary>
    /// The number of minutes in an astronomical (Julian) year.
    /// </summary>
    public const int MinutesPerYear = 525960;

    /// <summary>
    /// The number of nanoseconds in an SI day.
    /// </summary>
    public const long NanosecondsPerDay = 86400000000000;

    /// <summary>
    /// The number of nanoseconds in an hour.
    /// </summary>
    public const long NanosecondsPerHour = 3600000000000;

    /// <summary>
    /// The number of nanoseconds in a microsecond.
    /// </summary>
    public const int NanosecondsPerMicrosecond = 1000;

    /// <summary>
    /// The number of nanoseconds in a millisecond.
    /// </summary>
    public const int NanosecondsPerMillisecond = 1000000;

    /// <summary>
    /// The number of nanoseconds in a minute.
    /// </summary>
    public const long NanosecondsPerMinute = 60000000000;

    /// <summary>
    /// The number of nanoseconds in a second.
    /// </summary>
    public const int NanosecondsPerSecond = 1000000000;

    /// <summary>
    /// The number of nanoseconds in a Tick, as used by <see cref="DateTime"/> and <see
    /// cref="TimeSpan"/>.
    /// </summary>
    public const long NanosecondsPerTick = 100;

    /// <summary>
    /// The number of nanoseconds in an astronomical (Julian) year.
    /// </summary>
    public const long NanosecondsPerYear = 31557600000000000;

    /// <summary>
    /// The number of picoseconds in an SI day.
    /// </summary>
    public const long PicosecondsPerDay = 86400000000000000;

    /// <summary>
    /// The number of picoseconds in an hour.
    /// </summary>
    public const long PicosecondsPerHour = 3600000000000000;

    /// <summary>
    /// The number of picoseconds in a microsecond.
    /// </summary>
    public const int PicosecondsPerMicrosecond = 1000000;

    /// <summary>
    /// The number of picoseconds in a millisecond.
    /// </summary>
    public const int PicosecondsPerMillisecond = 1000000000;

    /// <summary>
    /// The number of picoseconds in a minute.
    /// </summary>
    public const long PicosecondsPerMinute = 60000000000000;

    /// <summary>
    /// The number of picoseconds in a nanosecond.
    /// </summary>
    public const int PicosecondsPerNanosecond = 1000;

    /// <summary>
    /// The number of picoseconds in a second.
    /// </summary>
    public const long PicosecondsPerSecond = 1000000000000;

    /// <summary>
    /// The number of seconds in an astronomical aeon (1000000 years).
    /// </summary>
    public const long SecondsPerAeon = 31557600000000;

    /// <summary>
    /// The number of seconds in an SI day.
    /// </summary>
    public const int SecondsPerDay = 86400;

    /// <summary>
    /// The number of seconds in an hour.
    /// </summary>
    public const int SecondsPerHour = 3600;

    /// <summary>
    /// The number of seconds in a minute.
    /// </summary>
    public const int SecondsPerMinute = 60;

    /// <summary>
    /// The number of seconds in an astronomical (Julian) year.
    /// </summary>
    public const int SecondsPerYear = 31557600;

    /// <summary>
    /// The number of years in an astronomical aeon (1000000 years).
    /// </summary>
    public const int YearsPerAeon = 1000000;

    /// <summary>
    /// The number of yoctoseconds in an attosecond.
    /// </summary>
    public const int YoctosecondsPerAttosecond = 1000000;

    /// <summary>
    /// The number of yoctoseconds in a femtosecond.
    /// </summary>
    public const int YoctosecondsPerFemtosecond = 1000000000;

    /// <summary>
    /// The number of yoctoseconds in a microsecond.
    /// </summary>
    public const long YoctosecondsPerMicrosecond = 1000000000000000000;

    /// <summary>
    /// The number of yoctoseconds in a nanosecond.
    /// </summary>
    public const long YoctosecondsPerNanosecond = 1000000000000000;

    /// <summary>
    /// The number of yoctoseconds in a picosecond.
    /// </summary>
    public const long YoctosecondsPerPicosecond = 1000000000000;

    /// <summary>
    /// The number of yoctoseconds in a zeptosecond.
    /// </summary>
    public const int YoctosecondsPerZeptosecond = 1000;

    /// <summary>
    /// The number of zeptoseconds in a microsecond.
    /// </summary>
    public const long ZeptosecondsPerMicrosecond = 1000000000000000;

    /// <summary>
    /// The number of zeptoseconds in a millisecond.
    /// </summary>
    public const long ZeptosecondsPerMillisecond = 1000000000000000000;

    /// <summary>
    /// The number of zeptoseconds in a nanosecond.
    /// </summary>
    public const long ZeptosecondsPerNanosecond = 1000000000000;

    /// <summary>
    /// The number of zeptoseconds in an attosecond.
    /// </summary>
    public const int ZeptosecondsPerAttosecond = 1000;

    /// <summary>
    /// The number of zeptoseconds in a femtosecond.
    /// </summary>
    public const int ZeptosecondsPerFemtosecond = 1000000;

    /// <summary>
    /// The number of zeptoseconds in a picosecond.
    /// </summary>
    public const int ZeptosecondsPerPicosecond = 1000000000;

    /// <summary>
    /// The approximate number of Planck time units in a yoctosecond.
    /// </summary>
    /// <remarks>
    /// This is not, in fact, an integral value. It is given to a precision of 7 significant
    /// digits, and used to determine an approximate rollover point between Planck time and one
    /// yoctosecond.
    /// </remarks>
    public static BigInteger PlanckTimePerYoctosecond { get; } = BigInteger.Parse("185486100000000000000");

    /// <summary>
    /// The approximate number of Planck time units in a yoctosecond.
    /// </summary>
    /// <remarks>
    /// This is not, in fact, an integral value. It is given to a precision of 7 significant
    /// digits, and used to determine an approximate rollover point between Planck time and one
    /// yoctosecond.
    /// </remarks>
    private const decimal PlanckTimePerYoctosecondDecimal = 185486100000000000000m;

    /// <summary>
    /// The approximate number of Planck time units in a yoctosecond.
    /// </summary>
    /// <remarks>
    /// This is not, in fact, an integral value. It is given to a precision of 7 significant
    /// digits, and used to determine an approximate rollover point between Planck time and one
    /// yoctosecond.
    /// </remarks>
    private const double PlanckTimePerYoctosecondDouble = 185486100000000000000.0;

    /// <summary>
    /// A duration representing 1 astronomical aeon (1000000 years). Read-only.
    /// </summary>
    public static Duration OneAeon { get; } = new(
        false,
        false,
        null,
        0,
        0,
        0,
        BigInteger.One);

    /// <summary>
    /// A duration representing 1 attosecond. Read-only.
    /// </summary>
    public static Duration OneAttosecond { get; } = new(
        false,
        false,
        null,
        YoctosecondsPerAttosecond,
        0,
        0,
        null);

    /// <summary>
    /// A duration representing 1 SI day (86400 seconds). Read-only.
    /// </summary>
    public static Duration OneDay { get; } = new(
        false,
        false,
        null,
        0,
        NanosecondsPerDay,
        0,
        null);

    /// <summary>
    /// A duration representing 1 femtosecond. Read-only.
    /// </summary>
    public static Duration OneFemtosecond { get; } = new(
        false,
        false,
        null,
        YoctosecondsPerFemtosecond,
        0,
        0,
        null);

    /// <summary>
    /// A duration representing 1 hour. Read-only.
    /// </summary>
    public static Duration OneHour { get; } = new(
        false,
        false,
        null,
        0,
        NanosecondsPerHour,
        0,
        null);

    /// <summary>
    /// A duration representing 1 microsecond. Read-only.
    /// </summary>
    public static Duration OneMicrosecond { get; } = new(
        false,
        false,
        null,
        0,
        NanosecondsPerMicrosecond,
        0,
        null);

    /// <summary>
    /// A duration representing 1 millisecond. Read-only.
    /// </summary>
    public static Duration OneMillisecond { get; } = new(
        false,
        false,
        null,
        0,
        NanosecondsPerMillisecond,
        0,
        null);

    /// <summary>
    /// A duration representing 1 minute. Read-only.
    /// </summary>
    public static Duration OneMinute { get; } = new(
        false,
        false,
        null,
        0,
        NanosecondsPerMinute,
        0,
        null);

    /// <summary>
    /// A duration representing 1 nanosecond. Read-only.
    /// </summary>
    public static Duration OneNanosecond { get; } = new(
        false,
        false,
        null,
        0,
        1,
        0,
        null);

    /// <summary>
    /// A duration representing 1 picosecond. Read-only.
    /// </summary>
    public static Duration OnePicosecond { get; } = new(
        false,
        false,
        null,
        YoctosecondsPerPicosecond,
        0,
        0,
        null);

    /// <summary>
    /// A duration representing 1 Planck time. Read-only.
    /// </summary>
    public static Duration OnePlanckTime { get; } = new(
        false,
        false,
        BigInteger.One,
        0,
        0,
        0,
        null);

    /// <summary>
    /// A duration representing 1s. Read-only.
    /// </summary>
    public static Duration OneSecond { get; } = new(
        false,
        false,
        null,
        0,
        NanosecondsPerSecond,
        0,
        null);

    /// <summary>
    /// A duration representing 1 astronomical year (31557600 seconds). Read-only.
    /// </summary>
    public static Duration OneYear { get; } = new(
        false,
        false,
        null,
        0,
        0,
        1,
        null);

    /// <summary>
    /// A duration representing 1 yoctosecond. Read-only.
    /// </summary>
    public static Duration OneYoctosecond { get; } = new(
        false,
        false,
        null,
        1,
        0,
        0,
        null);

    /// <summary>
    /// A duration representing 1 zeptosecond. Read-only.
    /// </summary>
    public static Duration OneZeptosecond { get; } = new(
        false,
        false,
        null,
        YoctosecondsPerZeptosecond,
        0,
        0,
        null);

    /// <summary>
    /// Gets the approximate number of total aeons represented by this <see cref="Duration"/>
    /// (including fractional amounts), as a <see cref="double"/>.
    /// </summary>
    /// <returns>The approximate number of total aeons represented by this instance.</returns>
    /// <remarks>
    /// Note that if the total exceeds <see cref="double.MaxValue"/>, this method may return
    /// <see cref="double.PositiveInfinity"/> even for a <see cref="Duration"/> for which <see
    /// cref="IsPerpetual"/> is <see langword="false"/>.
    /// </remarks>
    public double ToAeons() => (double)(Aeons ?? BigInteger.Zero) + FractionalAeons();

    /// <summary>
    /// Gets the approximate number of total attoseconds represented by this <see
    /// cref="Duration"/> (including fractional amounts), as a <see cref="double"/>.
    /// </summary>
    /// <returns>The approximate number of total attoseconds represented by this
    /// instance.</returns>
    /// <remarks>
    /// Note that if the total exceeds <see cref="double.MaxValue"/>, this method may return
    /// <see cref="double.PositiveInfinity"/> even for a <see cref="Duration"/> for which <see
    /// cref="IsPerpetual"/> is <see langword="false"/>.
    /// </remarks>
    public double ToAttoseconds() => ToNanoseconds() * AttosecondsPerNanosecond;

    /// <summary>
    /// Gets the approximate number of total days represented by this <see cref="Duration"/>
    /// (including fractional amounts), as a <see cref="double"/>.
    /// </summary>
    /// <returns>The approximate number of total days represented by this instance.</returns>
    /// <remarks>
    /// Note that if the total exceeds <see cref="double.MaxValue"/>, this method may return
    /// <see cref="double.PositiveInfinity"/> even for a <see cref="Duration"/> for which <see
    /// cref="IsPerpetual"/> is <see langword="false"/>.
    /// </remarks>
    public double ToDays() => ToYears() * DaysPerYear;

    /// <summary>
    /// Gets the approximate number of total femtoseconds represented by this <see
    /// cref="Duration"/> (including fractional amounts), as a <see cref="double"/>.
    /// </summary>
    /// <returns>The approximate number of total femtoseconds represented by this
    /// instance.</returns>
    /// <remarks>
    /// Note that if the total exceeds <see cref="double.MaxValue"/>, this method may return
    /// <see cref="double.PositiveInfinity"/> even for a <see cref="Duration"/> for which <see
    /// cref="IsPerpetual"/> is <see langword="false"/>.
    /// </remarks>
    public double ToFemtoseconds() => ToNanoseconds() * FemtosecondsPerNanosecond;

    /// <summary>
    /// Gets the approximate number of total hours represented by this <see cref="Duration"/>
    /// (including fractional amounts), as a <see cref="double"/>.
    /// </summary>
    /// <returns>The approximate number of total hours represented by this instance.</returns>
    /// <remarks>
    /// Note that if the total exceeds <see cref="double.MaxValue"/>, this method may return
    /// <see cref="double.PositiveInfinity"/> even for a <see cref="Duration"/> for which <see
    /// cref="IsPerpetual"/> is <see langword="false"/>.
    /// </remarks>
    public double ToHours() => ToYears() * HoursPerYear;

    /// <summary>
    /// Gets the approximate number of total microseconds represented by this <see
    /// cref="Duration"/> (including fractional amounts), as a <see cref="double"/>.
    /// </summary>
    /// <returns>The approximate number of total microseconds represented by this
    /// instance.</returns>
    /// <remarks>
    /// Note that if the total exceeds <see cref="double.MaxValue"/>, this method may return
    /// <see cref="double.PositiveInfinity"/> even for a <see cref="Duration"/> for which <see
    /// cref="IsPerpetual"/> is <see langword="false"/>.
    /// </remarks>
    public double ToMicroseconds() => ToYears() * MicrosecondsPerYear;

    /// <summary>
    /// Gets the approximate number of total milliseconds represented by this <see
    /// cref="Duration"/> (including fractional amounts), as a <see cref="double"/>.
    /// </summary>
    /// <returns>The approximate number of total milliseconds represented by this
    /// instance.</returns>
    /// <remarks>
    /// Note that if the total exceeds <see cref="double.MaxValue"/>, this method may return
    /// <see cref="double.PositiveInfinity"/> even for a <see cref="Duration"/> for which <see
    /// cref="IsPerpetual"/> is <see langword="false"/>.
    /// </remarks>
    public double ToMilliseconds() => ToYears() * MillisecondsPerYear;

    /// <summary>
    /// Gets the approximate number of total minutes represented by this <see cref="Duration"/>
    /// (including fractional amounts), as a <see cref="double"/>.
    /// </summary>
    /// <returns>The approximate number of total minutes represented by this instance.</returns>
    /// <remarks>
    /// Note that if the total exceeds <see cref="double.MaxValue"/>, this method may return
    /// <see cref="double.PositiveInfinity"/> even for a <see cref="Duration"/> for which <see
    /// cref="IsPerpetual"/> is <see langword="false"/>.
    /// </remarks>
    public double ToMinutes() => ToYears() * MinutesPerYear;

    /// <summary>
    /// Gets the approximate number of total yoctoseconds represented by this <see
    /// cref="Duration"/> (including fractional amounts), as a <see cref="double"/>.
    /// </summary>
    /// <returns>The approximate number of total yoctoseconds represented by this
    /// instance.</returns>
    /// <remarks>
    /// Note that if the total exceeds <see cref="double.MaxValue"/>, this method may return
    /// <see cref="double.PositiveInfinity"/> even for a <see cref="Duration"/> for which <see
    /// cref="IsPerpetual"/> is <see langword="false"/>.
    /// </remarks>
    public double ToNanoseconds() => AllNanoseconds() + FractionalNanoseconds();

    /// <summary>
    /// Gets the approximate number of total picoseconds represented by this <see
    /// cref="Duration"/> (including fractional amounts), as a <see cref="double"/>.
    /// </summary>
    /// <returns>The approximate number of total picoseconds represented by this
    /// instance.</returns>
    /// <remarks>
    /// Note that if the total exceeds <see cref="double.MaxValue"/>, this method may return
    /// <see cref="double.PositiveInfinity"/> even for a <see cref="Duration"/> for which <see
    /// cref="IsPerpetual"/> is <see langword="false"/>.
    /// </remarks>
    public double ToPicoseconds() => ToNanoseconds() * PicosecondsPerNanosecond;

    /// <summary>
    /// Gets the total Planck Time represented by this <see cref="Duration"/>, as a
    /// <see cref="double"/>.
    /// </summary>
    /// <returns>The approximate total Planck Time represented by this instance.</returns>
    /// <remarks>
    /// Note that if the total exceeds <see cref="double.MaxValue"/>, this method may return
    /// <see cref="double.PositiveInfinity"/> even for a <see cref="Duration"/> for which <see
    /// cref="IsPerpetual"/> is <see langword="false"/>.
    /// </remarks>
    public double ToPlanckTime()
        => (AllYoctoseconds() * PlanckTimePerYoctosecondDouble) + (double)(PlanckTime ?? BigInteger.Zero);

    /// <summary>
    /// Gets the approximate number of total seconds represented by this <see cref="Duration"/>
    /// (including fractional amounts), as a <see cref="double"/>.
    /// </summary>
    /// <returns>The approximate number of total seconds represented by this instance.</returns>
    /// <remarks>
    /// Note that if the total exceeds <see cref="double.MaxValue"/>, this method may return
    /// <see cref="double.PositiveInfinity"/> even for a <see cref="Duration"/> for which <see
    /// cref="IsPerpetual"/> is <see langword="false"/>.
    /// </remarks>
    public double ToSeconds() => ToYears() * SecondsPerYear;

    /// <summary>
    /// Gets the approximate number of total years represented by this <see cref="Duration"/>
    /// (including fractional amounts), as a <see cref="double"/>.
    /// </summary>
    /// <returns>The approximate number of total years represented by this instance.</returns>
    /// <remarks>
    /// Note that if the total exceeds <see cref="double.MaxValue"/>, this method may return
    /// <see cref="double.PositiveInfinity"/> even for a <see cref="Duration"/> for which <see
    /// cref="IsPerpetual"/> is <see langword="false"/>.
    /// </remarks>
    public double ToYears() => AllYears() + FractionalYears();

    /// <summary>
    /// Gets the approximate number of total yoctoseconds represented by this <see
    /// cref="Duration"/> (including fractional amounts), as a <see cref="double"/>.
    /// </summary>
    /// <returns>
    /// The approximate number of total yoctoseconds represented by this instance.
    /// </returns>
    /// <remarks>
    /// Note that if the total exceeds <see cref="double.MaxValue"/>, this method may return
    /// <see cref="double.PositiveInfinity"/> even for a <see cref="Duration"/> for which <see
    /// cref="IsPerpetual"/> is <see langword="false"/>.
    /// </remarks>
    public double ToYoctoseconds() => AllYoctoseconds() + FractionalYoctoseconds();

    /// <summary>
    /// Gets the approximate number of total zeptoseconds represented by this <see
    /// cref="Duration"/> (including fractional amounts), as a <see cref="double"/>.
    /// </summary>
    /// <returns>
    /// The approximate number of total zeptoseconds represented by this instance.
    /// </returns>
    /// <remarks>
    /// Note that if the total exceeds <see cref="double.MaxValue"/>, this method may return
    /// <see cref="double.PositiveInfinity"/> even for a <see cref="Duration"/> for which <see
    /// cref="IsPerpetual"/> is <see langword="false"/>.
    /// </remarks>
    public double ToZeptoseconds() => ToNanoseconds() * ZeptosecondsPerNanosecond;

    private double AllYears() => ((double)(Aeons ?? BigInteger.Zero) * YearsPerAeon) + Years;

    private double AllNanoseconds() => (AllYears() * NanosecondsPerYear) + TotalNanoseconds;

    private double AllYoctoseconds() => (AllNanoseconds() * YoctosecondsPerNanosecond) + TotalYoctoseconds;

    private double FractionalAeons() => (Years + FractionalYears()) / YearsPerAeon;

    private double FractionalNanoseconds() => (TotalYoctoseconds + FractionalYoctoseconds()) / YoctosecondsPerNanosecond;

    internal double FractionalSeconds() => (TotalNanoseconds + FractionalNanoseconds()) / NanosecondsPerSecond;

    private double FractionalYears() => (TotalNanoseconds + FractionalNanoseconds()) / NanosecondsPerYear;

    private double FractionalYoctoseconds() => PlanckTime.HasValue
        ? (double)PlanckTime.Value / PlanckTimePerYoctosecondDouble
        : 0;
}
