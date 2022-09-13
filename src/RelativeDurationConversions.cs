using System.Numerics;

namespace Tavenem.Time;

public partial struct RelativeDuration
{
    /// <summary>
    /// Gets a new <see cref="RelativeDuration"/> instance with the given value.
    /// </summary>
    /// <param name="value">
    /// A number of aeons (1,000,000 years).
    /// </param>
    /// <returns>A new <see cref="RelativeDuration"/> instance.</returns>
    public static RelativeDuration FromAeons(decimal value)
        => new(Duration.FromAeons(value));

    /// <summary>
    /// Gets a new <see cref="RelativeDuration"/> instance with the given value.
    /// </summary>
    /// <param name="value">
    /// A number of aeons (1,000,000 years).
    /// </param>
    /// <returns>A new <see cref="RelativeDuration"/> instance.</returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="value"/> is <see cref="double.NaN"/>.
    /// </exception>
    public static RelativeDuration FromAeons(double value)
        => new(Duration.FromAeons(value));

    /// <summary>
    /// Gets a new <see cref="RelativeDuration"/> instance with the given value.
    /// </summary>
    /// <param name="value">
    /// A number of aeons (1,000,000 years).
    /// </param>
    /// <returns>A new <see cref="RelativeDuration"/> instance.</returns>
    public static RelativeDuration FromAeons(long value)
        => new(Duration.FromAeons(value));

    /// <summary>
    /// Gets a new <see cref="RelativeDuration"/> instance with the given value.
    /// </summary>
    /// <param name="value">
    /// A number of aeons (1,000,000 years).
    /// </param>
    /// <returns>A new <see cref="RelativeDuration"/> instance.</returns>
    public static RelativeDuration FromAeonsInteger<T>(T value) where T : INumber<T>
        => new(Duration.FromAeonsInteger(value));

    /// <summary>
    /// Gets a new <see cref="RelativeDuration"/> instance with the given value.
    /// </summary>
    /// <param name="value">
    /// A number of aeons (1,000,000 years).
    /// </param>
    /// <returns>A new <see cref="RelativeDuration"/> instance.</returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="value"/> satisfies <see cref="INumberBase{TSelf}.IsNaN(TSelf)"/>.
    /// </exception>
    public static RelativeDuration FromAeonsFloatingPoint<T>(T value) where T : IFloatingPoint<T>
        => new(Duration.FromAeonsFloatingPoint(value));

    /// <summary>
    /// Gets a new <see cref="RelativeDuration"/> instance with the given value.
    /// </summary>
    /// <param name="value">
    /// A number of attoseconds.
    /// </param>
    /// <returns>A new <see cref="RelativeDuration"/> instance.</returns>
    public static RelativeDuration FromAttoseconds(decimal value)
        => new(Duration.FromAttoseconds(value));

    /// <summary>
    /// Gets a new <see cref="RelativeDuration"/> instance with the given value.
    /// </summary>
    /// <param name="value">
    /// A number of attoseconds.
    /// </param>
    /// <returns>A new <see cref="RelativeDuration"/> instance.</returns>
    /// <exception cref="ArgumentException"><paramref name="value"/> is <see
    /// cref="double.NaN"/>.</exception>
    public static RelativeDuration FromAttoseconds(double value)
        => new(Duration.FromAttoseconds(value));

    /// <summary>
    /// Gets a new <see cref="RelativeDuration"/> instance with the given value.
    /// </summary>
    /// <param name="value">
    /// A number of attoseconds.
    /// </param>
    /// <returns>A new <see cref="RelativeDuration"/> instance.</returns>
    public static RelativeDuration FromAttoseconds(long value)
        => new(Duration.FromAttoseconds(value));

    /// <summary>
    /// Gets a new <see cref="RelativeDuration"/> instance with the given value.
    /// </summary>
    /// <param name="value">
    /// A number of attoseconds.
    /// </param>
    /// <returns>A new <see cref="RelativeDuration"/> instance.</returns>
    public static RelativeDuration FromAttosecondsInteger<T>(T value) where T : INumber<T>
        => new(Duration.FromAttosecondsInteger(value));

    /// <summary>
    /// Gets a new <see cref="RelativeDuration"/> instance with the given value.
    /// </summary>
    /// <param name="value">
    /// A number of attoseconds.
    /// </param>
    /// <returns>A new <see cref="RelativeDuration"/> instance.</returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="value"/> satisfies <see cref="INumberBase{TSelf}.IsNaN(TSelf)"/>.
    /// </exception>
    public static RelativeDuration FromAttosecondsFloatingPoint<T>(T value) where T : IFloatingPoint<T>
        => new(Duration.FromAttosecondsFloatingPoint(value));

    /// <summary>
    /// Converts the given <see cref="DateTime"/> value to a <see cref="RelativeDuration"/>
    /// value.
    /// </summary>
    /// <param name="dateTime">A <see cref="DateTime"/> value to convert.</param>
    /// <returns>An equivalent <see cref="RelativeDuration"/> value.</returns>
    /// <remarks>
    /// <para>
    /// The current aeon sequence of our universe is presumed to be 99731, based on current best
    /// estimates (1.3799e10±2.1e7 years).
    /// </para>
    /// <para>
    /// The <see cref="DateTime"/> value is converted to a <see cref="RelativeDuration"/>
    /// instance by presuming that the current aeon begins at the same moment as <see
    /// cref="DateTime.Ticks"/> begins counting. The <see cref="DateTime.Ticks"/> of the <see
    /// cref="DateTime"/> value are converted into the timekeeping system of <see
    /// cref="RelativeDuration"/> based on that assumption.
    /// </para>
    /// <para>
    /// The <see cref="DateTime.ToUniversalTime"/> method is used to ensure that timezone
    /// information is stripped prior to conversion. This means that converting various <see
    /// cref="DateTime"/> instances with differing timezone information should result in uniform
    /// <see cref="RelativeDuration"/> representations.
    /// </para>
    /// </remarks>
    public static RelativeDuration FromDateTime(DateTime dateTime)
        => new(Duration.FromDateTime(dateTime));

