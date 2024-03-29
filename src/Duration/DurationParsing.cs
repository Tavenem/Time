﻿using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;

namespace Tavenem.Time;

public partial struct Duration
{
    /// <summary>
    /// Converts the specified string representation to a <see cref="Duration"/>.
    /// </summary>
    /// <param name="s">A string containing a duration to convert.</param>
    /// <returns>The <see cref="Duration"/> value equivalent to the duration contained in
    /// <paramref name="s"/>.</returns>
    /// <param name="provider">An object that supplies culture-specific formatting
    /// information.</param>
    /// <exception cref="FormatException"><paramref name="s"/> is empty, or contains only white
    /// space, contains invalid <see cref="Duration"/> data, or the format cannot be
    /// determined.</exception>
    public static Duration Parse(ReadOnlySpan<char> s, IFormatProvider? provider = null)
    {
        if (s.IsEmpty || s.IsWhiteSpace())
        {
            throw new FormatException();
        }
        if (TryParse(s, provider, out var result))
        {
            return result;
        }
        throw new FormatException();
    }

    /// <summary>
    /// Converts the specified string representation to a <see cref="Duration"/>.
    /// </summary>
    /// <param name="s">A string containing a duration to convert.</param>
    /// <returns>
    /// The <see cref="Duration"/> value equivalent to the duration contained in <paramref
    /// name="s"/>.
    /// </returns>
    /// <param name="provider">An object that supplies culture-specific formatting
    /// information.</param>
    /// <exception cref="FormatException">
    /// <paramref name="s"/> is an empty string (""), or contains only white space, contains invalid
    /// <see cref="Duration"/> data, or the format cannot be determined.
    /// </exception>
    public static Duration Parse(string s, IFormatProvider? provider = null)
        => Parse(s.AsSpan(), provider);

    /// <summary>
    /// Converts the specified string representation to a <see cref="Duration"/>.
    /// </summary>
    /// <param name="s">A string containing a duration to convert.</param>
    /// <returns>The <see cref="Duration"/> value equivalent to the duration contained in
    /// <paramref name="s"/>.</returns>
    /// <exception cref="FormatException"><paramref name="s"/> is empty, or contains only white
    /// space, contains invalid <see cref="Duration"/> data, or the format cannot be
    /// determined.</exception>
    public static Duration Parse(ReadOnlySpan<char> s) => Parse(s, null);

    /// <summary>
    /// Converts the specified string representation to a <see cref="Duration"/>.
    /// </summary>
    /// <param name="s">A string containing a duration to convert.</param>
    /// <returns>The <see cref="Duration"/> value equivalent to the duration contained in
    /// <paramref name="s"/>.</returns>
    /// <exception cref="FormatException"><paramref name="s"/> is an empty string (""), or
    /// contains only white space, contains invalid <see cref="Duration"/> data, or the format
    /// cannot be determined.</exception>
    public static Duration Parse(string s) => Parse(s, null);

    /// <summary>
    /// Converts the specified string representation to a <see cref="Duration"/>. The format of
    /// the string representation must match the specified format exactly.
    /// </summary>
    /// <param name="s">A string containing a duration to convert.</param>
    /// <param name="format">A format specifier that defines the required format of <paramref
    /// name="s"/>.</param>
    /// <param name="provider">An object that supplies culture-specific formatting
    /// information.</param>
    /// <returns>The <see cref="Duration"/> value equivalent to the duration contained in
    /// <paramref name="s"/>.</returns>
    /// <exception cref="FormatException"><paramref name="s"/> or <paramref name="format"/> is
    /// empty, or <paramref name="s"/> contains only white space, contains invalid <see
    /// cref="Duration"/> data, or the format cannot be determined.</exception>
    public static Duration ParseExact(ReadOnlySpan<char> s, ReadOnlySpan<char> format, IFormatProvider? provider = null)
    {
        if (s.IsEmpty || s.IsWhiteSpace())
        {
            throw new FormatException();
        }
        if (TryParseExact(s, format, provider, out var result))
        {
            return result;
        }
        throw new FormatException();
    }

    /// <summary>
    /// Converts the specified string representation to a <see cref="Duration"/>. The format of the
    /// string representation must match the specified format exactly.
    /// </summary>
    /// <param name="s">A string containing a duration to convert.</param>
    /// <param name="format">
    /// A format specifier that defines the required format of <paramref name="s"/>.
    /// </param>
    /// <param name="provider">
    /// An object that supplies culture-specific formatting information.
    /// </param>
    /// <returns>
    /// The <see cref="Duration"/> value equivalent to the duration contained in <paramref
    /// name="s"/>.
    /// </returns>
    /// <exception cref="FormatException">
    /// <paramref name="s"/> or <paramref name="format"/> is an empty string (""), or <paramref
    /// name="s"/> contains only white space, contains invalid <see cref="Duration"/> data, or the
    /// format cannot be determined.
    /// </exception>
    public static Duration ParseExact(string s, string? format = null, IFormatProvider? provider = null)
        => ParseExact(s.AsSpan(), format is null ? new ReadOnlySpan<char>() : format.AsSpan(), provider);

