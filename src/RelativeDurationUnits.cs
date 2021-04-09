using Tavenem.HugeNumbers;

namespace Tavenem.Time
{
    public partial struct RelativeDuration
    {
        /// <summary>
        /// A duration representing 1 attosecond. Read-only.
        /// </summary>
        public static readonly RelativeDuration OneAttosecond = new(Duration.OneAttosecond);

        /// <summary>
        /// A duration representing 1 femtosecond. Read-only.
        /// </summary>
        public static readonly RelativeDuration OneFemtosecond = new(Duration.OneFemtosecond);

        /// <summary>
        /// A duration representing 1 hour. Read-only.
        /// </summary>
        public static readonly RelativeDuration OneHour = new(Duration.OneHour);

        /// <summary>
        /// A duration representing 1 local day (1 rotation of the local planet around its axis). Read-only.
        /// </summary>
        public static readonly RelativeDuration OneLocalDay = new(1.0, RelativeDurationType.ProportionOfDay);

        /// <summary>
        /// A duration representing 1 local year (1 revolution of the local planet around its orbit). Read-only.
        /// </summary>
        public static readonly RelativeDuration OneLocalYear = new(1.0, RelativeDurationType.ProportionOfYear);

        /// <summary>
        /// A duration representing 1 microsecond. Read-only.
        /// </summary>
        public static readonly RelativeDuration OneMicrosecond = new(Duration.OneMicrosecond);

        /// <summary>
        /// A duration representing 1 millisecond. Read-only.
        /// </summary>
        public static readonly RelativeDuration OneMillisecond = new(Duration.OneMillisecond);

        /// <summary>
        /// A duration representing 1 minute. Read-only.
        /// </summary>
        public static readonly RelativeDuration OneMinute = new(Duration.OneMinute);

        /// <summary>
        /// A duration representing 1 nanosecond. Read-only.
        /// </summary>
        public static readonly RelativeDuration OneNanosecond = new(Duration.OneNanosecond);

        /// <summary>
        /// A duration representing 1 picosecond. Read-only.
        /// </summary>
        public static readonly RelativeDuration OnePicosecond = new(Duration.OnePicosecond);

        /// <summary>
        /// A duration representing 1 Planck time. Read-only.
        /// </summary>
        public static readonly RelativeDuration OnePlanckTime = new(Duration.OnePlanckTime);

        /// <summary>
        /// A duration representing 1 season of 1 local year (0.25 revolutions of the local planet around its orbit). Read-only.
        /// </summary>
        public static readonly RelativeDuration OneSeason = new(0.25, RelativeDurationType.ProportionOfYear);

        /// <summary>
        /// A duration representing 1s. Read-only.
        /// </summary>
        public static readonly RelativeDuration OneSecond = new(Duration.OneSecond);

        /// <summary>
        /// A duration representing 1 SI day (86400 seconds). Read-only.
        /// </summary>
        public static readonly RelativeDuration OneSIDay = new(Duration.OneDay);

        /// <summary>
        /// A duration representing 1 astronomical year (31557600 seconds). Read-only.
        /// </summary>
        public static readonly RelativeDuration OneSIYear = new(Duration.OneYear);

        /// <summary>
        /// A duration representing 1 yoctosecond. Read-only.
        /// </summary>
        public static readonly RelativeDuration OneYoctosecond = new(Duration.OneYoctosecond);

        /// <summary>
        /// A duration representing 1 zeptosecond. Read-only.
        /// </summary>
        public static readonly RelativeDuration OneZeptosecond = new(Duration.OneZeptosecond);

        /// <summary>
        /// Gets the approximate number of total aeons represented by this <see
        /// cref="RelativeDuration"/> (including fractional amounts), as a double, given the
        /// absolute durations of the local year and day.
        /// </summary>
        /// <param name="localYear">The duration of the local year.</param>
        /// <param name="localDay">The duration of the local day.</param>
        /// <returns>The approximate number of total aeons represented by this instance.</returns>
        /// <remarks>
        /// Note that if the total exceeds <see cref="HugeNumber.MaxValue"/>, this method
        /// may return <see cref="HugeNumber.PositiveInfinity"/> even for a <see
        /// cref="RelativeDuration"/> for which <see cref="IsPerpetual"/> is <see
        /// langword="false"/>.
        /// </remarks>
        public HugeNumber ToAeons(Duration localYear, Duration localDay)
            => ToUniversalDuration(localYear, localDay).ToAeons();

