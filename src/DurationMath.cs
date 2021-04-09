using System;
using System.Collections.Generic;
using System.Linq;
using Tavenem.HugeNumbers;

namespace Tavenem.Time
{
    public partial struct Duration
    {
        /// <summary>
        /// Gets the maximum of two <see cref="Duration"/> instances.
        /// </summary>
        /// <param name="first">The first instance.</param>
        /// <param name="second">The second instance.</param>
        /// <returns>The maximum of two <see cref="Duration"/> instances.</returns>
        public static Duration Max(Duration first, Duration second)
            => first.CompareTo(second) >= 0 ? first : second;

        /// <summary>
        /// Gets the maximum of a <see cref="Duration"/> instance and a number of seconds.
        /// </summary>
        /// <param name="first">The first instance.</param>
        /// <param name="seconds">A number of seconds.</param>
        /// <returns>The maximum of a <see cref="Duration"/> instance and a number of
        /// seconds.</returns>
        public static Duration Max(Duration first, decimal seconds)
            => first.CompareTo(seconds) >= 0 ? first : Duration.FromSeconds(seconds);

        /// <summary>
        /// Gets the maximum of a <see cref="Duration"/> instance and a number of seconds.
        /// </summary>
        /// <param name="first">The first instance.</param>
        /// <param name="seconds">A number of seconds.</param>
        /// <returns>The maximum of a <see cref="Duration"/> instance and a number of
        /// seconds.</returns>
        public static Duration Max(Duration first, double seconds)
            => first.CompareTo(seconds) >= 0 ? first : Duration.FromSeconds(seconds);

        /// <summary>
        /// Gets the minimum of two <see cref="Duration"/> instances.
        /// </summary>
        /// <param name="first">The first instance.</param>
        /// <param name="second">The second instance.</param>
        /// <returns>The minimum of two <see cref="Duration"/> instances.</returns>
        public static Duration Min(Duration first, Duration second)
            => first.CompareTo(second) <= 0 ? first : second;

        /// <summary>
        /// Gets the minimum of a <see cref="Duration"/> instance and a number of seconds.
        /// </summary>
        /// <param name="first">The first instance.</param>
        /// <param name="seconds">A number of seconds.</param>
        /// <returns>The minimum of a <see cref="Duration"/> instance and a number of
        /// seconds.</returns>
        public static Duration Min(Duration first, decimal seconds)
            => first.CompareTo(seconds) <= 0 ? first : Duration.FromSeconds(seconds);

        /// <summary>
        /// Gets the minimum of a <see cref="Duration"/> instance and a number of seconds.
        /// </summary>
        /// <param name="first">The first instance.</param>
        /// <param name="seconds">A number of seconds.</param>
        /// <returns>The minimum of a <see cref="Duration"/> instance and a number of
        /// seconds.</returns>
        public static Duration Min(Duration first, double seconds)
            => first.CompareTo(seconds) <= 0 ? first : Duration.FromSeconds(seconds);

        /// <summary>
        /// Adds the given <paramref name="duration"/> to this instance, and returns a result
        /// representing their combination.
        /// </summary>
        /// <param name="duration">A <see cref="Duration"/>.</param>
        /// <returns>A new <see cref="Duration"/> instance.</returns>
        /// <exception cref="OverflowException">Result gets a value for <see cref="AeonSequence"/>
        /// with more than <see cref="int.MaxValue"/> significant digits.</exception>
        public Duration Add(Duration duration)
        {
            if (IsPerpetual || duration.IsPerpetual)
            {
                if (IsNegative == duration.IsNegative)
                {
                    return this;
                }
                else
                {
                    return Zero;
                }
            }

            var isNegative = false;
            if (IsNegative)
            {
                if (duration.IsNegative)
                {
                    isNegative = true;
                }
                else
                {
                    return duration.Subtract(this);
                }
            }
            else if (duration.IsNegative)
            {
                return Subtract(duration);
            }

            var newAeonSequence = AeonSequence?.ToList() ?? new List<ulong>();
            var aeon = 0ul;
            var index = 0;
            while ((aeon > 0 || index < duration.AeonSequence?.Count)
                && index <= MaxAeonSequenceLength)
            {
                if (index >= newAeonSequence.Count)
                {
                    var diff = MaxAeonSequence - aeon;
                    if (diff < duration.AeonSequence![index])
                    {
                        aeon = duration.AeonSequence[index] - diff;
                        newAeonSequence.Add(MaxAeonSequence);
                    }
                    else
                    {
                        newAeonSequence.Add(duration.AeonSequence[index] + aeon);
                        aeon = 0;
                    }
                }
                else
                {
                    var diff = MaxAeonSequence - newAeonSequence[index] - aeon;
                    if (diff < duration.AeonSequence![index])
                    {
                        aeon = duration.AeonSequence[index] - diff;
                        newAeonSequence[index] = MaxAeonSequence;
                    }
                    else
                    {
                        newAeonSequence[index] += duration.AeonSequence[index] + aeon;
                        aeon = 0;
                    }
                }
                index++;
            }
            if (aeon > 0)
            {
                throw new OverflowException($"Provided parameters result in a value for ${nameof(AeonSequence)} with more than {nameof(MaxAeonSequenceLength)} terms.");
            }

            return new Duration(
                isNegative,
                aeonSequence: (newAeonSequence.Count == 0 || newAeonSequence.All(x => x == 0)) ? null : newAeonSequence,
                years: Years + duration.Years,
                nanoseconds: TotalNanoseconds + duration.TotalNanoseconds,
                yoctoseconds: TotalYoctoseconds + duration.TotalYoctoseconds,
                planckTime: PlanckTime + duration.PlanckTime);
        }

