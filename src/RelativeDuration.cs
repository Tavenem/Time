using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Tavenem.HugeNumbers;

namespace Tavenem.Time
{
    /// <summary>
    /// <para>
    /// Represents a duration which can be either absolute (<seealso
    /// cref="Duration"/>), or relative to the duration of a local year or day (i.e.
    /// the revolution or rotation period of a planet), when the actual durations of the local year
    /// and/or day are unknown. For example, "one quarter of one day" can be represented in a
    /// uniform, consistent way even when the length of a day might vary from place to place.
    /// Capable of accurately representing extremely large and small time intervals, suitable for
    /// time scales such as the age of a universe, with precision down to Planck time.
    /// </para>
    /// <para>
    /// Also able to represent an infinite (perpetual) duration.
    /// </para>
    /// </summary>
    [Serializable]
    [DataContract]
    [JsonConverter(typeof(RelativeDurationConverter))]
    public partial struct RelativeDuration :
        IEquatable<RelativeDuration>,
        IEquatable<Duration>,
        IEquatable<DateTime>,
        IEquatable<TimeSpan>,
        ISerializable
    {
        private const string DayString = "Dx";
        private const string YearString = "Yx";

        /// <summary>
        /// A duration with the maximum value that can be represented without being infinite.
        /// Read-only.
        /// </summary>
        public static readonly RelativeDuration MaxValue = new(Duration.MaxValue);

        /// <summary>
        /// A duration representing an infinite amount of time into the past. Read-only.
        /// </summary>
        public static readonly RelativeDuration NegativeInfinity = new(Duration.NegativeInfinity);

        /// <summary>
        /// A duration representing an infinite amount of time. Read-only.
        /// </summary>
        public static readonly RelativeDuration PositiveInfinity = new(Duration.PositiveInfinity);

        /// <summary>
        /// A duration representing no time. Read-only.
        /// </summary>
        public static readonly RelativeDuration Zero = new(Duration.Zero);

        /// <summary>
        /// The absolute duration represented by this instance. Will be <see cref="Duration.Zero"/>
        /// if <see cref="Relativity"/> is not <see cref="RelativeDurationType.Absolute"/>.
        /// </summary>
        [DataMember(Order = 1)]
        public Duration Duration { get; }

        /// <summary>
        /// Indicates that this value represents an infinite amount of time.
        /// </summary>
        public bool IsPerpetual => Relativity == RelativeDurationType.Absolute && Duration.IsPerpetual;

        /// <summary>
        /// Whether this value represents zero time.
        /// </summary>
        public bool IsZero => Relativity == RelativeDurationType.Absolute ? Duration.IsZero : Proportion == 0;

        /// <summary>
        /// The proportion of a local day or year represented by this instance. Will be 0 if <see
        /// cref="Relativity"/> is <see cref="RelativeDurationType.Absolute"/>.
        /// </summary>
        [DataMember(Order = 2)]
        public double Proportion { get; }

        /// <summary>
        /// <para>
        /// Indicates the type of time measurement used by this instance: absolute, relative to a
        /// local day, or relative to a local year.
        /// </para>
        /// <para>
        /// Determines whether <see cref="Duration"/> or <see cref="Proportion"/> is used to
        /// determine the duration represented by this instance.
        /// </para>
        /// </summary>
        [DataMember(Order = 3)]
        public RelativeDurationType Relativity { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="RelativeDuration"/> with the given absolute
        /// <see cref="Duration"/>.
        /// </summary>
        /// <param name="absoluteDuration">A <see cref="Duration"/> to set as the absolute
        /// value of this instance.</param>
        public RelativeDuration(Duration absoluteDuration)
        {
            Duration = absoluteDuration;
            Proportion = 0;
            Relativity = RelativeDurationType.Absolute;
        }

        private RelativeDuration(HugeNumber proportion, RelativeDurationType timeType)
        {
            Duration = Duration.Zero;
            Proportion = (double)HugeNumber.Max(0, proportion);
            Relativity = timeType;
        }

        private RelativeDuration(decimal proportion, RelativeDurationType timeType)
        {
            Duration = Duration.Zero;
            Proportion = (double)Math.Max(0, proportion);
            Relativity = timeType;
        }

        private RelativeDuration(double proportion, RelativeDurationType timeType)
        {
            Duration = Duration.Zero;
            Proportion = Math.Max(0, proportion);
            Relativity = timeType;
        }

        private RelativeDuration(Duration absoluteDuration, double proportion, RelativeDurationType timeType)
        {
            Duration = absoluteDuration;
            Proportion = proportion;
            Relativity = timeType;
        }

        private RelativeDuration(SerializationInfo info, StreamingContext context) : this(
            (Duration?)info.GetValue(nameof(Duration), typeof(Duration)) ?? default,
            (double?)info.GetValue(nameof(Proportion), typeof(double)) ?? default,
            (RelativeDurationType?)info.GetValue(nameof(Relativity), typeof(RelativeDurationType)) ?? RelativeDurationType.Absolute)
        { }

        /// <summary>
        /// Gets the maximum of two <see cref="RelativeDuration"/> instances.
        /// </summary>
        /// <param name="first">The first instance.</param>
        /// <param name="localYearFirst">The duration of the local year for <paramref
        /// name="first"/>.</param>
        /// <param name="localDayFirst">The duration of the local day for <paramref name="first"/>.</param>
        /// <param name="second">The second instance.</param>
        /// <param name="localYearSecond">The duration of the local year for <paramref
        /// name="second"/>.</param>
        /// <param name="localDaySecond">The duration of the local day for <paramref name="second"/>.</param>
        /// <returns>The maximum of two <see cref="RelativeDuration"/> instances.</returns>
        public static RelativeDuration Max(
            RelativeDuration first, Duration localYearFirst, Duration localDayFirst,
            RelativeDuration second, Duration localYearSecond, Duration localDaySecond)
            => first.ToUniversalDuration(localYearFirst, localDayFirst)
            .CompareTo(second.ToUniversalDuration(localYearSecond, localDaySecond)) >= 0 ? first : second;

        /// <summary>
        /// Gets the maximum of a <see cref="RelativeDuration"/> instance and a number of seconds.
        /// </summary>
        /// <param name="first">The first instance.</param>
        /// <param name="localYear">The duration of the local year.</param>
        /// <param name="localDay">The duration of the local day.</param>
        /// <param name="seconds">A number of seconds.</param>
        /// <returns>The maximum of a <see cref="RelativeDuration"/> instance and a number of
        /// seconds.</returns>
        public static RelativeDuration Max(RelativeDuration first, Duration localYear, Duration localDay, double seconds)
            => first.ToUniversalDuration(localYear, localDay).CompareTo(seconds) >= 0 ? first : RelativeDuration.FromSeconds(seconds);

        /// <summary>
        /// Gets the minimum of two <see cref="RelativeDuration"/> instances.
        /// </summary>
        /// <param name="first">The first instance.</param>
        /// <param name="localYearFirst">The duration of the local year for <paramref
        /// name="first"/>.</param>
        /// <param name="localDayFirst">The duration of the local day for <paramref name="first"/>.</param>
        /// <param name="second">The second instance.</param>
        /// <param name="localYearSecond">The duration of the local year for <paramref
        /// name="second"/>.</param>
        /// <param name="localDaySecond">The duration of the local day for <paramref name="second"/>.</param>
        /// <returns>The minimum of two <see cref="RelativeDuration"/> instances.</returns>
        public static RelativeDuration Min(
            RelativeDuration first, Duration localYearFirst, Duration localDayFirst,
            RelativeDuration second, Duration localYearSecond, Duration localDaySecond)
            => first.ToUniversalDuration(localYearFirst, localDayFirst)
            .CompareTo(second.ToUniversalDuration(localYearSecond, localDaySecond)) <= 0 ? first : second;

        /// <summary>
        /// Gets the minimum of a <see cref="RelativeDuration"/> instance and a number of seconds.
        /// </summary>
        /// <param name="first">The first instance.</param>
        /// <param name="localYear">The duration of the local year, in seconds.</param>
        /// <param name="localDay">The duration of the local day, in seconds.</param>
        /// <param name="seconds">A number of seconds.</param>
        /// <returns>The minimum of a <see cref="RelativeDuration"/> instance and a number of
        /// seconds.</returns>
        public static RelativeDuration Min(RelativeDuration first, Duration localYear, Duration localDay, double seconds)
            => first.ToUniversalDuration(localYear, localDay).CompareTo(seconds) <= 0 ? first : FromSeconds(seconds);

        /// <summary>
        /// <para>
        /// Divides this instance by the given amount.
        /// </para>
        /// <para>
        /// Results in <see cref="Zero"/> if this <see cref="IsZero"/> is <see langword="true"/>
        /// for this instance and <paramref name="divisor"/> is zero.
        /// </para>
        /// <para>
        /// Results in <see cref="PositiveInfinity"/> if <see cref="IsPerpetual"/> is <see langword="true"/>
        /// for this instance, or <paramref name="divisor"/> is zero.
        /// </para>
        /// <para>
        /// Results in <see cref="Zero"/> if <paramref name="divisor"/> is negative or <see
        /// cref="double.PositiveInfinity"/>.
        /// </para>
        /// </summary>
        /// <param name="divisor">An amount by which to divide this instance.</param>
        /// <returns>A new <see cref="RelativeDuration"/> instance.</returns>
        public RelativeDuration Divide(double divisor)
        {
            if (IsZero)
            {
                return Zero;
            }
            if (IsPerpetual || divisor == 0)
            {
                return PositiveInfinity;
            }
            if (divisor < 0 || double.IsInfinity(divisor))
            {
                return Zero;
            }

            return Multiply(1 / divisor);
        }

        /// <summary>
        /// Gets the approximate number of total seconds represented by this <see
        /// cref="RelativeDuration"/> (including fractional amounts), as a double.
        /// </summary>
        /// <param name="localYear">The duration of the local year, in seconds.</param>
        /// <param name="localDay">The duration of the local day, in seconds.</param>
        /// <returns>The approximate number of total seconds represented by this instance.</returns>
        /// <remarks>
        /// Note that if the total exceeds <see cref="HugeNumber.MaxValue"/>, this method
        /// may return
        /// <see cref="HugeNumber.PositiveInfinity"/>.
        /// </remarks>
        public HugeNumber GetTotalSeconds(Duration localYear, Duration localDay)
            => ToUniversalDuration(localYear, localDay).ToSeconds();

        /// <summary>
        /// Gets the approximate number of total years represented by this <see
        /// cref="RelativeDuration"/> (including fractional amounts), as a double.
        /// </summary>
        /// <param name="localYear">The duration of the local year, in seconds.</param>
        /// <param name="localDay">The duration of the local day, in seconds.</param>
        /// <returns>The approximate number of total years represented by this instance.</returns>
        /// <remarks>
        /// Note that if the total exceeds <see cref="HugeNumber.MaxValue"/>, this method
        /// may return
        /// <see cref="HugeNumber.PositiveInfinity"/>.
        /// </remarks>
        public HugeNumber GetTotalYears(Duration localYear, Duration localDay)
            => ToUniversalDuration(localYear, localDay).ToYears();

        /// <summary>
        /// <para>
        /// Multiplies this instance by the given amount.
        /// </para>
        /// <para>
        /// Results in <see cref="Zero"/> if <paramref name="factor"/> is negative.
        /// </para>
        /// </summary>
        /// <param name="factor">An amount by which to multiply this instance.</param>
        /// <returns>A new <see cref="RelativeDuration"/> instance.</returns>
        /// <exception cref="ArgumentException"><paramref name="factor"/> is <see
        /// cref="double.NaN"/>.</exception>
        /// <exception cref="OverflowException">Result gets a value for <see
        /// cref="Duration.AeonSequence"/> with more than <see cref="int.MaxValue"/> significant
        /// digits.</exception>
        public RelativeDuration Multiply(double factor)
        {
            if (double.IsNaN(factor))
            {
                throw new ArgumentException($"{nameof(factor)} was NaN.");
            }
            if (factor <= 0)
            {
                return Zero;
            }
            if (factor == 0)
            {
                return Zero;
            }
            if (IsPerpetual || double.IsPositiveInfinity(factor))
            {
                return PositiveInfinity;
            }

            return Relativity switch
            {
                RelativeDurationType.Absolute => new RelativeDuration(Duration * factor),
                RelativeDurationType.ProportionOfDay => new RelativeDuration(Proportion * factor, RelativeDurationType.ProportionOfDay),
                RelativeDurationType.ProportionOfYear => new RelativeDuration(Proportion * factor, RelativeDurationType.ProportionOfYear),
                _ => Zero,
            };
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns>
        /// <see langword="true"/> if the object and the current instance are equal; otherwise <see
        /// langword="false"/>.
        /// </returns>
        public override bool Equals(object? obj) => obj is RelativeDuration other && Equals(other);

        /// <summary>
        /// Indicates whether this <see cref="RelativeDuration"/> instance and another are equal.
        /// </summary>
        /// <param name="other">The <see cref="RelativeDuration"/> instance to compare with this
        /// one.</param>
        /// <returns>
        /// <see langword="true"/> if the <see cref="RelativeDuration"/> instance and this one are
        /// equal; otherwise <see langword="false"/>.
        /// </returns>
        public bool Equals(RelativeDuration other)
            => Duration == other.Duration
            && Proportion == other.Proportion
            && Relativity == other.Relativity;

        /// <summary>
        /// Indicates whether this <see cref="RelativeDuration"/> instance and a <see
        /// cref="Duration"/> instance are equal.
        /// </summary>
        /// <param name="other">The <see cref="Duration"/> instance to compare with this
        /// <see cref="RelativeDuration"/> instance.</param>
        /// <returns>
        /// <see langword="true"/> if the <see cref="Duration"/> instance and this <see
        /// cref="RelativeDuration"/> instance are equal; otherwise <see langword="false"/>.
        /// </returns>
        public bool Equals(Duration other)
            => Relativity == RelativeDurationType.Absolute && other.Equals(Duration);

        /// <summary>
        /// Indicates whether this <see cref="RelativeDuration"/> instance and a <see
        /// cref="DateTime"/> instance are equal.
        /// </summary>
        /// <param name="other">The <see cref="DateTime"/> instance to compare with this
        /// <see cref="RelativeDuration"/> instance.</param>
        /// <returns>
        /// <see langword="true"/> if the <see cref="DateTime"/> instance and this <see
        /// cref="RelativeDuration"/> instance are equal; otherwise <see langword="false"/>.
        /// </returns>
        public bool Equals(DateTime other)
            => Relativity == RelativeDurationType.Absolute && Duration.Equals(other);

        /// <summary>
        /// Indicates whether this <see cref="RelativeDuration"/> instance and a <see
        /// cref="DateTimeOffset"/> instance are equal.
        /// </summary>
        /// <param name="other">The <see cref="DateTimeOffset"/> instance to compare with this
        /// <see cref="RelativeDuration"/> instance.</param>
        /// <returns>
        /// <see langword="true"/> if the <see cref="DateTimeOffset"/> instance and this <see
        /// cref="RelativeDuration"/> instance are equal; otherwise <see langword="false"/>.
        /// </returns>
        public bool Equals(DateTimeOffset other)
            => Relativity == RelativeDurationType.Absolute && Duration.Equals(other);

        /// <summary>
        /// Indicates whether this <see cref="RelativeDuration"/> instance and a <see
        /// cref="TimeSpan"/> instance are equal.
        /// </summary>
        /// <param name="other">The <see cref="TimeSpan"/> instance to compare with this
        /// <see cref="RelativeDuration"/> instance.</param>
        /// <returns>
        /// <see langword="true"/> if the <see cref="TimeSpan"/> instance and this <see
        /// cref="RelativeDuration"/> instance are equal; otherwise <see langword="false"/>.
        /// </returns>
        public bool Equals(TimeSpan other)
            => Relativity == RelativeDurationType.Absolute && Duration.Equals(other);

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>The hash code for this instance.</returns>
        public override int GetHashCode()
        {
            var hash = 391 + Duration.GetHashCode();
            hash = (hash * 23) + Proportion.GetHashCode();
            return (hash * 23) + Relativity.GetHashCode();
        }

        /// <summary>Populates a <see cref="SerializationInfo"></see> with the data needed to
        /// serialize the target object.</summary>
        /// <param name="info">The <see cref="SerializationInfo"></see> to populate with
        /// data.</param>
        /// <param name="context">The destination (see <see cref="StreamingContext"></see>) for this
        /// serialization.</param>
        /// <exception cref="System.Security.SecurityException">The caller does not have the
        /// required permission.</exception>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(Duration), Duration);
            info.AddValue(nameof(Proportion), Proportion);
            info.AddValue(nameof(Relativity), Relativity);
        }
    }
}
