using System.Globalization;

namespace Tavenem.Time;

public partial struct RelativeDuration
{
    /// <summary>
    /// Converts the value of the current <see cref="RelativeDuration"/> object to its
    /// equivalent string representation using the specified <paramref name="format"/> and
    /// culture-specific format information.
    /// </summary>
    /// <param name="format">
    /// <para>
    /// A standard or custom date and time format string.
    /// </para>
    /// <para>
    /// Not all <see cref="DateTime"/> format strings are recognized. See Remarks for more info.
    /// </para>
    /// </param>
    /// <param name="formatProvider">An object that supplies culture-specific formatting
    /// information.</param>
    /// <returns>A string representation of value of the current <see cref="RelativeDuration"/>
    /// object as specified by <paramref name="format"/> and <paramref
    /// name="formatProvider"/>.</returns>
    /// <remarks>
    /// <para>
    /// When <see cref="Relativity"/> is <see cref="RelativeDurationType.ProportionOfDay"/> or <see
    /// cref="RelativeDurationType.ProportionOfYear"/>, <paramref name="format"/> is disregarded and the
    /// string representation is "Dx" for day, or "Yx" for year, followed by <see
    /// cref="Proportion"/> as a G-formatted <see cref="double"/>.
    /// </para>
    /// <para>
    /// The <paramref name="format"/> parameter can contain any of these single-character
    /// standard format strings:
    /// </para>
    /// <list type="table">
    /// <listheader>
    /// <term>Format specifier</term>
    /// <term>Description</term>
    /// </listheader>
    /// <item>
    /// <term>"d"</term>
    /// <description>Short date format. Corresponds to the "y d" custom format
    /// string.</description>
    /// </item>
    /// <item>
    /// <term>"D"</term>
    /// <description>Long date format. Corresponds to the "a d" custom format
    /// string.</description>
    /// </item>
    /// <item>
    /// <term>"E"</term>
    /// <description>General extended format. Corresponds to the "y d
    /// HH:mm:ss:MMM:uuu:nnn:ppp:fff:aaa:zzz:YYY:P" custom format
    /// string.</description>
    /// </item>
    /// <item>
    /// <term>"f"</term>
    /// <description>Full date short time format. Corresponds to the Long date and short time,
    /// separated by a space.</description>
    /// </item>
    /// <item>
    /// <term>"F"</term>
    /// <description>Full date long time format. Corresponds to the Long date and short time,
    /// separated by a space.</description>
    /// </item>
    /// <item>
    /// <term>"g"</term>
    /// <description>General short format. Corresponds to the "y d HH:mm" custom format
    /// string.</description>
    /// </item>
    /// <item>
    /// <term>"G"</term>
    /// <description>General long format. The default if none is selected. Corresponds to the "y
    /// d HH:mm:ss" custom format string.</description>
    /// </item>
    /// <item>
    /// <term>"O", "o"</term>
    /// <description>Round trip format. Corresponds to the "e'-'n':'Y':'P"
    /// custom format string. Because the length of certain segments varies (unavoidably, to
    /// prevent strings from being longer than <see cref="int.MaxValue"/> to account for <see
    /// cref="Duration"/> instances with the maximum number of aeons), this format is unsuitable
    /// for sorting.</description>
    /// </item>
    /// <item>
    /// <term>"t"</term>
    /// <description>Short time format. Corresponds to the "HH:mm" custom format
    /// string.</description>
    /// </item>
    /// <item>
    /// <term>"T"</term>
    /// <description>Long time format. Corresponds to the "HH:mm:ss" custom format
    /// string.</description>
    /// </item>
    /// <item>
    /// <term>"X"</term>
    /// <description>
    /// <para>
    /// Extensible format. Corresponds to the "e' a 'd' d 'H' h 'm' min 's' s 'M' ms 'u' μs 'n'
    /// ns 'p' ps 'f' fs 'a' as 'z' zs 'Y' ys 'P' tP" custom format string, except that each
    /// unit is displayed only if it is non-zero.
    /// </para>
    /// <para>
    /// The symbols denoting each unit are in SI notation, and are not culture-specific. In
    /// particular it should be noted that the symbol "a" is the SI symbol for years, which may
    /// confuse English readers on first glance.
    /// </para>
    /// </description>
    /// </item>
    /// </list>
    /// <para>
    /// The <paramref name="format"/> parameter can also or a custom format pattern with any of
    /// the following format strings:
    /// </para>
    /// <list type="table">
    /// <listheader>
    /// <term>Format specifier</term>
    /// <term>Description</term>
    /// </listheader>
    /// <item>
    /// <term>"a", "aa", "aaa"</term>
    /// <description>
    /// The number of attoseconds, from 0 to 999. The number of "a" characters in the format
    /// string represents the minimum number of significant digits; leading zeros will be added
    /// as needed.
    /// </description>
    /// </item>
    /// <item>
    /// <term>"d", "dd", "ddd"</term>
    /// <description>
    /// The number of days, from 0 to 365. Always displayed using as many digits as required,
    /// regardless of the number of "d" characters in the format string, but will not be
    /// displayed with <i>fewer</i> digits than there are "d" characters in the string; leading
    /// zeros will be added as needed.
    /// </description>
    /// </item>
    /// <item>
    /// <term>"dddd"</term>
    /// <description>Not recognized; <see cref="Duration"/> does not count weeks.</description>
    /// </item>
    /// <item>
    /// <term>"e", "ee", "eee", etc.</term>
    /// The number of years, <i>including</i> those represented by aeons. Always displayed in G
    /// format, with precision equal to the number of "e" characters, except when only 1 "e" is
    /// present, which indicates "G29" format. Successful round-trip operations are not possible
    /// for values larger than can be accurately represented with the chosen specification, and
    /// since aeons may contain a number of significant digits greater than can be recorded in a
    /// <see cref="string"/> without causing memory overflow, loss of precision may be
    /// unavoidable.
    /// </item>
    /// <item>
    /// <term>"f", "ff", "fff"</term>
    /// <description>
    /// The number of femtoseconds, from 0 to 999. The number of "f" characters in the format
    /// string represents the minimum number of significant digits; leading zeros will be added
    /// as needed.
    /// </description>
    /// </item>
    /// <item>
    /// <term>"F", "FF", "FFF", etc.</term>
    /// <description>
    /// <para>
    /// Displays fractions of a second to as many significant digits as there are "F"
    /// characters. Note that unlike <see cref="DateTime"/> format strings, the lower case "f"
    /// is not used to display a variable number of fractional seconds. The "f" is reserved in
    /// <see cref="Duration"/> format strings for femtoseconds.
    /// </para>
    /// <para>
    /// Note that if fractional seconds and a unit of time smaller than seconds are both
    /// displayed, aside from causing confusion this will result in failure to round-trip (the
    /// values of the fractional seconds and the smaller units each be counted on parse, causing
    /// the value to be larger than expected).
    /// </para>
    /// </description>
    /// </item>
    /// <item>
    /// <term>"g", "gg"</term>
    /// <description>Not recognized; <see cref="Duration"/> does not count periods or
    /// eras.</description>
    /// </item>
    /// <item>
    /// <term>"h", "H"</term>
    /// <description>The number of hours, from 0 to 23. 12-hour time is not recognized, since a
    /// <see cref="Duration"/> represents an amount of time, not a time of day.</description>
    /// </item>
    /// <item>
    /// <term>"hh", "HH", "hH", "Hh"</term>
    /// <description>The number of hours, from 00 to 23. Always displays two digits, even for
    /// single-digit values. 12-hour time is not recognized, since a <see cref="Duration"/>
    /// represents an amount of time, not a time of day.</description>
    /// </item>
    /// <item>
    /// <term>"K"</term>
    /// <description>Not recognized; <see cref="Duration"/> does not count time
    /// zones.</description>
    /// </item>
    /// <item>
    /// <term>"m", "mm"</term>
    /// <description>
    /// The number of minutes, from 0 to 59. The number of "m" characters in the format string
    /// represents the minimum number of significant digits; leading zeros will be added as
    /// needed.
    /// </description>
    /// </item>
    /// <item>
    /// <term>"M", "MM", "MMM"</term>
    /// <description>
    /// <para>
    /// The number of milliseconds, from 0 to 999. The number of "M" characters in the format
    /// string represents the minimum number of significant digits; leading zeros will be added
    /// as needed.
    /// </para>
    /// <para>
    /// Note carefully that this is in contrast to <see cref="DateTime"/> format strings, which
    /// uses "M" to denote months.
    /// </para>
    /// </description>
    /// </item>
    /// <item>
    /// <term>"n", "nn", "nnn"</term>
    /// <description>
    /// The number of nanoseconds, from 0 to 999. The number of "n" characters in the format
    /// string represents the minimum number of significant digits; leading zeros will be added
    /// as needed.
    /// </description>
    /// </item>
    /// <item>
    /// <term>"p", "pp", "ppp"</term>
    /// <description>
    /// The number of picoseconds, from 0 to 999. The number of "p" characters in the format
    /// string represents the minimum number of significant digits; leading zeros will be added
    /// as needed.
    /// </description>
    /// </item>
    /// <item>
    /// <term>"P", "PP", "PPP", etc.</term>
    /// <description>
    /// The amount of Planck time. Always displayed in G format, with precision equal to the
    /// number of "p" characters, except when only 1 "p" is present, which indicates "G" format.
    /// 17 should be used to round-trip.
    /// </description>
    /// </item>
    /// <item>
    /// <term>"s", "ss"</term>
    /// <description>
    /// The number of seconds, from 0 to 59. The number of "s" characters in the format string
    /// represents the minimum number of significant digits; leading zeros will be added as
    /// needed.
    /// </description>
    /// </item>
    /// <item>
    /// <term>"t", "tt"</term>
    /// <description>Not recognized; <see cref="Duration"/> represents an amount of time, not a
    /// time of day.</description>
    /// </item>
    /// <item>
    /// <term>"u", "uu", "uuu"</term>
    /// <description>
    /// The number of microseconds, from 0 to 999. The number of "U" characters represents the
    /// minimum number of significant digits displayed.
    /// </description>
    /// </item>
    /// <item>
    /// <term>"y", "yy", "yyy", etc.</term>
    /// <description>
    /// The number of years, not counting those represented by aeons. The number of "y"
    /// characters in the format string represents the minimum number of significant digits;
    /// leading zeros will be added as needed.
    /// </description>
    /// </item>
    /// <item>
    /// <term>"Y", "YY", "YYY"</term>
    /// <description>
    /// The number of yoctoseconds, from 0 to 999. The number of "Y" characters in the format
    /// string represents the minimum number of significant digits; leading zeros will be added
    /// as needed.
    /// </description>
    /// </item>
    /// <item>
    /// <term>"z", "zz", "zzz"</term>
    /// <description>
    /// <para>
    /// The number of zeptoseconds, from 0 to 999. The number of "z" characters in the format
    /// string represents the minimum number of significant digits; leading zeros will be added
    /// as needed.
    /// </para>
    /// <para>
    /// Note carefully that this is in contrast to <see cref="DateTime"/> format strings, which
    /// uses "z" to denote time zone.
    /// </para>
    /// </description>
    /// </item>
    /// <item>
    /// <term>":"</term>
    /// <description>The time separator.</description>
    /// </item>
    /// <item>
    /// <term>"/"</term>
    /// <description>The date separator.</description>
    /// </item>
    /// <item>
    /// <term>"<i>string</i>", '<i>string</i>'</term>
    /// <description>Literal string delimiters.</description>
    /// </item>
    /// <item>
    /// <term>"%"</term>
    /// <description>Defines the following character as a custom format specifier.</description>
    /// </item>
    /// <item>
    /// <term>"\"</term>
    /// <description>The escape character.</description>
    /// </item>
    /// <item>
    /// <term>Any other character</term>
    /// <description>The character is copied to the result string unchanged.</description>
    /// </item>
    /// </list>
    /// <para>
    /// Since the length of various format elements can vary independently of the length of the
    /// format string, custom formats which do not provide a separator between these elements
    /// and the next (e.g. "aaaddd") may result in a string representation which cannot be
    /// parsed, even with an exact format string parameter. Therefore a separator is always
    /// recommended (even whitespace is sufficient: " ").
    /// </para>
    /// <para>
    /// If <paramref name="format"/> is empty, or an unrecognized single character, the standard
    /// format specifier ("G") is used.
    /// </para>
    /// <para>
    /// A perpetual <see cref="RelativeDuration"/> is represented with the infinity symbol ("∞")
    /// regardless of format.
    /// </para>
    /// </remarks>
    public string ToString(string? format, IFormatProvider? formatProvider)
        => DurationFormatProvider.Instance.Format(format, this, formatProvider);