        /// <summary>
        /// Adds the given number of seconds to this instance, and returns a result representing
        /// their combination.
        /// </summary>
        /// <param name="seconds">A number of seconds.</param>
        /// <returns>A new <see cref="Duration"/> instance.</returns>
        public Duration Add(HugeNumber seconds) => AddSeconds(seconds);

        /// <summary>
        /// Adds the given number of seconds to this instance, and returns a result representing
        /// their combination.
        /// </summary>
        /// <param name="seconds">A number of seconds.</param>
        /// <returns>A new <see cref="Duration"/> instance.</returns>
        public Duration Add(decimal seconds) => AddSeconds(seconds);

        /// <summary>
        /// Adds the given number of seconds to this instance, and returns a result representing
        /// their combination.
        /// </summary>
        /// <param name="seconds">A number of seconds.</param>
        /// <returns>A new <see cref="Duration"/> instance.</returns>
        public Duration Add(double seconds) => AddSeconds(seconds);

        /// <summary>
        /// Adds the given number of aeons to this instance, and returns a result representing
        /// their combination, or <see cref="Zero"/> if the result would be a negative duration.
        /// </summary>
        /// <param name="value">A number of aeons (1,000,000 years).</param>
        /// <returns>A new <see cref="Duration"/> instance.</returns>
        /// <exception cref="OverflowException">Result gets a value for <see cref="AeonSequence"/>
        /// with more than <see cref="int.MaxValue"/> significant digits.</exception>
        public Duration AddAeons(HugeNumber value)
            => value < 0 ? Subtract(FromAeons(-value)) : Add(FromAeons(value));

        /// <summary>
        /// Adds the given number of aeons to this instance, and returns a result representing
        /// their combination, or <see cref="Zero"/> if the result would be a negative duration.
        /// </summary>
        /// <param name="value">A number of aeons (1,000,000 years).</param>
        /// <returns>A new <see cref="Duration"/> instance.</returns>
        /// <exception cref="OverflowException">Result gets a value for <see cref="AeonSequence"/>
        /// with more than <see cref="int.MaxValue"/> significant digits.</exception>
        public Duration AddAeons(decimal value)
            => value < 0 ? Subtract(FromAeons(-value)) : Add(FromAeons(value));

        /// <summary>
        /// Adds the given number of aeons to this instance, and returns a result representing
        /// their combination, or <see cref="Zero"/> if the result would be a negative duration.
        /// </summary>
        /// <param name="value">A number of aeons (1,000,000 years).</param>
        /// <returns>A new <see cref="Duration"/> instance.</returns>
        /// <exception cref="OverflowException">Result gets a value for <see cref="AeonSequence"/>
        /// with more than <see cref="int.MaxValue"/> significant digits.</exception>
        public Duration AddAeons(double value)
            => value < 0 ? Subtract(FromAeons(-value)) : Add(FromAeons(value));

        /// <summary>
        /// Adds the given number of microseconds to this instance, and returns a result
        /// representing their combination, or <see cref="Zero"/> if the result would be a negative
        /// duration.
        /// </summary>
        /// <param name="value">A number of microseconds.</param>
        /// <returns>A new <see cref="Duration"/> instance.</returns>
        /// <exception cref="OverflowException">Result gets a value for <see cref="AeonSequence"/>
        /// with more than <see cref="int.MaxValue"/> significant digits.</exception>
        public Duration AddMicroseconds(HugeNumber value)
            => value < 0 ? Subtract(FromMicroseconds(-value)) : Add(FromMicroseconds(value));

        /// <summary>
        /// Adds the given number of microseconds to this instance, and returns a result
        /// representing their combination, or <see cref="Zero"/> if the result would be a negative
        /// duration.
        /// </summary>
        /// <param name="value">A number of microseconds.</param>
        /// <returns>A new <see cref="Duration"/> instance.</returns>
        /// <exception cref="OverflowException">Result gets a value for <see cref="AeonSequence"/>
        /// with more than <see cref="int.MaxValue"/> significant digits.</exception>
        public Duration AddMicroseconds(decimal value)
            => value < 0 ? Subtract(FromMicroseconds(-value)) : Add(FromMicroseconds(value));

        /// <summary>
        /// Adds the given number of microseconds to this instance, and returns a result
        /// representing their combination, or <see cref="Zero"/> if the result would be a negative
        /// duration.
        /// </summary>
        /// <param name="value">A number of microseconds.</param>
        /// <returns>A new <see cref="Duration"/> instance.</returns>
        /// <exception cref="OverflowException">Result gets a value for <see cref="AeonSequence"/>
        /// with more than <see cref="int.MaxValue"/> significant digits.</exception>
        public Duration AddMicroseconds(double value)
            => value < 0 ? Subtract(FromMicroseconds(-value)) : Add(FromMicroseconds(value));