    /// <summary>
    /// Converts the given <see cref="DateTimeOffset"/> value to a <see
    /// cref="RelativeDuration"/>
    /// value.
    /// </summary>
    /// <param name="dateTime">A <see cref="DateTimeOffset"/> value to convert.</param>
    /// <returns>An equivalent <see cref="RelativeDuration"/> value.</returns>
    /// <remarks>
    /// <para>
    /// The current aeon sequence of our universe is presumed to be 99731, based on current best
    /// estimates (1.3799e10±2.1e7 years).
    /// </para>
    /// <para>
    /// The <see cref="DateTimeOffset"/> value is converted to a <see cref="RelativeDuration"/>
    /// instance by presuming that the current aeon begins at the same moment as <see
    /// cref="DateTimeOffset.Ticks"/> begins counting. The <see cref="DateTimeOffset.Ticks"/> of
    /// the <see cref="DateTimeOffset"/> value are converted into the timekeeping system of <see
    /// cref="RelativeDuration"/> based on that assumption.
    /// </para>
    /// <para>
    /// The <see cref="DateTimeOffset.ToUniversalTime"/> method is used to ensure that timezone
    /// information is stripped prior to conversion. This means that converting various <see
    /// cref="DateTimeOffset"/> instances with differing timezone information should result in
    /// uniform <see cref="RelativeDuration"/> representations.
    /// </para>
    /// </remarks>
    public static RelativeDuration FromDateTimeOffset(DateTimeOffset dateTime)
        => new(Duration.FromDateTimeOffset(dateTime));

    /// <summary>
    /// Gets a new <see cref="RelativeDuration"/> instance with the given value.
    /// </summary>
    /// <param name="value">
    /// A number of days.
    /// </param>
    /// <returns>A new <see cref="RelativeDuration"/> instance.</returns>
    public static RelativeDuration FromDays(decimal value)
        => new(Duration.FromDays(value));

    /// <summary>
    /// Gets a new <see cref="RelativeDuration"/> instance with the given value.
    /// </summary>
    /// <param name="value">
    /// A number of days.
    /// </param>
    /// <returns>A new <see cref="RelativeDuration"/> instance.</returns>
    /// <exception cref="ArgumentException"><paramref name="value"/> is <see
    /// cref="double.NaN"/>.</exception>
    public static RelativeDuration FromDays(double value)
        => new(Duration.FromDays(value));

    /// <summary>
    /// Gets a new <see cref="RelativeDuration"/> instance with the given value.
    /// </summary>
    /// <param name="value">
    /// A number of days.
    /// </param>
    /// <returns>A new <see cref="RelativeDuration"/> instance.</returns>
    public static RelativeDuration FromDays(long value)
        => new(Duration.FromDays(value));

    /// <summary>
    /// Gets a new <see cref="RelativeDuration"/> instance with the given value.
    /// </summary>
    /// <param name="value">
    /// A number of days.
    /// </param>
    /// <returns>A new <see cref="RelativeDuration"/> instance.</returns>
    public static RelativeDuration FromDaysInteger<T>(T value) where T : INumber<T>
        => new(Duration.FromDaysInteger(value));

    /// <summary>
    /// Gets a new <see cref="RelativeDuration"/> instance with the given value.
    /// </summary>
    /// <param name="value">
    /// A number of days.
    /// </param>
    /// <returns>A new <see cref="RelativeDuration"/> instance.</returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="value"/> satisfies <see cref="INumberBase{TSelf}.IsNaN(TSelf)"/>.
    /// </exception>
    public static RelativeDuration FromDaysFloatingPoint<T>(T value) where T : IFloatingPoint<T>
        => new(Duration.FromDaysFloatingPoint(value));

    /// <summary>
    /// Gets a new <see cref="RelativeDuration"/> instance with the given value.
    /// </summary>
    /// <param name="value">
    /// A number of femtoseconds.
    /// </param>
    /// <returns>A new <see cref="RelativeDuration"/> instance.</returns>
    public static RelativeDuration FromFemtoseconds(decimal value)
        => new(Duration.FromFemtoseconds(value));

    /// <summary>
    /// Gets a new <see cref="RelativeDuration"/> instance with the given value.
    /// </summary>
    /// <param name="value">
    /// A number of femtoseconds.
    /// </param>
    /// <returns>A new <see cref="RelativeDuration"/> instance.</returns>
    /// <exception cref="ArgumentException"><paramref name="value"/> is <see
    /// cref="double.NaN"/>.</exception>
    public static RelativeDuration FromFemtoseconds(double value)
        => new(Duration.FromFemtoseconds(value));

    /// <summary>
    /// Gets a new <see cref="RelativeDuration"/> instance with the given value.
    /// </summary>
    /// <param name="value">
    /// A number of femtoseconds.
    /// </param>
    /// <returns>A new <see cref="RelativeDuration"/> instance.</returns>
    public static RelativeDuration FromFemtoseconds(long value)
        => new(Duration.FromFemtoseconds(value));

    /// <summary>
    /// Gets a new <see cref="RelativeDuration"/> instance with the given value.
    /// </summary>
    /// <param name="value">
    /// A number of femtoseconds.
    /// </param>
    /// <returns>A new <see cref="RelativeDuration"/> instance.</returns>
    public static RelativeDuration FromFemtosecondsInteger<T>(T value) where T : INumber<T>
        => new(Duration.FromFemtosecondsInteger(value));

