using System.Numerics;
using Tavenem.Mathematics;

namespace Tavenem.Time;

public partial struct Duration
{
    private const long AeonUnit = 1000000000000000000;

    /// <summary>
    /// Gets a new <see cref="Duration"/> instance with the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    /// A number of aeons (1,000,000 years).
    /// </param>
    /// <returns>A new <see cref="Duration"/> instance.</returns>
    public static Duration FromAeons(decimal value)
    {
        if (value == 0)
        {
            return Zero;
        }
        var isNegative = value < 0;
        value = Math.Abs(value);

        var aeons = Math.Floor(value);
        value -= aeons;

        var d = new Duration(
            false,
            false,
            null,
            0,
            0,
            0,
            (BigInteger)aeons);
        if (value > 0)
        {
            d += FromYears(value / YearsPerAeon);
        }
        if (isNegative)
        {
            d = d.Negate();
        }
        return d;
    }

    /// <summary>
    /// Gets a new <see cref="Duration"/> instance with the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    /// A number of aeons (1,000,000 years).
    /// </param>
    /// <returns>A new <see cref="Duration"/> instance.</returns>
    /// <exception cref="ArgumentException"><paramref name="value"/> is <see
    /// cref="double.NaN"/>.</exception>
    public static Duration FromAeons(double value)
    {
        if (double.IsNaN(value))
        {
            throw new ArgumentException($"{nameof(value)} was NaN.");
        }
        if (value.IsNearlyZero())
        {
            return Zero;
        }
        if (double.IsPositiveInfinity(value))
        {
            return PositiveInfinity;
        }
        if (double.IsNegativeInfinity(value))
        {
            return NegativeInfinity;
        }
        var isNegative = value < 0;
        value = Math.Abs(value);

        var aeons = Math.Floor(value);
        value -= aeons;

        var d = new Duration(
            false,
            false,
            null,
            0,
            0,
            0,
            (BigInteger)aeons);
        if (value > 0)
        {
            d += FromYears(value / YearsPerAeon);
        }
        if (isNegative)
        {
            d = d.Negate();
        }
        return d;
    }

    /// <summary>
    /// Gets a new <see cref="Duration"/> instance with the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    /// A number of aeons (1,000,000 years).
    /// </param>
    /// <returns>A new <see cref="Duration"/> instance.</returns>
    public static Duration FromAeons(long value) => value == 0
        ? Zero
        : new Duration(
            value < 0,
            false,
            null,
            0,
            0,
            0,
            Math.Abs(value));

    /// <summary>
    /// Gets a new <see cref="Duration"/> instance with the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    /// A number of aeons (1,000,000 years).
    /// </param>
    /// <returns>A new <see cref="Duration"/> instance.</returns>
    public static Duration FromAeonsInteger<T>(T value) where T : INumber<T> => value == T.Zero
        ? Zero
        : new Duration(
            value < T.Zero,
            false,
            null,
            0,
            0,
            0,
            BigInteger.Parse(T.Abs(value)
                .ToString("F0", System.Globalization.CultureInfo.InvariantCulture)));

    /// <summary>
    /// Gets a new <see cref="Duration"/> instance with the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    /// A number of aeons (1,000,000 years).
    /// </param>
    /// <returns>A new <see cref="Duration"/> instance.</returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="value"/> is <see cref="IFloatingPoint{TSelf}.NaN"/>.
    /// </exception>
    public static Duration FromAeonsFloatingPoint<T>(T value) where T : IFloatingPoint<T>
    {
        if (T.IsNaN(value))
        {
            throw new ArgumentException($"{nameof(value)} was NaN.");
        }
        if (value.IsNearlyZero())
        {
            return Zero;
        }
        if (T.IsPositiveInfinity(value))
        {
            return PositiveInfinity;
        }
        if (T.IsNegativeInfinity(value))
        {
            return NegativeInfinity;
        }
        var isNegative = value < T.Zero;
        value = T.Abs(value);

        var aeons = T.Floor(value);
        value -= aeons;

        var d = new Duration(
            false,
            false,
            null,
            0,
            0,
            0,
            BigInteger.Parse(aeons.ToString("F0", System.Globalization.CultureInfo.InvariantCulture)));
        if (value > T.Zero)
        {
            d += FromYearsFloatingPoint(value / T.Create(YearsPerAeon));
        }
        if (isNegative)
        {
            d = d.Negate();
        }
        return d;
    }

    /// <summary>
    /// Gets a new <see cref="Duration"/> instance with the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    /// A number of attoseconds.
    /// </param>
    /// <returns>A new <see cref="Duration"/> instance.</returns>
    public static Duration FromAttoseconds(decimal value)
        => FromNanoseconds(value / AttosecondsPerNanosecond);

    /// <summary>
    /// Gets a new <see cref="Duration"/> instance with the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    /// A number of attoseconds.
    /// </param>
    /// <returns>A new <see cref="Duration"/> instance.</returns>
    /// <exception cref="ArgumentException"><paramref name="value"/> is <see
    /// cref="double.NaN"/>.</exception>
    public static Duration FromAttoseconds(double value)
        => FromNanoseconds(value / AttosecondsPerNanosecond);

    /// <summary>
    /// Gets a new <see cref="Duration"/> instance with the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    /// A number of attoseconds.
    /// </param>
    /// <returns>A new <see cref="Duration"/> instance.</returns>
    public static Duration FromAttoseconds(long value)
    {
        var remainder = value % AttosecondsPerNanosecond;
        var d = FromNanoseconds(value / AttosecondsPerNanosecond);
        return d + FromYoctoseconds(remainder * YoctosecondsPerAttosecond);
    }

    /// <summary>
    /// Gets a new <see cref="Duration"/> instance with the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    /// A number of attoseconds.
    /// </param>
    /// <returns>A new <see cref="Duration"/> instance.</returns>
    public static Duration FromAttosecondsInteger<T>(T value) where T : INumber<T>
    {
        var attosecondsPerNanosecond = T.Create(AttosecondsPerNanosecond);
        var remainder = value % attosecondsPerNanosecond;
        var d = FromNanosecondsInteger(value / attosecondsPerNanosecond);
        return d + FromYoctosecondsInteger(remainder * T.Create(YoctosecondsPerAttosecond));
    }

    /// <summary>
    /// Gets a new <see cref="Duration"/> instance with the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    /// A number of attoseconds.
    /// </param>
    /// <returns>A new <see cref="Duration"/> instance.</returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="value"/> is <see cref="IFloatingPoint{TSelf}.NaN"/>.
    /// </exception>
    public static Duration FromAttosecondsFloatingPoint<T>(T value) where T : IFloatingPoint<T>
        => FromNanosecondsFloatingPoint(value / T.Create(AttosecondsPerNanosecond));

