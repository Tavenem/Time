using System;
using Tavenem.HugeNumbers;

namespace Tavenem.Time
{
    public partial struct Duration
    {
#pragma warning disable CS1591

        public static bool operator ==(Duration first, Duration second) => first.Equals(second);

        public static bool operator ==(Duration first, DateTime second) => first.Equals(second);

        public static bool operator ==(DateTime first, Duration second) => second.Equals(first);

        public static bool operator ==(Duration first, DateTimeOffset second) => first.Equals(second);

        public static bool operator ==(DateTimeOffset first, Duration second) => second.Equals(first);

        public static bool operator ==(TimeSpan first, Duration second) => second.Equals(first);

        public static bool operator !=(Duration first, Duration second) => !first.Equals(second);

        public static bool operator !=(Duration first, DateTime second) => !first.Equals(second);

        public static bool operator !=(DateTime first, Duration second) => !second.Equals(first);

        public static bool operator !=(Duration first, DateTimeOffset second) => !first.Equals(second);

        public static bool operator !=(DateTimeOffset first, Duration second) => !second.Equals(first);

        public static bool operator !=(TimeSpan first, Duration second) => !second.Equals(first);

        public static Duration operator +(Duration first, Duration second)
            => first.Add(second);

        public static Duration operator +(Duration? first, Duration second)
            => (first ?? Zero).Add(second);

        public static Duration operator +(Duration first, Duration? second)
            => first.Add(second ?? Zero);

        public static Duration? operator +(Duration? first, Duration? second)
            => first.HasValue || second.HasValue
            ? (first ?? Zero).Add(second ?? Zero)
            : null;

        public static Duration operator +(Duration first, HugeNumber second)
            => first.Add(second);

        public static Duration operator +(Duration? first, HugeNumber second)
            => (first ?? Zero).Add(second);

        public static Duration operator +(Duration first, HugeNumber? second)
            => first.Add(second ?? HugeNumber.Zero);

        public static Duration? operator +(Duration? first, HugeNumber? second)
            => first.HasValue || second.HasValue
            ? (first ?? Zero).Add(second ?? HugeNumber.Zero)
            : null;

        public static Duration operator +(Duration first, decimal second)
            => first.Add(second);

        public static Duration operator +(Duration? first, decimal second)
            => (first ?? Zero).Add(second);

        public static Duration operator +(Duration first, decimal? second)
            => first.Add(second ?? 0);

        public static Duration? operator +(Duration? first, decimal? second)
            => first.HasValue || second.HasValue
            ? (first ?? Zero).Add(second ?? 0)
            : null;

        public static Duration operator +(Duration first, double second)
            => first.Add(second);

        public static Duration operator +(Duration? first, double second)
            => (first ?? Zero).Add(second);

        public static Duration operator +(Duration first, double? second)
            => first.Add(second ?? 0);

        public static Duration? operator +(Duration? first, double? second)
            => first.HasValue || second.HasValue
            ? (first ?? Zero).Add(second ?? 0)
            : null;

        public static Duration operator -(Duration first, Duration second)
            => first.Subtract(second);

        public static Duration operator -(Duration? first, Duration second)
            => (first ?? Zero).Subtract(second);

        public static Duration operator -(Duration first, Duration? second)
            => first.Subtract(second ?? Zero);

        public static Duration? operator -(Duration? first, Duration? second)
            => first.HasValue || second.HasValue
            ? (first ?? Zero).Subtract(second ?? Zero)
            : null;

        public static Duration operator -(Duration first, HugeNumber second)
            => first.Subtract(second);

        public static Duration operator -(Duration? first, HugeNumber second)
            => (first ?? Zero).Subtract(second);

        public static Duration operator -(Duration first, HugeNumber? second)
            => first.Subtract(second ?? HugeNumber.Zero);

        public static Duration? operator -(Duration? first, HugeNumber? second)
            => first.HasValue || second.HasValue
            ? (first ?? Zero).Subtract(second ?? HugeNumber.Zero)
            : null;

        public static Duration operator -(Duration first, decimal second)
            => first.Subtract(second);

        public static Duration operator -(Duration? first, decimal second)
            => (first ?? Zero).Subtract(second);

        public static Duration operator -(Duration first, decimal? second)
            => first.Subtract(second ?? 0);

        public static Duration? operator -(Duration? first, decimal? second)
            => first.HasValue || second.HasValue
            ? (first ?? Zero).Subtract(second ?? 0)
            : null;

        public static Duration operator -(Duration first, double second)
            => first.Subtract(second);

        public static Duration operator -(Duration? first, double second)
            => (first ?? Zero).Subtract(second);