        /// <summary>
        /// Adds the given number of milliseconds to this instance, and returns a result
        /// representing their combination, or <see cref="Zero"/> if the result would be a negative
        /// duration.
        /// </summary>
        /// <param name="value">A number of milliseconds.</param>
        /// <returns>A new <see cref="Duration"/> instance.</returns>
        /// <exception cref="OverflowException">Result gets a value for <see cref="AeonSequence"/>
        /// with more than <see cref="int.MaxValue"/> significant digits.</exception>
        public Duration AddMilliseconds(HugeNumber value)
            => value < 0 ? Subtract(FromMilliseconds(-value)) : Add(FromMilliseconds(value));

        /// <summary>
        /// Adds the given number of milliseconds to this instance, and returns a result
        /// representing their combination, or <see cref="Zero"/> if the result would be a negative
        /// duration.
        /// </summary>
        /// <param name="value">A number of milliseconds.</param>
        /// <returns>A new <see cref="Duration"/> instance.</returns>
        /// <exception cref="OverflowException">Result gets a value for <see cref="AeonSequence"/>
        /// with more than <see cref="int.MaxValue"/> significant digits.</exception>
        public Duration AddMilliseconds(decimal value)
            => value < 0 ? Subtract(FromMilliseconds(-value)) : Add(FromMilliseconds(value));

        /// <summary>
        /// Adds the given number of milliseconds to this instance, and returns a result
        /// representing their combination, or <see cref="Zero"/> if the result would be a negative
        /// duration.
        /// </summary>
        /// <param name="value">A number of milliseconds.</param>
        /// <returns>A new <see cref="Duration"/> instance.</returns>
        /// <exception cref="OverflowException">Result gets a value for <see cref="AeonSequence"/>
        /// with more than <see cref="int.MaxValue"/> significant digits.</exception>
        public Duration AddMilliseconds(double value)
            => value < 0 ? Subtract(FromMilliseconds(-value)) : Add(FromMilliseconds(value));

        /// <summary>
        /// Adds the given number of nanoseconds to this instance, and returns a result
        /// representing their combination, or <see cref="Zero"/> if the result would be a negative
        /// duration.
        /// </summary>
        /// <param name="value">A number of nanoseconds.</param>
        /// <returns>A new <see cref="Duration"/> instance.</returns>
        /// <exception cref="OverflowException">Result gets a value for <see cref="AeonSequence"/>
        /// with more than <see cref="int.MaxValue"/> significant digits.</exception>
        public Duration AddNanoseconds(HugeNumber value)
            => value < 0 ? Subtract(FromNanoseconds(-value)) : Add(FromNanoseconds(value));

        /// <summary>
        /// Adds the given number of nanoseconds to this instance, and returns a result
        /// representing their combination, or <see cref="Zero"/> if the result would be a negative
        /// duration.
        /// </summary>
        /// <param name="value">A number of nanoseconds.</param>
        /// <returns>A new <see cref="Duration"/> instance.</returns>
        /// <exception cref="OverflowException">Result gets a value for <see cref="AeonSequence"/>
        /// with more than <see cref="int.MaxValue"/> significant digits.</exception>
        public Duration AddNanoseconds(decimal value)
            => value < 0 ? Subtract(FromNanoseconds(-value)) : Add(FromNanoseconds(value));

        /// <summary>
        /// Adds the given number of nanoseconds to this instance, and returns a result
        /// representing their combination, or <see cref="Zero"/> if the result would be a negative
        /// duration.
        /// </summary>
        /// <param name="value">A number of nanoseconds.</param>
        /// <returns>A new <see cref="Duration"/> instance.</returns>
        /// <exception cref="OverflowException">Result gets a value for <see cref="AeonSequence"/>
        /// with more than <see cref="int.MaxValue"/> significant digits.</exception>
        public Duration AddNanoseconds(double value)
            => value < 0 ? Subtract(FromNanoseconds(-value)) : Add(FromNanoseconds(value));

        /// <summary>
        /// Adds the given amount of Planck time to this instance, and returns a result representing
        /// their combination, or <see cref="Zero"/> if the result would be a negative duration.
        /// </summary>
        /// <param name="value">An amount of Planck time.</param>
        /// <returns>A new <see cref="Duration"/> instance.</returns>
        /// <exception cref="OverflowException">Result gets a value for <see cref="AeonSequence"/>
        /// with more than <see cref="int.MaxValue"/> significant digits.</exception>
        public Duration AddPlanckTime(HugeNumber value)
            => value < 0 ? Subtract(FromPlanckTime(-value)) : Add(FromPlanckTime(value));

        /// <summary>
        /// Adds the given amount of Planck time to this instance, and returns a result representing
        /// their combination, or <see cref="Zero"/> if the result would be a negative duration.
        /// </summary>
        /// <param name="value">An amount of Planck time.</param>
        /// <returns>A new <see cref="Duration"/> instance.</returns>
        /// <exception cref="OverflowException">Result gets a value for <see cref="AeonSequence"/>
        /// with more than <see cref="int.MaxValue"/> significant digits.</exception>
        public Duration AddPlanckTime(decimal value)
            => value < 0 ? Subtract(FromPlanckTime(-value)) : Add(FromPlanckTime(value));