    /// <summary>
    /// Converts the given <see cref="DateTime"/> value to a <see cref="Duration"/> value.
    /// </summary>
    /// <param name="dateTime">A <see cref="DateTime"/> value to convert.</param>
    /// <returns>An equivalent <see cref="Duration"/> value.</returns>
    /// <remarks>
    /// <para>
    /// The current aeon sequence of our universe is presumed to be 13799, based on current best
    /// estimates (1.3799e10±2.1e7 years).
    /// </para>
    /// <para>
    /// The <see cref="DateTime"/> value is converted to a <see cref="Duration"/> instance by
    /// presuming that the current aeon begins at the same moment as <see
    /// cref="DateTime.Ticks"/> begins counting. The <see cref="DateTime.Ticks"/> of the <see
    /// cref="DateTime"/> value are converted into the timekeeping system of <see
    /// cref="Duration"/> based on that assumption.
    /// </para>
    /// <para>
    /// The <see cref="DateTime.ToUniversalTime"/> method is used to ensure that timezone
    /// information is stripped prior to conversion. This means that converting various <see
    /// cref="DateTime"/> instances with differing timezone information should result in uniform
    /// <see cref="Duration"/> representations.
    /// </para>
    /// <para>
    /// For more accurate <see cref="DateTime"/> conversion, consider using <see
    /// cref="CosmicTime.FromDateTime(DateTime)"/>, which uses a much more precise estimation of the
    /// relationship between the age of the universe and the basis used by <see
    /// cref="DateTime"/>.
    /// </para>
    /// </remarks>
    public static Duration FromDateTime(DateTime dateTime)
        => new(nanoseconds: (ulong)(dateTime.ToUniversalTime().Ticks * NanosecondsPerTick), aeons: _DefaultAeons);

    /// <summary>
    /// Converts the given <see cref="DateTimeOffset"/> value to a <see cref="Duration"/> value.
    /// </summary>
    /// <param name="dateTime">A <see cref="DateTimeOffset"/> value to convert.</param>
    /// <returns>An equivalent <see cref="Duration"/> value.</returns>
    /// <remarks>
    /// <para>
    /// The current aeon sequence of our universe is presumed to be 13799, based on current best
    /// estimates (1.3799e10±2.1e7 years).
    /// </para>
    /// <para>
    /// The <see cref="DateTimeOffset"/> value is converted to a <see cref="Duration"/> instance
    /// by presuming that the current aeon begins at the same moment as <see
    /// cref="DateTimeOffset.Ticks"/> begins counting. The <see cref="DateTimeOffset.Ticks"/> of
    /// the <see cref="DateTimeOffset"/> value are converted into the timekeeping system of <see
    /// cref="Duration"/> based on that assumption.
    /// </para>
    /// <para>
    /// The <see cref="DateTimeOffset.ToUniversalTime"/> method is used to ensure that timezone
    /// information is stripped prior to conversion. This means that converting various <see
    /// cref="DateTimeOffset"/> instances with differing timezone information should result in
    /// uniform
    /// <see cref="Duration"/> representations.
    /// </para>
    /// <para>
    /// For more accurate <see cref="DateTimeOffset"/> conversion, consider using <see
    /// cref="CosmicTime.FromDateTimeOffset(DateTimeOffset)"/>, which uses a much more precise
    /// estimation of the relationship between the age of the universe and the basis used by
    /// <see cref="DateTimeOffset"/>.
    /// </para>
    /// </remarks>
    public static Duration FromDateTimeOffset(DateTimeOffset dateTime)
        => new(nanoseconds: (ulong)(dateTime.ToUniversalTime().Ticks * NanosecondsPerTick), aeons: _DefaultAeons);

    /// <summary>
    /// Gets a new <see cref="Duration"/> instance with the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    /// A number of days.
    /// </param>
    /// <returns>A new <see cref="Duration"/> instance.</returns>
    public static Duration FromDays(decimal value)
        => FromNanoseconds(value * NanosecondsPerDay);

    /// <summary>
    /// Gets a new <see cref="Duration"/> instance with the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    /// A number of days.
    /// </param>
    /// <returns>A new <see cref="Duration"/> instance.</returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="value"/> is <see cref="double.NaN"/>.
    /// </exception>
    public static Duration FromDays(double value)
        => FromNanoseconds(value * NanosecondsPerDay);

    /// <summary>
    /// Gets a new <see cref="Duration"/> instance with the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    /// A number of days.
    /// </param>
    /// <returns>A new <see cref="Duration"/> instance.</returns>
    public static Duration FromDays(long value)
        => FromDays((decimal)value);

    /// <summary>
    /// Gets a new <see cref="Duration"/> instance with the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    /// A number of days.
    /// </param>
    /// <returns>A new <see cref="Duration"/> instance.</returns>
    public static Duration FromDaysInteger<T>(T value) where T : INumber<T>
        => FromDays(value.Create<T, decimal>());

    /// <summary>
    /// Gets a new <see cref="Duration"/> instance with the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    /// A number of days.
    /// </param>
    /// <returns>A new <see cref="Duration"/> instance.</returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="value"/> is <see cref="IFloatingPoint{TSelf}.NaN"/>.
    /// </exception>
    public static Duration FromDaysFloatingPoint<T>(T value) where T : IFloatingPoint<T>
        => FromNanosecondsFloatingPoint(value * T.Create(NanosecondsPerDay));

    /// <summary>
    /// Gets a new <see cref="Duration"/> instance with the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    /// A number of femtoseconds.
    /// </param>
    /// <returns>A new <see cref="Duration"/> instance.</returns>
    public static Duration FromFemtoseconds(decimal value)
        => FromNanoseconds(value / FemtosecondsPerNanosecond);

    /// <summary>
    /// Gets a new <see cref="Duration"/> instance with the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    /// A number of femtoseconds.
    /// </param>
    /// <returns>A new <see cref="Duration"/> instance.</returns>
    /// <exception cref="ArgumentException"><paramref name="value"/> is <see
    /// cref="double.NaN"/>.</exception>
    public static Duration FromFemtoseconds(double value)
        => FromNanoseconds(value / FemtosecondsPerNanosecond);

    /// <summary>
    /// Gets a new <see cref="Duration"/> instance with the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    /// A number of femtoseconds.
    /// </param>
    /// <returns>A new <see cref="Duration"/> instance.</returns>
    public static Duration FromFemtoseconds(long value)
    {
        var remainder = value % FemtosecondsPerNanosecond;
        var d = FromNanoseconds(value / FemtosecondsPerNanosecond);
        return d + FromYoctoseconds(remainder * YoctosecondsPerFemtosecond);
    }

    /// <summary>
    /// Gets a new <see cref="Duration"/> instance with the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    /// A number of femtoseconds.
    /// </param>
    /// <returns>A new <see cref="Duration"/> instance.</returns>
    public static Duration FromFemtosecondsInteger<T>(T value) where T : INumber<T>
    {
        var femtosecondsPerNanosecond = T.Create(FemtosecondsPerNanosecond);
        var remainder = value % femtosecondsPerNanosecond;
        var d = FromNanosecondsInteger(value / femtosecondsPerNanosecond);
        return d + FromYoctosecondsInteger(remainder * T.Create(YoctosecondsPerFemtosecond));
    }

    /// <summary>
    /// Gets a new <see cref="Duration"/> instance with the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    /// A number of femtoseconds.
    /// </param>
    /// <returns>A new <see cref="Duration"/> instance.</returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="value"/> is <see cref="IFloatingPoint{TSelf}.NaN"/>.
    /// </exception>
    public static Duration FromFemtosecondsFloatingPoint<T>(T value) where T : IFloatingPoint<T>
        => FromNanosecondsFloatingPoint(value / T.Create(FemtosecondsPerNanosecond));

    /// <summary>
    /// Gets a new <see cref="Duration"/> instance with the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    /// A number of hours.
    /// </param>
    /// <returns>A new <see cref="Duration"/> instance.</returns>
    public static Duration FromHours(decimal value)
        => FromNanoseconds(value * NanosecondsPerHour);

    /// <summary>
    /// Gets a new <see cref="Duration"/> instance with the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    /// A number of hours.
    /// </param>
    /// <returns>A new <see cref="Duration"/> instance.</returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="value"/> is <see cref="double.NaN"/>.
    /// </exception>
    public static Duration FromHours(double value)
        => FromNanoseconds(value * NanosecondsPerHour);