    /// <summary>
    /// Gets a new <see cref="RelativeDuration"/> instance with the given value.
    /// </summary>
    /// <param name="value">
    /// A number of femtoseconds.
    /// </param>
    /// <returns>A new <see cref="RelativeDuration"/> instance.</returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="value"/> satisfies <see cref="INumberBase{TSelf}.IsNaN(TSelf)"/>.
    /// </exception>
    public static RelativeDuration FromFemtosecondsFloatingPoint<T>(T value) where T : IFloatingPoint<T>
        => new(Duration.FromFemtosecondsFloatingPoint(value));

    /// <summary>
    /// Gets a new <see cref="RelativeDuration"/> instance with the given value.
    /// </summary>
    /// <param name="value">
    /// A number of hours.
    /// </param>
    /// <returns>A new <see cref="RelativeDuration"/> instance.</returns>
    public static RelativeDuration FromHours(decimal value)
        => new(Duration.FromHours(value));

    /// <summary>
    /// Gets a new <see cref="RelativeDuration"/> instance with the given value.
    /// </summary>
    /// <param name="value">
    /// A number of hours.
    /// </param>
    /// <returns>A new <see cref="RelativeDuration"/> instance.</returns>
    /// <exception cref="ArgumentException"><paramref name="value"/> is <see
    /// cref="double.NaN"/>.</exception>
    public static RelativeDuration FromHours(double value)
        => new(Duration.FromHours(value));

    /// <summary>
    /// Gets a new <see cref="RelativeDuration"/> instance with the given value.
    /// </summary>
    /// <param name="value">
    /// A number of hours.
    /// </param>
    /// <returns>A new <see cref="RelativeDuration"/> instance.</returns>
    public static RelativeDuration FromHours(long value)
        => new(Duration.FromHours(value));

    /// <summary>
    /// Gets a new <see cref="RelativeDuration"/> instance with the given value.
    /// </summary>
    /// <param name="value">
    /// A number of hours.
    /// </param>
    /// <returns>A new <see cref="RelativeDuration"/> instance.</returns>
    public static RelativeDuration FromHoursInteger<T>(T value) where T : INumber<T>
        => new(Duration.FromHoursInteger(value));

    /// <summary>
    /// Gets a new <see cref="RelativeDuration"/> instance with the given value.
    /// </summary>
    /// <param name="value">
    /// A number of days.
    /// </param>
    /// <returns>A new <see cref="RelativeDuration"/> instance.</returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="value"/> satisfies <see cref="INumberBase{TSelf}.IsNaN(TSelf)"/>.
    /// </exception>
    public static RelativeDuration FromHoursFloatingPoint<T>(T value) where T : IFloatingPoint<T>
        => new(Duration.FromHoursFloatingPoint(value));

    /// <summary>
    /// Gets a new <see cref="RelativeDuration"/> instance with the given value.
    /// </summary>
    /// <param name="value">
    /// A number of microseconds.
    /// </param>
    /// <returns>A new <see cref="RelativeDuration"/> instance.</returns>
    public static RelativeDuration FromMicroseconds(decimal value)
        => new(Duration.FromMicroseconds(value));

    /// <summary>
    /// Gets a new <see cref="RelativeDuration"/> instance with the given value.
    /// </summary>
    /// <param name="value">
    /// A number of microseconds.
    /// </param>
    /// <returns>A new <see cref="RelativeDuration"/> instance.</returns>
    /// <exception cref="ArgumentException"><paramref name="value"/> is <see
    /// cref="double.NaN"/>.</exception>
    public static RelativeDuration FromMicroseconds(double value)
        => new(Duration.FromMicroseconds(value));

    /// <summary>
    /// Gets a new <see cref="RelativeDuration"/> instance with the given value.
    /// </summary>
    /// <param name="value">
    /// A number of microseconds.
    /// </param>
    /// <returns>A new <see cref="RelativeDuration"/> instance.</returns>
    public static RelativeDuration FromMicroseconds(long value)
        => new(Duration.FromMicroseconds(value));

    /// <summary>
    /// Gets a new <see cref="RelativeDuration"/> instance with the given value.
    /// </summary>
    /// <param name="value">
    /// A number of microseconds.
    /// </param>
    /// <returns>A new <see cref="RelativeDuration"/> instance.</returns>
    public static RelativeDuration FromMicrosecondsInteger<T>(T value) where T : INumber<T>
        => new(Duration.FromMicrosecondsInteger(value));

    /// <summary>
    /// Gets a new <see cref="RelativeDuration"/> instance with the given value.
    /// </summary>
    /// <param name="value">
    /// A number of microseconds.
    /// </param>
    /// <returns>A new <see cref="RelativeDuration"/> instance.</returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="value"/> satisfies <see cref="INumberBase{TSelf}.IsNaN(TSelf)"/>.
    /// </exception>
    public static RelativeDuration FromMicrosecondsFloatingPoint<T>(T value) where T : IFloatingPoint<T>
        => new(Duration.FromMicrosecondsFloatingPoint(value));

    /// <summary>
    /// Gets a new <see cref="RelativeDuration"/> instance with the given value.
    /// </summary>
    /// <param name="value">
    /// A number of milliseconds.
    /// </param>
    /// <returns>A new <see cref="RelativeDuration"/> instance.</returns>
    public static RelativeDuration FromMilliseconds(decimal value)
        => new(Duration.FromMilliseconds(value));