        /// <summary>
        /// Adds the given amount of Planck time to this instance, and returns a result representing
        /// their combination, or <see cref="Zero"/> if the result would be a negative duration.
        /// </summary>
        /// <param name="value">An amount of Planck time.</param>
        /// <returns>A new <see cref="Duration"/> instance.</returns>
        /// <exception cref="OverflowException">Result gets a value for <see cref="AeonSequence"/>
        /// with more than <see cref="int.MaxValue"/> significant digits.</exception>
        public Duration AddPlanckTime(double value)
            => value < 0 ? Subtract(FromPlanckTime(-value)) : Add(FromPlanckTime(value));

        /// <summary>
        /// Adds the given number of seconds to this instance, and returns a result representing
        /// their combination, or <see cref="Zero"/> if the result would be a negative duration.
        /// </summary>
        /// <param name="value">A number of seconds.</param>
        /// <returns>A new <see cref="Duration"/> instance.</returns>
        /// <exception cref="OverflowException">Result gets a value for <see cref="AeonSequence"/>
        /// with more than <see cref="int.MaxValue"/> significant digits.</exception>
        public Duration AddSeconds(HugeNumber value)
            => value < 0 ? Subtract(FromSeconds(-value)) : Add(FromSeconds(value));

        /// <summary>
        /// Adds the given number of seconds to this instance, and returns a result representing
        /// their combination, or <see cref="Zero"/> if the result would be a negative duration.
        /// </summary>
        /// <param name="value">A number of seconds.</param>
        /// <returns>A new <see cref="Duration"/> instance.</returns>
        /// <exception cref="OverflowException">Result gets a value for <see cref="AeonSequence"/>
        /// with more than <see cref="int.MaxValue"/> significant digits.</exception>
        public Duration AddSeconds(decimal value)
            => value < 0 ? Subtract(FromSeconds(-value)) : Add(FromSeconds(value));

        /// <summary>
        /// Adds the given number of seconds to this instance, and returns a result representing
        /// their combination, or <see cref="Zero"/> if the result would be a negative duration.
        /// </summary>
        /// <param name="value">A number of seconds.</param>
        /// <returns>A new <see cref="Duration"/> instance.</returns>
        /// <exception cref="OverflowException">Result gets a value for <see cref="AeonSequence"/>
        /// with more than <see cref="int.MaxValue"/> significant digits.</exception>
        public Duration AddSeconds(double value)
            => value < 0 ? Subtract(FromSeconds(-value)) : Add(FromSeconds(value));

        /// <summary>
        /// Adds the given number of years to this instance, and returns a result representing
        /// their combination, or <see cref="Zero"/> if the result would be a negative duration.
        /// </summary>
        /// <param name="value">A number of years.</param>
        /// <returns>A new <see cref="Duration"/> instance.</returns>
        /// <exception cref="OverflowException">Result gets a value for <see cref="AeonSequence"/>
        /// with more than <see cref="int.MaxValue"/> significant digits.</exception>
        public Duration AddYears(HugeNumber value)
            => value < 0 ? Subtract(FromYears(-value)) : Add(FromYears(value));

        /// <summary>
        /// Adds the given number of years to this instance, and returns a result representing
        /// their combination, or <see cref="Zero"/> if the result would be a negative duration.
        /// </summary>
        /// <param name="value">A number of years.</param>
        /// <returns>A new <see cref="Duration"/> instance.</returns>
        /// <exception cref="OverflowException">Result gets a value for <see cref="AeonSequence"/>
        /// with more than <see cref="int.MaxValue"/> significant digits.</exception>
        public Duration AddYears(decimal value)
            => value < 0 ? Subtract(FromYears(-value)) : Add(FromYears(value));

        /// <summary>
        /// Adds the given number of years to this instance, and returns a result representing
        /// their combination, or <see cref="Zero"/> if the result would be a negative duration.
        /// </summary>
        /// <param name="value">A number of years.</param>
        /// <returns>A new <see cref="Duration"/> instance.</returns>
        /// <exception cref="OverflowException">Result gets a value for <see cref="AeonSequence"/>
        /// with more than <see cref="int.MaxValue"/> significant digits.</exception>
        public Duration AddYears(double value)
            => value < 0 ? Subtract(FromYears(-value)) : Add(FromYears(value));

        /// <summary>
        /// Adds the given number of yoctoseconds to this instance, and returns a result
        /// representing their combination, or <see cref="Zero"/> if the result would be a negative
        /// duration.
        /// </summary>
        /// <param name="value">A number of yoctoseconds.</param>
        /// <returns>A new <see cref="Duration"/> instance.</returns>
        /// <exception cref="OverflowException">Result gets a value for <see cref="AeonSequence"/>
        /// with more than <see cref="int.MaxValue"/> significant digits.</exception>
        public Duration AddYoctoseconds(HugeNumber value)
            => value < 0 ? Subtract(FromYoctoseconds(-value)) : Add(FromYoctoseconds(value));

        /// <summary>
        /// Adds the given number of yoctoseconds to this instance, and returns a result
        /// representing their combination, or <see cref="Zero"/> if the result would be a negative
        /// duration.
        /// </summary>
        /// <param name="value">A number of yoctoseconds.</param>
        /// <returns>A new <see cref="Duration"/> instance.</returns>
        /// <exception cref="OverflowException">Result gets a value for <see cref="AeonSequence"/>
        /// with more than <see cref="int.MaxValue"/> significant digits.</exception>
        public Duration AddYoctoseconds(decimal value)
            => value < 0 ? Subtract(FromYoctoseconds(-value)) : Add(FromYoctoseconds(value));