    /// <summary>
    /// Gets a new <see cref="Duration"/> instance with the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    /// A number of hours.
    /// </param>
    /// <returns>A new <see cref="Duration"/> instance.</returns>
    public static Duration FromHours(long value)
        => FromHours((decimal)value);

    /// <summary>
    /// Gets a new <see cref="Duration"/> instance with the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    /// A number of hours.
    /// </param>
    /// <returns>A new <see cref="Duration"/> instance.</returns>
    public static Duration FromHoursInteger<T>(T value) where T : INumber<T>
        => FromHours(value.Create<T, decimal>());

    /// <summary>
    /// Gets a new <see cref="Duration"/> instance with the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    /// A number of hours.
    /// </param>
    /// <returns>A new <see cref="Duration"/> instance.</returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="value"/> is <see cref="IFloatingPoint{TSelf}.NaN"/>.
    /// </exception>
    public static Duration FromHoursFloatingPoint<T>(T value) where T : IFloatingPoint<T>
        => FromNanosecondsFloatingPoint(value * T.Create(NanosecondsPerHour));

    /// <summary>
    /// Gets a new <see cref="Duration"/> instance with the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    /// A number of microseconds.
    /// </param>
    /// <returns>A new <see cref="Duration"/> instance.</returns>
    public static Duration FromMicroseconds(decimal value)
        => FromNanoseconds(value * NanosecondsPerMicrosecond);

    /// <summary>
    /// Gets a new <see cref="Duration"/> instance with the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    /// A number of microseconds.
    /// </param>
    /// <returns>A new <see cref="Duration"/> instance.</returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="value"/> is <see cref="double.NaN"/>.
    /// </exception>
    public static Duration FromMicroseconds(double value)
        => FromNanoseconds(value * NanosecondsPerMicrosecond);

    /// <summary>
    /// Gets a new <see cref="Duration"/> instance with the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    /// A number of microseconds.
    /// </param>
    /// <returns>A new <see cref="Duration"/> instance.</returns>
    public static Duration FromMicroseconds(long value)
        => FromMicroseconds((decimal)value);

    /// <summary>
    /// Gets a new <see cref="Duration"/> instance with the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    /// A number of minutes.
    /// </param>
    /// <returns>A new <see cref="Duration"/> instance.</returns>
    public static Duration FromMicrosecondsInteger<T>(T value) where T : INumber<T>
        => FromMicroseconds(value.Create<T, decimal>());

    /// <summary>
    /// Gets a new <see cref="Duration"/> instance with the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    /// A number of minutes.
    /// </param>
    /// <returns>A new <see cref="Duration"/> instance.</returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="value"/> is <see cref="IFloatingPoint{TSelf}.NaN"/>.
    /// </exception>
    public static Duration FromMicrosecondsFloatingPoint<T>(T value) where T : IFloatingPoint<T>
        => FromNanosecondsFloatingPoint(value * T.Create(NanosecondsPerMicrosecond));

    /// <summary>
    /// Gets a new <see cref="Duration"/> instance with the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    /// A number of milliseconds.
    /// </param>
    /// <returns>A new <see cref="Duration"/> instance.</returns>
    public static Duration FromMilliseconds(decimal value)
        => FromNanoseconds(value * NanosecondsPerMillisecond);

    /// <summary>
    /// Gets a new <see cref="Duration"/> instance with the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    /// A number of milliseconds.
    /// </param>
    /// <returns>A new <see cref="Duration"/> instance.</returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="value"/> is <see cref="double.NaN"/>.
    /// </exception>
    public static Duration FromMilliseconds(double value)
        => FromNanoseconds(value * NanosecondsPerMillisecond);

    /// <summary>
    /// Gets a new <see cref="Duration"/> instance with the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    /// A number of milliseconds.
    /// </param>
    /// <returns>A new <see cref="Duration"/> instance.</returns>
    public static Duration FromMilliseconds(long value)
        => FromMilliseconds((decimal)value);

    /// <summary>
    /// Gets a new <see cref="Duration"/> instance with the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    /// A number of minutes.
    /// </param>
    /// <returns>A new <see cref="Duration"/> instance.</returns>
    public static Duration FromMillisecondsInteger<T>(T value) where T : INumber<T>
        => FromMilliseconds(value.Create<T, decimal>());

    /// <summary>
    /// Gets a new <see cref="Duration"/> instance with the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    /// A number of minutes.
    /// </param>
    /// <returns>A new <see cref="Duration"/> instance.</returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="value"/> is <see cref="IFloatingPoint{TSelf}.NaN"/>.
    /// </exception>
    public static Duration FromMillisecondsFloatingPoint<T>(T value) where T : IFloatingPoint<T>
        => FromNanosecondsFloatingPoint(value * T.Create(NanosecondsPerMillisecond));

    /// <summary>
    /// Gets a new <see cref="Duration"/> instance with the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    /// A number of minutes.
    /// </param>
    /// <returns>A new <see cref="Duration"/> instance.</returns>
    public static Duration FromMinutes(decimal value)
        => FromNanoseconds(value * NanosecondsPerMinute);

    /// <summary>
    /// Gets a new <see cref="Duration"/> instance with the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    /// A number of minutes.
    /// </param>
    /// <returns>A new <see cref="Duration"/> instance.</returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="value"/> is <see cref="double.NaN"/>.
    /// </exception>
    public static Duration FromMinutes(double value)
        => FromNanoseconds(value * NanosecondsPerMinute);

    /// <summary>
    /// Gets a new <see cref="Duration"/> instance with the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    /// A number of minutes.
    /// </param>
    /// <returns>A new <see cref="Duration"/> instance.</returns>
    public static Duration FromMinutes(long value)
        => FromMinutes((decimal)value);

    /// <summary>
    /// Gets a new <see cref="Duration"/> instance with the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    /// A number of minutes.
    /// </param>
    /// <returns>A new <see cref="Duration"/> instance.</returns>
    public static Duration FromMinutesInteger<T>(T value) where T : INumber<T>
        => FromMinutes(value.Create<T, decimal>());

    /// <summary>
    /// Gets a new <see cref="Duration"/> instance with the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    /// A number of minutes.
    /// </param>
    /// <returns>A new <see cref="Duration"/> instance.</returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="value"/> is <see cref="IFloatingPoint{TSelf}.NaN"/>.
    /// </exception>
    public static Duration FromMinutesFloatingPoint<T>(T value) where T : IFloatingPoint<T>
        => FromNanosecondsFloatingPoint(value * T.Create(NanosecondsPerMinute));

    /// <summary>
    /// Gets a new <see cref="Duration"/> instance with the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    /// A number of nanoseconds.
    /// </param>
    /// <returns>A new <see cref="Duration"/> instance.</returns>
    public static Duration FromNanoseconds(decimal value)
    {
        if (value == 0)
        {
            return Zero;
        }
        var isNegative = value < 0;
        value = Math.Abs(value);

        var years = Math.Floor(value / NanosecondsPerYear);
        value -= years * NanosecondsPerYear;

        var d = FromYears(years);

        if (value > 0)
        {
            var ns = (ulong)Math.Floor(value);
            value -= ns;

            d += new Duration(false, false, null, 0, ns, 0, null);

            if (value > 0)
            {
                d += FromYoctoseconds(value * YoctosecondsPerNanosecond);
            }
        }

        if (isNegative)
        {
            d = d.Negate();
        }
        return d;
    }