    /// <summary>
    /// Converts the value of the current <see cref="RelativeDuration"/> object to its
    /// equivalent string representation using the specified culture-specific format
    /// information.
    /// </summary>
    /// <param name="formatProvider">An object that supplies culture-specific formatting
    /// information.</param>
    /// <returns>A string representation of value of the current <see cref="RelativeDuration"/>
    /// object as specified by <paramref name="formatProvider"/>.</returns>
    /// <exception cref="FormatException">
    /// <paramref name="formatProvider"/> is not a valid provider for <see
    /// cref="RelativeDuration"/>.
    /// </exception>
    public string ToString(IFormatProvider? formatProvider)
        => DurationFormatProvider.Instance.Format(DurationFormatProvider.DefaultFormat, this, formatProvider);

    /// <summary>
    /// Converts the value of the current <see cref="RelativeDuration"/> object to its
    /// equivalent string representation using the specified <paramref name="format"/>.
    /// </summary>
    /// <param name="format">
    /// <para>
    /// A standard or custom date and time format string.
    /// </para>
    /// <para>
    /// Not all <see cref="DateTime"/> format strings are recognized. See Remarks for more info.
    /// </para>
    /// </param>
    /// <returns>A string representation of value of the current <see cref="RelativeDuration"/>
    /// object as specified by <paramref name="format"/>.</returns>
    /// <remarks>
    /// <para>
    /// When <see cref="Relativity"/> is <see cref="RelativeDurationType.ProportionOfDay"/> or <see
    /// cref="RelativeDurationType.ProportionOfYear"/>, <paramref name="format"/> is disregarded and the
    /// string representation is "Dx" for day, or "Yx" for year, followed by <see
    /// cref="Proportion"/> as a G-formatted <see cref="double"/>.
    /// </para>
    /// <para>
    /// The <paramref name="format"/> parameter can contain any of these single-character
    /// standard format strings:
    /// </para>
    /// <list type="table">
    /// <listheader>
    /// <term>Format specifier</term>
    /// <term>Description</term>
    /// </listheader>
    /// <item>
    /// <term>"d"</term>
    /// <description>Short date format. Corresponds to the "y d" custom format
    /// string.</description>
    /// </item>
    /// <item>
    /// <term>"D"</term>
    /// <description>Long date format. Corresponds to the "a d" custom format
    /// string.</description>
    /// </item>
    /// <item>
    /// <term>"E"</term>
    /// <description>General extended format. Corresponds to the "y d
    /// HH:mm:ss:MMM:uuu:nnn:ppp:fff:aaa:zzz:YYY:P" custom format
    /// string.</description>
    /// </item>
    /// <item>
    /// <term>"f"</term>
    /// <description>Full date short time format. Corresponds to the Long date and short time,
    /// separated by a space.</description>
    /// </item>
    /// <item>
    /// <term>"F"</term>
    /// <description>Full date long time format. Corresponds to the Long date and short time,
    /// separated by a space.</description>
    /// </item>
    /// <item>
    /// <term>"g"</term>
    /// <description>General short format. Corresponds to the "y d HH:mm" custom format
    /// string.</description>
    /// </item>
    /// <item>
    /// <term>"G"</term>
    /// <description>General long format. The default if none is selected. Corresponds to the "y
    /// d HH:mm:ss" custom format string.</description>
    /// </item>
    /// <item>
    /// <term>"O", "o"</term>
    /// <description>Round trip format. Corresponds to the "e'-'n':'Y':'P"
    /// custom format string. Because the length of certain segments varies (unavoidably, to
    /// prevent strings from being longer than <see cref="int.MaxValue"/> to account for <see
    /// cref="Duration"/> instances with the maximum number of aeons), this format is unsuitable
    /// for sorting.</description>
    /// </item>
    /// <item>
    /// <term>"t"</term>
    /// <description>Short time format. Corresponds to the "HH:mm" custom format
    /// string.</description>
    /// </item>
    /// <item>
    /// <term>"T"</term>
    /// <description>Long time format. Corresponds to the "HH:mm:ss" custom format
    /// string.</description>
    /// </item>
    /// <item>
    /// <term>"X"</term>
    /// <description>
    /// <para>
    /// Extensible format. Corresponds to the "e' a 'd' d 'H' h 'm' min 's' s 'M' ms 'u' μs 'n'
    /// ns 'p' ps 'f' fs 'a' as 'z' zs 'Y' ys 'P' tP" custom format string, except that each
    /// unit is displayed only if it is non-zero.
    /// </para>
    /// <para>
    /// The symbols denoting each unit are in SI notation, and are not culture-specific. In
    /// particular it should be noted that the symbol "a" is the SI symbol for years, which may
    /// confuse English readers on first glance.
    /// </para>
    /// </description>
    /// </item>
    /// </list>
    /// <para>
    /// The <paramref name="format"/> parameter can also or a custom format pattern with any of
    /// the following format strings:
    /// </para>
    /// <list type="table">
    /// <listheader>
    /// <term>Format specifier</term>
    /// <term>Description</term>
    /// </listheader>
    /// <item>
    /// <term>"a", "aa", "aaa"</term>
    /// <description>
    /// The number of attoseconds, from 0 to 999. The number of "a" characters in the format
    /// string represents the minimum number of significant digits; leading zeros will be added
    /// as needed.
    /// </description>
    /// </item>
    /// <item>
    /// <term>"d", "dd", "ddd"</term>
    /// <description>
    /// The number of days, from 0 to 365. Always displayed using as many digits as required,
    /// regardless of the number of "d" characters in the format string, but will not be
    /// displayed with <i>fewer</i> digits than there are "d" characters in the string; leading
    /// zeros will be added as needed.
    /// </description>
    /// </item>
    /// <item>
    /// <term>"dddd"</term>
    /// <description>Not recognized; <see cref="Duration"/> does not count weeks.</description>
    /// </item>
    /// <item>
    /// <term>"e", "ee", "eee", etc.</term>
    /// The number of years, <i>including</i> those represented by aeons. Always displayed in G
    /// format, with precision equal to the number of "e" characters, except when only 1 "e" is
    /// present, which indicates "G29" format. Successful round-trip operations are not possible
    /// for values larger than can be accurately represented with the chosen specification, and
    /// since aeons may contain a number of significant digits greater than can be recorded in a
    /// <see cref="string"/> without causing memory overflow, loss of precision may be
    /// unavoidable.
    /// </item>
    /// <item>
    /// <term>"f", "ff", "fff"</term>
    /// <description>
    /// The number of femtoseconds, from 0 to 999. The number of "f" characters in the format
    /// string represents the minimum number of significant digits; leading zeros will be added
    /// as needed.
    /// </description>
    /// </item>
    /// <item>
    /// <term>"F", "FF", "FFF", etc.</term>
    /// <description>
    /// <para>
    /// Displays fractions of a second to as many significant digits as there are "F"
    /// characters. Note that unlike <see cref="DateTime"/> format strings, the lower case "f"
    /// is not used to display a variable number of fractional seconds. The "f" is reserved in
    /// <see cref="Duration"/> format strings for femtoseconds.
    /// </para>
    /// <para>
    /// Note that if fractional seconds and a unit of time smaller than seconds are both
    /// displayed, aside from causing confusion this will result in failure to round-trip (the
    /// values of the fractional seconds and the smaller units each be counted on parse, causing
    /// the value to be larger than expected).
    /// </para>
    /// </description>
    /// </item>
    /// <item>
    /// <term>"g", "gg"</term>
    /// <description>Not recognized; <see cref="Duration"/> does not count periods or
    /// eras.</description>
    /// </item>
    /// <item>
    /// <term>"h", "H"</term>
    /// <description>The number of hours, from 0 to 23. 12-hour time is not recognized, since a
    /// <see cref="Duration"/> represents an amount of time, not a time of day.</description>
    /// </item>
    /// <item>
    /// <term>"hh", "HH", "hH", "Hh"</term>
    /// <description>The number of hours, from 00 to 23. Always displays two digits, even for
    /// single-digit values. 12-hour time is not recognized, since a <see cref="Duration"/>
    /// represents an amount of time, not a time of day.</description>
    /// </item>
    /// <item>
    /// <term>"K"</term>
    /// <description>Not recognized; <see cref="Duration"/> does not count time
    /// zones.</description>
    /// </item>
    /// <item>
    /// <term>"m", "mm"</term>
    /// <description>
    /// The number of minutes, from 0 to 59. The number of "m" characters in the format string
    /// represents the minimum number of significant digits; leading zeros will be added as
    /// needed.
    /// </description>
    /// </item>
    /// <item>
    /// <term>"M", "MM", "MMM"</term>
    /// <description>
    /// <para>
    /// The number of milliseconds, from 0 to 999. The number of "M" characters in the format
    /// string represents the minimum number of significant digits; leading zeros will be added
    /// as needed.
    /// </para>
    /// <para>
    /// Note carefully that this is in contrast to <see cref="DateTime"/> format strings, which
    /// uses "M" to denote months.
    /// </para>
    /// </description>
    /// </item>
    /// <item>
    /// <term>"n", "nn", "nnn"</term>
    /// <description>
    /// The number of nanoseconds, from 0 to 999. The number of "n" characters in the format
    /// string represents the minimum number of significant digits; leading zeros will be added
    /// as needed.
    /// </description>
    /// </item>
    /// <item>
    /// <term>"p", "pp", "ppp"</term>
    /// <description>
    /// The number of picoseconds, from 0 to 999. The number of "p" characters in the format
    /// string represents the minimum number of significant digits; leading zeros will be added
    /// as needed.
    /// </description>
    /// </item>
    /// <item>
    /// <term>"P", "PP", "PPP", etc.</term>
    /// <description>
    /// The amount of Planck time. Always displayed in G format, with precision equal to the
    /// number of "p" characters, except when only 1 "p" is present, which indicates "G" format.
    /// 17 should be used to round-trip.
    /// </description>
    /// </item>
    /// <item>
    /// <term>"s", "ss"</term>
    /// <description>
    /// The number of seconds, from 0 to 59. The number of "s" characters in the format string
    /// represents the minimum number of significant digits; leading zeros will be added as
    /// needed.
    /// </description>
    /// </item>
    /// <item>
    /// <term>"t", "tt"</term>
    /// <description>Not recognized; <see cref="Duration"/> represents an amount of time, not a
    /// time of day.</description>
    /// </item>
    /// <item>
    /// <term>"u", "uu", "uuu"</term>
    /// <description>
    /// The number of microseconds, from 0 to 999. The number of "U" characters represents the
    /// minimum number of significant digits displayed.
    /// </description>
    /// </item>
    /// <item>
    /// <term>"y", "yy", "yyy", etc.</term>
    /// <description>
    /// The number of years, not counting those represented by aeons. The number of "y"
    /// characters in the format string represents the minimum number of significant digits;
    /// leading zeros will be added as needed.
    /// </description>
    /// </item>
    /// <item>
    /// <term>"Y", "YY", "YYY"</term>
    /// <description>
    /// The number of yoctoseconds, from 0 to 999. The number of "Y" characters in the format
    /// string represents the minimum number of significant digits; leading zeros will be added
    /// as needed.
    /// </description>
    /// </item>
    /// <item>
    /// <term>"z", "zz", "zzz"</term>
    /// <description>
    /// <para>
    /// The number of zeptoseconds, from 0 to 999. The number of "z" characters in the format
    /// string represents the minimum number of significant digits; leading zeros will be added
    /// as needed.
    /// </para>
    /// <para>
    /// Note carefully that this is in contrast to <see cref="DateTime"/> format strings, which
    /// uses "z" to denote time zone.
    /// </para>
    /// </description>
    /// </item>
    /// <item>
    /// <term>":"</term>
    /// <description>The time separator.</description>
    /// </item>
    /// <item>
    /// <term>"/"</term>
    /// <description>The date separator.</description>
    /// </item>
    /// <item>
    /// <term>"<i>string</i>", '<i>string</i>'</term>
    /// <description>Literal string delimiters.</description>
    /// </item>
    /// <item>
    /// <term>"%"</term>
    /// <description>Defines the following character as a custom format specifier.</description>
    /// </item>
    /// <item>
    /// <term>"\"</term>
    /// <description>The escape character.</description>
    /// </item>
    /// <item>
    /// <term>Any other character</term>
    /// <description>The character is copied to the result string unchanged.</description>
    /// </item>
    /// </list>
    /// <para>
    /// Since the length of various format elements can vary independently of the length of the
    /// format string, custom formats which do not provide a separator between these elements
    /// and the next (e.g. "aaaddd") may result in a string representation which cannot be
    /// parsed, even with an exact format string parameter. Therefore a separator is always
    /// recommended (even whitespace is sufficient: " ").
    /// </para>
    /// <para>
    /// If <paramref name="format"/> is empty, or an unrecognized single character, the standard
    /// format specifier ("G") is used.
    /// </para>
    /// <para>
    /// A perpetual <see cref="RelativeDuration"/> is represented with the infinity symbol ("∞")
    /// regardless of format.
    /// </para>
    /// </remarks>
    public string ToString(string? format)
        => DurationFormatProvider.Instance.Format(format, this, CultureInfo.CurrentCulture);