        /// <summary>
        /// Adds the given number of yoctoseconds to this instance, and returns a result
        /// representing their combination, or <see cref="Zero"/> if the result would be a negative
        /// duration.
        /// </summary>
        /// <param name="value">A number of yoctoseconds.</param>
        /// <returns>A new <see cref="Duration"/> instance.</returns>
        /// <exception cref="OverflowException">Result gets a value for <see cref="AeonSequence"/>
        /// with more than <see cref="int.MaxValue"/> significant digits.</exception>
        public Duration AddYoctoseconds(double value)
            => value < 0 ? Subtract(FromYoctoseconds(-value)) : Add(FromYoctoseconds(value));

        /// <summary>
        /// Negates this instance.
        /// </summary>
        /// <returns>The negation of this instance.</returns>
        public Duration Negate()
            => new(
                !IsNegative,
                false,
                PlanckTime,
                TotalYoctoseconds,
                TotalNanoseconds,
                Years,
                AeonSequence?.ToList());

        /// <summary>
        /// Divides this instance by the given instance, and returns a <see cref="HugeNumber"/>.
        /// </summary>
        /// <param name="divisor">A <see cref="Duration"/> instance by which to divide this
        /// one.</param>
        /// <returns>The result of the division, as a double.</returns>
        /// <remarks>
        /// The division performed by this method is imprecise. Each <see cref="Duration"/>
        /// instance is converted to an equivalent floating-point value based on the lowest common
        /// unit of time which does not result in overflow to infinity. If all units up to years
        /// overflow, successive levels of aeons are divided. In the end overflow to infinity may be
        /// unavoidable if the duration of this instance is too much greater than that of the
        /// <paramref name="divisor"/>.
        /// </remarks>
        public HugeNumber Divide(Duration divisor)
        {
            if (IsZero)
            {
                if (divisor.IsZero)
                {
                    return HugeNumber.NaN;
                }
                else
                {
                    return 0;
                }
            }
            if (IsPerpetual || divisor == Zero)
            {
                return IsNegative
                    ? HugeNumber.NegativeInfinity
                    : HugeNumber.PositiveInfinity;
            }
            if (divisor.IsPerpetual)
            {
                return 0;
            }

            var first = ToPlanckTime();
            var second = divisor.ToPlanckTime();
            var result = HugeNumber.Zero;
            if (!first.IsInfinite && !second.IsInfinite)
            {
                result = first / second;
                if (!result.IsInfinite && !result.IsZero)
                {
                    return result;
                }
            }

            first = ToYoctoseconds();
            second = divisor.ToYoctoseconds();
            if (!first.IsInfinite && !second.IsInfinite)
            {
                result = first / second;
                if (!result.IsInfinite && !result.IsZero)
                {
                    return result;
                }
            }

            first = ToNanoseconds();
            second = divisor.ToNanoseconds();
            if (!first.IsInfinite && !second.IsInfinite)
            {
                result = first / second;
                if (!result.IsInfinite && !result.IsZero)
                {
                    return result;
                }
            }

            first = ToSeconds();
            second = divisor.ToSeconds();
            if (!first.IsInfinite && !second.IsInfinite)
            {
                result = first / second;
                if (!result.IsInfinite && !result.IsZero)
                {
                    return result;
                }
            }

            first = ToYears();
            second = divisor.ToYears();
            if (!first.IsInfinite && !second.IsInfinite)
            {
                result = first / second;
                if (!result.IsInfinite && !result.IsZero)
                {
                    return result;
                }
            }

            var skips = 0;
            var shift = 8;
            while ((first.IsInfinite || second.IsInfinite)
                && skips < (AeonSequence?.Count ?? 0) - 1
                && skips < (divisor.AeonSequence?.Count ?? 0) - 1)
            {
                first = AeonSequence!.Skip(skips).Select((x, i) => (value: x, index: i)).Sum(x => new HugeNumber(x.value, x.index + shift));
                second = divisor.AeonSequence!.Skip(skips).Select((x, i) => (value: x, index: i)).Sum(x => new HugeNumber(x.value, x.index + shift));

                if (shift > 0)
                {
                    shift--;
                }
                else
                {
                    skips++;
                }
            }

            return first / second; // May result in infinity, but at this point it can no longer be helped.
        }

        /// <summary>
        /// <para>
        /// Divides this instance by the given amount.
        /// </para>
        /// <para>
        /// Results in <see cref="Zero"/> if <see cref="IsZero"/> is <see langword="true"/>
        /// for this instance and <paramref name="divisor"/> is zero.
        /// </para>
        /// <para>
        /// Results in <see cref="PositiveInfinity"/> if <see cref="IsPerpetual"/> is <see langword="true"/>
        /// for this instance, or <paramref name="divisor"/> is zero.
        /// </para>
        /// </summary>
        /// <param name="divisor">An amount by which to divide this instance.</param>
        /// <returns>A new <see cref="Duration"/> instance.</returns>
        public Duration Divide(HugeNumber divisor)
        {
            if (IsZero)
            {
                return Zero;
            }
            if (IsPerpetual || divisor == 0)
            {
                return Sign != HugeNumber.Sign(divisor)
                    ? NegativeInfinity
                    : PositiveInfinity;
            }
            if (divisor.IsInfinite)
            {
                return Zero;
            }

            return Multiply(1 / divisor);
        }