    /// <summary>
    /// Gets a new <see cref="Duration"/> instance with the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    /// A number of nanoseconds.
    /// </param>
    /// <returns>A new <see cref="Duration"/> instance.</returns>
    /// <exception cref="ArgumentException"><paramref name="value"/> is <see
    /// cref="double.NaN"/>.</exception>
    public static Duration FromNanoseconds(double value)
    {
        if (double.IsNaN(value))
        {
            throw new ArgumentException($"{nameof(value)} was NaN.");
        }
        if (value.IsNearlyZero())
        {
            return Zero;
        }
        if (double.IsPositiveInfinity(value))
        {
            return PositiveInfinity;
        }
        if (double.IsNegativeInfinity(value))
        {
            return NegativeInfinity;
        }
        var isNegative = value < 0;
        value = Math.Abs(value);

        var years = Math.Floor(value / NanosecondsPerYear);
        value -= years * NanosecondsPerYear;

        var d = FromYears(years);

        if (value > 0)
        {
            var ns = (ulong)Math.Floor(value);
            value -= ns;

            d += new Duration(false, false, null, 0, ns, 0, null);

            if (value > 0)
            {
                d += FromYoctoseconds(value * YoctosecondsPerNanosecond);
            }
        }

        if (isNegative)
        {
            d = d.Negate();
        }
        return d;
    }

    /// <summary>
    /// Gets a new <see cref="Duration"/> instance with the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    /// A number of nanoseconds.
    /// </param>
    /// <returns>A new <see cref="Duration"/> instance.</returns>
    public static Duration FromNanoseconds(long value)
    {
        if (value == 0)
        {
            return Zero;
        }
        var isNegative = value < 0;
        value = Math.Abs(value);

        var years = value / NanosecondsPerYear;
        value -= years * NanosecondsPerYear;

        var d = FromYearsInteger(years);

        if (value > 0)
        {
            d += new Duration(
                false,
                false,
                null,
                0,
                (uint)value,
                0,
                null);
        }

        if (isNegative)
        {
            d = d.Negate();
        }
        return d;
    }

    /// <summary>
    /// Gets a new <see cref="Duration"/> instance with the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    /// A number of nanoseconds.
    /// </param>
    /// <returns>A new <see cref="Duration"/> instance.</returns>
    public static Duration FromNanosecondsInteger<T>(T value) where T : INumber<T>
    {
        if (value == T.Zero)
        {
            return Zero;
        }
        var isNegative = value < T.Zero;
        value = T.Abs(value);

        var nanosecondsPerYear = T.Create(NanosecondsPerYear);
        var years = value / nanosecondsPerYear;
        value -= years * nanosecondsPerYear;

        var d = FromYearsInteger(years);

        if (value > T.Zero)
        {
            d += new Duration(
                false,
                false,
                null,
                0,
                value.Create<T, ulong>(),
                0,
                null);
        }

        if (isNegative)
        {
            d = d.Negate();
        }
        return d;
    }

    /// <summary>
    /// Gets a new <see cref="Duration"/> instance with the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    /// A number of nanoseconds.
    /// </param>
    /// <returns>A new <see cref="Duration"/> instance.</returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="value"/> is <see cref="IFloatingPoint{TSelf}.NaN"/>.
    /// </exception>
    public static Duration FromNanosecondsFloatingPoint<T>(T value) where T : IFloatingPoint<T>
    {
        if (T.IsNaN(value))
        {
            throw new ArgumentException($"{nameof(value)} was NaN.");
        }
        if (value.IsNearlyZero())
        {
            return Zero;
        }
        if (T.IsPositiveInfinity(value))
        {
            return PositiveInfinity;
        }
        if (T.IsNegativeInfinity(value))
        {
            return NegativeInfinity;
        }
        var isNegative = value < T.Zero;
        value = T.Abs(value);

        var nanosecondsPerYear = T.Create(NanosecondsPerYear);
        var years = T.Floor(value / nanosecondsPerYear);
        value -= years * nanosecondsPerYear;

        var d = FromYearsFloatingPoint(years);

        if (value > T.Zero)
        {
            var ns = T.Floor(value);
            value -= ns;

            d += new Duration(
                false,
                false,
                null,
                0,
                ns.Create<T, ulong>(),
                0,
                null);

            if (value > T.Zero)
            {
                d += FromYoctosecondsFloatingPoint(value * T.Create(YoctosecondsPerNanosecond));
            }
        }

        if (isNegative)
        {
            d = d.Negate();
        }
        return d;
    }

    /// <summary>
    /// Gets a new <see cref="Duration"/> instance with the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    /// A number of picoseconds.
    /// </param>
    /// <returns>A new <see cref="Duration"/> instance.</returns>
    public static Duration FromPicoseconds(decimal value)
        => FromNanoseconds(value / PicosecondsPerNanosecond);

    /// <summary>
    /// Gets a new <see cref="Duration"/> instance with the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    /// A number of picoseconds.
    /// </param>
    /// <returns>A new <see cref="Duration"/> instance.</returns>
    /// <exception cref="ArgumentException"><paramref name="value"/> is <see
    /// cref="double.NaN"/>.</exception>
    public static Duration FromPicoseconds(double value)
        => FromNanoseconds(value / PicosecondsPerNanosecond);

    /// <summary>
    /// Gets a new <see cref="Duration"/> instance with the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    /// A number of picoseconds.
    /// </param>
    /// <returns>A new <see cref="Duration"/> instance.</returns>
    public static Duration FromPicoseconds(long value)
    {
        var remainder = value % PicosecondsPerNanosecond;
        var d = FromNanoseconds(value / PicosecondsPerNanosecond);
        return d + FromYoctoseconds(remainder * YoctosecondsPerPicosecond);
    }

    /// <summary>
    /// Gets a new <see cref="Duration"/> instance with the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    /// A number of picoseconds.
    /// </param>
    /// <returns>A new <see cref="Duration"/> instance.</returns>
    public static Duration FromPicosecondsInteger<T>(T value) where T : INumber<T>
    {
        var picosecondsPerNanosecond = T.Create(PicosecondsPerNanosecond);
        var remainder = value % picosecondsPerNanosecond;
        var d = FromNanosecondsInteger(value / picosecondsPerNanosecond);
        return d + FromYoctosecondsInteger(remainder * T.Create(YoctosecondsPerPicosecond));
    }

    /// <summary>
    /// Gets a new <see cref="Duration"/> instance with the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    /// A number of picoseconds.
    /// </param>
    /// <returns>A new <see cref="Duration"/> instance.</returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="value"/> is <see cref="IFloatingPoint{TSelf}.NaN"/>.
    /// </exception>
    public static Duration FromPicosecondsFloatingPoint<T>(T value) where T : IFloatingPoint<T>
        => FromNanosecondsFloatingPoint(value / T.Create(PicosecondsPerNanosecond));

    /// <summary>
    /// Gets a new <see cref="Duration"/> instance based on the proportions of a local
    /// day.
    /// </summary>
    /// <param name="proportionOfDay">
    /// <para>The proportion of the current local day.
    /// </para>
    /// <para>
    /// Negative values are treated as zero.
    /// </para>
    /// </param>
    /// <param name="dayDuration">The duration of the current local day.</param>
    /// <returns>A new <see cref="Duration"/> instance.</returns>
    public static Duration FromProportionOfDay(decimal proportionOfDay, Duration dayDuration)
        => (proportionOfDay > 0 && !dayDuration.IsZero) ? dayDuration * proportionOfDay : Zero;

