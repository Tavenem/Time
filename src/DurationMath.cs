using System.Numerics;
using Tavenem.Mathematics;

namespace Tavenem.Time;

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
    /// Gets the minimum of two <see cref="Duration"/> instances.
    /// </summary>
    /// <param name="first">The first instance.</param>
    /// <param name="second">The second instance.</param>
    /// <returns>The minimum of two <see cref="Duration"/> instances.</returns>
    public static Duration Min(Duration first, Duration second)
        => first.CompareTo(second) <= 0 ? first : second;

    /// <summary>
    /// Returns the absolute value of this instance: a positive amount of time with the same magnitude.
    /// </summary>
    /// <returns>The absolute value of this instance.</returns>
    public Duration Abs() => new(
        false,
        IsPerpetual,
        PlanckTime,
        TotalYoctoseconds,
        TotalNanoseconds,
        Years,
        Aeons);

    /// <summary>
    /// Adds the given <paramref name="duration"/> to this instance, and returns a result
    /// representing their combination.
    /// </summary>
    /// <param name="duration">A <see cref="Duration"/>.</param>
    /// <returns>A new <see cref="Duration"/> instance.</returns>
    public Duration Add(Duration duration)
    {
        if (IsPerpetual)
        {
            if (duration.IsPerpetual)
            {
                return duration.Sign == Sign
                    ? this
                    : Zero;
            }
            return this;
        }
        if (duration.IsPerpetual)
        {
            return duration;
        }
        if (IsZero)
        {
            return duration;
        }
        if (duration.IsZero)
        {
            return this;
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
                return duration.Subtract(Negate());
            }
        }
        else if (duration.IsNegative)
        {
            return Subtract(duration.Negate());
        }

        return new Duration(
            isNegative,
            aeons: Aeons + duration.Aeons,
            years: Years + duration.Years,
            nanoseconds: TotalNanoseconds + duration.TotalNanoseconds,
            yoctoseconds: TotalYoctoseconds + duration.TotalYoctoseconds,
            planckTime: PlanckTime + duration.PlanckTime);
    }

    /// <summary>
    /// Negates this instance.
    /// </summary>
    /// <returns>The negation of this instance.</returns>
    public Duration Negate() => new(
        !IsNegative,
        IsPerpetual,
        PlanckTime,
        TotalYoctoseconds,
        TotalNanoseconds,
        Years,
        Aeons);

    /// <summary>
    /// Divides this instance by the given instance, and returns a <see cref="double"/>.
    /// </summary>
    /// <param name="divisor">
    /// A <see cref="Duration"/> instance by which to divide this one.
    /// </param>
    /// <returns>The result of the division, as a double.</returns>
    /// <remarks>
    /// The division performed by this method is imprecise. Each <see cref="Duration"/>
    /// instance is converted to an equivalent floating-point value based on the lowest common
    /// unit of time which does not result in overflow to infinity. In the end overflow to infinity
    /// may be unavoidable if the duration of this instance is too much greater than that of the
    /// <paramref name="divisor"/>.
    /// </remarks>
    public double Divide(Duration divisor)
    {
        if (IsZero)
        {
            return divisor.IsZero
                ? double.NaN
                : 0;
        }
        if (IsPerpetual || divisor == Zero)
        {
            return IsNegative
                ? double.NegativeInfinity
                : double.PositiveInfinity;
        }
        if (divisor.IsPerpetual)
        {
            return 0;
        }

        var first = ToPlanckTime();
        var second = divisor.ToPlanckTime();
        double result;
        if (!double.IsInfinity(first)
            && !double.IsInfinity(second))
        {
            result = first / second;
            if (!double.IsInfinity(result)
                && !result.IsNearlyZero())
            {
                return result;
            }
        }

        first = ToYoctoseconds();
        second = divisor.ToYoctoseconds();
        if (!double.IsInfinity(first)
            && !double.IsInfinity(second))
        {
            result = first / second;
            if (!double.IsInfinity(result)
                && !result.IsNearlyZero())
            {
                return result;
            }
        }

        first = ToNanoseconds();
        second = divisor.ToNanoseconds();
        if (!double.IsInfinity(first)
            && !double.IsInfinity(second))
        {
            result = first / second;
            if (!double.IsInfinity(result)
                && !result.IsNearlyZero())
            {
                return result;
            }
        }

        first = ToSeconds();
        second = divisor.ToSeconds();
        if (!double.IsInfinity(first)
            && !double.IsInfinity(second))
        {
            result = first / second;
            if (!double.IsInfinity(result)
                && !result.IsNearlyZero())
            {
                return result;
            }
        }

        first = ToYears();
        second = divisor.ToYears();
        if (!double.IsInfinity(first)
            && !double.IsInfinity(second))
        {
            result = first / second;
            if (!double.IsInfinity(result)
                && !result.IsNearlyZero())
            {
                return result;
            }
        }

        first = ToAeons();
        second = divisor.ToAeons();
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
    /// <exception cref="ArgumentException">
    /// <paramref name="divisor"/> is <see cref="double.NaN"/>.
    /// </exception>
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
    public Duration DivideInteger<T>(T divisor) where T : INumber<T>
    {
        if (IsZero)
        {
            return Zero;
        }
        if (IsPerpetual || divisor == T.Zero)
        {
            return Sign != T.Sign(divisor)
                ? NegativeInfinity
                : PositiveInfinity;
        }

        return MultiplyInteger(1 / decimal.CreateChecked(divisor));
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
    /// <exception cref="ArgumentException">
    /// <paramref name="divisor"/> satisfies <see cref="INumberBase{TSelf}.IsNaN(TSelf)"/>.
    /// </exception>
    public Duration DivideFloatingPoint<T>(T divisor) where T : IFloatingPoint<T>
    {
        if (IsZero)
        {
            return Zero;
        }
        if (IsPerpetual || divisor == T.Zero)
        {
            return Sign != T.Sign(divisor)
                ? NegativeInfinity
                : PositiveInfinity;
        }
        if (T.IsInfinity(divisor))
        {
            return Zero;
        }

        return MultiplyFloatingPoint(T.One / divisor);
    }

    /// <summary>
    /// Finds the remainder of division of this instance by the given <paramref
    /// name="divisor"/>.
    /// </summary>
    /// <param name="divisor">A <see cref="Duration"/> instance by which to divide this
    /// one.</param>
    /// <returns>A new <see cref="Duration"/> instance.</returns>
    public Duration Modulus(Duration divisor)
    {
        var abs = Abs();
        var divisorAbs = divisor.Abs();
        var result = abs.Subtract(divisorAbs.Multiply(Math.Floor(abs.Divide(divisorAbs))));
        return IsNegative
            ? result.Negate()
            : result;
    }

    /// <summary>
    /// Multiplies this instance by the given amount.
    /// </summary>
    /// <param name="factor">An amount by which to multiply this instance.</param>
    /// <returns>A new <see cref="Duration"/> instance.</returns>
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

        BigInteger? newAeons = null;
        var y = 0.0m;
        if (Aeons.HasValue)
        {
            var ae = (decimal)Aeons.Value * factor;
            newAeons = (BigInteger)ae;
            y = ae % 1;
        }

        var d = new Duration(
            false,
            false,
            null,
            0,
            0,
            0,
            newAeons);
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
        if (PlanckTime.HasValue)
        {
            d += FromPlanckTime((BigInteger)((decimal)PlanckTime.Value * factor));
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
    /// <exception cref="ArgumentException">
    /// <paramref name="factor"/> is <see cref="double.NaN"/>.
    /// </exception>
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

        BigInteger? newAeons = null;
        var y = 0.0;
        if (Aeons.HasValue)
        {
            var ae = (double)Aeons.Value * factor;
            newAeons = (BigInteger)ae;
            y = ae % 1;
        }

        var d = new Duration(
            false,
            false,
            null,
            0,
            0,
            0,
            newAeons);
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
        if (PlanckTime.HasValue)
        {
            d += FromPlanckTime((BigInteger)((double)PlanckTime.Value * factor));
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
    public Duration MultiplyInteger<T>(T factor) where T : INumber<T>
    {
        if (factor == T.Zero)
        {
            return Zero;
        }
        var isNegative = Sign != T.Sign(factor);
        if (IsPerpetual)
        {
            return isNegative
                ? NegativeInfinity
                : PositiveInfinity;
        }
        factor = T.Abs(factor);

        BigInteger? newAeons = null;
        var biFactor = Aeons.HasValue || PlanckTime.HasValue
            ? BigInteger.Parse(factor.ToString("F0", System.Globalization.CultureInfo.InvariantCulture))
            : BigInteger.Zero;
        if (Aeons.HasValue)
        {
            newAeons = Aeons.Value * biFactor;
        }

        var d = new Duration(
            false,
            false,
            null,
            0,
            0,
            0,
            newAeons);
        if (Years > 0)
        {
            d += FromYearsInteger(T.CreateChecked(Years) * factor);
        }
        if (TotalNanoseconds > 0)
        {
            d += FromNanosecondsInteger(T.CreateChecked(TotalNanoseconds) * factor);
        }
        if (TotalYoctoseconds > 0)
        {
            d += FromYoctosecondsInteger(T.CreateChecked(TotalYoctoseconds) * factor);
        }
        if (PlanckTime.HasValue)
        {
            d += FromPlanckTime(PlanckTime.Value * biFactor);
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
    /// <exception cref="ArgumentException">
    /// <paramref name="factor"/> satisfies <see cref="INumberBase{TSelf}.IsNaN(TSelf)"/>.
    /// </exception>
    public Duration MultiplyFloatingPoint<T>(T factor) where T : IFloatingPoint<T>
    {
        if (T.IsNaN(factor))
        {
            throw new ArgumentException($"{nameof(factor)} was NaN.");
        }
        if (factor == T.Zero)
        {
            return Zero;
        }
        var isNegative = Sign != T.Sign(factor);
        if (IsPerpetual || T.IsInfinity(factor))
        {
            return isNegative
                ? NegativeInfinity
                : PositiveInfinity;
        }
        factor = T.Abs(factor);

        BigInteger? newAeons = null;
        var y = 0.0m;
        if (Aeons.HasValue)
        {
            var ae = (decimal)Aeons.Value * decimal.CreateChecked(factor);
            newAeons = (BigInteger)ae;
            y = ae % 1;
        }

        var d = new Duration(
            false,
            false,
            null,
            0,
            0,
            0,
            newAeons);
        if (Years + y > 0)
        {
            d += FromYearsFloatingPoint(T.CreateChecked(Years + y) * factor);
        }
        if (TotalNanoseconds > 0)
        {
            d += FromNanosecondsFloatingPoint(T.CreateChecked(TotalNanoseconds) * factor);
        }
        if (TotalYoctoseconds > 0)
        {
            d += FromYoctosecondsFloatingPoint(T.CreateChecked(TotalYoctoseconds) * factor);
        }
        if (PlanckTime.HasValue)
        {
            d += FromPlanckTime((BigInteger)((decimal)PlanckTime.Value * decimal.CreateChecked(factor)));
        }
        if (isNegative)
        {
            d = d.Negate();
        }
        return d;
    }

    /// <summary>
    /// Subtracts the given <paramref name="duration"/> from this instance, and returns a result
    /// representing their combination.
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
                return duration.Sign == Sign
                    ? Zero
                    : this;
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
        if (IsZero)
        {
            return duration.Negate();
        }
        if (duration.IsZero)
        {
            return this;
        }

        if (duration.IsNegative)
        {
            return Add(duration.Negate());
        }

        if (IsNegative)
        {
            return Negate().Add(duration).Negate();
        }

        if (duration > this)
        {
            return duration.Subtract(this).Negate();
        }

        BigInteger? ae = null;
        if (Aeons.HasValue || duration.Aeons.HasValue)
        {
            ae = (ae ?? BigInteger.Zero) + ((Aeons ?? BigInteger.Zero) - (duration.Aeons ?? BigInteger.Zero));
        }

        var y = (long)Years - duration.Years;
        if (y < 0 && ae > BigInteger.Zero)
        {
            ae = ae.Value - BigInteger.One;
            y += YearsPerAeon;
        }

        var ns = (long)TotalNanoseconds - (long)duration.TotalNanoseconds;
        if (ns < 0 && y > 0)
        {
            y--;
            ns += NanosecondsPerYear;
        }

        var ys = (long)TotalYoctoseconds - (long)duration.TotalYoctoseconds;
        if (ys < 0 && ns > 0)
        {
            ns--;
            ys += YoctosecondsPerNanosecond;
        }

        BigInteger? pt = null;
        if (PlanckTime.HasValue || duration.PlanckTime.HasValue)
        {
            pt = (PlanckTime ?? BigInteger.Zero) - (duration.PlanckTime ?? BigInteger.Zero);
            if (pt.HasValue && pt < 0)
            {
                ys = -1;
                pt = pt.Value + PlanckTimePerYoctosecond;
            }
        }

        var isNegative = false;
        if (ae.HasValue && ae < BigInteger.Zero)
        {
            isNegative = true;
            ae = BigInteger.Abs(ae.Value);
        }

        return new Duration(
            isNegative,
            aeons: ae,
            years: (uint)y,
            nanoseconds: (ulong)ns,
            yoctoseconds: (ulong)ys,
            planckTime: pt);
    }
}