        /// <summary>
        /// <para>
        /// Divides this instance by the given amount.
        /// </para>
        /// <para>
        /// Results in <see cref="Zero"/> if <see cref="IsZero"/> is <see langword="true"/>
        /// for this instance and <paramref name="divisor"/> is zero.
        /// </para>
        /// <para>
        /// Results in <see cref="PositiveInfinity"/> if <see cref="IsPerpetual"/> is <see langword="true"/>
        /// for this instance, or <paramref name="divisor"/> is zero.
        /// </para>
        /// </summary>
        /// <param name="divisor">An amount by which to divide this instance.</param>
        /// <returns>A new <see cref="Duration"/> instance.</returns>
        public Duration Divide(decimal divisor)
        {
            if (IsZero)
            {
                return Zero;
            }
            if (IsPerpetual || divisor == 0)
            {
                return Sign != Math.Sign(divisor)
                    ? NegativeInfinity
                    : PositiveInfinity;
            }

            return Multiply(1 / divisor);
        }

        /// <summary>
        /// <para>
        /// Divides this instance by the given amount.
        /// </para>
        /// <para>
        /// Results in <see cref="Zero"/> if <see cref="IsZero"/> is <see langword="true"/>
        /// for this instance and <paramref name="divisor"/> is zero.
        /// </para>
        /// <para>
        /// Results in <see cref="PositiveInfinity"/> if <see cref="IsPerpetual"/> is <see langword="true"/>
        /// for this instance, or <paramref name="divisor"/> is zero.
        /// </para>
        /// </summary>
        /// <param name="divisor">An amount by which to divide this instance.</param>
        /// <returns>A new <see cref="Duration"/> instance.</returns>
        public Duration Divide(double divisor)
        {
            if (IsZero)
            {
                return Zero;
            }
            if (IsPerpetual || divisor == 0)
            {
                return Sign != Math.Sign(divisor)
                    ? NegativeInfinity
                    : PositiveInfinity;
            }
            if (double.IsInfinity(divisor))
            {
                return Zero;
            }

            return Multiply(1 / divisor);
        }

        /// <summary>
        /// Finds the remainder of division of this instance by the given <paramref
        /// name="divisor"/>.
        /// </summary>
        /// <param name="divisor">A <see cref="Duration"/> instance by which to divide this
        /// one.</param>
        /// <returns>A new <see cref="Duration"/> instance.</returns>
        public Duration Modulus(Duration divisor)
            => Subtract(divisor.Multiply(HugeNumber.Floor(Divide(divisor))));

        /// <summary>
        /// Multiplies this instance by the given amount.
        /// </summary>
        /// <param name="factor">An amount by which to multiply this instance.</param>
        /// <returns>A new <see cref="Duration"/> instance.</returns>
        /// <exception cref="ArgumentException"><paramref name="factor"/> is <see
        /// cref="double.NaN"/>.</exception>
        /// <exception cref="OverflowException">Result gets a value for <see cref="AeonSequence"/>
        /// with more than <see cref="int.MaxValue"/> significant digits.</exception>
        public Duration Multiply(HugeNumber factor)
        {
            if (factor.IsNaN)
            {
                throw new ArgumentException($"{nameof(factor)} was NaN.");
            }
            if (factor.IsZero)
            {
                return Zero;
            }
            var isNegative = Sign != HugeNumber.Sign(factor);
            if (IsPerpetual || factor.IsInfinite)
            {
                return isNegative
                    ? NegativeInfinity
                    : PositiveInfinity;
            }
            factor = HugeNumber.Abs(factor);

            var newAeonSequence = AeonSequence?.ToList() ?? new List<ulong>();
            var a = HugeNumber.Zero;
            var y = HugeNumber.Zero;
            for (var index = 0; (a > 0 || index < (AeonSequence?.Count ?? 0)) && index <= int.MaxValue; index++)
            {
                a += (HugeNumber)newAeonSequence[index] * (index + 1) * factor;

                byte placeValue = 0;
                if (a > 0)
                {
                    if (a < 1)
                    {
                        var i = index - 1;
                        while (a < 1 && a > 0 && i >= 0)
                        {
                            a /= 10;
                            var rem = HugeNumber.Floor(a);
                            a -= rem;
                            newAeonSequence[i] += (byte)rem;
                            i--;
                        }
                        if (a > 0)
                        {
                            var rem = a * 1000000;
                            a = 0;
                            y += HugeNumber.Floor(rem);
                        }
                    }
                    else
                    {
                        var tens = new HugeNumber(1, index + 1);
                        placeValue = (byte)HugeNumber.Round(a / tens % 1 * 10);
                        a -= placeValue * (tens / 10);
                        a += placeValue;
                    }
                }

                newAeonSequence[index] = placeValue;
            }
            if (a > 0)
            {
                throw new OverflowException($"Provided parameters result in a value for ${nameof(AeonSequence)} with more than {nameof(Int32)}.{nameof(Int32.MaxValue)} significant digits.");
            }

            var d = new Duration(false, false, 0, 0, 0, 0, (newAeonSequence.Count == 0 || newAeonSequence.All(x => x == 0)) ? null : newAeonSequence);
            if (Years + y > 0)
            {
                d += FromYears((Years + y) * factor);
            }
            if (TotalNanoseconds > 0)
            {
                d += FromNanoseconds(TotalNanoseconds * factor);
            }
            if (TotalYoctoseconds > 0)
            {
                d += FromYoctoseconds(TotalYoctoseconds * factor);
            }
            if (PlanckTime != 0)
            {
                d += FromPlanckTime(PlanckTime * (decimal)factor);
            }
            if (isNegative)
            {
                d = d.Negate();
            }
            return d;
        }