    /// <summary>
    /// Gets a new <see cref="RelativeDuration"/> instance with the given value.
    /// </summary>
    /// <param name="value">
    /// A number of milliseconds.
    /// </param>
    /// <returns>A new <see cref="RelativeDuration"/> instance.</returns>
    /// <exception cref="ArgumentException"><paramref name="value"/> is <see
    /// cref="double.NaN"/>.</exception>
    public static RelativeDuration FromMilliseconds(double value)
        => new(Duration.FromMilliseconds(value));

    /// <summary>
    /// Gets a new <see cref="RelativeDuration"/> instance with the given value.
    /// </summary>
    /// <param name="value">
    /// A number of milliseconds.
    /// </param>
    /// <returns>A new <see cref="RelativeDuration"/> instance.</returns>
    public static RelativeDuration FromMilliseconds(long value)
        => new(Duration.FromMilliseconds(value));

    /// <summary>
    /// Gets a new <see cref="RelativeDuration"/> instance with the given value.
    /// </summary>
    /// <param name="value">
    /// A number of milliseconds.
    /// </param>
    /// <returns>A new <see cref="RelativeDuration"/> instance.</returns>
    public static RelativeDuration FromMillisecondsInteger<T>(T value) where T : INumber<T>
        => new(Duration.FromMillisecondsInteger(value));

    /// <summary>
    /// Gets a new <see cref="RelativeDuration"/> instance with the given value.
    /// </summary>
    /// <param name="value">
    /// A number of milliseconds.
    /// </param>
    /// <returns>A new <see cref="RelativeDuration"/> instance.</returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="value"/> satisfies <see cref="INumberBase{TSelf}.IsNaN(TSelf)"/>.
    /// </exception>
    public static RelativeDuration FromMillisecondsFloatingPoint<T>(T value) where T : IFloatingPoint<T>
        => new(Duration.FromMillisecondsFloatingPoint(value));

    /// <summary>
    /// Gets a new <see cref="RelativeDuration"/> instance with the given value.
    /// </summary>
    /// <param name="value">
    /// A number of minutes.
    /// </param>
    /// <returns>A new <see cref="RelativeDuration"/> instance.</returns>
    public static RelativeDuration FromMinutes(decimal value)
        => new(Duration.FromMinutes(value));

    /// <summary>
    /// Gets a new <see cref="RelativeDuration"/> instance with the given value.
    /// </summary>
    /// <param name="value">
    /// A number of minutes.
    /// </param>
    /// <returns>A new <see cref="RelativeDuration"/> instance.</returns>
    /// <exception cref="ArgumentException"><paramref name="value"/> is <see
    /// cref="double.NaN"/>.</exception>
    public static RelativeDuration FromMinutes(double value)
        => new(Duration.FromMinutes(value));

    /// <summary>
    /// Gets a new <see cref="RelativeDuration"/> instance with the given value.
    /// </summary>
    /// <param name="value">
    /// A number of minutes.
    /// </param>
    /// <returns>A new <see cref="RelativeDuration"/> instance.</returns>
    public static RelativeDuration FromMinutes(long value)
        => new(Duration.FromMinutes(value));

    /// <summary>
    /// Gets a new <see cref="RelativeDuration"/> instance with the given value.
    /// </summary>
    /// <param name="value">
    /// A number of minutes.
    /// </param>
    /// <returns>A new <see cref="RelativeDuration"/> instance.</returns>
    public static RelativeDuration FromMinutesInteger<T>(T value) where T : INumber<T>
        => new(Duration.FromMinutesInteger(value));

    /// <summary>
    /// Gets a new <see cref="RelativeDuration"/> instance with the given value.
    /// </summary>
    /// <param name="value">
    /// A number of minutes.
    /// </param>
    /// <returns>A new <see cref="RelativeDuration"/> instance.</returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="value"/> satisfies <see cref="INumberBase{TSelf}.IsNaN(TSelf)"/>.
    /// </exception>
    public static RelativeDuration FromMinutesFloatingPoint<T>(T value) where T : IFloatingPoint<T>
        => new(Duration.FromMinutesFloatingPoint(value));

    /// <summary>
    /// Gets a new <see cref="RelativeDuration"/> instance with the given value.
    /// </summary>
    /// <param name="value">
    /// A number of nanoseconds.
    /// </param>
    /// <returns>A new <see cref="RelativeDuration"/> instance.</returns>
    public static RelativeDuration FromNanoseconds(decimal value)
        => new(Duration.FromNanoseconds(value));

    /// <summary>
    /// Gets a new <see cref="RelativeDuration"/> instance with the given value.
    /// </summary>
    /// <param name="value">
    /// A number of nanoseconds.
    /// </param>
    /// <returns>A new <see cref="RelativeDuration"/> instance.</returns>
    /// <exception cref="ArgumentException"><paramref name="value"/> is <see
    /// cref="double.NaN"/>.</exception>
    public static RelativeDuration FromNanoseconds(double value)
        => new(Duration.FromNanoseconds(value));

    /// <summary>
    /// Gets a new <see cref="RelativeDuration"/> instance with the given value.
    /// </summary>
    /// <param name="value">
    /// A number of nanoseconds.
    /// </param>
    /// <returns>A new <see cref="RelativeDuration"/> instance.</returns>
    public static RelativeDuration FromNanoseconds(long value)
        => new(Duration.FromNanoseconds(value));

    /// <summary>
    /// Gets a new <see cref="RelativeDuration"/> instance with the given value.
    /// </summary>
    /// <param name="value">
    /// A number of nanoseconds.
    /// </param>
    /// <returns>A new <see cref="RelativeDuration"/> instance.</returns>
    public static RelativeDuration FromNanosecondsInteger<T>(T value) where T : INumber<T>
        => new(Duration.FromNanosecondsInteger(value));