        public static Duration operator -(Duration first, double? second)
            => first.Subtract(second ?? 0);

        public static Duration? operator -(Duration? first, double? second)
            => first.HasValue || second.HasValue
            ? (first ?? Zero).Subtract(second ?? 0)
            : null;

        public static Duration operator *(Duration value, HugeNumber factor)
            => value.Multiply(factor);

        public static Duration operator *(HugeNumber factor, Duration value)
            => value.Multiply(factor);

        public static Duration operator *(Duration value, decimal factor)
            => value.Multiply(factor);

        public static Duration operator *(decimal factor, Duration value)
            => value.Multiply(factor);

        public static Duration operator *(Duration value, double factor)
            => value.Multiply(factor);

        public static Duration operator *(double factor, Duration value)
            => value.Multiply(factor);

        public static HugeNumber operator /(Duration dividend, Duration divisor)
            => dividend.Divide(divisor);

        public static Duration operator /(Duration dividend, HugeNumber divisor)
            => dividend.Divide(divisor);

        public static Duration operator /(Duration dividend, decimal divisor)
            => dividend.Divide(divisor);

        public static Duration operator /(Duration dividend, double divisor)
            => dividend.Divide(divisor);

        public static Duration operator %(Duration dividend, Duration divisor)
            => dividend.Modulus(divisor);

        public static bool operator <(Duration first, Duration second)
            => first.CompareTo(second) < 0;

        public static bool operator >(Duration first, Duration second)
            => first.CompareTo(second) > 0;

        public static bool operator <=(Duration first, Duration second)
            => first.CompareTo(second) <= 0;

        public static bool operator >=(Duration first, Duration second)
            => first.CompareTo(second) >= 0;

        public static bool operator <(Duration first, DateTime second)
            => first.CompareTo(second) < 0;

        public static bool operator >(Duration first, DateTime second)
            => first.CompareTo(second) > 0;

        public static bool operator <=(Duration first, DateTime second)
            => first.CompareTo(second) <= 0;

        public static bool operator >=(Duration first, DateTime second)
            => first.CompareTo(second) >= 0;

        public static bool operator <(DateTime first, Duration second)
            => second.CompareTo(first) >= 0;

        public static bool operator >(DateTime first, Duration second)
            => second.CompareTo(first) <= 0;

        public static bool operator <=(DateTime first, Duration second)
            => second.CompareTo(first) > 0;

        public static bool operator >=(DateTime first, Duration second)
            => second.CompareTo(first) < 0;

        public static bool operator <(Duration first, DateTimeOffset second)
            => first.CompareTo(second) < 0;

        public static bool operator >(Duration first, DateTimeOffset second)
            => first.CompareTo(second) > 0;

        public static bool operator <=(Duration first, DateTimeOffset second)
            => first.CompareTo(second) <= 0;

        public static bool operator >=(Duration first, DateTimeOffset second)
            => first.CompareTo(second) >= 0;

        public static bool operator <(DateTimeOffset first, Duration second)
            => second.CompareTo(first) >= 0;

        public static bool operator >(DateTimeOffset first, Duration second)
            => second.CompareTo(first) <= 0;

        public static bool operator <=(DateTimeOffset first, Duration second)
            => second.CompareTo(first) > 0;

        public static bool operator >=(DateTimeOffset first, Duration second)
            => second.CompareTo(first) < 0;

        public static bool operator <(Duration first, TimeSpan second)
            => first.CompareTo(second) < 0;

        public static bool operator >(Duration first, TimeSpan second)
            => first.CompareTo(second) > 0;

        public static bool operator <=(Duration first, TimeSpan second)
            => first.CompareTo(second) <= 0;

        public static bool operator >=(Duration first, TimeSpan second)
            => first.CompareTo(second) >= 0;

        public static bool operator <(TimeSpan first, Duration second)
            => second.CompareTo(first) >= 0;

        public static bool operator >(TimeSpan first, Duration second)
            => second.CompareTo(first) <= 0;

        public static bool operator <=(TimeSpan first, Duration second)
            => second.CompareTo(first) > 0;

        public static bool operator >=(TimeSpan first, Duration second)
            => second.CompareTo(first) < 0;

        public static explicit operator Duration(DateTime value) => FromDateTime(value);

        public static explicit operator DateTime(Duration value) => value.ToDateTime();

        public static explicit operator Duration(DateTimeOffset value) => FromDateTimeOffset(value);

        public static explicit operator DateTimeOffset(Duration value) => value.ToDateTimeOffset();

        public static explicit operator Duration(TimeSpan value) => FromTimeSpan(value);

        public static explicit operator TimeSpan(Duration value) => value.ToTimeSpan();

#pragma warning restore CS1591
    }
}