        /// <summary>
        /// Gets the approximate number of total attoseconds represented by this <see
        /// cref="RelativeDuration"/> (including fractional amounts), as a double, given the
        /// absolute durations of the local year and day.
        /// </summary>
        /// <param name="localYear">The duration of the local year.</param>
        /// <param name="localDay">The duration of the local day.</param>
        /// <returns>The approximate number of total attoseconds represented by this
        /// instance.</returns>
        /// <remarks>
        /// Note that if the total exceeds <see cref="HugeNumber.MaxValue"/>, this method
        /// may return <see cref="HugeNumber.PositiveInfinity"/> even for a <see
        /// cref="RelativeDuration"/> for which <see cref="IsPerpetual"/> is <see
        /// langword="false"/>.
        /// </remarks>
        public HugeNumber ToAttoseconds(Duration localYear, Duration localDay)
            => ToUniversalDuration(localYear, localDay).ToAttoseconds();

        /// <summary>
        /// Gets the approximate number of total days represented by this <see
        /// cref="RelativeDuration"/> (including fractional amounts), as a double, given the
        /// absolute durations of the local year and day.
        /// </summary>
        /// <param name="localYear">The duration of the local year.</param>
        /// <param name="localDay">The duration of the local day.</param>
        /// <returns>The approximate number of total days represented by this instance.</returns>
        /// <remarks>
        /// Note that if the total exceeds <see cref="HugeNumber.MaxValue"/>, this method
        /// may return <see cref="HugeNumber.PositiveInfinity"/> even for a <see
        /// cref="RelativeDuration"/> for which <see cref="IsPerpetual"/> is <see
        /// langword="false"/>.
        /// </remarks>
        public HugeNumber ToDays(Duration localYear, Duration localDay)
            => ToUniversalDuration(localYear, localDay).ToDays();

        /// <summary>
        /// Gets the approximate number of total femtoseconds represented by this <see
        /// cref="RelativeDuration"/> (including fractional amounts), as a double, given the
        /// absolute durations of the local year and day.
        /// </summary>
        /// <param name="localYear">The duration of the local year.</param>
        /// <param name="localDay">The duration of the local day.</param>
        /// <returns>The approximate number of total femtoseconds represented by this
        /// instance.</returns>
        /// <remarks>
        /// Note that if the total exceeds <see cref="HugeNumber.MaxValue"/>, this method
        /// may return <see cref="HugeNumber.PositiveInfinity"/> even for a <see
        /// cref="RelativeDuration"/> for which <see cref="IsPerpetual"/> is <see
        /// langword="false"/>.
        /// </remarks>
        public HugeNumber ToFemtoseconds(Duration localYear, Duration localDay)
            => ToUniversalDuration(localYear, localDay).ToFemtoseconds();

        /// <summary>
        /// Gets the approximate number of total hours represented by this <see
        /// cref="RelativeDuration"/> (including fractional amounts), as a double, given the
        /// absolute durations of the local year and day.
        /// </summary>
        /// <param name="localYear">The duration of the local year.</param>
        /// <param name="localDay">The duration of the local day.</param>
        /// <returns>The approximate number of total hours represented by this instance.</returns>
        /// <remarks>
        /// Note that if the total exceeds <see cref="HugeNumber.MaxValue"/>, this method
        /// may return <see cref="HugeNumber.PositiveInfinity"/> even for a <see
        /// cref="RelativeDuration"/> for which <see cref="IsPerpetual"/> is <see
        /// langword="false"/>.
        /// </remarks>
        public HugeNumber ToHours(Duration localYear, Duration localDay)
            => ToUniversalDuration(localYear, localDay).ToHours();

        /// <summary>
        /// Gets the approximate number of total microseconds represented by this <see
        /// cref="RelativeDuration"/> (including fractional amounts), as a double, given the
        /// absolute durations of the local year and day.
        /// </summary>
        /// <param name="localYear">The duration of the local year.</param>
        /// <param name="localDay">The duration of the local day.</param>
        /// <returns>The approximate number of total microseconds represented by this
        /// instance.</returns>
        /// <remarks>
        /// Note that if the total exceeds <see cref="HugeNumber.MaxValue"/>, this method
        /// may return <see cref="HugeNumber.PositiveInfinity"/> even for a <see
        /// cref="RelativeDuration"/> for which <see cref="IsPerpetual"/> is <see
        /// langword="false"/>.
        /// </remarks>
        public HugeNumber ToMicroseconds(Duration localYear, Duration localDay)
            => ToUniversalDuration(localYear, localDay).ToMicroseconds();