    /// <summary>
    /// Gets a new <see cref="RelativeDuration"/> instance with the given value.
    /// </summary>
    /// <param name="value">
    /// A number of nanoseconds.
    /// </param>
    /// <returns>A new <see cref="RelativeDuration"/> instance.</returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="value"/> satisfies <see cref="INumberBase{TSelf}.IsNaN(TSelf)"/>.
    /// </exception>
    public static RelativeDuration FromNanosecondsFloatingPoint<T>(T value) where T : IFloatingPoint<T>
        => new(Duration.FromNanosecondsFloatingPoint(value));

    /// <summary>
    /// Gets a new <see cref="RelativeDuration"/> instance with the given value.
    /// </summary>
    /// <param name="value">
    /// A number of picoseconds.
    /// </param>
    /// <returns>A new <see cref="RelativeDuration"/> instance.</returns>
    public static RelativeDuration FromPicoseconds(decimal value)
        => new(Duration.FromPicoseconds(value));

    /// <summary>
    /// Gets a new <see cref="RelativeDuration"/> instance with the given value.
    /// </summary>
    /// <param name="value">
    /// A number of picoseconds.
    /// </param>
    /// <returns>A new <see cref="RelativeDuration"/> instance.</returns>
    /// <exception cref="ArgumentException"><paramref name="value"/> is <see
    /// cref="double.NaN"/>.</exception>
    public static RelativeDuration FromPicoseconds(double value)
        => new(Duration.FromPicoseconds(value));

    /// <summary>
    /// Gets a new <see cref="RelativeDuration"/> instance with the given value.
    /// </summary>
    /// <param name="value">
    /// A number of picoseconds.
    /// </param>
    /// <returns>A new <see cref="RelativeDuration"/> instance.</returns>
    public static RelativeDuration FromPicoseconds(long value)
        => new(Duration.FromPicoseconds(value));

    /// <summary>
    /// Gets a new <see cref="RelativeDuration"/> instance with the given value.
    /// </summary>
    /// <param name="value">
    /// A number of picoseconds.
    /// </param>
    /// <returns>A new <see cref="RelativeDuration"/> instance.</returns>
    public static RelativeDuration FromPicosecondsInteger<T>(T value) where T : INumber<T>
        => new(Duration.FromPicosecondsInteger(value));

    /// <summary>
    /// Gets a new <see cref="RelativeDuration"/> instance with the given value.
    /// </summary>
    /// <param name="value">
    /// A number of picoseconds.
    /// </param>
    /// <returns>A new <see cref="RelativeDuration"/> instance.</returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="value"/> satisfies <see cref="INumberBase{TSelf}.IsNaN(TSelf)"/>.
    /// </exception>
    public static RelativeDuration FromPicosecondsFloatingPoint<T>(T value) where T : IFloatingPoint<T>
        => new(Duration.FromPicosecondsFloatingPoint(value));

    /// <summary>
    /// Gets a new instance of <see cref="RelativeDuration"/> representing the given proportion
    /// of a local day.
    /// </summary>
    /// <param name="proportion">A proportion pf a local day to set as the absolute value of
    /// this instance. Negative values are treated as zero.</param>
    /// <returns>
    /// A new instance of <see cref="RelativeDuration"/> representing the given proportion of a
    /// local day.
    /// </returns>
    public static RelativeDuration FromProportionOfDay(decimal proportion)
        => new(proportion, RelativeDurationType.ProportionOfDay);

    /// <summary>
    /// Gets a new instance of <see cref="RelativeDuration"/> representing the given proportion
    /// of a local day.
    /// </summary>
    /// <param name="proportion">A proportion pf a local day to set as the absolute value of
    /// this instance. Negative values are treated as zero.</param>
    /// <returns>
    /// A new instance of <see cref="RelativeDuration"/> representing the given proportion of a
    /// local day.
    /// </returns>
    public static RelativeDuration FromProportionOfDay(double proportion)
        => new(proportion, RelativeDurationType.ProportionOfDay);

    /// <summary>
    /// Gets a new instance of <see cref="RelativeDuration"/> representing the given proportion
    /// of a local year.
    /// </summary>
    /// <param name="proportion">A proportion pf a local year to set as the absolute value of
    /// this instance. Negative values are treated as zero.</param>
    /// <returns>
    /// A new instance of <see cref="RelativeDuration"/> representing the given proportion of a
    /// local year.
    /// </returns>
    public static RelativeDuration FromProportionOfYear(decimal proportion)
        => new(proportion, RelativeDurationType.ProportionOfYear);

    /// <summary>
    /// Gets a new instance of <see cref="RelativeDuration"/> representing the given proportion
    /// of a local year.
    /// </summary>
    /// <param name="proportion">A proportion pf a local year to set as the absolute value of
    /// this instance. Negative values are treated as zero.</param>
    /// <returns>
    /// A new instance of <see cref="RelativeDuration"/> representing the given proportion of a
    /// local year.
    /// </returns>
    public static RelativeDuration FromProportionOfYear(double proportion)
        => new(proportion, RelativeDurationType.ProportionOfYear);

    /// <summary>
    /// Gets a new <see cref="RelativeDuration"/> instance with the given value.
    /// </summary>
    /// <param name="value">
    /// An amount of Planck time.
    /// </param>
    /// <returns>A new <see cref="RelativeDuration"/> instance.</returns>
    public static RelativeDuration FromPlanckTime(decimal value)
        => new(Duration.FromPlanckTime(value));

    /// <summary>
    /// Gets a new <see cref="RelativeDuration"/> instance with the given value.
    /// </summary>
    /// <param name="value">
    /// An amount of Planck time.
    /// </param>
    /// <returns>A new <see cref="RelativeDuration"/> instance.</returns>
    /// <exception cref="ArgumentException"><paramref name="value"/> is <see
    /// cref="double.NaN"/>.</exception>
    public static RelativeDuration FromPlanckTime(double value)
        => new(Duration.FromPlanckTime(value));

