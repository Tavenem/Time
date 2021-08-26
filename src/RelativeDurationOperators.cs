namespace Tavenem.Time;

#pragma warning disable CS1591
public partial struct RelativeDuration
{
    public static bool operator ==(RelativeDuration first, RelativeDuration second) => first.Equals(second);

    public static bool operator ==(RelativeDuration first, Duration second) => first.Equals(second);

    public static bool operator ==(RelativeDuration first, DateTime second) => first.Equals(second);

    public static bool operator ==(RelativeDuration first, DateTimeOffset second) => first.Equals(second);

    public static bool operator ==(RelativeDuration first, TimeSpan second) => first.Equals(second);

    public static bool operator ==(Duration first, RelativeDuration second) => second.Equals(first);

    public static bool operator ==(DateTime first, RelativeDuration second) => second.Equals(first);

    public static bool operator ==(DateTimeOffset first, RelativeDuration second) => second.Equals(first);

    public static bool operator ==(TimeSpan first, RelativeDuration second) => second.Equals(first);

    public static bool operator !=(RelativeDuration first, RelativeDuration second) => !first.Equals(second);

    public static bool operator !=(RelativeDuration first, Duration second) => !first.Equals(second);

    public static bool operator !=(RelativeDuration first, DateTime second) => !first.Equals(second);

    public static bool operator !=(RelativeDuration first, DateTimeOffset second) => !first.Equals(second);

    public static bool operator !=(RelativeDuration first, TimeSpan second) => !first.Equals(second);

    public static bool operator !=(Duration first, RelativeDuration second) => !second.Equals(first);

    public static bool operator !=(DateTime first, RelativeDuration second) => !second.Equals(first);

    public static bool operator !=(DateTimeOffset first, RelativeDuration second) => !second.Equals(first);

    public static bool operator !=(TimeSpan first, RelativeDuration second) => !second.Equals(first);

    public static RelativeDuration operator *(RelativeDuration value, double factor)
        => value.Multiply(factor);

    public static RelativeDuration operator /(RelativeDuration dividend, double divisor)
        => dividend.Divide(divisor);

    public static explicit operator RelativeDuration(Duration value) => new(value);

    public static explicit operator RelativeDuration(DateTime value) => FromDateTime(value);

    public static explicit operator RelativeDuration(DateTimeOffset value) => FromDateTimeOffset(value);

    public static explicit operator RelativeDuration(TimeSpan value) => FromTimeSpan(value);
}
#pragma warning restore CS1591