        /// <summary>
        /// Gets the approximate number of total milliseconds represented by this <see
        /// cref="RelativeDuration"/> (including fractional amounts), as a double, given the
        /// absolute durations of the local year and day.
        /// </summary>
        /// <param name="localYear">The duration of the local year.</param>
        /// <param name="localDay">The duration of the local day.</param>
        /// <returns>The approximate number of total milliseconds represented by this
        /// instance.</returns>
        /// <remarks>
        /// Note that if the total exceeds <see cref="HugeNumber.MaxValue"/>, this method
        /// may return <see cref="HugeNumber.PositiveInfinity"/> even for a <see
        /// cref="RelativeDuration"/> for which <see cref="IsPerpetual"/> is <see
        /// langword="false"/>.
        /// </remarks>
        public HugeNumber ToMilliseconds(Duration localYear, Duration localDay)
            => ToUniversalDuration(localYear, localDay).ToMilliseconds();

        /// <summary>
        /// Gets the approximate number of total minutes represented by this <see
        /// cref="RelativeDuration"/> (including fractional amounts), as a double, given the
        /// absolute durations of the local year and day.
        /// </summary>
        /// <param name="localYear">The duration of the local year.</param>
        /// <param name="localDay">The duration of the local day.</param>
        /// <returns>The approximate number of total minutes represented by this instance.</returns>
        /// <remarks>
        /// Note that if the total exceeds <see cref="HugeNumber.MaxValue"/>, this method
        /// may return <see cref="HugeNumber.PositiveInfinity"/> even for a <see
        /// cref="RelativeDuration"/> for which <see cref="IsPerpetual"/> is <see
        /// langword="false"/>.
        /// </remarks>
        public HugeNumber ToMinutes(Duration localYear, Duration localDay)
            => ToUniversalDuration(localYear, localDay).ToMinutes();

        /// <summary>
        /// Gets the approximate number of total nanoseconds represented by this <see
        /// cref="RelativeDuration"/> (including fractional amounts), as a double, given the
        /// absolute durations of the local year and day.
        /// </summary>
        /// <param name="localYear">The duration of the local year.</param>
        /// <param name="localDay">The duration of the local day.</param>
        /// <returns>The approximate number of total nanoseconds represented by this
        /// instance.</returns>
        /// <remarks>
        /// Note that if the total exceeds <see cref="HugeNumber.MaxValue"/>, this method
        /// may return <see cref="HugeNumber.PositiveInfinity"/> even for a <see
        /// cref="RelativeDuration"/> for which <see cref="IsPerpetual"/> is <see
        /// langword="false"/>.
        /// </remarks>
        public HugeNumber ToNanoseconds(Duration localYear, Duration localDay)
            => ToUniversalDuration(localYear, localDay).ToNanoseconds();

        /// <summary>
        /// Gets the approximate number of total picoseconds represented by this <see
        /// cref="RelativeDuration"/> (including fractional amounts), as a double, given the
        /// absolute durations of the local year and day.
        /// </summary>
        /// <param name="localYear">The duration of the local year.</param>
        /// <param name="localDay">The duration of the local day.</param>
        /// <returns>The approximate number of total picoseconds represented by this
        /// instance.</returns>
        /// <remarks>
        /// Note that if the total exceeds <see cref="HugeNumber.MaxValue"/>, this method
        /// may return <see cref="HugeNumber.PositiveInfinity"/> even for a <see
        /// cref="RelativeDuration"/> for which <see cref="IsPerpetual"/> is <see
        /// langword="false"/>.
        /// </remarks>
        public HugeNumber ToPicoseconds(Duration localYear, Duration localDay)
            => ToUniversalDuration(localYear, localDay).ToPicoseconds();