    /// <summary>
    /// Gets a new <see cref="RelativeDuration"/> instance with the given value.
    /// </summary>
    /// <param name="value">
    /// An amount of Planck time.
    /// </param>
    /// <returns>A new <see cref="RelativeDuration"/> instance.</returns>
    public static RelativeDuration FromPlanckTime(long value)
        => new(Duration.FromPlanckTime(value));

    /// <summary>
    /// Gets a new <see cref="RelativeDuration"/> instance with the given value.
    /// </summary>
    /// <param name="value">
    /// An amount of Planck time.
    /// </param>
    /// <returns>A new <see cref="RelativeDuration"/> instance.</returns>
    public static RelativeDuration FromPlanckTime<T>(T value) where T : INumber<T>
        => new(Duration.FromPlanckTime(value));

    /// <summary>
    /// Gets a new <see cref="RelativeDuration"/> instance with the given value.
    /// </summary>
    /// <param name="value">
    /// A number of seconds.
    /// </param>
    /// <returns>A new <see cref="RelativeDuration"/> instance.</returns>
    public static RelativeDuration FromSeconds(decimal value)
        => new(Duration.FromSeconds(value));

    /// <summary>
    /// Gets a new <see cref="RelativeDuration"/> instance with the given value.
    /// </summary>
    /// <param name="value">
    /// A number of seconds.
    /// </param>
    /// <returns>A new <see cref="RelativeDuration"/> instance.</returns>
    /// <exception cref="ArgumentException"><paramref name="value"/> is <see
    /// cref="double.NaN"/>.</exception>
    public static RelativeDuration FromSeconds(double value)
        => new(Duration.FromSeconds(value));

    /// <summary>
    /// Gets a new <see cref="RelativeDuration"/> instance with the given value.
    /// </summary>
    /// <param name="value">
    /// A number of seconds.
    /// </param>
    /// <returns>A new <see cref="RelativeDuration"/> instance.</returns>
    public static RelativeDuration FromSeconds(long value)
        => new(Duration.FromSeconds(value));

    /// <summary>
    /// Gets a new <see cref="RelativeDuration"/> instance with the given value.
    /// </summary>
    /// <param name="value">
    /// A number of seconds.
    /// </param>
    /// <returns>A new <see cref="RelativeDuration"/> instance.</returns>
    public static RelativeDuration FromSecondsInteger<T>(T value) where T : INumber<T>
        => new(Duration.FromSecondsInteger(value));

    /// <summary>
    /// Gets a new <see cref="RelativeDuration"/> instance with the given value.
    /// </summary>
    /// <param name="value">
    /// A number of seconds.
    /// </param>
    /// <returns>A new <see cref="RelativeDuration"/> instance.</returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="value"/> satisfies <see cref="INumberBase{TSelf}.IsNaN(TSelf)"/>.
    /// </exception>
    public static RelativeDuration FromSecondsFloatingPoint<T>(T value) where T : IFloatingPoint<T>
        => new(Duration.FromSecondsFloatingPoint(value));

    /// <summary>
    /// Converts the given <see cref="TimeSpan"/> value to a <see cref="RelativeDuration"/>
    /// value.
    /// </summary>
    /// <param name="timeSpan">A <see cref="TimeSpan"/> value to convert.</param>
    /// <returns>An equivalent <see cref="RelativeDuration"/> value.</returns>
    /// <remarks>
    /// <para>
    /// The current aeon sequence of our universe is presumed to be 99731, based on current best
    /// estimates (1.3799e10±2.1e7 years).
    /// </para>
    /// <para>
    /// The <see cref="TimeSpan"/> value is converted to a <see cref="RelativeDuration"/>
    /// instance by presuming that the current aeon begins at the same moment as <see
    /// cref="TimeSpan.Ticks"/> begins counting. The <see cref="TimeSpan.Ticks"/> of the <see
    /// cref="TimeSpan"/> value are converted into the timekeeping system of <see
    /// cref="RelativeDuration"/> based on that assumption.
    /// </para>
    /// </remarks>
    public static RelativeDuration FromTimeSpan(TimeSpan timeSpan)
        => new(Duration.FromTimeSpan(timeSpan));

    /// <summary>
    /// Gets a new <see cref="RelativeDuration"/> instance with the given value.
    /// </summary>
    /// <param name="value">
    /// A number of years.
    /// </param>
    /// <returns>A new <see cref="RelativeDuration"/> instance.</returns>
    public static RelativeDuration FromYears(decimal value)
        => new(Duration.FromYears(value));

    /// <summary>
    /// Gets a new <see cref="RelativeDuration"/> instance with the given value.
    /// </summary>
    /// <param name="value">
    /// A number of years.
    /// </param>
    /// <returns>A new <see cref="RelativeDuration"/> instance.</returns>
    /// <exception cref="ArgumentException"><paramref name="value"/> is <see
    /// cref="double.NaN"/>.</exception>
    public static RelativeDuration FromYears(double value)
        => new(Duration.FromYears(value));

    /// <summary>
    /// Gets a new <see cref="RelativeDuration"/> instance with the given value.
    /// </summary>
    /// <param name="value">
    /// A number of years.
    /// </param>
    /// <returns>A new <see cref="RelativeDuration"/> instance.</returns>
    public static RelativeDuration FromYears(long value)
        => new(Duration.FromYears(value));