    /// <summary>
    /// Converts the specified string representation to a <see cref="Duration"/>. The format of
    /// the string representation must match the specified format exactly.
    /// </summary>
    /// <param name="s">A string containing a duration to convert.</param>
    /// <param name="format">A format specifier that defines the required format of <paramref
    /// name="s"/>.</param>
    /// <returns>The <see cref="Duration"/> value equivalent to the duration contained in
    /// <paramref name="s"/>.</returns>
    /// <exception cref="FormatException"><paramref name="s"/> or <paramref name="format"/> is
    /// an empty string (""), or <paramref name="s"/> contains only white space, contains
    /// invalid <see cref="Duration"/> data, or the format cannot be determined.</exception>
    public static Duration ParseExact(string s, string? format = null)
        => ParseExact(s, format, null);

    /// <summary>
    /// Converts the specified string representation to a <see cref="Duration"/>. The format of
    /// the string representation must match the specified format exactly.
    /// </summary>
    /// <param name="s">A string containing a duration to convert.</param>
    /// <param name="format">A format specifier that defines the required format of <paramref
    /// name="s"/>.</param>
    /// <returns>The <see cref="Duration"/> value equivalent to the duration contained in
    /// <paramref name="s"/>.</returns>
    /// <exception cref="FormatException"><paramref name="s"/> or <paramref name="format"/> is
    /// empty, or <paramref name="s"/> contains only white space, contains invalid <see
    /// cref="Duration"/> data, or the format cannot be determined.</exception>
    public static Duration ParseExact(ReadOnlySpan<char> s, ReadOnlySpan<char> format)
        => ParseExact(s, format, null);

