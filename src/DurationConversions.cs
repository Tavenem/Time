using System;
using System.Collections.Generic;
using Tavenem.HugeNumbers;

namespace Tavenem.Time
{
    public partial struct Duration
    {
        private const long AeonUnit = 1000000000000000000;
        private static readonly HugeNumber _AeonUnit_Number = new(1000000000000000000);

        /// <summary>
        /// Gets a new <see cref="Duration"/> instance with the given <paramref name="value"/>.
        /// </summary>
        /// <param name="value">
        /// A number of aeons (1,000,000 years).
        /// </param>
        /// <returns>A new <see cref="Duration"/> instance.</returns>
        /// <exception cref="ArgumentException"><paramref name="value"/> is <see
        /// cref="HugeNumber.NaN"/>.</exception>
        public static Duration FromAeons(HugeNumber value)
        {
            if (value.IsNaN)
            {
                throw new ArgumentException($"{nameof(value)} was NaN.");
            }
            if (value.IsZero)
            {
                return Zero;
            }
            if (value.IsPositiveInfinity)
            {
                return PositiveInfinity;
            }
            if (value.IsNegativeInfinity)
            {
                return NegativeInfinity;
            }
            var isNegative = value < 0;
            value = HugeNumber.Abs(value);

            var aeons = HugeNumber.Floor(value);
            value -= aeons;

            var aeonSequence = new List<ulong>();
            if (aeons > 0)
            {
                while (aeons > 0)
                {
                    var term = (ulong)(aeons % _AeonUnit_Number);
                    aeonSequence.Add(term);
                    aeons = (aeons / _AeonUnit_Number).Round();
                }
            }

            var d = new Duration(false, false, 0, 0, 0, 0, aeonSequence);
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

            var aeonSequence = new List<ulong>();
            if (aeons > 0)
            {
                while (aeons > 0)
                {
                    var term = (ulong)(aeons % AeonUnit);
                    aeonSequence.Add(term);
                    aeons = Math.Round(aeons / AeonUnit);
                }
            }

            var d = new Duration(false, false, 0, 0, 0, 0, aeonSequence);
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

            var aeonSequence = new List<ulong>();
            if (aeons > 0)
            {
                while (aeons > 0)
                {
                    var term = (ulong)(aeons % AeonUnit);
                    aeonSequence.Add(term);
                    aeons = Math.Round(aeons / AeonUnit);
                }
            }

            var d = new Duration(false, false, 0, 0, 0, 0, aeonSequence);
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
        public static Duration FromAeons(long value)
            => FromAeons((decimal)value);

        /// <summary>
        /// Gets a new <see cref="Duration"/> instance with the given <paramref name="value"/>.
        /// </summary>
        /// <param name="value">
        /// A number of attoseconds.
        /// </param>
        /// <returns>A new <see cref="Duration"/> instance.</returns>
        /// <exception cref="ArgumentException"><paramref name="value"/> is <see
        /// cref="HugeNumber.NaN"/>.</exception>
        public static Duration FromAttoseconds(HugeNumber value)
            => FromNanoseconds(value / AttosecondsPerNanosecond);

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
            => FromAttoseconds((decimal)value);

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
            => new(nanoseconds: (ulong)(dateTime.ToUniversalTime().Ticks * NanosecondsPerTick), aeonSequence: _HomeAeonSequence);

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
            => new(nanoseconds: (ulong)(dateTime.ToUniversalTime().Ticks * NanosecondsPerTick), aeonSequence: _HomeAeonSequence);

        /// <summary>
        /// Gets a new <see cref="Duration"/> instance with the given <paramref name="value"/>.
        /// </summary>
        /// <param name="value">
        /// A number of femtoseconds.
        /// </param>
        /// <returns>A new <see cref="Duration"/> instance.</returns>
        /// <exception cref="ArgumentException"><paramref name="value"/> is <see
        /// cref="HugeNumber.NaN"/>.</exception>
        public static Duration FromFemtoseconds(HugeNumber value)
            => FromNanoseconds(value / FemtosecondsPerNanosecond);

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
            => FromFemtoseconds((decimal)value);

        /// <summary>
        /// Gets a new <see cref="Duration"/> instance with the given <paramref name="value"/>.
        /// </summary>
        /// <param name="value">
        /// A number of microseconds.
        /// </param>
        /// <returns>A new <see cref="Duration"/> instance.</returns>
        /// <exception cref="ArgumentException"><paramref name="value"/> is <see
        /// cref="HugeNumber.NaN"/>.</exception>
        public static Duration FromMicroseconds(HugeNumber value)
            => FromNanoseconds(value * NanosecondsPerMicrosecond);

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
        /// <exception cref="ArgumentException"><paramref name="value"/> is <see
        /// cref="double.NaN"/>.</exception>
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
        /// A number of milliseconds.
        /// </param>
        /// <returns>A new <see cref="Duration"/> instance.</returns>
        /// <exception cref="ArgumentException"><paramref name="value"/> is <see
        /// cref="HugeNumber.NaN"/>.</exception>
        public static Duration FromMilliseconds(HugeNumber value)
            => FromNanoseconds(value * NanosecondsPerMillisecond);

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
        /// <exception cref="ArgumentException"><paramref name="value"/> is <see
        /// cref="double.NaN"/>.</exception>
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
        /// A number of nanoseconds.
        /// </param>
        /// <returns>A new <see cref="Duration"/> instance.</returns>
        /// <exception cref="ArgumentException"><paramref name="value"/> is <see
        /// cref="HugeNumber.NaN"/>.</exception>
        public static Duration FromNanoseconds(HugeNumber value)
        {
            if (value.IsNaN)
            {
                throw new ArgumentException($"{nameof(value)} was NaN.");
            }
            if (value.IsZero)
            {
                return Zero;
            }
            if (value.IsPositiveInfinity)
            {
                return PositiveInfinity;
            }
            if (value.IsNegativeInfinity)
            {
                return NegativeInfinity;
            }
            var isNegative = value < 0;
            value = HugeNumber.Abs(value);

            var years = HugeNumber.Floor(value / NanosecondsPerYear);
            value -= years * NanosecondsPerYear;

            var d = FromYears(years);

            if (value > 0)
            {
                var ns = (ulong)HugeNumber.Floor(value);
                value -= ns;

                d += new Duration(false, false, 0, 0, ns, 0, null);

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

                d += new Duration(false, false, 0, 0, ns, 0, null);

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

                d += new Duration(false, false, 0, 0, ns, 0, null);

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
            => FromNanoseconds((decimal)value);

        /// <summary>
        /// Gets a new <see cref="Duration"/> instance with the given <paramref name="value"/>.
        /// </summary>
        /// <param name="value">
        /// A number of picoseconds.
        /// </param>
        /// <returns>A new <see cref="Duration"/> instance.</returns>
        /// <exception cref="ArgumentException"><paramref name="value"/> is <see
        /// cref="HugeNumber.NaN"/>.</exception>
        public static Duration FromPicoseconds(HugeNumber value)
            => FromNanoseconds(value / PicosecondsPerNanosecond);

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
            => FromPicoseconds((decimal)value);

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
        public static Duration FromProportionOfDay(HugeNumber proportionOfDay, Duration dayDuration)
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
        public static Duration FromProportionOfYear(HugeNumber proportionOfYear, Duration yearDuration)
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
        public static Duration FromProportions(HugeNumber proportionOfYear, Duration yearDuration, HugeNumber proportionOfDay, Duration dayDuration)
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
        /// Gets a new <see cref="Duration"/> instance with the given <paramref name="value"/>.
        /// </summary>
        /// <param name="value">
        /// An amount of Planck time.
        /// </param>
        /// <returns>A new <see cref="Duration"/> instance.</returns>
        /// <exception cref="ArgumentException"><paramref name="value"/> is <see
        /// cref="HugeNumber.NaN"/>.</exception>
        public static Duration FromPlanckTime(HugeNumber value)
        {
            if (value.IsNaN)
            {
                throw new ArgumentException($"{nameof(value)} was NaN.");
            }
            if (value.IsZero)
            {
                return Zero;
            }
            if (value.IsPositiveInfinity)
            {
                return PositiveInfinity;
            }
            if (value.IsNegativeInfinity)
            {
                return NegativeInfinity;
            }
            var isNegative = value < 0;
            value = HugeNumber.Abs(value);

            var ys = HugeNumber.Floor(value / (HugeNumber)PlanckTimePerYoctosecond);
            value -= ys * (HugeNumber)PlanckTimePerYoctosecond;

            var d = FromYoctoseconds(ys);

            if (value > 0)
            {
                d += new Duration(false, false, (decimal)value, 0, 0, 0, null);
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
        /// An amount of Planck time.
        /// </param>
        /// <returns>A new <see cref="Duration"/> instance.</returns>
        /// <exception cref="ArgumentException"><paramref name="value"/> is <see
        /// cref="double.NaN"/>.</exception>
        public static Duration FromPlanckTime(decimal value)
        {
            if (value == 0)
            {
                return Zero;
            }
            var isNegative = value < 0;
            value = Math.Abs(value);

            var ys = Math.Floor(value / PlanckTimePerYoctosecond);
            value -= ys * PlanckTimePerYoctosecond;

            var d = FromYoctoseconds(ys);

            if (value > 0)
            {
                d += new Duration(false, false, value, 0, 0, 0, null);
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
        /// An amount of Planck time.
        /// </param>
        /// <returns>A new <see cref="Duration"/> instance.</returns>
        /// <exception cref="ArgumentException"><paramref name="value"/> is <see
        /// cref="double.NaN"/>.</exception>
        public static Duration FromPlanckTime(double value)
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

            var decimalValue = (decimal)value;
            var ys = Math.Floor(decimalValue / PlanckTimePerYoctosecond);
            decimalValue -= ys * PlanckTimePerYoctosecond;

            var d = FromYoctoseconds(ys);

            if (value > 0)
            {
                d += new Duration(false, false, decimalValue, 0, 0, 0, null);
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
        /// An amount of Planck time.
        /// </param>
        /// <returns>A new <see cref="Duration"/> instance.</returns>
        /// <exception cref="ArgumentException"><paramref name="value"/> is <see
        /// cref="double.NaN"/>.</exception>
        public static Duration FromPlanckTime(long value)
            => FromPlanckTime((decimal)value);

        /// <summary>
        /// Gets a new <see cref="Duration"/> instance with the given <paramref name="value"/>.
        /// </summary>
        /// <param name="value">
        /// A number of seconds.
        /// </param>
        /// <returns>A new <see cref="Duration"/> instance.</returns>
        /// <exception cref="ArgumentException"><paramref name="value"/> is <see
        /// cref="HugeNumber.NaN"/>.</exception>
        public static Duration FromSeconds(HugeNumber value)
            => FromNanoseconds(value * NanosecondsPerSecond);

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
        /// <exception cref="ArgumentException"><paramref name="value"/> is <see
        /// cref="double.NaN"/>.</exception>
        public static Duration FromSeconds(double value)
            => FromNanoseconds(value * NanosecondsPerSecond);

        /// <summary>
        /// Gets a new <see cref="Duration"/> instance with the given <paramref name="value"/>.
        /// </summary>
        /// <param name="value">
        /// A number of seconds.
        /// </param>
        /// <returns>A new <see cref="Duration"/> instance.</returns>
        /// <exception cref="ArgumentException"><paramref name="value"/> is <see
        /// cref="double.NaN"/>.</exception>
        public static Duration FromSeconds(long value)
            => FromSeconds((decimal)value);

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
            return new Duration(isNegative, nanoseconds: ns, aeonSequence: _HomeAeonSequence);
        }

        /// <summary>
        /// Converts the given <see cref="TimeSpan"/> value to a <see cref="Duration"/> value.
        /// </summary>
        /// <param name="timeSpan">A <see cref="TimeSpan"/> value to convert.</param>
        /// <param name="aeonSequence">The number of aeons, expressed as a list of significant
        /// digits in ascending order, starting with the value of the 6th significant digit
        /// (1e6).</param>
        /// <returns>An equivalent <see cref="Duration"/> value.</returns>
        /// <remarks>
        /// <para>
        /// The <see cref="TimeSpan"/> value is converted to a <see cref="Duration"/> instance by
        /// presuming that the current aeon begins at the same moment as <see
        /// cref="TimeSpan.Ticks"/> begins counting. The <see cref="TimeSpan.Ticks"/> of the <see
        /// cref="TimeSpan"/> value are converted into the timekeeping system of <see
        /// cref="Duration"/> based on that assumption.
        /// </para>
        /// </remarks>
        public static Duration FromTimeSpan(TimeSpan timeSpan, IList<ulong> aeonSequence)
        {
            var ticks = timeSpan.Ticks;
            if (ticks == 0)
            {
                return Zero;
            }
            var isNegative = ticks < 0;
            ticks = Math.Abs(ticks);
            var ns = (ulong)ticks * NanosecondsPerTick;
            return new Duration(isNegative, nanoseconds: ns, aeonSequence: aeonSequence);
        }

        /// <summary>
        /// Gets a new <see cref="Duration"/> instance with the given <paramref name="value"/>.
        /// </summary>
        /// <param name="value">
        /// A number of years.
        /// </param>
        /// <returns>A new <see cref="Duration"/> instance.</returns>
        /// <exception cref="ArgumentException"><paramref name="value"/> is <see
        /// cref="HugeNumber.NaN"/>.</exception>
        public static Duration FromYears(HugeNumber value)
        {
            if (value.IsNaN)
            {
                throw new ArgumentException($"{nameof(value)} was NaN.");
            }
            if (value.IsZero)
            {
                return Zero;
            }
            if (value.IsPositiveInfinity)
            {
                return PositiveInfinity;
            }
            if (value.IsNegativeInfinity)
            {
                return NegativeInfinity;
            }
            var isNegative = value < 0;
            value = HugeNumber.Abs(value);

            var aeons = HugeNumber.Floor(value / YearsPerAeon);
            value -= aeons * YearsPerAeon;

            var d = FromAeons(aeons);

            if (value > 0)
            {
                var years = (uint)HugeNumber.Floor(value);
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
            => FromYears((decimal)value);

        /// <summary>
        /// Gets a new <see cref="Duration"/> instance with the given <paramref name="value"/>.
        /// </summary>
        /// <param name="value">
        /// A number of yoctoseconds.
        /// </param>
        /// <returns>A new <see cref="Duration"/> instance.</returns>
        /// <exception cref="ArgumentException"><paramref name="value"/> is <see
        /// cref="HugeNumber.NaN"/>.</exception>
        public static Duration FromYoctoseconds(HugeNumber value)
        {
            if (value.IsNaN)
            {
                throw new ArgumentException($"{nameof(value)} was NaN.");
            }
            if (value.IsZero)
            {
                return Zero;
            }
            if (value.IsPositiveInfinity)
            {
                return PositiveInfinity;
            }
            if (value.IsNegativeInfinity)
            {
                return NegativeInfinity;
            }
            var isNegative = value < 0;
            value = HugeNumber.Abs(value);

            var ns = HugeNumber.Floor(value / YoctosecondsPerNanosecond);
            value -= ns * YoctosecondsPerNanosecond;

            var d = FromNanoseconds(ns);

            if (value > 0)
            {
                var ys = (ulong)HugeNumber.Floor(value);
                value -= ys;

                d += new Duration(false, false, (decimal)value * PlanckTimePerYoctosecond, ys, 0, 0, null);
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

                d += new Duration(false, false, value * PlanckTimePerYoctosecond, ys, 0, 0, null);
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

                d += new Duration(false, false, (decimal)value * PlanckTimePerYoctosecond, ys, 0, 0, null);
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
            => FromYoctoseconds((decimal)value);

        /// <summary>
        /// Gets a new <see cref="Duration"/> instance with the given <paramref name="value"/>.
        /// </summary>
        /// <param name="value">
        /// A number of picoseconds.
        /// </param>
        /// <returns>A new <see cref="Duration"/> instance.</returns>
        /// <exception cref="ArgumentException"><paramref name="value"/> is <see
        /// cref="HugeNumber.NaN"/>.</exception>
        public static Duration FromZeptoseconds(HugeNumber value)
            => FromNanoseconds(value / ZeptosecondsPerNanosecond);

        /// <summary>
        /// Gets a new <see cref="Duration"/> instance with the given <paramref name="value"/>.
        /// </summary>
        /// <param name="value">
        /// A number of picoseconds.
        /// </param>
        /// <returns>A new <see cref="Duration"/> instance.</returns>
        public static Duration FromZeptoseconds(decimal value)
            => FromNanoseconds(value / ZeptosecondsPerNanosecond);

        /// <summary>
        /// Gets a new <see cref="Duration"/> instance with the given <paramref name="value"/>.
        /// </summary>
        /// <param name="value">
        /// A number of picoseconds.
        /// </param>
        /// <returns>A new <see cref="Duration"/> instance.</returns>
        /// <exception cref="ArgumentException"><paramref name="value"/> is <see
        /// cref="double.NaN"/>.</exception>
        public static Duration FromZeptoseconds(double value)
            => FromNanoseconds(value / ZeptosecondsPerNanosecond);

        /// <summary>
        /// Gets a new <see cref="Duration"/> instance with the given <paramref name="value"/>.
        /// </summary>
        /// <param name="value">
        /// A number of picoseconds.
        /// </param>
        /// <returns>A new <see cref="Duration"/> instance.</returns>
        public static Duration FromZeptoseconds(long value)
            => FromZeptoseconds((decimal)value);

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
                || AeonSequence?.Count > 0
                || Years > 9998)
            {
                throw new ArgumentOutOfRangeException();
            }
            var ticks = ((Years * (ulong)NanosecondsPerYear)
                + TotalNanoseconds
                + (ulong)Math.Round((TotalYoctoseconds + (PlanckTime / PlanckTimePerYoctosecond)) / YoctosecondsPerNanosecond))
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
                || AeonSequence?.Count > 0
                || Years > 9998)
            {
                throw new ArgumentOutOfRangeException();
            }
            var ticks = ((Years * (ulong)NanosecondsPerYear)
                + TotalNanoseconds
                + (ulong)Math.Round((TotalYoctoseconds + (PlanckTime / PlanckTimePerYoctosecond)) / YoctosecondsPerNanosecond))
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
                || AeonSequence?.Count > 0
                || Years > 29227)
            {
                throw new ArgumentOutOfRangeException();
            }
            var ticks = ((Years * (ulong)NanosecondsPerYear)
                + TotalNanoseconds
                + (ulong)Math.Round((TotalYoctoseconds + (PlanckTime / PlanckTimePerYoctosecond)) / YoctosecondsPerNanosecond))
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
}