    /// <summary>
    /// Gets a new <see cref="RelativeDuration"/> instance with the given value.
    /// </summary>
    /// <param name="value">
    /// A number of years.
    /// </param>
    /// <returns>A new <see cref="RelativeDuration"/> instance.</returns>
    public static RelativeDuration FromYearsInteger<T>(T value) where T : INumber<T>
        => new(Duration.FromYearsInteger(value));

    /// <summary>
    /// Gets a new <see cref="RelativeDuration"/> instance with the given value.
    /// </summary>
    /// <param name="value">
    /// A number of years.
    /// </param>
    /// <returns>A new <see cref="RelativeDuration"/> instance.</returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="value"/> satisfies <see cref="INumberBase{TSelf}.IsNaN(TSelf)"/>.
    /// </exception>
    public static RelativeDuration FromYearsFloatingPoint<T>(T value) where T : IFloatingPoint<T>
        => new(Duration.FromYearsFloatingPoint(value));

    /// <summary>
    /// Gets a new <see cref="RelativeDuration"/> instance with the given value.
    /// </summary>
    /// <param name="value">
    /// A number of yoctoseconds.
    /// </param>
    /// <returns>A new <see cref="RelativeDuration"/> instance.</returns>
    public static RelativeDuration FromYoctoseconds(decimal value)
        => new(Duration.FromYoctoseconds(value));

    /// <summary>
    /// Gets a new <see cref="RelativeDuration"/> instance with the given value.
    /// </summary>
    /// <param name="value">
    /// A number of yoctoseconds.
    /// </param>
    /// <returns>A new <see cref="RelativeDuration"/> instance.</returns>
    /// <exception cref="ArgumentException"><paramref name="value"/> is <see
    /// cref="double.NaN"/>.</exception>
    public static RelativeDuration FromYoctoseconds(double value)
        => new(Duration.FromYoctoseconds(value));

    /// <summary>
    /// Gets a new <see cref="RelativeDuration"/> instance with the given value.
    /// </summary>
    /// <param name="value">
    /// A number of yoctoseconds.
    /// </param>
    /// <returns>A new <see cref="RelativeDuration"/> instance.</returns>
    public static RelativeDuration FromYoctoseconds(long value)
        => new(Duration.FromYoctoseconds(value));

    /// <summary>
    /// Gets a new <see cref="RelativeDuration"/> instance with the given value.
    /// </summary>
    /// <param name="value">
    /// A number of yoctoseconds.
    /// </param>
    /// <returns>A new <see cref="RelativeDuration"/> instance.</returns>
    public static RelativeDuration FromYoctosecondsInteger<T>(T value) where T : INumber<T>
        => new(Duration.FromYoctosecondsInteger(value));

    /// <summary>
    /// Gets a new <see cref="RelativeDuration"/> instance with the given value.
    /// </summary>
    /// <param name="value">
    /// A number of yoctoseconds.
    /// </param>
    /// <returns>A new <see cref="RelativeDuration"/> instance.</returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="value"/> satisfies <see cref="INumberBase{TSelf}.IsNaN(TSelf)"/>.
    /// </exception>
    public static RelativeDuration FromYoctosecondsFloatingPoint<T>(T value) where T : IFloatingPoint<T>
        => new(Duration.FromYoctosecondsFloatingPoint(value));

    /// <summary>
    /// Gets a new <see cref="RelativeDuration"/> instance with the given value.
    /// </summary>
    /// <param name="value">
    /// A number of zeptoseconds.
    /// </param>
    /// <returns>A new <see cref="RelativeDuration"/> instance.</returns>
    public static RelativeDuration FromZeptoseconds(decimal value)
        => new(Duration.FromZeptoseconds(value));

    /// <summary>
    /// Gets a new <see cref="RelativeDuration"/> instance with the given value.
    /// </summary>
    /// <param name="value">
    /// A number of zeptoseconds.
    /// </param>
    /// <returns>A new <see cref="RelativeDuration"/> instance.</returns>
    /// <exception cref="ArgumentException"><paramref name="value"/> is <see
    /// cref="double.NaN"/>.</exception>
    public static RelativeDuration FromZeptoseconds(double value)
        => new(Duration.FromZeptoseconds(value));

    /// <summary>
    /// Gets a new <see cref="RelativeDuration"/> instance with the given value.
    /// </summary>
    /// <param name="value">
    /// A number of zeptoseconds.
    /// </param>
    /// <returns>A new <see cref="RelativeDuration"/> instance.</returns>
    public static RelativeDuration FromZeptoseconds(long value)
        => new(Duration.FromZeptoseconds(value));

    /// <summary>
    /// Gets a new <see cref="RelativeDuration"/> instance with the given value.
    /// </summary>
    /// <param name="value">
    /// A number of zeptoseconds.
    /// </param>
    /// <returns>A new <see cref="RelativeDuration"/> instance.</returns>
    public static RelativeDuration FromZeptosecondsInteger<T>(T value) where T : INumber<T>
        => new(Duration.FromZeptosecondsInteger(value));

    /// <summary>
    /// Gets a new <see cref="RelativeDuration"/> instance with the given value.
    /// </summary>
    /// <param name="value">
    /// A number of zeptoseconds.
    /// </param>
    /// <returns>A new <see cref="RelativeDuration"/> instance.</returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="value"/> satisfies <see cref="INumberBase{TSelf}.IsNaN(TSelf)"/>.
    /// </exception>
    public static RelativeDuration FromZeptosecondsFloatingPoint<T>(T value) where T : IFloatingPoint<T>
        => new(Duration.FromZeptosecondsFloatingPoint(value));