    /// <summary>
    /// Gets a new <see cref="Duration"/> instance based on the proportions of a local
    /// day.
    /// </summary>
    /// <param name="proportionOfDay">
    /// <para>The proportion of the current local day.
    /// </para>
    /// <para>
    /// Negative values are treated as zero.
    /// </para>
    /// </param>
    /// <param name="dayDuration">The duration of the current local day.</param>
    /// <returns>A new <see cref="Duration"/> instance.</returns>
    public static Duration FromProportionOfDay(double proportionOfDay, Duration dayDuration)
        => (proportionOfDay > 0 && !dayDuration.IsZero) ? dayDuration * proportionOfDay : Zero;

    /// <summary>
    /// Gets a new <see cref="Duration"/> instance based on the proportions of a local
    /// day.
    /// </summary>
    /// <param name="proportionOfDay">
    /// <para>The proportion of the current local day.
    /// </para>
    /// <para>
    /// Negative values are treated as zero.
    /// </para>
    /// </param>
    /// <param name="dayDuration">The duration of the current local day.</param>
    /// <returns>A new <see cref="Duration"/> instance.</returns>
    public static Duration FromProportionOfDay<T>(T proportionOfDay, Duration dayDuration) where T : IFloatingPoint<T>
        => (proportionOfDay > T.Zero && !dayDuration.IsZero) ? dayDuration.MultiplyFloatingPoint(proportionOfDay) : Zero;

    /// <summary>
    /// Gets a new <see cref="Duration"/> instance based on the proportions of a local
    /// year.
    /// </summary>
    /// <param name="proportionOfYear">
    /// <para>
    /// The proportion of the current local year.
    /// </para>
    /// <para>
    /// Negative values are treated as zero.
    /// </para>
    /// </param>
    /// <param name="yearDuration">The duration of the current local year.</param>
    /// <returns>A new <see cref="Duration"/> instance.</returns>
    public static Duration FromProportionOfYear(decimal proportionOfYear, Duration yearDuration)
        => (proportionOfYear > 0 && !yearDuration.IsZero) ? yearDuration * proportionOfYear : Zero;

    /// <summary>
    /// Gets a new <see cref="Duration"/> instance based on the proportions of a local
    /// year.
    /// </summary>
    /// <param name="proportionOfYear">
    /// <para>
    /// The proportion of the current local year.
    /// </para>
    /// <para>
    /// Negative values are treated as zero.
    /// </para>
    /// </param>
    /// <param name="yearDuration">The duration of the current local year.</param>
    /// <returns>A new <see cref="Duration"/> instance.</returns>
    public static Duration FromProportionOfYear(double proportionOfYear, Duration yearDuration)
        => (proportionOfYear > 0 && !yearDuration.IsZero) ? yearDuration * proportionOfYear : Zero;

    /// <summary>
    /// Gets a new <see cref="Duration"/> instance based on the proportions of a local
    /// year.
    /// </summary>
    /// <param name="proportionOfYear">
    /// <para>
    /// The proportion of the current local year.
    /// </para>
    /// <para>
    /// Negative values are treated as zero.
    /// </para>
    /// </param>
    /// <param name="yearDuration">The duration of the current local year.</param>
    /// <returns>A new <see cref="Duration"/> instance.</returns>
    public static Duration FromProportionOfYear<T>(T proportionOfYear, Duration yearDuration) where T : IFloatingPoint<T>
        => (proportionOfYear > T.Zero && !yearDuration.IsZero) ? yearDuration.MultiplyFloatingPoint(proportionOfYear) : Zero;

    /// <summary>
    /// Converts a relative duration to an absolute duration, given the absolute durations of
    /// the local year and day.
    /// </summary>
    /// <param name="relativeDuration">The <see cref="RelativeDuration"/> to convert.</param>
    /// <param name="yearDuration">The duration of the local year.</param>
    /// <param name="dayDuration">The duration of the local day.</param>
    /// <returns>A <see cref="Duration"/> equivalent to the given <see
    /// cref="RelativeDuration"/>.</returns>
    public static Duration FromRelativeDuration(RelativeDuration relativeDuration, Duration yearDuration, Duration dayDuration)
        => relativeDuration.ToUniversalDuration(yearDuration, dayDuration);

    /// <summary>
    /// Gets a new <see cref="Duration"/> instance based on the proportions of a local
    /// year and/or day.
    /// </summary>
    /// <param name="proportionOfYear">
    /// <para>
    /// The proportion of the current local year.
    /// </para>
    /// <para>
    /// Negative values are treated as zero.
    /// </para>
    /// </param>
    /// <param name="yearDuration">The duration of the current local year, in seconds.</param>
    /// <param name="proportionOfDay">
    /// <para>The proportion of the current local day.
    /// </para>
    /// <para>
    /// Negative values are treated as zero.
    /// </para>
    /// </param>
    /// <param name="dayDuration">The duration of the current local day, in seconds.</param>
    /// <returns>A new <see cref="Duration"/> instance.</returns>
    public static Duration FromProportions(decimal proportionOfYear, Duration yearDuration, decimal proportionOfDay, Duration dayDuration)
    {
        var duration = Zero;

        if (proportionOfYear > 0 && !yearDuration.IsZero)
        {
            duration += yearDuration * proportionOfYear;
        }

        if (proportionOfDay > 0 && !dayDuration.IsZero)
        {
            duration += dayDuration * proportionOfDay;
        }

        return duration;
    }

    /// <summary>
    /// Gets a new <see cref="Duration"/> instance based on the proportions of a local
    /// year and/or day.
    /// </summary>
    /// <param name="proportionOfYear">
    /// <para>
    /// The proportion of the current local year.
    /// </para>
    /// <para>
    /// Negative values are treated as zero.
    /// </para>
    /// </param>
    /// <param name="yearDuration">The duration of the current local year, in seconds.</param>
    /// <param name="proportionOfDay">
    /// <para>The proportion of the current local day.
    /// </para>
    /// <para>
    /// Negative values are treated as zero.
    /// </para>
    /// </param>
    /// <param name="dayDuration">The duration of the current local day, in seconds.</param>
    /// <returns>A new <see cref="Duration"/> instance.</returns>
    public static Duration FromProportions(double proportionOfYear, Duration yearDuration, double proportionOfDay, Duration dayDuration)
    {
        var duration = Zero;

        if (proportionOfYear > 0 && !yearDuration.IsZero)
        {
            duration += yearDuration * proportionOfYear;
        }

        if (proportionOfDay > 0 && !dayDuration.IsZero)
        {
            duration += dayDuration * proportionOfDay;
        }

        return duration;
    }

    /// <summary>
    /// Gets a new <see cref="Duration"/> instance based on the proportions of a local
    /// year and/or day.
    /// </summary>
    /// <param name="proportionOfYear">
    /// <para>
    /// The proportion of the current local year.
    /// </para>
    /// <para>
    /// Negative values are treated as zero.
    /// </para>
    /// </param>
    /// <param name="yearDuration">The duration of the current local year, in seconds.</param>
    /// <param name="proportionOfDay">
    /// <para>The proportion of the current local day.
    /// </para>
    /// <para>
    /// Negative values are treated as zero.
    /// </para>
    /// </param>
    /// <param name="dayDuration">The duration of the current local day, in seconds.</param>
    /// <returns>A new <see cref="Duration"/> instance.</returns>
    public static Duration FromProportions<T>(T proportionOfYear, Duration yearDuration, T proportionOfDay, Duration dayDuration)
        where T : IFloatingPoint<T>
    {
        var duration = Zero;

        if (proportionOfYear > T.Zero && !yearDuration.IsZero)
        {
            duration += yearDuration.MultiplyFloatingPoint(proportionOfYear);
        }

        if (proportionOfDay > T.Zero && !dayDuration.IsZero)
        {
            duration += dayDuration.MultiplyFloatingPoint(proportionOfDay);
        }

        return duration;
    }