    /// <summary>
    /// <para>
    /// Converts the value of the current <see cref="RelativeDuration"/> object to its
    /// equivalent string representation using the default format and current culture.
    /// </para>
    /// <para>
    /// A perpetual <see cref="RelativeDuration"/> is represented with the infinity symbol ("∞")
    /// regardless of format.
    /// </para>
    /// </summary>
    /// <remarks>
    /// When <see cref="Relativity"/> is <see cref="RelativeDurationType.ProportionOfDay"/> or <see
    /// cref="RelativeDurationType.ProportionOfYear"/>, formatting is disregarded and the string
    /// representation is "Dx" for day, or "Yx" for year, followed by <see cref="Proportion"/>
    /// as a G-formatted <see cref="double"/>.
    /// </remarks>
    public override string ToString()
        => DurationFormatProvider.Instance.Format(DurationFormatProvider.DefaultFormat, this, CultureInfo.CurrentCulture);

    /// <summary>
    /// Attempts to write this instance to the given <see cref="Span{T}"/>.
    /// </summary>
    /// <param name="destination">The <see cref="Span{T}"/> to write to.</param>
    /// <param name="charsWritten">
    /// When this method returns, this will contains the number of characters written to <paramref name="destination"/>.
    /// </param>
    /// <param name="format">A format string containing formatting specifications.</param>
    /// <param name="provider">
    /// An object that supplies format information about the current instance.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if this instance was successfully written to the
    /// <paramref name="destination"/>; otherwise <see langword="false"/>.
    /// </returns>
    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
        => DurationFormatProvider.TryFormat(this, destination, out charsWritten, format, provider);
}