    /// <summary>
    /// Converts this instance to a <see cref="DateTime"/> value, given the absolute durations
    /// of the local year and day.
    /// </summary>
    /// <param name="localYear">The duration of the local year.</param>
    /// <param name="localDay">The duration of the local day.</param>
    /// <returns>A <see cref="DateTime"/> value equivalent to this instance.</returns>
    /// <remarks>
    /// <para>
    /// This instance is converted to a <see cref="DateTime"/> value by presuming that the
    /// current aeon begins at the same moment as <see cref="DateTime.Ticks"/> begins counting.
    /// A <see cref="DateTime.Ticks"/> value for the <see cref="DateTime"/> value is generated
    /// from the timekeeping system of <see cref="RelativeDuration"/> based on that assumption.
    /// </para>
    /// <para>
    /// The <see cref="Duration.Aeons"/> are ignored during conversion, as <see
    /// cref="DateTime"/> does not allow for such large values, except at the lowest end of the
    /// possible range. This means that round-trip conversions will fail if the aeon changes
    /// between conversions to and from <see cref="RelativeDuration"/>. It is recommended that
    /// if your use-case calls for conversion between <see cref="RelativeDuration"/> and <see
    /// cref="DateTime"/> in both directions, care must be taken to ensure that the aeon
    /// boundary is not crossed, or handled appropriately by calling code if it is.
    /// </para>
    /// <para>
    /// The returned <see cref="DateTime"/> will have a <see cref="DateTime.Kind"/> of <see
    /// cref="DateTimeKind.Unspecified"/>.
    /// </para>
    /// </remarks>
    public DateTime ToDateTime(Duration localYear, Duration localDay)
        => ToUniversalDuration(localYear, localDay).ToDateTime();

    /// <summary>
    /// Converts this instance to a <see cref="DateTimeOffset"/> value, given the absolute
    /// durations of the local year and day.
    /// </summary>
    /// <param name="localYear">The duration of the local year.</param>
    /// <param name="localDay">The duration of the local day.</param>
    /// <returns>A <see cref="DateTimeOffset"/> value equivalent to this instance.</returns>
    /// <remarks>
    /// <para>
    /// This instance is converted to a <see cref="DateTimeOffset"/> value by presuming that the
    /// current aeon begins at the same moment as <see cref="DateTimeOffset.Ticks"/> begins
    /// counting. A <see cref="DateTimeOffset.Ticks"/> value for the <see
    /// cref="DateTimeOffset"/> value is generated from the timekeeping system of <see
    /// cref="RelativeDuration"/> based on that assumption.
    /// </para>
    /// <para>
    /// The <see cref="Duration.Aeons"/> are ignored during conversion, as <see
    /// cref="DateTimeOffset"/> does not allow for such large values, except at the lowest end
    /// of the possible range. This means that round-trip conversions will fail if the aeon
    /// changes between conversions to and from <see cref="RelativeDuration"/>. It is
    /// recommended that if your use-case calls for conversion between <see
    /// cref="RelativeDuration"/> and <see cref="DateTimeOffset"/> in both directions, care must
    /// be taken to ensure that the aeon boundary is not crossed, or handled appropriately by
    /// calling code if it is.
    /// </para>
    /// <para>
    /// The returned <see cref="DateTimeOffset"/> will have an <see
    /// cref="DateTimeOffset.Offset"/> of <see cref="TimeSpan.Zero"/>.
    /// </para>
    /// </remarks>
    public DateTimeOffset ToDateTimeOffset(Duration localYear, Duration localDay)
        => ToUniversalDuration(localYear, localDay).ToDateTimeOffset();

    /// <summary>
    /// Converts this instance to a <see cref="TimeSpan"/> value, given the absolute durations
    /// of the local year and day.
    /// </summary>
    /// <param name="localYear">The duration of the local year.</param>
    /// <param name="localDay">The duration of the local day.</param>
    /// <returns>A <see cref="TimeSpan"/> value equivalent to this instance.</returns>
    /// <remarks>
    /// <para>
    /// This instance is converted to a <see cref="TimeSpan"/> value by presuming that the
    /// current aeon begins at the same moment as <see cref="TimeSpan.Ticks"/> begins counting.
    /// A <see cref="TimeSpan.Ticks"/> value for the <see cref="TimeSpan"/> value is generated
    /// from the timekeeping system of <see cref="RelativeDuration"/> based on that assumption.
    /// </para>
    /// <para>
    /// The <see cref="Duration.Aeons"/> are ignored during conversion, as <see
    /// cref="TimeSpan"/> does not allow for such large values, except at the lowest end of the
    /// possible range. This means that round-trip conversions will fail if the aeon changes
    /// between conversions to and from <see cref="RelativeDuration"/>. It is recommended that
    /// if your use-case calls for conversion between <see cref="RelativeDuration"/> and <see
    /// cref="TimeSpan"/> in both directions, care must be taken to ensure that the aeon
    /// boundary is not crossed, or handled appropriately by calling code if it is.
    /// </para>
    /// </remarks>
    public TimeSpan ToTimeSpan(Duration localYear, Duration localDay)
        => ToUniversalDuration(localYear, localDay).ToTimeSpan();

    /// <summary>
    /// Converts a relative duration to a universal duration, given the absolute durations of
    /// the local year and day.
    /// </summary>
    /// <param name="localYear">The duration of the local year.</param>
    /// <param name="localDay">The duration of the local day.</param>
    /// <returns>A <see cref="Duration"/> equivalent to this <see
    /// cref="RelativeDuration"/>.</returns>
    public Duration ToUniversalDuration(Duration localYear, Duration localDay) => Relativity switch
    {
        RelativeDurationType.Absolute => Duration,
        RelativeDurationType.ProportionOfDay => Duration.FromProportionOfDay(Proportion, localDay),
        RelativeDurationType.ProportionOfYear => Duration.FromProportionOfYear(Proportion, localYear),
        _ => Duration.Zero,
    };
}