    /// <summary>
    /// Gets a new <see cref="Duration"/> instance with the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    /// An amount of Planck time.
    /// </param>
    /// <returns>A new <see cref="Duration"/> instance.</returns>
    public static Duration FromPlanckTime(BigInteger value)
    {
        if (value == 0)
        {
            return Zero;
        }
        var isNegative = value < 0;
        value = BigInteger.Abs(value);

        var ys = 0UL;
        var ns = 0UL;
        var y = 0U;
        BigInteger? aes = null;
        if (value >= PlanckTimePerYoctosecond)
        {
            var pys = value / PlanckTimePerYoctosecond;
            value %= PlanckTimePerYoctosecond;

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

        return new Duration(
            isNegative,
            false,
            value,
            ys,
            ns,
            y,
            aes);
    }

    /// <summary>
    /// Gets a new <see cref="Duration"/> instance with the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    /// An amount of Planck time.
    /// </param>
    /// <returns>A new <see cref="Duration"/> instance.</returns>
    public static Duration FromPlanckTime<T>(T value) where T : INumber<T> => FromPlanckTime(
        BigInteger.Parse(value
            .ToString("F0", System.Globalization.CultureInfo.InvariantCulture)));

    /// <summary>
    /// Gets a new <see cref="Duration"/> instance with the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    /// A number of seconds.
    /// </param>
    /// <returns>A new <see cref="Duration"/> instance.</returns>
    public static Duration FromSeconds(decimal value)
        => FromNanoseconds(value * NanosecondsPerSecond);

    /// <summary>
    /// Gets a new <see cref="Duration"/> instance with the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    /// A number of seconds.
    /// </param>
    /// <returns>A new <see cref="Duration"/> instance.</returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="value"/> is <see cref="double.NaN"/>.
    /// </exception>
    public static Duration FromSeconds(double value)
        => FromNanoseconds(value * NanosecondsPerSecond);

    /// <summary>
    /// Gets a new <see cref="Duration"/> instance with the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    /// A number of seconds.
    /// </param>
    /// <returns>A new <see cref="Duration"/> instance.</returns>
    public static Duration FromSeconds(long value)
        => FromSeconds((decimal)value);

    /// <summary>
    /// Gets a new <see cref="Duration"/> instance with the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    /// A number of minutes.
    /// </param>
    /// <returns>A new <see cref="Duration"/> instance.</returns>
    public static Duration FromSecondsInteger<T>(T value) where T : INumber<T>
        => FromSeconds(value.Create<T, decimal>());

    /// <summary>
    /// Gets a new <see cref="Duration"/> instance with the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    /// A number of minutes.
    /// </param>
    /// <returns>A new <see cref="Duration"/> instance.</returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="value"/> is <see cref="IFloatingPoint{TSelf}.NaN"/>.
    /// </exception>
    public static Duration FromSecondsFloatingPoint<T>(T value) where T : IFloatingPoint<T>
        => FromNanosecondsFloatingPoint(value * T.Create(NanosecondsPerSecond));

    /// <summary>
    /// Converts the given <see cref="TimeSpan"/> value to a <see cref="Duration"/> value.
    /// </summary>
    /// <param name="timeSpan">A <see cref="TimeSpan"/> value to convert.</param>
    /// <returns>An equivalent <see cref="Duration"/> value.</returns>
    /// <remarks>
    /// <para>
    /// The current aeon sequence of our universe is presumed to be 99731, based on current best
    /// estimates (1.3799e10±2.1e7 years).
    /// </para>
    /// <para>
    /// The <see cref="TimeSpan"/> value is converted to a <see cref="Duration"/> instance by
    /// presuming that the current aeon begins at the same moment as <see
    /// cref="TimeSpan.Ticks"/> begins counting. The <see cref="TimeSpan.Ticks"/> of the <see
    /// cref="TimeSpan"/> value are converted into the timekeeping system of <see
    /// cref="Duration"/> based on that assumption.
    /// </para>
    /// </remarks>
    public static Duration FromTimeSpan(TimeSpan timeSpan)
    {
        var ticks = timeSpan.Ticks;
        if (ticks == 0)
        {
            return Zero;
        }
        var isNegative = ticks < 0;
        ticks = Math.Abs(ticks);
        var ns = (ulong)ticks * NanosecondsPerTick;
        return new Duration(isNegative, nanoseconds: ns, aeons: _DefaultAeons);
    }

    /// <summary>
    /// Gets a new <see cref="Duration"/> instance with the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    /// A number of years.
    /// </param>
    /// <returns>A new <see cref="Duration"/> instance.</returns>
    public static Duration FromYears(decimal value)
    {
        if (value == 0)
        {
            return Zero;
        }
        var isNegative = value < 0;
        value = Math.Abs(value);

        var aeons = Math.Floor(value / YearsPerAeon);
        value -= aeons * YearsPerAeon;

        var d = FromAeons(aeons);

        if (value > 0)
        {
            var years = (uint)Math.Floor(value);
            value -= years;

            d += new Duration(false, false, 0, 0, 0, years, null);

            if (value > 0)
            {
                d += FromSeconds(value * SecondsPerYear);
            }
        }

        if (isNegative)
        {
            d = d.Negate();
        }
        return d;
    }

    /// <summary>
    /// Gets a new <see cref="Duration"/> instance with the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    /// A number of years.
    /// </param>
    /// <returns>A new <see cref="Duration"/> instance.</returns>
    /// <exception cref="ArgumentException"><paramref name="value"/> is <see
    /// cref="double.NaN"/>.</exception>
    public static Duration FromYears(double value)
    {
        if (double.IsNaN(value))
        {
            throw new ArgumentException($"{nameof(value)} was NaN.");
        }
        if (value.IsNearlyZero())
        {
            return Zero;
        }
        if (double.IsPositiveInfinity(value))
        {
            return PositiveInfinity;
        }
        if (double.IsNegativeInfinity(value))
        {
            return NegativeInfinity;
        }
        var isNegative = value < 0;
        value = Math.Abs(value);

        var aeons = Math.Floor(value / YearsPerAeon);
        value -= aeons * YearsPerAeon;

        var d = FromAeons(aeons);

        if (value > 0)
        {
            var years = (uint)Math.Floor(value);
            value -= years;

            d += new Duration(false, false, 0, 0, 0, years, null);

            if (value > 0)
            {
                d += FromSeconds(value * SecondsPerYear);
            }
        }

        if (isNegative)
        {
            d = d.Negate();
        }
        return d;
    }

    /// <summary>
    /// Gets a new <see cref="Duration"/> instance with the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    /// A number of years.
    /// </param>
    /// <returns>A new <see cref="Duration"/> instance.</returns>
    public static Duration FromYears(long value)
    {
        if (value == 0)
        {
            return Zero;
        }
        var isNegative = value < 0;
        value = Math.Abs(value);

        var aeons = value / YearsPerAeon;
        value -= aeons * YearsPerAeon;

        var d = FromAeonsInteger(aeons);

        if (value > 0)
        {
            d += new Duration(
                false,
                false,
                null,
                0,
                0,
                (uint)value,
                null);
        }

        if (isNegative)
        {
            d = d.Negate();
        }
        return d;
    }

    /// <summary>
    /// Gets a new <see cref="Duration"/> instance with the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    /// A number of years.
    /// </param>
    /// <returns>A new <see cref="Duration"/> instance.</returns>
    public static Duration FromYearsInteger<T>(T value) where T : INumber<T>
    {
        if (value == T.Zero)
        {
            return Zero;
        }
        var isNegative = value < T.Zero;
        value = T.Abs(value);

        var yearsPerAeon = T.Create(YearsPerAeon);
        var aeons = value / yearsPerAeon;
        value -= aeons * yearsPerAeon;

        var d = FromAeonsInteger(aeons);

        if (value > T.Zero)
        {
            d += new Duration(
                false,
                false,
                null,
                0,
                0,
                value.Create<T, uint>(),
                null);
        }

        if (isNegative)
        {
            d = d.Negate();
        }
        return d;
    }

    /// <summary>
    /// Gets a new <see cref="Duration"/> instance with the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    /// A number of years.
    /// </param>
    /// <returns>A new <see cref="Duration"/> instance.</returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="value"/> is <see cref="IFloatingPoint{TSelf}.NaN"/>.
    /// </exception>
    public static Duration FromYearsFloatingPoint<T>(T value) where T : IFloatingPoint<T>
    {
        if (T.IsNaN(value))
        {
            throw new ArgumentException($"{nameof(value)} was NaN.");
        }
        if (value.IsNearlyZero())
        {
            return Zero;
        }
        if (T.IsPositiveInfinity(value))
        {
            return PositiveInfinity;
        }
        if (T.IsNegativeInfinity(value))
        {
            return NegativeInfinity;
        }
        var isNegative = value < T.Zero;
        value = T.Abs(value);

        var yearsPerAeon = T.Create(YearsPerAeon);
        var aeons = T.Floor(value / yearsPerAeon);
        value -= aeons * yearsPerAeon;

        var d = FromAeonsFloatingPoint(aeons);

        if (value > T.Zero)
        {
            var years = T.Floor(value);
            value -= years;

            d += new Duration(
                false,
                false,
                null,
                0,
                0,
                years.Create<T, uint>(),
                null);

            if (value > T.Zero)
            {
                d += FromSecondsFloatingPoint(value * T.Create(SecondsPerYear));
            }
        }

        if (isNegative)
        {
            d = d.Negate();
        }
        return d;
    }

    /// <summary>
    /// Gets a new <see cref="Duration"/> instance with the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    /// A number of yoctoseconds.
    /// </param>
    /// <returns>A new <see cref="Duration"/> instance.</returns>
    public static Duration FromYoctoseconds(decimal value)
    {
        if (value == 0)
        {
            return Zero;
        }
        var isNegative = value < 0;
        value = Math.Abs(value);

        var ns = Math.Floor(value / YoctosecondsPerNanosecond);
        value -= ns * YoctosecondsPerNanosecond;

        var d = FromNanoseconds(ns);

        if (value > 0)
        {
            var ys = (uint)Math.Floor(value);
            value -= ys;

            d += new Duration(
                false,
                false,
                (BigInteger)(value * PlanckTimePerYoctosecondDecimal),
                ys,
                0,
                0,
                null);
        }

        if (isNegative)
        {
            d = d.Negate();
        }
        return d;
    }

    /// <summary>
    /// Gets a new <see cref="Duration"/> instance with the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    /// A number of yoctoseconds.
    /// </param>
    /// <returns>A new <see cref="Duration"/> instance.</returns>
    /// <exception cref="ArgumentException"><paramref name="value"/> is <see
    /// cref="double.NaN"/>.</exception>
    public static Duration FromYoctoseconds(double value)
    {
        if (double.IsNaN(value))
        {
            throw new ArgumentException($"{nameof(value)} was NaN.");
        }
        if (value.IsNearlyZero())
        {
            return Zero;
        }
        if (double.IsPositiveInfinity(value))
        {
            return PositiveInfinity;
        }
        if (double.IsNegativeInfinity(value))
        {
            return NegativeInfinity;
        }
        var isNegative = value < 0;
        value = Math.Abs(value);

        var ns = Math.Floor(value / YoctosecondsPerNanosecond);
        value -= ns * YoctosecondsPerNanosecond;

        var d = FromNanoseconds(ns);

        if (value > 0)
        {
            var ys = (uint)Math.Floor(value);
            value -= ys;

            d += new Duration(
                false,
                false,
                (BigInteger)(value * PlanckTimePerYoctosecondDouble),
                ys,
                0,
                0,
                null);
        }

        if (isNegative)
        {
            d = d.Negate();
        }
        return d;
    }

    /// <summary>
    /// Gets a new <see cref="Duration"/> instance with the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    /// A number of yoctoseconds.
    /// </param>
    /// <returns>A new <see cref="Duration"/> instance.</returns>
    public static Duration FromYoctoseconds(long value)
    {
        if (value == 0)
        {
            return Zero;
        }
        var isNegative = value < 0;
        value = Math.Abs(value);

        var ns = value / YoctosecondsPerNanosecond;
        value -= ns * YoctosecondsPerNanosecond;

        var d = FromNanosecondsInteger(ns);

        if (value > 0)
        {
            d += new Duration(
                false,
                false,
                null,
                (uint)value,
                0,
                0,
                null);
        }

        if (isNegative)
        {
            d = d.Negate();
        }
        return d;
    }

    /// <summary>
    /// Gets a new <see cref="Duration"/> instance with the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    /// A number of yoctoseconds.
    /// </param>
    /// <returns>A new <see cref="Duration"/> instance.</returns>
    public static Duration FromYoctosecondsInteger<T>(T value) where T : INumber<T>
    {
        if (value == T.Zero)
        {
            return Zero;
        }
        var isNegative = value < T.Zero;
        value = T.Abs(value);

        var yoctosecondsPerNanosecond = T.Create(YoctosecondsPerNanosecond);
        var ns = value / yoctosecondsPerNanosecond;
        value -= ns * yoctosecondsPerNanosecond;

        var d = FromNanosecondsInteger(ns);

        if (value > T.Zero)
        {
            d += new Duration(
                false,
                false,
                null,
                value.Create<T, uint>(),
                0,
                0,
                null);
        }

        if (isNegative)
        {
            d = d.Negate();
        }
        return d;
    }

    /// <summary>
    /// Gets a new <see cref="Duration"/> instance with the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    /// A number of yoctoseconds.
    /// </param>
    /// <returns>A new <see cref="Duration"/> instance.</returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="value"/> is <see cref="IFloatingPoint{TSelf}.NaN"/>.
    /// </exception>
    public static Duration FromYoctosecondsFloatingPoint<T>(T value) where T : IFloatingPoint<T>
    {
        if (T.IsNaN(value))
        {
            throw new ArgumentException($"{nameof(value)} was NaN.");
        }
        if (value.IsNearlyZero())
        {
            return Zero;
        }
        if (T.IsPositiveInfinity(value))
        {
            return PositiveInfinity;
        }
        if (T.IsNegativeInfinity(value))
        {
            return NegativeInfinity;
        }
        var isNegative = value < T.Zero;
        value = T.Abs(value);

        var yoctosecondsPerNanosecond = T.Create(YoctosecondsPerNanosecond);
        var ns = T.Floor(value / yoctosecondsPerNanosecond);
        value -= ns * yoctosecondsPerNanosecond;

        var d = FromNanosecondsFloatingPoint(ns);

        if (value > T.Zero)
        {
            var ys = T.Floor(value);
            value -= ys;

            d += new Duration(
                false,
                false,
                (BigInteger)(value.Create<T, decimal>() * PlanckTimePerYoctosecondDecimal),
                ys.Create<T, uint>(),
                0,
                0,
                null);
        }

        if (isNegative)
        {
            d = d.Negate();
        }
        return d;
    }

    /// <summary>
    /// Gets a new <see cref="Duration"/> instance with the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    /// A number of zeptoseconds.
    /// </param>
    /// <returns>A new <see cref="Duration"/> instance.</returns>
    public static Duration FromZeptoseconds(decimal value)
        => FromNanoseconds(value / ZeptosecondsPerNanosecond);

    /// <summary>
    /// Gets a new <see cref="Duration"/> instance with the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    /// A number of zeptoseconds.
    /// </param>
    /// <returns>A new <see cref="Duration"/> instance.</returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="value"/> is <see cref="double.NaN"/>.
    /// </exception>
    public static Duration FromZeptoseconds(double value)
        => FromNanoseconds(value / ZeptosecondsPerNanosecond);

    /// <summary>
    /// Gets a new <see cref="Duration"/> instance with the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    /// A number of zeptoseconds.
    /// </param>
    /// <returns>A new <see cref="Duration"/> instance.</returns>
    public static Duration FromZeptoseconds(long value)
    {
        var remainder = value % ZeptosecondsPerNanosecond;
        var d = FromNanoseconds(value / ZeptosecondsPerNanosecond);
        return d + FromYoctoseconds(remainder * YoctosecondsPerZeptosecond);
    }

    /// <summary>
    /// Gets a new <see cref="Duration"/> instance with the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    /// A number of zeptoseconds.
    /// </param>
    /// <returns>A new <see cref="Duration"/> instance.</returns>
    public static Duration FromZeptosecondsInteger<T>(T value) where T : INumber<T>
    {
        var zeptosecondsPerNanosecond = T.Create(ZeptosecondsPerNanosecond);
        var remainder = value % zeptosecondsPerNanosecond;
        var d = FromNanosecondsInteger(value / zeptosecondsPerNanosecond);
        return d + FromYoctosecondsInteger(remainder * T.Create(YoctosecondsPerZeptosecond));
    }

    /// <summary>
    /// Gets a new <see cref="Duration"/> instance with the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    /// A number of zeptoseconds.
    /// </param>
    /// <returns>A new <see cref="Duration"/> instance.</returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="value"/> is <see cref="IFloatingPoint{TSelf}.NaN"/>.
    /// </exception>
    public static Duration FromZeptosecondsFloatingPoint<T>(T value) where T : IFloatingPoint<T>
        => FromNanosecondsFloatingPoint(value / T.Create(ZeptosecondsPerNanosecond));

    /// <summary>
    /// Converts this instance to a <see cref="DateTime"/> value.
    /// </summary>
    /// <returns>A <see cref="DateTime"/> value equivalent to this instance.</returns>
    /// <remarks>
    /// <para>
    /// This instance is converted to a <see cref="DateTime"/> value by presuming that the
    /// current aeon begins at the same moment as <see cref="DateTime.Ticks"/> begins counting.
    /// A <see cref="DateTime.Ticks"/> value for the <see cref="DateTime"/> value is generated
    /// from the timekeeping system of <see cref="Duration"/> based on that assumption.
    /// </para>
    /// <para>
    /// The returned <see cref="DateTime"/> will have a <see cref="DateTime.Kind"/> of <see
    /// cref="DateTimeKind.Utc"/>.
    /// </para>
    /// <para>
    /// For more accurate <see cref="DateTime"/> conversion, consider using <see
    /// cref="CosmicTime.ToDateTime(Duration)"/>, which uses a much more precise estimation of the
    /// relationship between the age of the universe and the basis used by <see
    /// cref="DateTime"/>.
    /// </para>
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException"/>
    public DateTime ToDateTime()
    {
        if (IsNegative)
        {
            throw new ArgumentOutOfRangeException();
        }
        if (IsPerpetual
            || Aeons > BigInteger.Zero
            || Years > 9998)
        {
            throw new ArgumentOutOfRangeException();
        }
        var ticks = ((Years * (ulong)NanosecondsPerYear)
            + TotalNanoseconds
            + (TotalYoctoseconds / YoctosecondsPerNanosecond))
            * NanosecondsPerTick;
        if (ticks > (ulong)DateTime.MaxValue.Ticks)
        {
            throw new ArgumentOutOfRangeException();
        }
        return new DateTime((long)ticks, DateTimeKind.Utc);
    }

    /// <summary>
    /// Converts this instance to a <see cref="DateTimeOffset"/> value.
    /// </summary>
    /// <returns>A <see cref="DateTimeOffset"/> value equivalent to this instance.</returns>
    /// <remarks>
    /// <para>
    /// This instance is converted to a <see cref="DateTimeOffset"/> value by presuming that the
    /// current aeon begins at the same moment as <see cref="DateTimeOffset.Ticks"/> begins
    /// counting. A <see cref="DateTimeOffset.Ticks"/> value for the <see
    /// cref="DateTimeOffset"/> value is generated from the timekeeping system of <see
    /// cref="Duration"/> based on that assumption.
    /// </para>
    /// <para>
    /// The returned <see cref="DateTimeOffset"/> will have a <see
    /// cref="DateTimeOffset.Offset"/> of <see cref="TimeSpan.Zero"/>.
    /// </para>
    /// <para>
    /// For more accurate <see cref="DateTimeOffset"/> conversion, consider using <see
    /// cref="CosmicTime.ToDateTime(Duration)"/>, which uses a much more precise estimation of the
    /// relationship between the age of the universe and the basis used by <see
    /// cref="DateTimeOffset"/>.
    /// </para>
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException"/>
    public DateTimeOffset ToDateTimeOffset()
    {
        if (IsNegative)
        {
            throw new ArgumentOutOfRangeException();
        }
        if (IsPerpetual
            || Aeons > BigInteger.Zero
            || Years > 9998)
        {
            throw new ArgumentOutOfRangeException();
        }
        var ticks = ((Years * (ulong)NanosecondsPerYear)
            + TotalNanoseconds
            + (TotalYoctoseconds / YoctosecondsPerNanosecond))
            * NanosecondsPerTick;
        if (ticks > (ulong)DateTimeOffset.MaxValue.Ticks)
        {
            throw new ArgumentOutOfRangeException();
        }
        return new DateTimeOffset((long)ticks, TimeSpan.Zero);
    }

    /// <summary>
    /// Converts this instance to a <see cref="TimeSpan"/> value.
    /// </summary>
    /// <returns>A <see cref="TimeSpan"/> value equivalent to this instance.</returns>
    /// <exception cref="ArgumentOutOfRangeException"/>
    public TimeSpan ToTimeSpan()
    {
        if (IsPerpetual
            || Aeons > BigInteger.Zero
            || Years > 29227)
        {
            throw new ArgumentOutOfRangeException();
        }
        var ticks = ((Years * (ulong)NanosecondsPerYear)
            + TotalNanoseconds
            + (TotalYoctoseconds / YoctosecondsPerNanosecond))
            * NanosecondsPerTick;
        if (IsNegative)
        {
            if (ticks > (ulong)-TimeSpan.MinValue.Ticks)
            {
                throw new ArgumentOutOfRangeException();
            }
        }
        else if (ticks > (ulong)TimeSpan.MaxValue.Ticks)
        {
            throw new ArgumentOutOfRangeException();
        }
        return new TimeSpan(Sign * (long)ticks);
    }
}