        /// <summary>
        /// Multiplies this instance by the given amount.
        /// </summary>
        /// <param name="factor">An amount by which to multiply this instance.</param>
        /// <returns>A new <see cref="Duration"/> instance.</returns>
        /// <exception cref="OverflowException">Result gets a value for <see cref="AeonSequence"/>
        /// with more than <see cref="int.MaxValue"/> significant digits.</exception>
        public Duration Multiply(decimal factor)
        {
            if (factor == 0)
            {
                return Zero;
            }
            var isNegative = Sign != Math.Sign(factor);
            if (IsPerpetual)
            {
                return isNegative
                    ? NegativeInfinity
                    : PositiveInfinity;
            }
            factor = Math.Abs(factor);

            var newAeonSequence = AeonSequence?.ToList() ?? new List<ulong>();
            var a = 0.0m;
            var y = 0.0m;
            for (var index = 0; (a > 0 || index < (AeonSequence?.Count ?? 0)) && index <= int.MaxValue; index++)
            {
                a += (decimal)newAeonSequence[index] * (index + 1) * factor;

                byte placeValue = 0;
                if (a > 0)
                {
                    if (a < 1)
                    {
                        var i = index - 1;
                        while (a < 1 && a > 0 && i >= 0)
                        {
                            a /= 10;
                            var rem = Math.Floor(a);
                            a -= rem;
                            newAeonSequence[i] += (byte)rem;
                            i--;
                        }
                        if (a > 0)
                        {
                            var rem = a * 1000000;
                            a = 0;
                            y += Math.Floor(rem);
                        }
                    }
                    else
                    {
                        var tens = (decimal)Math.Pow(10, index + 1);
                        placeValue = (byte)Math.Round(a / tens % 1 * 10);
                        a -= placeValue * (tens / 10);
                        a += placeValue;
                    }
                }

                newAeonSequence[index] = placeValue;
            }
            if (a > 0)
            {
                throw new OverflowException($"Provided parameters result in a value for ${nameof(AeonSequence)} with more than {nameof(Int32)}.{nameof(Int32.MaxValue)} significant digits.");
            }

            var d = new Duration(false, false, 0, 0, 0, 0, (newAeonSequence.Count == 0 || newAeonSequence.All(x => x == 0)) ? null : newAeonSequence);
            if (Years + y > 0)
            {
                d += FromYears((Years + y) * factor);
            }
            if (TotalNanoseconds > 0)
            {
                d += FromNanoseconds(TotalNanoseconds * factor);
            }
            if (TotalYoctoseconds > 0)
            {
                d += FromYoctoseconds(TotalYoctoseconds * factor);
            }
            if (PlanckTime != 0)
            {
                d += FromPlanckTime(PlanckTime * factor);
            }
            if (isNegative)
            {
                d = d.Negate();
            }
            return d;
        }

        /// <summary>
        /// Multiplies this instance by the given amount.
        /// </summary>
        /// <param name="factor">An amount by which to multiply this instance.</param>
        /// <returns>A new <see cref="Duration"/> instance.</returns>
        /// <exception cref="ArgumentException"><paramref name="factor"/> is <see
        /// cref="double.NaN"/>.</exception>
        /// <exception cref="OverflowException">Result gets a value for <see cref="AeonSequence"/>
        /// with more than <see cref="int.MaxValue"/> significant digits.</exception>
        public Duration Multiply(double factor)
        {
            if (double.IsNaN(factor))
            {
                throw new ArgumentException($"{nameof(factor)} was NaN.");
            }
            if (factor.IsNearlyZero())
            {
                return Zero;
            }
            var isNegative = Sign != Math.Sign(factor);
            if (IsPerpetual || double.IsInfinity(factor))
            {
                return isNegative
                    ? NegativeInfinity
                    : PositiveInfinity;
            }
            factor = Math.Abs(factor);

            var newAeonSequence = AeonSequence?.ToList() ?? new List<ulong>();
            var a = 0.0;
            var y = 0.0;
            for (var index = 0; (a > 0 || index < (AeonSequence?.Count ?? 0)) && index <= int.MaxValue; index++)
            {
                a += (double)newAeonSequence[index] * (index + 1) * factor;

                byte placeValue = 0;
                if (a > 0)
                {
                    if (a < 1)
                    {
                        var i = index - 1;
                        while (a < 1 && a > 0 && i >= 0)
                        {
                            a /= 10;
                            var rem = Math.Floor(a);
                            a -= rem;
                            newAeonSequence[i] += (byte)rem;
                            i--;
                        }
                        if (a > 0)
                        {
                            var rem = a * 1000000;
                            a = 0;
                            y += Math.Floor(rem);
                        }
                    }
                    else
                    {
                        var tens = Math.Pow(10, index + 1);
                        placeValue = (byte)Math.Round(a / tens % 1 * 10);
                        a -= placeValue * (tens / 10);
                        a += placeValue;
                    }
                }

                newAeonSequence[index] = placeValue;
            }
            if (a > 0)
            {
                throw new OverflowException($"Provided parameters result in a value for ${nameof(AeonSequence)} with more than {nameof(Int32)}.{nameof(Int32.MaxValue)} significant digits.");
            }

            var d = new Duration(false, false, 0, 0, 0, 0, (newAeonSequence.Count == 0 || newAeonSequence.All(x => x == 0)) ? null : newAeonSequence);
            if (Years + y > 0)
            {
                d += FromYears((Years + y) * factor);
            }
            if (TotalNanoseconds > 0)
            {
                d += FromNanoseconds(TotalNanoseconds * factor);
            }
            if (TotalYoctoseconds > 0)
            {
                d += FromYoctoseconds(TotalYoctoseconds * factor);
            }
            if (PlanckTime != 0)
            {
                d += FromPlanckTime(PlanckTime * (decimal)factor);
            }
            if (isNegative)
            {
                d = d.Negate();
            }
            return d;
        }

