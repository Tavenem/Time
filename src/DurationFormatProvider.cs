using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Tavenem.Time
{
    /// <summary>
    /// A custom formatter and format provider for <see cref="Duration"/>.
    /// </summary>
    public class DurationFormatProvider : IFormatProvider, ICustomFormatter
    {
        internal const string DefaultFormat = GeneralDateLongTimeFormat;
        internal const string ExtendedFormat = "y d HH:mm:ss:MMM:uuu:nnn:ppp:fff:aaa:zzz:YYY:PPP";
        internal const string GeneralDateLongTimeFormat = "y d HH:mm:ss";
        internal const string GeneralDateShortTimeFormat = "y d HH:mm";
        internal const string LongDateFormat = "e d";
        internal const string LongTimeFormat = "HH:mm:ss";
        internal const string RoundTripFormat = "e'-'n':'Y':'P";
        internal const string ShortDateFormat = "y d";
        internal const string ShortTimeFormat = "HH:mm";
        internal const char AeonFormatChar = 'e';
        internal const char AttoFormatChar = 'a';
        internal const char DayFormatChar = 'd';
        internal const char FemtoFormatChar = 'f';
        internal const char MicroFormatChar = 'u';
        internal const char MilliFormatChar = 'M';
        internal const char MinuteFormatChar = 'm';
        internal const char NanoFormatChar = 'n';
        internal const char PicoFormatChar = 'p';
        internal const char PlanckFormatChar = 'P';
        internal const char SecondFormatChar = 's';
        internal const char SecondFractionFormatChar = 'F';
        internal const char YearFormatChar = 'y';
        internal const char YottoFormatChar = 'Y';
        internal const char ZeptoFormatChar = 'z';

        /// <summary>
        /// A static instance of <see cref="DurationFormatProvider"/>.
        /// </summary>
        public static readonly DurationFormatProvider Instance = new();

        internal static readonly string[] _AllFormats = new string[]
        {
            GeneralDateLongTimeFormat,
            RoundTripFormat,
            ExtendedFormat,
            GeneralDateShortTimeFormat,
            LongDateFormat,
            LongTimeFormat,
            ShortDateFormat,
            ShortTimeFormat,
        };

        private static readonly char[] _HourFormatChars = new char[] { 'h', 'H' };
        internal static readonly Dictionary<FormatUnit, FormatUnitInfo> _FormatInfo = new()
        {
            { FormatUnit.Aeon, new FormatUnitInfo(FormatUnit.Aeon, AeonFormatChar) },
            { FormatUnit.Atto, new FormatUnitInfo(FormatUnit.Atto, AttoFormatChar) },
            { FormatUnit.Day, new FormatUnitInfo(FormatUnit.Day, DayFormatChar) },
            { FormatUnit.Femto, new FormatUnitInfo(FormatUnit.Femto, FemtoFormatChar) },
            { FormatUnit.Hour, new FormatUnitInfo(FormatUnit.Hour, _HourFormatChars) },
            { FormatUnit.Micro, new FormatUnitInfo(FormatUnit.Micro, MicroFormatChar) },
            { FormatUnit.Milli, new FormatUnitInfo(FormatUnit.Milli, MilliFormatChar) },
            { FormatUnit.Minute, new FormatUnitInfo(FormatUnit.Minute, MinuteFormatChar) },
            { FormatUnit.Nano, new FormatUnitInfo(FormatUnit.Nano, NanoFormatChar) },
            { FormatUnit.Pico, new FormatUnitInfo(FormatUnit.Pico, PicoFormatChar) },
            { FormatUnit.Planck, new FormatUnitInfo(FormatUnit.Planck, PlanckFormatChar) },
            { FormatUnit.Second, new FormatUnitInfo(FormatUnit.Second, SecondFormatChar) },
            { FormatUnit.SecondFraction, new FormatUnitInfo(FormatUnit.SecondFraction, SecondFractionFormatChar) },
            { FormatUnit.Year, new FormatUnitInfo(FormatUnit.Year, YearFormatChar) },
            { FormatUnit.Yocto, new FormatUnitInfo(FormatUnit.Yocto, YottoFormatChar) },
            { FormatUnit.Zepto, new FormatUnitInfo(FormatUnit.Zepto, ZeptoFormatChar) },
        };

        internal static string FullDateLongTimeFormat => $"{LongDateFormat} {LongTimeFormat}";
        internal static string FullDateShortTimeFormat => $"{LongDateFormat} {ShortTimeFormat}";

        /// <summary>Converts the value of a specified object to an equivalent string representation
        /// using specified format and culture-specific formatting information.</summary>
        /// <param name="format">A format string containing formatting specifications.</param>
        /// <param name="arg">An object to format.</param>
        /// <param name="formatProvider">An object that supplies format information about the
        /// current instance.</param>
        /// <returns>The string representation of the value of <paramref name="arg">arg</paramref>,
        /// formatted as specified by <paramref name="format">format</paramref> and <paramref
        /// name="formatProvider">formatProvider</paramref>.</returns>
        public string Format(string? format, object? arg, IFormatProvider? formatProvider)
        {
            if (arg is not Duration d)
            {
                if (arg is not RelativeDuration rd)
                {
                    try
                    {
                        return HandleOtherFormats(format, arg);
                    }
                    catch (FormatException e)
                    {
                        throw new FormatException($"The format of {format} is invalid.", e);
                    }
                }

                return rd.Relativity switch
                {
                    RelativeDurationType.Absolute => Format(rd.Duration, format, NumberFormatInfo.GetInstance(formatProvider ?? CultureInfo.CurrentCulture)),
                    RelativeDurationType.ProportionOfDay => $"Dx{rd.Proportion:G}",
                    RelativeDurationType.ProportionOfYear => $"Yx{rd.Proportion:G}",
                    _ => string.Empty,
                };
            }

            return Format(d, format, NumberFormatInfo.GetInstance(formatProvider ?? CultureInfo.CurrentCulture));
        }

        /// <summary>Returns an object that provides formatting services for the specified
        /// type.</summary>
        /// <param name="formatType">An object that specifies the type of format object to
        /// return.</param>
        /// <returns>An instance of the object specified by <paramref
        /// name="formatType">formatType</paramref>, if the <see cref="IFormatProvider"></see>
        /// implementation can supply that type of object; otherwise, <see
        /// langword="null"/>.</returns>
        public object? GetFormat(Type? formatType)
            => typeof(ICustomFormatter).IsAssignableFrom(formatType) ? this : null;

        private static string HandleOtherFormats(string? format, object? arg)
        {
            if (arg is IFormattable formattable)
            {
                return formattable.ToString(format, CultureInfo.CurrentCulture);
            }
            else if (arg != null)
            {
                return arg.ToString() ?? string.Empty;
            }
            else
            {
                return string.Empty;
            }
        }

        private static string Format(Duration duration, string? format, NumberFormatInfo nfi)
        {
            if (format is null)
            {
                return FormatGeneralDateLongTime(duration, nfi);
            }

            return Format(duration, format.AsSpan(), nfi);
        }

        private static string Format(Duration duration, in ReadOnlySpan<char> format, NumberFormatInfo nfi)
        {
            if (duration.IsPerpetual)
            {
                return duration.IsNegative
                    ? nfi.NegativeInfinitySymbol
                    : nfi.PositiveInfinitySymbol;
            }

            if (format.Length == 0 || format.IsWhiteSpace())
            {
                return FormatGeneralDateLongTime(duration, nfi);
            }

            if (format.Length == 1)
            {
                return format[0] switch
                {
                    'd' => FormatShortDate(duration, nfi),
                    'D' => FormatLongDate(duration, nfi),
                    'E' => FormatExtended(duration, nfi),
                    'f' => FormatFullDateShortTime(duration, nfi),
                    'F' => FormatFullDateLongTime(duration, nfi),
                    'g' => FormatGeneralDateShortTime(duration, nfi),
                    'o' or 'O' => FormatRoundTrip(duration, nfi),
                    't' => FormatShortTime(duration, nfi),
                    'T' => FormatLongTime(duration, nfi),
                    'X' => FormatExtensible(duration, nfi),
                    _ => FormatGeneralDateLongTime(duration, nfi),
                };
            }

            var dtfi = DateTimeFormatInfo.CurrentInfo;

            var sb = new StringBuilder();

            var broken = false;
            var doubleQuote = false;
            var processing = FormatUnit.None;
            var singleQuote = false;
            var unitCount = 0;

            var yearValue = duration.Years;
            var nanosecondValue = duration.TotalNanoseconds;
            var yoctosecondValue = duration.TotalYoctoseconds;

            var superNanoBalanced = false;
            void BalanceSuperNanoUnits()
            {
                if (superNanoBalanced)
                {
                    return;
                }

                nanosecondValue = duration.Nanoseconds;

                superNanoBalanced = true;
            }

            var superYoctoBalanced = false;
            void BalanceSuperYoctoUnits()
            {
                if (superYoctoBalanced)
                {
                    return;
                }

                yoctosecondValue = duration.Yoctoseconds;

                superYoctoBalanced = true;
            }

            void FinishUnit()
            {
                switch (processing)
                {
                    case FormatUnit.Aeon:
                        sb.Append(AeonYearString(duration, nfi, unitCount == 1 ? 29 : unitCount));
                        yearValue = 0;
                        break;
                    case FormatUnit.Year:
                        if (yearValue > 9999 && unitCount <= 4)
                        {
                            sb.Append(yearValue.ToString("N"));
                        }
                        else
                        {
                            sb.Append(yearValue.ToString($"D{unitCount}"));
                        }
                        break;
                    case FormatUnit.Day:
                        BalanceSuperNanoUnits();
                        sb.Append(duration.Days.ToString($"D{unitCount}"));
                        break;
                    case FormatUnit.Hour:
                        BalanceSuperNanoUnits();
                        sb.Append(duration.Hours.ToString($"D{unitCount}"));
                        break;
                    case FormatUnit.Minute:
                        BalanceSuperNanoUnits();
                        sb.Append(duration.Minutes.ToString($"D{unitCount}"));
                        break;
                    case FormatUnit.Second:
                        BalanceSuperNanoUnits();
                        sb.Append(duration.Seconds.ToString($"D{unitCount}"));
                        break;
                    case FormatUnit.SecondFraction:
                        var s = duration.FractionalSeconds().ToString($"F{unitCount}");
                        sb.Append(s, s.IndexOf(nfi.NumberDecimalSeparator) + nfi.NumberDecimalSeparator.Length, s.Length - s.IndexOf(nfi.NumberDecimalSeparator) + nfi.NumberDecimalSeparator.Length);
                        break;
                    case FormatUnit.Milli:
                        BalanceSuperNanoUnits();
                        sb.Append(duration.Milliseconds.ToString($"D{unitCount}"));
                        break;
                    case FormatUnit.Micro:
                        BalanceSuperNanoUnits();
                        sb.Append(duration.Microseconds.ToString($"D{unitCount}"));
                        break;
                    case FormatUnit.Nano:
                        sb.Append(nanosecondValue.ToString($"D{unitCount}"));
                        break;
                    case FormatUnit.Pico:
                        BalanceSuperYoctoUnits();
                        sb.Append(duration.Picoseconds.ToString($"D{unitCount}"));
                        break;
                    case FormatUnit.Femto:
                        BalanceSuperYoctoUnits();
                        sb.Append(duration.Femtoseconds.ToString($"D{unitCount}"));
                        break;
                    case FormatUnit.Atto:
                        BalanceSuperYoctoUnits();
                        sb.Append(duration.Attoseconds.ToString($"D{unitCount}"));
                        break;
                    case FormatUnit.Zepto:
                        BalanceSuperYoctoUnits();
                        sb.Append(duration.Zeptoseconds.ToString($"D{unitCount}"));
                        break;
                    case FormatUnit.Yocto:
                        sb.Append(yoctosecondValue.ToString($"D{unitCount}"));
                        break;
                    case FormatUnit.Planck:
                        if (unitCount == 1)
                        {
                            sb.Append(duration.PlanckTime);
                        }
                        else
                        {
                            sb.Append(duration.PlanckTime.ToString($"G{unitCount}"));
                        }
                        break;
                }
                processing = FormatUnit.None;
                unitCount = 0;
            }

            foreach (var ch in format)
            {
                if (ch == '"')
                {
                    if (singleQuote)
                    {
                        sb.Append(ch);
                    }
                    else if (broken)
                    {
                        sb.Append(ch);
                        broken = false;
                    }
                    else
                    {
                        if (processing != FormatUnit.None)
                        {
                            FinishUnit();
                        }
                        doubleQuote = !doubleQuote;
                    }
                }
                else if (ch == '\'')
                {
                    if (doubleQuote)
                    {
                        sb.Append(ch);
                    }
                    else if (broken)
                    {
                        sb.Append(ch);
                        broken = false;
                    }
                    else
                    {
                        if (processing != FormatUnit.None)
                        {
                            FinishUnit();
                        }
                        singleQuote = !singleQuote;
                    }
                }
                else if (ch == '\\')
                {
                    if (broken)
                    {
                        sb.Append(ch);
                    }
                    else
                    {
                        if (processing != FormatUnit.None)
                        {
                            FinishUnit();
                        }
                        broken = true;
                    }
                }
                else if (singleQuote || doubleQuote)
                {
                    if (!broken)
                    {
                        sb.Append(ch);
                    }
                }
                else if (broken)
                {
                    sb.Append(ch);
                    broken = false;
                }
                else if (ch == ':')
                {
                    if (processing != FormatUnit.None)
                    {
                        FinishUnit();
                    }
                    sb.Append(dtfi.TimeSeparator);
                }
                else if (ch == '/')
                {
                    if (processing != FormatUnit.None)
                    {
                        FinishUnit();
                    }
                    sb.Append(dtfi.DateSeparator);
                }
                else if (ch != '%')
                {
                    var found = false;
                    foreach (var formatInfo in _FormatInfo.Values)
                    {
                        if (formatInfo.Match(ch))
                        {
                            found = true;
                            if (processing != formatInfo.Unit && processing != FormatUnit.None)
                            {
                                FinishUnit();
                            }
                            processing = formatInfo.Unit;
                            unitCount++;
                            break;
                        }
                    }
                    if (!found)
                    {
                        FinishUnit();
                        sb.Append(ch);
                    }
                }
            }

            FinishUnit();

            return sb.ToString();
        }

        private static string AeonYearString(Duration duration, NumberFormatInfo nfi, int n)
        {
            if (duration.AeonSequence is null
                || duration.AeonSequence.Count == 0
                || (duration.AeonSequence.Count == 1
                && duration.AeonSequence[0] == 0))
            {
                return duration.Years.ToString($"G{n}");
            }

            var sb = new StringBuilder();

            var exp = (uint)((duration.AeonSequence.Count - 1) * 18)
                + (duration.AeonSequence[duration.AeonSequence.Count - 1] == 0
                    ? 1
                    : (int)Math.Log10(duration.AeonSequence[duration.AeonSequence.Count - 1]) + 1)
                + 5;
            var sci = exp >= n;

            if (sci)
            {
                for (var i = duration.AeonSequence.Count - 1; i >= 0; i--)
                {
                    sb.Append(duration.AeonSequence[i]);
                    if (sb.Length > n)
                    {
                        break;
                    }
                }
                if (sb.Length < n)
                {
                    sb.Append(duration.Years.ToString("D5"));
                }
                sb.Length = n;
                sb.Insert(0, nfi.NumberDecimalSeparator);
                sb.Insert(0, '0');
                sb.Append("E+");
                sb.Append(exp.ToString("D2"));
            }
            else
            {
                for (var i = duration.AeonSequence.Count - 1; i >= 0; i--)
                {
                    sb.Append(duration.AeonSequence[i]);
                }
                sb.Append(duration.Years.ToString("D5"));
            }

            return sb.ToString();
        }

        private static string FormatExtended(Duration duration, NumberFormatInfo nfi)
            => Format(duration, ExtendedFormat.AsSpan(), nfi);

        private static string FormatExtensible(Duration duration, NumberFormatInfo nfi)
        {
            if (duration.IsPerpetual)
            {
                return nfi.PositiveInfinitySymbol;
            }

            var sb = new StringBuilder();

            if (duration.AeonSequence?.Count > 1 || (duration.AeonSequence?.Count == 1 && duration.AeonSequence[0] > 1) || duration.Years > 0)
            {
                sb.Append(AeonYearString(duration, nfi, 29));
                sb.Append(" y");
            }
            var d = duration.Days;
            if (d > 0)
            {
                if (sb.Length > 0)
                {
                    sb.Append(' ');
                }
                sb.Append(d);
                sb.Append(" d");
            }
            var h = duration.Hours;
            if (h > 0)
            {
                if (sb.Length > 0)
                {
                    sb.Append(' ');
                }
                sb.Append(h);
                sb.Append(" h");
            }
            var m = duration.Minutes;
            if (m > 0)
            {
                if (sb.Length > 0)
                {
                    sb.Append(' ');
                }
                sb.Append(m);
                sb.Append(" min");
            }
            var s = duration.Seconds;
            if (s > 0)
            {
                if (sb.Length > 0)
                {
                    sb.Append(' ');
                }
                sb.Append(s);
                sb.Append(" s");
            }
            var ms = duration.Milliseconds;
            if (ms > 0)
            {
                if (sb.Length > 0)
                {
                    sb.Append(' ');
                }
                sb.Append(ms);
                sb.Append(" ms");
            }
            var us = duration.Microseconds;
            if (us > 0)
            {
                if (sb.Length > 0)
                {
                    sb.Append(' ');
                }
                sb.Append(us);
                sb.Append(" μs");
            }
            var ns = duration.Nanoseconds;
            if (ns > 0)
            {
                if (sb.Length > 0)
                {
                    sb.Append(' ');
                }
                sb.Append(ns);
                sb.Append(" ns");
            }
            var ps = duration.Picoseconds;
            if (ps > 0)
            {
                if (sb.Length > 0)
                {
                    sb.Append(' ');
                }
                sb.Append(ps);
                sb.Append(" ps");
            }
            var fs = duration.Femtoseconds;
            if (fs > 0)
            {
                if (sb.Length > 0)
                {
                    sb.Append(' ');
                }
                sb.Append(fs);
                sb.Append(" fs");
            }
            var at = duration.Attoseconds;
            if (at > 0)
            {
                if (sb.Length > 0)
                {
                    sb.Append(' ');
                }
                sb.Append(at);
                sb.Append(" as");
            }
            var zs = duration.Zeptoseconds;
            if (zs > 0)
            {
                if (sb.Length > 0)
                {
                    sb.Append(' ');
                }
                sb.Append(zs);
                sb.Append(" zs");
            }
            var ys = duration.Yoctoseconds;
            if (ys > 0)
            {
                if (sb.Length > 0)
                {
                    sb.Append(' ');
                }
                sb.Append(ys);
                sb.Append(" ys");
            }
            if (duration.PlanckTime > 0)
            {
                if (sb.Length > 0)
                {
                    sb.Append(' ');
                }
                sb.Append(duration.PlanckTime);
                sb.Append(" tP");
            }

            if (sb.Length == 0)
            {
                sb.Append(0);
            }

            return sb.ToString();
        }

        private static string FormatFullDateLongTime(Duration duration, NumberFormatInfo nfi)
            => Format(duration, FullDateLongTimeFormat.AsSpan(), nfi);

        private static string FormatFullDateShortTime(Duration duration, NumberFormatInfo nfi)
            => Format(duration, FullDateShortTimeFormat.AsSpan(), nfi);

        private static string FormatGeneralDateLongTime(Duration duration, NumberFormatInfo nfi)
            => Format(duration, GeneralDateLongTimeFormat.AsSpan(), nfi);

        private static string FormatGeneralDateShortTime(Duration duration, NumberFormatInfo nfi)
            => Format(duration, GeneralDateShortTimeFormat.AsSpan(), nfi);

        private static string FormatLongDate(Duration duration, NumberFormatInfo nfi)
            => Format(duration, LongDateFormat.AsSpan(), nfi);

        private static string FormatLongTime(Duration duration, NumberFormatInfo nfi)
            => Format(duration, LongTimeFormat.AsSpan(), nfi);

        private static string FormatRoundTrip(Duration duration, NumberFormatInfo nfi)
            => Format(duration, RoundTripFormat.AsSpan(), nfi);

        private static string FormatShortDate(Duration duration, NumberFormatInfo nfi)
            => Format(duration, ShortDateFormat.AsSpan(), nfi);

        private static string FormatShortTime(Duration duration, NumberFormatInfo nfi)
            => Format(duration, ShortTimeFormat.AsSpan(), nfi);
    }
}
