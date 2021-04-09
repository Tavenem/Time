using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Tavenem.Time
{
    /// <summary>
    /// <para>
    /// Represents an absolute duration. Capable of accurately representing extremely large and
    /// small time intervals, suitable for time scales such as the age of a universe, with precision
    /// down to Planck time.
    /// </para>
    /// <para>
    /// Uses SI days (86,400 seconds exactly) and astronomical (Julian) years (365.25 days, or
    /// 31,557,600s exactly), rather than days or years based on the solar cycles of Earth (or any
    /// other celestial movements). This avoids complications such as time zones and leap years (or
    /// leap seconds, or leap anything else).
    /// </para>
    /// <para>
    /// Also able to represent an infinite (perpetual) duration.
    /// </para>
    /// </summary>
    [Serializable]
    [DataContract]
    [JsonConverter(typeof(DurationConverter))]
    public partial struct Duration :
        IEquatable<Duration>,
        IEquatable<RelativeDuration>,
        IEquatable<DateTime>,
        IEquatable<DateTimeOffset>,
        IEquatable<TimeSpan>,
        IComparable,
        IComparable<Duration>,
        IComparable<DateTime>,
        IComparable<DateTimeOffset>,
        IComparable<TimeSpan>,
        ISerializable
    {
        /// <summary>
        /// The maximum number of terms in the <see cref="AeonSequence"/>.
        /// </summary>
        public const int MaxAeonSequenceLength = 100;

        private const ulong MaxAeonSequence = 999999999999999999;

        /// <summary>
        /// A duration with the maximum value that can be represented without being infinite.
        /// Read-only.
        /// </summary>
        public static readonly Duration MaxValue = new(
            false,
            false,
            PlanckTimePerYoctosecond - 1,
            YoctosecondsPerNanosecond - 1,
            NanosecondsPerYear - 1,
            YearsPerAeon - 1,
            Enumerable.Repeat(MaxAeonSequence, MaxAeonSequenceLength).ToList());

        /// <summary>
        /// A duration representing an infinite amount of time into the past. Read-only.
        /// </summary>
        public static readonly Duration NegativeInfinity = new(true, true, 0, 0, 0, 0, Array.Empty<ulong>());

        /// <summary>
        /// A duration representing an infinite amount of time. Read-only.
        /// </summary>
        public static readonly Duration PositiveInfinity = new(false, true, 0, 0, 0, 0, Array.Empty<ulong>());

        /// <summary>
        /// A duration representing no time. Read-only.
        /// </summary>
        public static readonly Duration Zero = new(false, false, 0, 0, 0, 0, Array.Empty<ulong>());

        private static readonly ulong[] _HomeAeonSequence = new ulong[] { 13799 };

        /// <summary>
        /// <para>
        /// The number of aeons represented by this <see cref="Duration"/>, expressed as a list of
        /// <see cref="ulong"/> values which indicate sections of significant digits in ascending
        /// order, starting with the value of the 6th through the 23th significant digits.
        /// </para>
        /// <para>
        /// A maximum of <see cref="MaxAeonSequenceLength"/> values are permitted, resulting in a
        /// maximum year of 1e1807-1.
        /// </para>
        /// </summary>
        /// <example>
        /// For example, the series [62, 281, 43] would refer to the year
        /// 4,300,000,000,000,028,100,000,000,000,000,006,200,000.
        /// </example>
        [DataMember(Order = 1)]
        public IReadOnlyList<ulong>? AeonSequence { get; }

        /// <summary>
        /// The number of whole attoseconds represented by this <see cref="Duration"/> beyond its
        /// <see cref="Femtoseconds"/>. This is a calculated property which reflects the value
        /// of <see cref="TotalYoctoseconds"/>.
        /// </summary>
        public uint Attoseconds => (uint)(TotalYoctoseconds / YoctosecondsPerAttosecond % AttosecondsPerFemtosecond);

        /// <summary>
        /// <para>
        /// The number of whole days represented by this <see cref="Duration"/> beyond its
        /// <see cref="Years"/>.
        /// </para>
        /// <para>
        /// This is a calculated property which reflects the value of <see cref="TotalNanoseconds"/>.
        /// </para>
        /// <para>
        /// An SI day is exactly 86400 seconds long.
        /// </para>
        /// </summary>
        public uint Days => (uint)(TotalNanoseconds / NanosecondsPerDay);

        /// <summary>
        /// The number of whole femtoseconds represented by this <see cref="Duration"/> beyond its
        /// <see cref="Picoseconds"/>. This is a calculated property which reflects the value
        /// of <see cref="TotalYoctoseconds"/>.
        /// </summary>
        public uint Femtoseconds => (uint)(TotalYoctoseconds / YoctosecondsPerFemtosecond % FemtosecondsPerPicosecond);

        /// <summary>
        /// <para>
        /// The number of whole hours represented by this <see cref="Duration"/> beyond its
        /// <see cref="Days"/>.
        /// </para>
        /// <para>
        /// This is a calculated property which reflects the value of <see cref="TotalNanoseconds"/>.
        /// </para>
        /// </summary>
        public uint Hours => (uint)(TotalNanoseconds / NanosecondsPerHour % HoursPerDay);

        /// <summary>
        /// Indicates that this <see cref="Duration"/> represents a negative duration.
        /// </summary>
        [DataMember(Order = 2)]
        public bool IsNegative { get; }

        /// <summary>
        /// Indicates that this <see cref="Duration"/> represents an infinite amount of
        /// time (positive or negative).
        /// </summary>
        [DataMember(Order = 3)]
        public bool IsPerpetual { get; }

        /// <summary>
        /// Indicates that this <see cref="Duration"/> represents an infinite amount of
        /// time in the positive direction.
        /// </summary>
        public bool IsPositiveInfinity => IsPerpetual && !IsNegative;

        /// <summary>
        /// Indicates that this <see cref="Duration"/> represents an infinite amount of
        /// time in the negative direction.
        /// </summary>
        public bool IsNegativeInfinity => IsPerpetual && IsNegative;

        /// <summary>
        /// Whether this <see cref="Duration"/> represents zero time.
        /// </summary>
        public bool IsZero => !IsPerpetual && Years == 0 && TotalNanoseconds == 0 && TotalYoctoseconds == 0 && PlanckTime == 0 && (AeonSequence?.Count ?? 0) == 0;

        /// <summary>
        /// The number of whole microseconds represented by this <see cref="Duration"/> beyond its
        /// <see cref="Milliseconds"/>. This is a calculated property which reflects the value of <see
        /// cref="TotalNanoseconds"/>.
        /// </summary>
        public uint Microseconds => (uint)(TotalNanoseconds / NanosecondsPerMicrosecond % MicrosecondsPerMillisecond);

        /// <summary>
        /// The number of whole milliseconds represented by this <see cref="Duration"/> beyond its
        /// <see cref="Nanoseconds"/>. This is a calculated property which reflects the value of <see
        /// cref="TotalNanoseconds"/>.
        /// </summary>
        public uint Milliseconds => (uint)(TotalNanoseconds / NanosecondsPerMillisecond % MillisecondsPerSecond);

        /// <summary>
        /// <para>
        /// The number of whole hours represented by this <see cref="Duration"/> beyond its
        /// <see cref="Hours"/>.
        /// </para>
        /// <para>
        /// This is a calculated property which reflects the value of <see cref="TotalNanoseconds"/>.
        /// </para>
        /// </summary>
        public uint Minutes => (uint)(TotalNanoseconds / NanosecondsPerMinute % MinutesPerHour);

        /// <summary>
        /// <para>
        /// The number of whole nanoseconds represented by this <see cref="Duration"/> beyond its
        /// <see cref="Microseconds"/>.
        /// </para>
        /// <para>
        /// This is a calculated property which reflects the value of <see
        /// cref="TotalNanoseconds"/>.
        /// </para>
        /// </summary>
        /// <remarks>
        /// The property <see cref="TotalNanoseconds"/> records all nanoseconds in excess of <see
        /// cref="Years"/>, rather than only those since the last microsecond. This calculated
        /// property is provided as a convenience for obtaining the more intuitive number of
        /// nanoseconds.
        /// </remarks>
        public uint Nanoseconds => (uint)(TotalNanoseconds % NanosecondsPerMicrosecond);

        /// <summary>
        /// The number of whole picoseconds represented by this <see cref="Duration"/> beyond its
        /// <see cref="Yoctoseconds"/>. This is a calculated property which reflects the value
        /// of <see cref="TotalYoctoseconds"/>.
        /// </summary>
        public uint Picoseconds => (uint)(TotalYoctoseconds / YoctosecondsPerPicosecond);

        /// <summary>
        /// <para>
        /// The amount of Planck time represented by this <see cref="Duration"/> beyond its
        /// <see cref="Yoctoseconds"/>.
        /// </para>
        /// <para>
        /// Cannot be negative; only the <see cref="Duration"/> as a whole can be negative.
        /// </para>
        /// </summary>
        [DataMember(Order = 4)]
        public decimal PlanckTime { get; }

        /// <summary>
        /// <para>
        /// The number of whole seconds represented by this <see cref="Duration"/> beyond its
        /// <see cref="Minutes"/>.
        /// </para>
        /// <para>
        /// This is a calculated property which reflects the value of <see cref="TotalNanoseconds"/>.
        /// </para>
        /// </summary>
        public uint Seconds => (uint)(TotalNanoseconds / NanosecondsPerSecond % SecondsPerMinute);

        /// <summary>
        /// A number that indicates the sign (negative, positive, or zero) of this instance.
        /// </summary>
        public int Sign
        {
            get
            {
                if (IsNegative)
                {
                    return -1;
                }
                else if (IsZero)
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
        }

        /// <summary>
        /// The number of nanoseconds represented by this <see cref="Duration"/> beyond its
        /// <see cref="Years"/>.
        /// </summary>
        /// <remarks>
        /// Note carefully that this is not nanoseconds in excess of <see cref="Microseconds"/>.
        /// <see cref="Milliseconds"/>, <see cref="Microseconds"/>, and <see cref="Nanoseconds"/>
        /// are all calculated properties based on <see cref="TotalNanoseconds"/>.
        /// </remarks>
        [DataMember(Order = 5)]
        public ulong TotalNanoseconds { get; }

        /// <summary>
        /// The number of yoctoseconds represented by this <see cref="Duration"/> beyond its
        /// <see cref="TotalNanoseconds"/>.
        /// </summary>
        /// <remarks>
        /// Note carefully that this is not yoctoseconds in excess of <see cref="Zeptoseconds"/>.
        /// <see cref="Picoseconds"/>, <see cref="Femtoseconds"/>, <see cref="Attoseconds"/>, <see
        /// cref="Zeptoseconds"/>, and <see cref="Yoctoseconds"/> are all calculated properties
        /// based on <see cref="TotalYoctoseconds"/>.
        /// </remarks>
        [DataMember(Order = 6)]
        public ulong TotalYoctoseconds { get; }

        /// <summary>
        /// <para>
        /// The number of astronomical years represented by this <see cref="Duration"/>
        /// beyond its <see cref="AeonSequence"/>.
        /// </para>
        /// <para>
        /// An astronomical (Julian) year is exactly 31557600 seconds long (365.25 * 86400).
        /// </para>
        /// </summary>
        [DataMember(Order = 7)]
        public uint Years { get; }

        /// <summary>
        /// <para>
        /// The number of whole yoctoseconds represented by this <see cref="Duration"/> beyond its
        /// <see cref="Zeptoseconds"/>.
        /// </para>
        /// <para>
        /// This is a calculated property which reflects the value of <see
        /// cref="TotalYoctoseconds"/>.
        /// </para>
        /// </summary>
        /// <remarks>
        /// The property <see cref="TotalYoctoseconds"/> records all seconds in excess of <see
        /// cref="TotalNanoseconds"/>, rather than only those since the last zeptosecond. This
        /// calculated property is provided as a convenience for obtaining the more intuitive number
        /// of yoctoseconds.
        /// </remarks>
        public uint Yoctoseconds => (uint)(TotalYoctoseconds % YoctosecondsPerZeptosecond);

        /// <summary>
        /// The number of whole zeptoseconds represented by this <see cref="Duration"/> beyond its
        /// <see cref="Attoseconds"/>. This is a calculated property which reflects the value of
        /// <see cref="TotalYoctoseconds"/>.
        /// </summary>
        public uint Zeptoseconds => (uint)(TotalYoctoseconds / YoctosecondsPerZeptosecond % ZeptosecondsPerAttosecond);

        /// <summary>
        /// Initializes a new instance of <see cref="Duration"/>
        /// </summary>
        /// <param name="isNegative">Whether the instance will represent a negative
        /// duration.</param>
        /// <param name="aeonSequence">The number of aeons, expressed as a list of significant
        /// digits in ascending order, starting with the value of the 6th significant digit
        /// (1e6).</param>
        /// <param name="years">The number of astronomical years.</param>
        /// <param name="days">The number of days.</param>
        /// <param name="hours">The number of hours.</param>
        /// <param name="minutes">The number of minutes.</param>
        /// <param name="seconds">The number of seconds.</param>
        /// <param name="milliseconds">The number of milliseconds.</param>
        /// <param name="microseconds">The number of microseconds.</param>
        /// <param name="nanoseconds">The number of nanoseconds.</param>
        /// <param name="picoseconds">The number of picoseconds.</param>
        /// <param name="femtoseconds">The number of femtoseconds.</param>
        /// <param name="attoseconds">The number of attoseconds.</param>
        /// <param name="zeptoseconds">The number of zeptoseconds.</param>
        /// <param name="yoctoseconds">The number of yoctoseconds.</param>
        /// <param name="planckTime">
        /// <para>
        /// The amount of Planck time.
        /// </para>
        /// <para>
        /// Cannot be negative; only the <see cref="Duration"/> as a whole can be negative. Values
        /// less than zero will be converted to 0.
        /// </para>
        /// </param>
        /// <exception cref="OverflowException">Provided parameters result in a value for <see
        /// cref="AeonSequence"/> with more than <see cref="int.MaxValue"/> significant
        /// digits.</exception>
        public Duration(
            bool isNegative = false,
            IEnumerable<ulong>? aeonSequence = null,
            uint years = 0,
            uint days = 0,
            uint hours = 0,
            uint minutes = 0,
            uint seconds = 0,
            uint milliseconds = 0,
            uint microseconds = 0,
            ulong nanoseconds = 0,
            uint picoseconds = 0,
            uint femtoseconds = 0,
            uint attoseconds = 0,
            uint zeptoseconds = 0,
            ulong yoctoseconds = 0,
            decimal planckTime = 0)
        {
            IsNegative = isNegative;
            IsPerpetual = false;

            PlanckTime = Math.Max(0, planckTime);

            var ys = 0ul;
            var ns = 0ul;
            if (PlanckTime >= PlanckTimePerYoctosecond)
            {
                ys += (ulong)Math.Floor(PlanckTime / PlanckTimePerYoctosecond);
                PlanckTime %= PlanckTimePerYoctosecond;
            }
            if (ys >= YoctosecondsPerNanosecond)
            {
                ns += (uint)(ys / YoctosecondsPerNanosecond);
                ys %= YoctosecondsPerNanosecond;
            }

            ys += zeptoseconds * (ulong)YoctosecondsPerZeptosecond;
            if (ys >= YoctosecondsPerNanosecond)
            {
                ns += (uint)(ys / YoctosecondsPerNanosecond);
                ys %= YoctosecondsPerNanosecond;
            }

            ys += attoseconds * (ulong)YoctosecondsPerAttosecond;
            if (ys >= YoctosecondsPerNanosecond)
            {
                ns += (uint)(ys / YoctosecondsPerNanosecond);
                ys %= YoctosecondsPerNanosecond;
            }

            ys += femtoseconds * (ulong)YoctosecondsPerFemtosecond;
            if (ys >= YoctosecondsPerNanosecond)
            {
                ns += (uint)(ys / YoctosecondsPerNanosecond);
                ys %= YoctosecondsPerNanosecond;
            }

            ys += picoseconds * (ulong)YoctosecondsPerPicosecond;
            if (ys >= YoctosecondsPerNanosecond)
            {
                ns += (uint)(ys / YoctosecondsPerNanosecond);
                ys %= YoctosecondsPerNanosecond;
            }

            ys += yoctoseconds;
            if (ys >= YoctosecondsPerNanosecond)
            {
                ns += (uint)(ys / YoctosecondsPerNanosecond);
                ys %= YoctosecondsPerNanosecond;
            }
            TotalYoctoseconds = ys;

            var y = 0u;
            if (ns >= NanosecondsPerYear)
            {
                y += (uint)(ns / NanosecondsPerYear);
                ns %= NanosecondsPerYear;
            }

            ns += microseconds * (ulong)NanosecondsPerMicrosecond;
            if (ns >= NanosecondsPerYear)
            {
                y += (uint)(ns / NanosecondsPerYear);
                ns %= NanosecondsPerYear;
            }

            ns += milliseconds * (ulong)NanosecondsPerMillisecond;
            if (ns >= NanosecondsPerYear)
            {
                y += (uint)(ns / NanosecondsPerYear);
                ns %= NanosecondsPerYear;
            }

            ns += nanoseconds;
            if (ns >= NanosecondsPerYear)
            {
                y += (uint)(ns / NanosecondsPerYear);
                ns %= NanosecondsPerYear;
            }

            ns += seconds * (ulong)NanosecondsPerSecond;
            if (ns >= NanosecondsPerYear)
            {
                y += (uint)(ns / NanosecondsPerYear);
                ns %= NanosecondsPerYear;
            }

            ns += minutes * (ulong)NanosecondsPerMinute;
            if (ns >= NanosecondsPerYear)
            {
                y += (uint)(ns / NanosecondsPerYear);
                ns %= NanosecondsPerYear;
            }

            ns += hours * (ulong)NanosecondsPerHour;
            if (ns >= NanosecondsPerYear)
            {
                y += (uint)(ns / NanosecondsPerYear);
                ns %= NanosecondsPerYear;
            }

            var extraYears = (uint)(days * (ulong)NanosecondsPerDay / NanosecondsPerYear);
            y += extraYears;
            ns += (days * (ulong)NanosecondsPerDay) - (extraYears * (ulong)NanosecondsPerYear);
            if (ns >= NanosecondsPerYear)
            {
                y += (uint)(ns / NanosecondsPerYear);
                ns %= NanosecondsPerYear;
            }
            TotalNanoseconds = ns;

            var aeons = 0ul;
            if (y >= YearsPerAeon)
            {
                aeons += y / YearsPerAeon;
                y %= YearsPerAeon;
            }

            y += years;
            if (y >= YearsPerAeon)
            {
                aeons += y / YearsPerAeon;
                y %= YearsPerAeon;
            }
            Years = y;

            var newAeonSequence = aeonSequence?.ToList() ?? new List<ulong>();
            for (var index = 0; aeons > 0 && index <= MaxAeonSequenceLength; index++)
            {
                newAeonSequence[index] += aeons;
                if (newAeonSequence[index] > MaxAeonSequence)
                {
                    aeons = MaxAeonSequence - newAeonSequence[index];
                    newAeonSequence[index] -= aeons;
                }
                else
                {
                    aeons = 0;
                }
            }
            if (aeons > 0)
            {
                throw new OverflowException($"Provided parameters result in a value for ${nameof(AeonSequence)} with more than {nameof(MaxAeonSequenceLength)} terms.");
            }
            if (newAeonSequence.Count == 0
                || (newAeonSequence.Count > 1 && newAeonSequence.All(x => x == 0)))
            {
                AeonSequence = null;
            }
            else
            {
                AeonSequence = newAeonSequence;
            }
        }

        private Duration(
            bool isNegative,
            bool isPerpetual,
            decimal planckTime,
            ulong totalYoctoseconds,
            ulong totalNanoseconds,
            uint years,
            IReadOnlyList<ulong>? aeonSequence)
        {
            IsNegative = isNegative;
            IsPerpetual = isPerpetual;
            PlanckTime = planckTime;
            TotalYoctoseconds = totalYoctoseconds;
            TotalNanoseconds = totalNanoseconds;
            Years = years;
            AeonSequence = aeonSequence is null
                || aeonSequence.Count == 0
                || (aeonSequence.Count == 1 && aeonSequence[0] == 0)
                ? null
                : aeonSequence.ToList();
        }

        private Duration(SerializationInfo info, StreamingContext context) : this(
            (bool?)info.GetValue(nameof(IsNegative), typeof(bool)) ?? default,
            (bool?)info.GetValue(nameof(IsPerpetual), typeof(bool)) ?? default,
            (decimal?)info.GetValue(nameof(PlanckTime), typeof(decimal)) ?? default,
            (ulong?)info.GetValue(nameof(TotalYoctoseconds), typeof(ulong)) ?? default,
            (ulong?)info.GetValue(nameof(TotalNanoseconds), typeof(ulong)) ?? default,
            (uint?)info.GetValue(nameof(Years), typeof(uint)) ?? default,
            (IReadOnlyList<ulong>?)info.GetValue(nameof(AeonSequence), typeof(IReadOnlyList<ulong>)))
        { }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns>
        /// <see langword="true"/> if the object and the current instance are equal; otherwise <see
        /// langword="false"/>.
        /// </returns>
        public override bool Equals(object? obj) => obj is Duration other && Equals(other);

        /// <summary>
        /// Indicates whether this <see cref="Duration"/> instance and another are equal.
        /// </summary>
        /// <param name="other">The <see cref="Duration"/> instance to compare with this
        /// one.</param>
        /// <returns>
        /// <see langword="true"/> if the <see cref="Duration"/> instance and this one are
        /// equal; otherwise <see langword="false"/>.
        /// </returns>
        public bool Equals(Duration other)
            => IsNegative == other.IsNegative
            && IsPerpetual == other.IsPerpetual
            && TotalYoctoseconds == other.TotalYoctoseconds
            && TotalNanoseconds == other.TotalNanoseconds
            && Years == other.Years
            && PlanckTime == other.PlanckTime
            && (AeonSequence is null
            ? other.AeonSequence is null
            : !(other.AeonSequence is null) && AeonSequence.SequenceEqual(other.AeonSequence));

        /// <summary>
        /// Indicates whether this <see cref="Duration"/> instance and a <see
        /// cref="RelativeDuration"/> instance are equal.
        /// </summary>
        /// <param name="other">The <see cref="RelativeDuration"/> instance to compare with this
        /// <see cref="Duration"/> instance.</param>
        /// <returns>
        /// <see langword="true"/> if the <see cref="RelativeDuration"/> instance and this <see
        /// cref="Duration"/> instance are equal; otherwise <see langword="false"/>.
        /// </returns>
        public bool Equals(RelativeDuration other)
            => other.Relativity == RelativeDurationType.Absolute && Equals(other.Duration);

        /// <summary>
        /// Indicates whether this <see cref="Duration"/> instance and a <see cref="DateTime"/>
        /// instance are equal.
        /// </summary>
        /// <param name="other">The <see cref="DateTime"/> instance to compare with this
        /// <see cref="Duration"/> instance.</param>
        /// <returns>
        /// <see langword="true"/> if the <see cref="DateTime"/> instance and this <see
        /// cref="Duration"/> instance are equal; otherwise <see langword="false"/>.
        /// </returns>
        public bool Equals(DateTime other) => Equals(FromDateTime(other));

        /// <summary>
        /// Indicates whether this <see cref="Duration"/> instance and a <see
        /// cref="DateTimeOffset"/> instance are equal.
        /// </summary>
        /// <param name="other">The <see cref="DateTimeOffset"/> instance to compare with this
        /// <see cref="Duration"/> instance.</param>
        /// <returns>
        /// <see langword="true"/> if the <see cref="DateTimeOffset"/> instance and this <see
        /// cref="Duration"/> instance are equal; otherwise <see langword="false"/>.
        /// </returns>
        public bool Equals(DateTimeOffset other) => Equals(FromDateTimeOffset(other));

        /// <summary>
        /// Indicates whether this <see cref="Duration"/> instance and a <see cref="TimeSpan"/>
        /// instance are equal.
        /// </summary>
        /// <param name="other">The <see cref="TimeSpan"/> instance to compare with this
        /// <see cref="Duration"/> instance.</param>
        /// <returns>
        /// <see langword="true"/> if the <see cref="TimeSpan"/> instance and this <see
        /// cref="Duration"/> instance are equal; otherwise <see langword="false"/>.
        /// </returns>
        public bool Equals(TimeSpan other) => Equals(FromTimeSpan(other));

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>The hash code for this instance.</returns>
        public override int GetHashCode()
        {
            var hash = 1861411795;
            hash = (hash * -1521134295) + IsNegative.GetHashCode();
            hash = (hash * -1521134295) + IsPerpetual.GetHashCode();
            hash = (hash * -1521134295) + TotalNanoseconds.GetHashCode();
            hash = (hash * -1521134295) + Years.GetHashCode();
            hash = (hash * -1521134295) + PlanckTime.GetHashCode();
            return (hash * -1521134295) + (AeonSequence?.GetHashCode() ?? 0);
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
            info.AddValue(nameof(IsNegative), IsNegative);
            info.AddValue(nameof(IsPerpetual), IsPerpetual);
            info.AddValue(nameof(PlanckTime), PlanckTime);
            info.AddValue(nameof(TotalYoctoseconds), TotalYoctoseconds);
            info.AddValue(nameof(TotalNanoseconds), TotalNanoseconds);
            info.AddValue(nameof(Years), Years);
            info.AddValue(nameof(AeonSequence), AeonSequence);
        }

        /// <summary>
        /// Returns a value which indicates whether this instance is less than, equal to, or greater
        /// than the given object.
        /// </summary>
        /// <param name="obj">An object to compare.</param>
        /// <returns>-1 if this instance is less than the given instance; 0 if they are equal, and 1
        /// if this instance is greater.</returns>
        public int CompareTo(object? obj) => obj is not Duration other ? 1 : CompareTo(other);

        /// <summary>
        /// Returns a value which indicates whether this instance is less than, equal to, or greater
        /// than the given <see cref="Duration"/> instance.
        /// </summary>
        /// <param name="other">A <see cref="Duration"/> to compare.</param>
        /// <returns>-1 if this instance is less than the given instance; 0 if they are equal, and 1
        /// if this instance is greater.</returns>
        public int CompareTo(Duration other)
        {
            if (IsNegative && !other.IsNegative)
            {
                return -1;
            }
            if (IsPerpetual)
            {
                return other.IsPerpetual ? 0 : 1 * Sign;
            }
            else if (other.IsPerpetual)
            {
                return -1 * Sign;
            }

            var comparison = (AeonSequence?.Count ?? 0).CompareTo(other.AeonSequence?.Count ?? 0);
            if (comparison != 0)
            {
                return comparison * Sign;
            }

            for (var i = 0; i < (AeonSequence?.Count ?? 0); i++)
            {
                comparison = AeonSequence![i].CompareTo(other.AeonSequence![i]);
                if (comparison != 0)
                {
                    return comparison * Sign;
                }
            }

            comparison = Years.CompareTo(other.Years);
            if (comparison != 0)
            {
                return comparison * Sign;
            }

            comparison = TotalNanoseconds.CompareTo(other.TotalNanoseconds);
            if (comparison != 0)
            {
                return comparison * Sign;
            }

            comparison = TotalYoctoseconds.CompareTo(other.TotalYoctoseconds);
            if (comparison != 0)
            {
                return comparison * Sign;
            }

            return PlanckTime.CompareTo(other.PlanckTime) * Sign;
        }

        /// <summary>
        /// Returns a value which indicates whether this instance is less than, equal to, or greater
        /// than the given <see cref="DateTime"/> instance.
        /// </summary>
        /// <param name="other">A <see cref="DateTime"/> to compare.</param>
        /// <returns>-1 if this instance is less than the given instance; 0 if they are equal, and 1
        /// if this instance is greater.</returns>
        public int CompareTo(DateTime other) => CompareTo(FromDateTime(other));

        /// <summary>
        /// Returns a value which indicates whether this instance is less than, equal to, or greater
        /// than the given <see cref="DateTimeOffset"/> instance.
        /// </summary>
        /// <param name="other">A <see cref="DateTimeOffset"/> to compare.</param>
        /// <returns>-1 if this instance is less than the given instance; 0 if they are equal, and 1
        /// if this instance is greater.</returns>
        public int CompareTo(DateTimeOffset other) => CompareTo(FromDateTimeOffset(other));

        /// <summary>
        /// Returns a value which indicates whether this instance is less than, equal to, or greater
        /// than the given <see cref="TimeSpan"/> instance.
        /// </summary>
        /// <param name="other">A <see cref="TimeSpan"/> to compare.</param>
        /// <returns>-1 if this instance is less than the given instance; 0 if they are equal, and 1
        /// if this instance is greater.</returns>
        public int CompareTo(TimeSpan other) => CompareTo(FromTimeSpan(other));
    }
}