        /// <summary>
        /// Subtracts the given <paramref name="duration"/> from this instance, and returns a result
        /// representing their combination, or <see cref="Zero"/> if their difference would result
        /// in a negative duration.
        /// </summary>
        /// <param name="duration">A <see cref="Duration"/>.</param>
        /// <returns>A new <see cref="Duration"/> instance.</returns>
        public Duration Subtract(Duration duration)
        {
            if (Equals(duration))
            {
                return Zero;
            }
            if (IsPerpetual)
            {
                if (duration.IsPerpetual)
                {
                    if (duration.Sign == Sign)
                    {
                        return Zero;
                    }
                    else
                    {
                        return this;
                    }
                }
                else
                {
                    return this;
                }
            }
            if (duration.IsPositiveInfinity)
            {
                return NegativeInfinity;
            }
            if (duration.IsNegative)
            {
                return PositiveInfinity;
            }
            if (duration > this)
            {
                return duration.Subtract(this).Negate();
            }

            var pt = PlanckTime - duration.PlanckTime;
            var ys = 0L;
            if (pt < 0)
            {
                ys = -1;
                pt += PlanckTimePerYoctosecond;
            }

            ys += (long)(TotalYoctoseconds - duration.TotalYoctoseconds);
            var ns = 0L;
            if (ys < 0)
            {
                ns = -1;
                ys += YoctosecondsPerNanosecond;
            }

            ns += (long)(TotalNanoseconds - duration.TotalNanoseconds);
            var y = 0L;
            if (ns < 0)
            {
                y = -1;
                ns += NanosecondsPerYear;
            }

            y += Years - duration.Years;
            var a = 0L;
            if (y < 0)
            {
                a = -1;
                y += YearsPerAeon;
            }

            var newAeonSequence = AeonSequence?.ToList() ?? new List<ulong>();
            var index = 0;
            while (index < (AeonSequence?.Count ?? 0) && index < (duration.AeonSequence?.Count ?? 0))
            {
                if (duration.AeonSequence![index] > newAeonSequence[index])
                {
                    newAeonSequence[index] = AeonUnit + newAeonSequence[index] - duration.AeonSequence![index];
                    if (a < 0)
                    {
                        newAeonSequence[index]--;
                    }
                    a = -1;
                }
                else
                {
                    newAeonSequence[index] -= duration.AeonSequence[index];
                    if (a < 0)
                    {
                        if (newAeonSequence[index] == 0)
                        {
                            newAeonSequence[index] = AeonUnit + newAeonSequence[index] - 1;
                        }
                        else
                        {
                            newAeonSequence[index]--;
                            a = 0;
                        }
                    }
                }
                index++;
            }
            while (index < (duration.AeonSequence?.Count ?? 0))
            {
                if (a < 0)
                {
                    if (duration.AeonSequence![index] == 0)
                    {
                        newAeonSequence.Add(MaxAeonSequence);
                    }
                    else
                    {
                        newAeonSequence.Add(duration.AeonSequence[index] - 1);
                        a = 0;
                    }
                }
                else
                {
                    newAeonSequence.Add(duration.AeonSequence![index]);
                }
                index++;
            }

            return new Duration(false, false, pt, (uint)ys, (uint)ns, (uint)y, (newAeonSequence.Count == 0 || newAeonSequence.All(x => x == 0)) ? null : newAeonSequence);
        }

        /// <summary>
        /// Subtracts the given number of seconds from this instance, and returns a result
        /// representing their combination.
        /// </summary>
        /// <param name="seconds">A number of seconds.</param>
        /// <returns>A new <see cref="Duration"/> instance.</returns>
        public Duration Subtract(HugeNumber seconds) => Subtract(FromSeconds(seconds));

        /// <summary>
        /// Subtracts the given number of seconds from this instance, and returns a result
        /// representing their combination.
        /// </summary>
        /// <param name="seconds">A number of seconds.</param>
        /// <returns>A new <see cref="Duration"/> instance.</returns>
        public Duration Subtract(decimal seconds) => Subtract(FromSeconds(seconds));

        /// <summary>
        /// Subtracts the given number of seconds from this instance, and returns a result
        /// representing their combination.
        /// </summary>
        /// <param name="seconds">A number of seconds.</param>
        /// <returns>A new <see cref="Duration"/> instance.</returns>
        public Duration Subtract(double seconds) => Subtract(FromSeconds(seconds));
    }
}