        /// <summary>
        /// Gets the approximate total Planck time represented by this <see
        /// cref="RelativeDuration"/> (including fractional amounts), as a double, given the
        /// absolute durations of the local year and day.
        /// </summary>
        /// <param name="localYear">The duration of the local year.</param>
        /// <param name="localDay">The duration of the local day.</param>
        /// <returns>The approximate total Planck time represented by this instance.</returns>
        /// <remarks>
        /// Note that if the total exceeds <see cref="HugeNumber.MaxValue"/>, this method
        /// may return <see cref="HugeNumber.PositiveInfinity"/> even for a <see
        /// cref="RelativeDuration"/> for which <see cref="IsPerpetual"/> is <see
        /// langword="false"/>.
        /// </remarks>
        public HugeNumber ToPlanckTime(Duration localYear, Duration localDay)
            => ToUniversalDuration(localYear, localDay).ToPlanckTime();

        /// <summary>
        /// Gets the approximate number of total seconds represented by this <see
        /// cref="RelativeDuration"/> (including fractional amounts), as a double, given the
        /// absolute durations of the local year and day.
        /// </summary>
        /// <param name="localYear">The duration of the local year.</param>
        /// <param name="localDay">The duration of the local day.</param>
        /// <returns>The approximate number of total seconds represented by this instance.</returns>
        /// <remarks>
        /// Note that if the total exceeds <see cref="HugeNumber.MaxValue"/>, this method
        /// may return <see cref="HugeNumber.PositiveInfinity"/> even for a <see
        /// cref="RelativeDuration"/> for which <see cref="IsPerpetual"/> is <see
        /// langword="false"/>.
        /// </remarks>
        public HugeNumber ToSeconds(Duration localYear, Duration localDay)
            => ToUniversalDuration(localYear, localDay).ToSeconds();

        /// <summary>
        /// Gets the approximate number of total years represented by this <see
        /// cref="RelativeDuration"/> (including fractional amounts), as a double, given the
        /// absolute durations of the local year and day.
        /// </summary>
        /// <param name="localYear">The duration of the local year.</param>
        /// <param name="localDay">The duration of the local day.</param>
        /// <returns>The approximate number of total years represented by this instance.</returns>
        /// <remarks>
        /// Note that if the total exceeds <see cref="HugeNumber.MaxValue"/>, this method
        /// may return <see cref="HugeNumber.PositiveInfinity"/> even for a <see
        /// cref="RelativeDuration"/> for which <see cref="IsPerpetual"/> is <see
        /// langword="false"/>.
        /// </remarks>
        public HugeNumber ToYears(Duration localYear, Duration localDay)
            => ToUniversalDuration(localYear, localDay).ToYears();

        /// <summary>
        /// Gets the approximate number of total yoctoseconds represented by this <see
        /// cref="RelativeDuration"/> (including fractional amounts), as a double, given the
        /// absolute durations of the local year and day.
        /// </summary>
        /// <param name="localYear">The duration of the local year.</param>
        /// <param name="localDay">The duration of the local day.</param>
        /// <returns>The approximate number of total aeons represented by this instance.</returns>
        /// <remarks>
        /// Note that if the total exceeds <see cref="HugeNumber.MaxValue"/>, this method
        /// may return <see cref="HugeNumber.PositiveInfinity"/> even for a <see
        /// cref="RelativeDuration"/> for which <see cref="IsPerpetual"/> is <see
        /// langword="false"/>.
        /// </remarks>
        public HugeNumber ToYoctoseconds(Duration localYear, Duration localDay)
            => ToUniversalDuration(localYear, localDay).ToYoctoseconds();

        /// <summary>
        /// Gets the approximate number of total zeptoseconds represented by this <see
        /// cref="RelativeDuration"/> (including fractional amounts), as a double, given the
        /// absolute durations of the local year and day.
        /// </summary>
        /// <param name="localYear">The duration of the local year.</param>
        /// <param name="localDay">The duration of the local day.</param>
        /// <returns>The approximate number of total zeptoseconds represented by this
        /// instance.</returns>
        /// <remarks>
        /// Note that if the total exceeds <see cref="HugeNumber.MaxValue"/>, this method
        /// may return <see cref="HugeNumber.PositiveInfinity"/> even for a <see
        /// cref="RelativeDuration"/> for which <see cref="IsPerpetual"/> is <see
        /// langword="false"/>.
        /// </remarks>
        public HugeNumber ToZeptoseconds(Duration localYear, Duration localDay)
            => ToUniversalDuration(localYear, localDay).ToZeptoseconds();
    }
}