    /// <summary>
    /// Attempts to convert the specified string representation of a <see cref="Duration"/> and
    /// returns a value that indicates whether the conversion succeeded.
    /// </summary>
    /// <param name="s">A string containing a duration to convert.</param>
    /// <param name="provider">An object that supplies culture-specific formatting
    /// information.</param>
    /// <param name="result">When this method returns, contains the <see cref="Duration"/> value
    /// equivalent to the duration contained in <paramref name="s"/>, if the conversion
    /// succeeded, or <see cref="Zero"/> if the conversion failed. The conversion fails if the
    /// <paramref name="s"/> parameter is <see langword="null"/>, or an empty string (""), or
    /// contains only white space, or contains invalid <see cref="Duration"/> data. This
    /// parameter is passed uninitialized.</param>
    /// <returns><see langword="true"/> if the <paramref name="s"/> parameter was converted
    /// successfully; otherwise, <see langword="false"/>.</returns>
    /// <remarks>
    /// <para>
    /// The <see cref="TryParse(ReadOnlySpan{char}, IFormatProvider?, out Duration)"/> method is
    /// similar to the <see cref="Parse(ReadOnlySpan{char}, IFormatProvider?)"/> method, except
    /// that the <see cref="TryParse(ReadOnlySpan{char}, IFormatProvider?, out Duration)"/>
    /// method does not throw an exception if the conversion fails.
    /// </para>
    /// <para>
    /// Only recognized formats can be parsed successfully. Even recognized formats may not
    /// round-trip values correctly, unless one of the formats specifically designed to do so is
    /// chosen <seealso cref="ToString(string, IFormatProvider)"/>
    /// </para>
    /// </remarks>
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out Duration result)
    {
        if (s.IsWhiteSpace())
        {
            result = Zero;
            return false;
        }

        if (s.Equals(NumberFormatInfo.GetInstance(provider).PositiveInfinitySymbol))
        {
            result = PositiveInfinity;
            return true;
        }

        return TryParseMultiple(s, provider, out result);
    }

    /// <summary>
    /// Attempts to convert the specified string representation of a <see cref="Duration"/> and
    /// returns a value that indicates whether the conversion succeeded.
    /// </summary>
    /// <param name="s">A string containing a duration to convert.</param>
    /// <param name="provider">An object that supplies culture-specific formatting
    /// information.</param>
    /// <param name="result">When this method returns, contains the <see cref="Duration"/> value
    /// equivalent to the duration contained in <paramref name="s"/>, if the conversion
    /// succeeded, or <see cref="Zero"/> if the conversion failed. The conversion fails if the
    /// <paramref name="s"/> parameter is <see langword="null"/>, or an empty string (""), or
    /// contains only white space, or contains invalid <see cref="Duration"/> data. This
    /// parameter is passed uninitialized.</param>
    /// <returns><see langword="true"/> if the <paramref name="s"/> parameter was converted
    /// successfully; otherwise, <see langword="false"/>.</returns>
    /// <remarks>
    /// <para>
    /// The <see cref="TryParse(string, IFormatProvider, out Duration)"/> method is similar to
    /// the <see cref="Parse(string, IFormatProvider)"/> method, except that the <see
    /// cref="TryParse(string, IFormatProvider, out Duration)"/> method does not throw an
    /// exception if the conversion fails.
    /// </para>
    /// <para>
    /// Only recognized formats can be parsed successfully. Even recognized formats may not
    /// round-trip values correctly, unless one of the formats specifically designed to do so is
    /// chosen <seealso cref="ToString(string, IFormatProvider)"/>
    /// </para>
    /// </remarks>
    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out Duration result)
    {
        if (string.IsNullOrWhiteSpace(s))
        {
            result = Zero;
            return false;
        }
        return TryParse(s.AsSpan(), provider, out result);
    }

    /// <summary>
    /// Attempts to convert the specified string representation of a <see cref="Duration"/> and
    /// returns a value that indicates whether the conversion succeeded.
    /// </summary>
    /// <param name="s">A string containing a duration to convert.</param>
    /// <param name="result">When this method returns, contains the <see cref="Duration"/> value
    /// equivalent to the duration contained in <paramref name="s"/>, if the conversion
    /// succeeded, or <see cref="Zero"/> if the conversion failed. The conversion fails if the
    /// <paramref name="s"/> parameter is <see langword="null"/>, or an empty string (""), or
    /// contains only white space, or contains invalid <see cref="Duration"/> data. This
    /// parameter is passed uninitialized.</param>
    /// <returns><see langword="true"/> if the <paramref name="s"/> parameter was converted
    /// successfully; otherwise, <see langword="false"/>.</returns>
    /// <remarks>
    /// <para>
    /// The <see cref="TryParse(string, out Duration)"/> method is similar to the <see
    /// cref="Parse(string)"/> method, except that the <see cref="TryParse(string, out
    /// Duration)"/> method does not throw an exception if the conversion fails.
    /// </para>
    /// <para>
    /// Only recognized formats can be parsed successfully. Even recognized formats may not
    /// round-trip values correctly, unless one of the formats specifically designed to do so is
    /// chosen <seealso cref="ToString(string, IFormatProvider)"/>
    /// </para>
    /// </remarks>
    public static bool TryParse([NotNullWhen(true)] string? s, out Duration result)
        => TryParse(s, null, out result);

    /// <summary>
    /// Attempts to convert the specified string representation of a <see cref="Duration"/> and
    /// returns a value that indicates whether the conversion succeeded.
    /// </summary>
    /// <param name="s">A string containing a duration to convert.</param>
    /// <param name="result">When this method returns, contains the <see cref="Duration"/> value
    /// equivalent to the duration contained in <paramref name="s"/>, if the conversion
    /// succeeded, or <see cref="Zero"/> if the conversion failed. The conversion fails if the
    /// <paramref name="s"/> parameter is <see langword="null"/>, or an empty string (""), or
    /// contains only white space, or contains invalid <see cref="Duration"/> data. This
    /// parameter is passed uninitialized.</param>
    /// <returns><see langword="true"/> if the <paramref name="s"/> parameter was converted
    /// successfully; otherwise, <see langword="false"/>.</returns>
    /// <remarks>
    /// <para>
    /// The <see cref="TryParse(string?, out Duration)"/> method is similar to the
    /// <see cref="Parse(ReadOnlySpan{char})"/> method, except that the <see
    /// cref="TryParse(string?, out Duration)"/> method does not throw an exception if the
    /// conversion fails.
    /// </para>
    /// <para>
    /// Only recognized formats can be parsed successfully. Even recognized formats may not
    /// round-trip values correctly, unless one of the formats specifically designed to do so is
    /// chosen <seealso cref="ToString(string, IFormatProvider)"/>
    /// </para>
    /// </remarks>
    public static bool TryParse(ReadOnlySpan<char> s, out Duration result)
        => TryParse(s, null, out result);

    /// <summary>
    /// Attempts to convert the specified string representation of a <see cref="Duration"/> and
    /// returns a value that indicates whether the conversion succeeded. The format of the
    /// string representation must match the specified format exactly.
    /// </summary>
    /// <param name="s">A string containing a duration to convert.</param>
    /// <param name="format">The required format of <paramref name="s"/>.</param>
    /// <param name="provider">An object that supplies culture-specific formatting
    /// information.</param>
    /// <param name="result">When this method returns, contains the <see cref="Duration"/> value
    /// equivalent to the duration contained in <paramref name="s"/>, if the conversion
    /// succeeded, or <see cref="Zero"/> if the conversion failed. The conversion fails if the
    /// <paramref name="s"/> parameter is <see langword="null"/>, or an empty string (""), or
    /// contains only white space, or contains invalid <see cref="Duration"/> data. This
    /// parameter is passed uninitialized.</param>
    /// <returns><see langword="true"/> if the <paramref name="s"/> parameter was converted
    /// successfully; otherwise, <see langword="false"/>.</returns>
    /// <remarks>
    /// <para>
    /// The <see cref="TryParseExact(ReadOnlySpan{char}, ReadOnlySpan{char}, IFormatProvider?, out Duration)"/>
    /// method is similar to the <see cref="ParseExact(ReadOnlySpan{char}, ReadOnlySpan{char}, IFormatProvider?)"/>
    /// method, except that the <see cref="TryParseExact(ReadOnlySpan{char}, ReadOnlySpan{char}, IFormatProvider?, out Duration)"/>
    /// method does not throw an exception if the conversion fails.
    /// </para>
    /// <para>
    /// Only recognized formats can be parsed successfully. Even recognized formats may not
    /// round-trip values correctly, unless one of the formats specifically designed to do so is
    /// chosen <seealso cref="ToString(string, IFormatProvider)"/>
    /// </para>
    /// </remarks>
    public static bool TryParseExact(ReadOnlySpan<char> s, ReadOnlySpan<char> format, IFormatProvider? provider, out Duration result)
    {
        if (s.IsWhiteSpace())
        {
            result = Zero;
            return false;
        }

        var nfi = NumberFormatInfo.GetInstance(provider);

        if (s.Equals(nfi.PositiveInfinitySymbol, StringComparison.Ordinal))
        {
            result = PositiveInfinity;
            return true;
        }
        if (s.Equals(nfi.NegativeInfinitySymbol, StringComparison.Ordinal))
        {
            result = NegativeInfinity;
            return true;
        }

        if (format.IsWhiteSpace())
        {
            return TryParseExact(s, DurationFormatProvider.GeneralDateLongTimeFormat.AsSpan(), provider, out result);
        }

        if (format.Length == 1)
        {
            return format[0] switch
            {
                'd' => TryParseExact(s, DurationFormatProvider.ShortDateFormat.AsSpan(), provider, out result),
                'D' => TryParseExact(s, DurationFormatProvider.LongDateFormat.AsSpan(), provider, out result),
                'E' => TryParseExact(s, DurationFormatProvider.ExtendedFormat.AsSpan(), provider, out result),
                'f' => TryParseExact(s, DurationFormatProvider.FullDateShortTimeFormat.AsSpan(), provider, out result),
                's' => TryParseExact(s, DurationFormatProvider.FullDateLongTimeFormat.AsSpan(), provider, out result),
                'g' => TryParseExact(s, DurationFormatProvider.GeneralDateShortTimeFormat.AsSpan(), provider, out result),
                'o' or 'O' => TryParseExact(s, DurationFormatProvider.RoundTripFormat.AsSpan(), provider, out result),
                't' => TryParseExact(s, DurationFormatProvider.ShortTimeFormat.AsSpan(), provider, out result),
                'T' => TryParseExact(s, DurationFormatProvider.LongTimeFormat.AsSpan(), provider, out result),
                'X' => TryParseExtensible(s, nfi, out result),
                _ => TryParseExact(s, DurationFormatProvider.GeneralDateLongTimeFormat.AsSpan(), provider, out result),
            };
        }

        return TryParseFormat(s, format, nfi, out result);
    }

    /// <summary>
    /// Attempts to convert the specified string representation of a <see cref="Duration"/> and
    /// returns a value that indicates whether the conversion succeeded. The format of the
    /// string representation must match the specified format exactly.
    /// </summary>
    /// <param name="s">A string containing a duration to convert.</param>
    /// <param name="format">The required format of <paramref name="s"/>.</param>
    /// <param name="provider">An object that supplies culture-specific formatting
    /// information.</param>
    /// <param name="result">When this method returns, contains the <see cref="Duration"/> value
    /// equivalent to the duration contained in <paramref name="s"/>, if the conversion
    /// succeeded, or <see cref="Zero"/> if the conversion failed. The conversion fails if the
    /// <paramref name="s"/> parameter is <see langword="null"/>, or an empty string (""), or
    /// contains only white space, or contains invalid <see cref="Duration"/> data. This
    /// parameter is passed uninitialized.</param>
    /// <returns><see langword="true"/> if the <paramref name="s"/> parameter was converted
    /// successfully; otherwise, <see langword="false"/>.</returns>
    /// <remarks>
    /// <para>
    /// The <see cref="TryParseExact(string, string, IFormatProvider, out Duration)"/> method is
    /// similar to the <see cref="ParseExact(string, string, IFormatProvider)"/> method, except
    /// that the <see cref="TryParseExact(string, string, IFormatProvider, out Duration)"/>
    /// method does not throw an exception if the conversion fails.
    /// </para>
    /// <para>
    /// Only recognized formats can be parsed successfully. Even recognized formats may not
    /// round-trip values correctly, unless one of the formats specifically designed to do so is
    /// chosen <seealso cref="ToString(string, IFormatProvider)"/>
    /// </para>
    /// </remarks>
    public static bool TryParseExact([NotNullWhen(true)] string? s, string? format, IFormatProvider? provider, out Duration result)
    {
        if (string.IsNullOrWhiteSpace(s))
        {
            result = Zero;
            return false;
        }

        return TryParseExact(s.AsSpan(), format is null ? new ReadOnlySpan<char>() : format.AsSpan(), provider, out result);
    }

    /// <summary>
    /// Attempts to convert the specified string representation of a <see cref="Duration"/> and
    /// returns a value that indicates whether the conversion succeeded. The format of the
    /// string representation must match the specified format exactly.
    /// </summary>
    /// <param name="s">A string containing a duration to convert.</param>
    /// <param name="format">The required format of <paramref name="s"/>.</param>
    /// <param name="result">When this method returns, contains the <see cref="Duration"/> value
    /// equivalent to the duration contained in <paramref name="s"/>, if the conversion
    /// succeeded, or <see cref="Zero"/> if the conversion failed. The conversion fails if the
    /// <paramref name="s"/> parameter is <see langword="null"/>, or an empty string (""), or
    /// contains only white space, or contains invalid <see cref="Duration"/> data. This
    /// parameter is passed uninitialized.</param>
    /// <returns><see langword="true"/> if the <paramref name="s"/> parameter was converted
    /// successfully; otherwise, <see langword="false"/>.</returns>
    /// <remarks>
    /// <para>
    /// The <see cref="TryParseExact(string, string, out Duration)"/> method is similar to the
    /// <see cref="ParseExact(string, string)"/> method, except that the <see
    /// cref="TryParseExact(string, string, out Duration)"/> method does not throw an exception
    /// if the conversion fails.
    /// </para>
    /// <para>
    /// Only recognized formats can be parsed successfully. Even recognized formats may not
    /// round-trip values correctly, unless one of the formats specifically designed to do so is
    /// chosen <seealso cref="ToString(string, IFormatProvider)"/>
    /// </para>
    /// </remarks>
    public static bool TryParseExact([NotNullWhen(true)] string? s, string? format, out Duration result)
        => TryParseExact(s, format, null, out result);

    /// <summary>
    /// Attempts to convert the specified string representation of a <see cref="Duration"/> and
    /// returns a value that indicates whether the conversion succeeded. The format of the
    /// string representation must match the specified format exactly.
    /// </summary>
    /// <param name="s">A string containing a duration to convert.</param>
    /// <param name="format">The required format of <paramref name="s"/>.</param>
    /// <param name="result">When this method returns, contains the <see cref="Duration"/> value
    /// equivalent to the duration contained in <paramref name="s"/>, if the conversion
    /// succeeded, or <see cref="Zero"/> if the conversion failed. The conversion fails if the
    /// <paramref name="s"/> parameter is <see langword="null"/>, or an empty string (""), or
    /// contains only white space, or contains invalid <see cref="Duration"/> data. This
    /// parameter is passed uninitialized.</param>
    /// <returns><see langword="true"/> if the <paramref name="s"/> parameter was converted
    /// successfully; otherwise, <see langword="false"/>.</returns>
    /// <remarks>
    /// <para>
    /// The <see cref="TryParseExact(ReadOnlySpan{char}, ReadOnlySpan{char}, out Duration)"/>
    /// method is similar to the <see cref="ParseExact(ReadOnlySpan{char}, ReadOnlySpan{char})"/>
    /// method, except that the <see cref="TryParseExact(ReadOnlySpan{char}, ReadOnlySpan{char}, out Duration)"/>
    /// method does not throw an exception if the conversion fails.
    /// </para>
    /// <para>
    /// Only recognized formats can be parsed successfully. Even recognized formats may not
    /// round-trip values correctly, unless one of the formats specifically designed to do so is
    /// chosen <seealso cref="ToString(string, IFormatProvider)"/>
    /// </para>
    /// </remarks>
    public static bool TryParseExact(ReadOnlySpan<char> s, ReadOnlySpan<char> format, out Duration result)
        => TryParseExact(s, format, null, out result);

    private static bool TryParseExtensible(in ReadOnlySpan<char> input, NumberFormatInfo nfi, out Duration result)
    {
        result = Zero;

        var isNegative = false;
        BigInteger? aeons = null;
        var yearValue = 0u;
        var nanosecondValue = 0ul;
        var yoctosecondValue = 0ul;
        BigInteger? planckTimeValue = null;

        var slices = new List<(int start, int length, bool isDigit)>();

        var index = 0;
        var sliceStart = 0;
        var sliceLength = 0;
        var digit = false;

        while (index < input.Length)
        {
            if ((uint)(input[index] - '0') <= 9)
            {
                if (!digit)
                {
                    slices.Add((sliceStart, sliceLength, digit));
                    sliceStart = index;
                    sliceLength = 0;
                }
                digit = true;
                sliceLength++;
                index++;
                continue;
            }
            if (input.Slice(index, nfi.NumberGroupSeparator.Length).Equals(nfi.NumberGroupSeparator))
            {
                sliceLength += nfi.NumberGroupSeparator.Length;
                index += nfi.NumberGroupSeparator.Length;
                continue;
            }
            if (input.Slice(index, nfi.NumberDecimalSeparator.Length).Equals(nfi.NumberDecimalSeparator))
            {
                sliceLength += nfi.NumberDecimalSeparator.Length;
                index += nfi.NumberDecimalSeparator.Length;
                continue;
            }
            if (input.Slice(index, nfi.NegativeSign.Length).Equals(nfi.NegativeSign))
            {
                sliceLength += nfi.NegativeSign.Length;
                index += nfi.NegativeSign.Length;
                isNegative = true;
                continue;
            }
            if (digit)
            {
                // scientific notation
                if ((input[index].Equals('e')
                    || input[index].Equals('E'))
                    && input.Length > index + 2
                    && (input[index + 1].Equals('+')
                    || input[index + 1].Equals('-'))
                    && (uint)(input[index + 2] - '0') <= 9)
                {
                    index += 3;
                    continue;
                }

                slices.Add((sliceStart, sliceLength, digit));
                sliceStart = index;
                sliceLength = 0;
                digit = false;
                index++;
            }
        }

        static bool TryParseUnit(in ReadOnlySpan<char> slice, out FormatUnit unit)
        {
            unit = FormatUnit.None;
            if (slice.Equals('a'))
            {
                unit = FormatUnit.Aeon;
                return true;
            }
            if (slice.Equals('d'))
            {
                unit = FormatUnit.Day;
                return true;
            }
            if (slice.Equals('h'))
            {
                unit = FormatUnit.Hour;
                return true;
            }
            if (slice.Equals("min"))
            {
                unit = FormatUnit.Minute;
                return true;
            }
            if (slice.Equals('s'))
            {
                unit = FormatUnit.Second;
                return true;
            }
            if (slice.Equals("ms"))
            {
                unit = FormatUnit.Milli;
                return true;
            }
            if (slice.Equals("μs"))
            {
                unit = FormatUnit.Micro;
                return true;
            }
            if (slice.Equals("ns"))
            {
                unit = FormatUnit.Nano;
                return true;
            }
            if (slice.Equals("ps"))
            {
                unit = FormatUnit.Pico;
                return true;
            }
            if (slice.Equals("fs"))
            {
                unit = FormatUnit.Femto;
                return true;
            }
            if (slice.Equals("as"))
            {
                unit = FormatUnit.Atto;
                return true;
            }
            if (slice.Equals("zs"))
            {
                unit = FormatUnit.Zepto;
                return true;
            }
            if (slice.Equals("ys"))
            {
                unit = FormatUnit.Yocto;
                return true;
            }
            if (slice.Equals("tP"))
            {
                unit = FormatUnit.Planck;
                return true;
            }
            return false;
        }

        void AddSecondFractions(uint value)
        {
            var sfs = value.ToString("D");
            if (sfs.Length <= 9)
            {
                var x = 9 - sfs.Length;
                while (x > 0)
                {
                    value *= 10;
                    x--;
                }
                nanosecondValue += value;
            }
            else
            {
                var sf = value;
                var sfl = sfs.Length;
                while (sfl > 9)
                {
                    sf /= 10;
                    sfl--;
                }
                nanosecondValue += sf;

                var r = uint.Parse(sfs[10..]);
                var rs = r.ToString("D");

                if (rs.Length <= 15)
                {
                    var x = 15 - rs.Length;
                    while (x > 0)
                    {
                        r *= 10;
                        x--;
                    }
                    yoctosecondValue += r;
                }
                else
                {
                    var yx = r;
                    var yxl = rs.Length;
                    while (yxl > 15)
                    {
                        yx /= 10;
                        yxl--;
                    }
                    yoctosecondValue += yx;
                    planckTimeValue = (planckTimeValue ?? BigInteger.Zero)
                        + (BigInteger)(decimal.Parse($"0.{rs[16..]}") * PlanckTimePerYoctosecondDecimal);
                }
            }
        }

        index = 0;
        while (index < slices.Count)
        {
            var adv = 1;

            var slice = input.Slice(slices[index].start, slices[index].length).Trim();

            if (slices[index].isDigit)
            {
                var unit = FormatUnit.None;
                if (slices.Count > index + 1 && !slices[index + 1].isDigit)
                {
                    adv = 2;
                    if (!TryParseUnit(input.Slice(slices[index + 1].start, slices[index + 1].length).Trim(), out unit))
                    {
                        return false;
                    }
                }
                if (unit == FormatUnit.None)
                {
                    return false;
                }

                var uintValue = 0u;
                if (unit != FormatUnit.Aeon
                    && unit != FormatUnit.None
                    && unit != FormatUnit.Planck
                    && !uint.TryParse(slice, out uintValue))
                {
                    return false;
                }
                switch (unit)
                {
                    case FormatUnit.Aeon:
                        if (BigInteger.TryParse(slice, NumberStyles.Integer, nfi, out var ae))
                        {
                            yearValue += (uint)(ae % YearsPerAeon);
                            ae /= YearsPerAeon;
                            aeons = ae;
                        }
                        else
                        {
                            return false;
                        }
                        break;
                    case FormatUnit.Year:
                        yearValue += uintValue;
                        break;
                    case FormatUnit.Day:
                        nanosecondValue += uintValue * (ulong)NanosecondsPerDay;
                        break;
                    case FormatUnit.Hour:
                        nanosecondValue += uintValue * (ulong)NanosecondsPerHour;
                        break;
                    case FormatUnit.Minute:
                        nanosecondValue += uintValue * (ulong)NanosecondsPerMinute;
                        break;
                    case FormatUnit.Second:
                        nanosecondValue += uintValue * NanosecondsPerSecond;
                        break;
                    case FormatUnit.SecondFraction:
                        AddSecondFractions(uintValue);
                        break;
                    case FormatUnit.Milli:
                        nanosecondValue += uintValue * NanosecondsPerMillisecond;
                        break;
                    case FormatUnit.Micro:
                        nanosecondValue += uintValue * NanosecondsPerMicrosecond;
                        break;
                    case FormatUnit.Nano:
                        nanosecondValue += uintValue;
                        break;
                    case FormatUnit.Pico:
                        yoctosecondValue += uintValue * (ulong)YoctosecondsPerPicosecond;
                        break;
                    case FormatUnit.Femto:
                        yoctosecondValue += uintValue * YoctosecondsPerFemtosecond;
                        break;
                    case FormatUnit.Atto:
                        yoctosecondValue += uintValue * YoctosecondsPerAttosecond;
                        break;
                    case FormatUnit.Zepto:
                        yoctosecondValue += uintValue * YoctosecondsPerZeptosecond;
                        break;
                    case FormatUnit.Yocto:
                        yoctosecondValue += uintValue;
                        break;
                    case FormatUnit.Planck:
                        if (BigInteger.TryParse(slice, NumberStyles.Integer, nfi, out var p))
                        {
                            planckTimeValue = (planckTimeValue ?? BigInteger.Zero) + p;
                        }
                        else
                        {
                            return false;
                        }
                        break;
                }
            }

            index += adv;
        }

        result = new Duration(isNegative, false, planckTimeValue, yoctosecondValue, nanosecondValue, yearValue, aeons);
        return true;
    }

    private static bool TryParseFormat(ReadOnlySpan<char> input, in ReadOnlySpan<char> format, NumberFormatInfo nfi, out Duration result)
    {
        result = Zero;

        BigInteger? aeons = null;
        var yearValue = 0u;
        var nanosecondValue = 0ul;
        var yoctosecondValue = 0ul;
        BigInteger? planckTimeValue = null;

        var broken = false;
        var doubleQuote = false;
        var processing = FormatUnit.None;
        char? sep = null;
        var sepLength = 0;
        var singleQuote = false;
        var startIndex = 0;
        var unitIndexCount = 0;
        var unitCount = 0;

        var units = new Dictionary<int, ParseUnitInfo>();

        var isNegative = false;
        if (input.StartsWith(nfi.NegativeSign))
        {
            isNegative = true;
            input = input[nfi.NegativeSign.Length..];
        }

        void CompleteUnit()
        {
            units[unitIndexCount] = new ParseUnitInfo(processing, unitIndexCount, unitCount, sep, sepLength);
            if (processing != FormatUnit.None)
            {
                unitIndexCount++;
                sep = null;
                sepLength = 0;
                unitCount = 0;
            }
        }

        void ProcessMisc(char ch)
        {
            if (processing == FormatUnit.None)
            {
                startIndex++;
            }
            else
            {
                if (sepLength == 0)
                {
                    sep = ch;
                }
                sepLength++;
            }
        }

        foreach (var ch in format)
        {
            if (ch == '"')
            {
                if (singleQuote)
                {
                    ProcessMisc(ch);
                }
                else if (broken)
                {
                    ProcessMisc(ch);
                    broken = false;
                }
                else
                {
                    doubleQuote = !doubleQuote;
                }
            }
            else if (ch == '\'')
            {
                if (doubleQuote)
                {
                    ProcessMisc(ch);
                }
                else if (broken)
                {
                    ProcessMisc(ch);
                    broken = false;
                }
                else
                {
                    singleQuote = !singleQuote;
                }
            }
            else if (ch == '\\')
            {
                if (broken)
                {
                    ProcessMisc(ch);
                }
                else
                {
                    broken = true;
                }
            }
            else if (singleQuote || doubleQuote)
            {
                if (!broken)
                {
                    ProcessMisc(ch);
                }
            }
            else if (broken)
            {
                ProcessMisc(ch);
                broken = false;
            }
            else if (ch != '%')
            {
                var found = false;
                foreach (var formatInfo in DurationFormatProvider._FormatInfo.Values)
                {
                    if (formatInfo.Match(ch))
                    {
                        found = true;
                        if (processing != formatInfo.Unit && processing != FormatUnit.None)
                        {
                            CompleteUnit();
                        }
                        processing = formatInfo.Unit;
                        unitCount++;
                        break;
                    }
                }
                if (!found)
                {
                    ProcessMisc(ch);
                }
            }
        }

        CompleteUnit();

        bool TryGetDigitSlice(in ReadOnlySpan<char> inp, int curI, int curUnitI, char? unitSep, int unitLength, out ReadOnlySpan<char> slice, out int length)
        {
            length = 0;
            slice = inp;
            if (curUnitI == unitIndexCount - 1)
            {
                length = inp.Length - curI;
                slice = inp[curI..];
                return true;
            }
            if (unitSep.HasValue)
            {
                var sepIndex = inp[curI..].IndexOf(unitSep.Value);
                if (sepIndex == -1)
                {
                    return false;
                }
                length = sepIndex;
            }
            else if (unitLength == 2)
            {
                length = 2;
            }
            else if (unitLength == 1)
            {
                length = 1;
            }
            else
            {
                return false;
            }
            slice = inp.Slice(curI, length);
            return true;
        }

        void AddSecondFractions(ulong value)
        {
            var sfs = value.ToString("D");
            if (sfs.Length <= 9)
            {
                var x = 9 - sfs.Length;
                while (x > 0)
                {
                    value *= 10;
                    x--;
                }
                nanosecondValue += value;
            }
            else
            {
                var sf = value;
                var sfl = sfs.Length;
                while (sfl > 9)
                {
                    sf /= 10;
                    sfl--;
                }
                nanosecondValue += sf;

                var r = uint.Parse(sfs[10..]);
                var rs = r.ToString("D");

                if (rs.Length <= 15)
                {
                    var x = 15 - rs.Length;
                    while (x > 0)
                    {
                        r *= 10;
                        x--;
                    }
                    yoctosecondValue += r;
                }
                else
                {
                    var yx = r;
                    var yxl = rs.Length;
                    while (yxl > 15)
                    {
                        yx /= 10;
                        yxl--;
                    }
                    yoctosecondValue += yx;
                    planckTimeValue = (planckTimeValue ?? BigInteger.Zero)
                        + (BigInteger)(decimal.Parse($"0.{rs[16..]}") * PlanckTimePerYoctosecondDecimal);
                }
            }
        }

        var index = 0;
        var unitIndex = 0;
        while (unitIndex < unitIndexCount)
        {
            var unit = units[unitIndex];
            if (!TryGetDigitSlice(input, index, unitIndex, unit.Separator, unit.Length, out var slice, out var length))
            {
                return false;
            }
            var ulongValue = 0UL;
            if (unit.Unit != FormatUnit.Aeon
                && unit.Unit != FormatUnit.None
                && unit.Unit != FormatUnit.Planck
                && !ulong.TryParse(slice, out ulongValue))
            {
                return false;
            }
            switch (unit.Unit)
            {
                case FormatUnit.Aeon:
                    if (BigInteger.TryParse(slice, NumberStyles.Integer, nfi, out var ae))
                    {
                        yearValue += (uint)(ae % YearsPerAeon);
                        ae /= YearsPerAeon;
                        aeons = (aeons ?? BigInteger.Zero) + ae;
                    }
                    else
                    {
                        return false;
                    }
                    break;
                case FormatUnit.Year:
                    if (ulongValue > uint.MaxValue)
                    {
                        if (BigInteger.TryParse(slice, NumberStyles.Integer, nfi, out var yearAeons))
                        {
                            aeons = (aeons ?? BigInteger.Zero) + (yearAeons / 1000000);
                            yearValue += (uint)(yearAeons % 1000000);
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        yearValue += (uint)ulongValue;
                    }
                    break;
                case FormatUnit.Day:
                    nanosecondValue += ulongValue * NanosecondsPerDay;
                    break;
                case FormatUnit.Hour:
                    nanosecondValue += ulongValue * NanosecondsPerHour;
                    break;
                case FormatUnit.Minute:
                    nanosecondValue += ulongValue * NanosecondsPerMinute;
                    break;
                case FormatUnit.Second:
                    nanosecondValue += ulongValue * NanosecondsPerSecond;
                    break;
                case FormatUnit.SecondFraction:
                    AddSecondFractions(ulongValue);
                    break;
                case FormatUnit.Milli:
                    nanosecondValue += ulongValue * NanosecondsPerMillisecond;
                    break;
                case FormatUnit.Micro:
                    nanosecondValue += ulongValue * NanosecondsPerMicrosecond;
                    break;
                case FormatUnit.Nano:
                    nanosecondValue += ulongValue;
                    break;
                case FormatUnit.Pico:
                    yoctosecondValue += ulongValue * YoctosecondsPerPicosecond;
                    break;
                case FormatUnit.Femto:
                    yoctosecondValue += ulongValue * YoctosecondsPerFemtosecond;
                    break;
                case FormatUnit.Atto:
                    yoctosecondValue += ulongValue * YoctosecondsPerAttosecond;
                    break;
                case FormatUnit.Zepto:
                    yoctosecondValue += ulongValue * YoctosecondsPerZeptosecond;
                    break;
                case FormatUnit.Yocto:
                    yoctosecondValue += ulongValue;
                    break;
                case FormatUnit.Planck:
                    if (BigInteger.TryParse(slice, NumberStyles.Integer, nfi, out var p))
                    {
                        planckTimeValue = (planckTimeValue ?? BigInteger.Zero) + p;
                    }
                    else
                    {
                        return false;
                    }
                    break;
            }
            unitIndex++;
            index += length;
            index += unit.SeparatorLength;
        }

        result = new Duration(isNegative, false, planckTimeValue, yoctosecondValue, nanosecondValue, yearValue, aeons);
        return true;
    }

    private static bool TryParseMultiple(in ReadOnlySpan<char> s, IFormatProvider? provider, out Duration result)
    {
        foreach (var format in DurationFormatProvider._AllFormats)
        {
            if (TryParseExact(s, format.AsSpan(), provider, out result))
            {
                return true;
            }
        }

        result = Zero;
        return false;
    }
}
