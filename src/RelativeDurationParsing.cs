namespace Tavenem.Time;

public partial struct RelativeDuration
{
    /// <summary>
    /// Converts the specified string representation to a <see cref="RelativeDuration"/>.
    /// </summary>
    /// <param name="s">A string containing a duration to convert.</param>
    /// <returns>The <see cref="RelativeDuration"/> value equivalent to the duration contained in
    /// <paramref name="s"/>.</returns>
    /// <param name="provider">An object that supplies culture-specific formatting
    /// information.</param>
    /// <exception cref="FormatException"><paramref name="s"/> is empty, or contains only white
    /// space, contains invalid <see cref="RelativeDuration"/> data, or the format cannot be
    /// determined.</exception>
    public static RelativeDuration Parse(ReadOnlySpan<char> s, IFormatProvider? provider = null)
    {
        if (TryParseRelativeDuration(s, out var result))
        {
            return result;
        }
        return new RelativeDuration(Duration.Parse(s, provider));
    }

    /// <summary>
    /// Converts the specified string representation to a <see cref="RelativeDuration"/>.
    /// </summary>
    /// <param name="s">A string containing a duration to convert.</param>
    /// <returns>The <see cref="RelativeDuration"/> value equivalent to the duration contained in
    /// <paramref name="s"/>.</returns>
    /// <param name="provider">An object that supplies culture-specific formatting
    /// information.</param>
    /// <exception cref="ArgumentNullException"><paramref name="s"/> is <see
    /// langword="null"/>.</exception>
    /// <exception cref="FormatException"><paramref name="s"/> is an empty string (""), or
    /// contains only white space, contains invalid <see cref="RelativeDuration"/> data, or the format
    /// cannot be determined.</exception>
    public static RelativeDuration Parse(string? s, IFormatProvider? provider = null)
    {
        if (s is null)
        {
            throw new ArgumentNullException(nameof(s));
        }
        return Parse(s.AsSpan(), provider);
    }

    /// <summary>
    /// Converts the specified string representation to a <see cref="RelativeDuration"/>.
    /// </summary>
    /// <param name="s">A string containing a duration to convert.</param>
    /// <returns>The <see cref="RelativeDuration"/> value equivalent to the duration contained in
    /// <paramref name="s"/>.</returns>
    /// <exception cref="FormatException"><paramref name="s"/> is empty, or contains only white
    /// space, contains invalid <see cref="RelativeDuration"/> data, or the format cannot be
    /// determined.</exception>
    public static RelativeDuration Parse(ReadOnlySpan<char> s) => Parse(s, null);

    /// <summary>
    /// Converts the specified string representation to a <see cref="RelativeDuration"/>.
    /// </summary>
    /// <param name="s">A string containing a duration to convert.</param>
    /// <returns>The <see cref="RelativeDuration"/> value equivalent to the duration contained in
    /// <paramref name="s"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="s"/> is <see
    /// langword="null"/>.</exception>
    /// <exception cref="FormatException"><paramref name="s"/> is an empty string (""), or
    /// contains only white space, contains invalid <see cref="RelativeDuration"/> data, or the format
    /// cannot be determined.</exception>
    public static RelativeDuration Parse(string? s) => Parse(s, null);

    /// <summary>
    /// Converts the specified string representation to a <see cref="RelativeDuration"/>. The
    /// format of the string representation must match the specified format exactly, unless it
    /// matches the relative day or year format.
    /// </summary>
    /// <param name="s">A string containing a duration to convert.</param>
    /// <param name="format">A format specifier that defines the required format of <paramref
    /// name="s"/>.</param>
    /// <param name="provider">An object that supplies culture-specific formatting
    /// information.</param>
    /// <returns>The <see cref="RelativeDuration"/> value equivalent to the duration contained
    /// in
    /// <paramref name="s"/>.</returns>
    /// <exception cref="FormatException"><paramref name="s"/> or <paramref name="format"/> is
    /// empty, or <paramref name="s"/> contains only white space, contains invalid <see
    /// cref="RelativeDuration"/> data, or the format cannot be determined.</exception>
    public static RelativeDuration ParseExact(ReadOnlySpan<char> s, ReadOnlySpan<char> format, IFormatProvider? provider = null)
    {
        if (TryParseRelativeDuration(s, out var result))
        {
            return result;
        }
        return new RelativeDuration(Duration.ParseExact(s, format, provider));
    }

    /// <summary>
    /// Converts the specified string representation to a <see cref="RelativeDuration"/>. The
    /// format of the string representation must match the specified format exactly, unless it
    /// matches the relative day or year format.
    /// </summary>
    /// <param name="s">A string containing a duration to convert.</param>
    /// <param name="format">A format specifier that defines the required format of <paramref
    /// name="s"/>.</param>
    /// <param name="provider">An object that supplies culture-specific formatting
    /// information.</param>
    /// <returns>The <see cref="RelativeDuration"/> value equivalent to the duration contained
    /// in
    /// <paramref name="s"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="s"/> or <paramref
    /// name="format"/> is <see langword="null"/>.</exception>
    /// <exception cref="FormatException"><paramref name="s"/> or <paramref name="format"/> is
    /// an empty string (""), or <paramref name="s"/> contains only white space, contains
    /// invalid <see cref="RelativeDuration"/> data, or the format cannot be
    /// determined.</exception>
    public static RelativeDuration ParseExact(string? s, string? format = null, IFormatProvider? provider = null)
    {
        if (s is null)
        {
            throw new ArgumentNullException(nameof(s));
        }
        return ParseExact(s.AsSpan(), format is null ? new ReadOnlySpan<char>() : format.AsSpan(), provider);
    }

    /// <summary>
    /// Converts the specified string representation to a <see cref="RelativeDuration"/>. The
    /// format of the string representation must match the specified format exactly, unless it
    /// matches the relative day or year format.
    /// </summary>
    /// <param name="s">A string containing a duration to convert.</param>
    /// <param name="format">A format specifier that defines the required format of <paramref
    /// name="s"/>.</param>
    /// <returns>The <see cref="RelativeDuration"/> value equivalent to the duration contained
    /// in
    /// <paramref name="s"/>.</returns>
    /// <exception cref="FormatException"><paramref name="s"/> or <paramref name="format"/> is
    /// empty, or <paramref name="s"/> contains only white space, contains invalid <see
    /// cref="RelativeDuration"/> data, or the format cannot be determined.</exception>
    public static RelativeDuration ParseExact(ReadOnlySpan<char> s, ReadOnlySpan<char> format)
        => ParseExact(s, format, null);

    /// <summary>
    /// Converts the specified string representation to a <see cref="RelativeDuration"/>. The
    /// format of the string representation must match the specified format exactly, unless it
    /// matches the relative day or year format.
    /// </summary>
    /// <param name="s">A string containing a duration to convert.</param>
    /// <param name="format">A format specifier that defines the required format of <paramref
    /// name="s"/>.</param>
    /// <returns>The <see cref="RelativeDuration"/> value equivalent to the duration contained
    /// in
    /// <paramref name="s"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="s"/> or <paramref
    /// name="format"/> is <see langword="null"/>.</exception>
    /// <exception cref="FormatException"><paramref name="s"/> or <paramref name="format"/> is
    /// an empty string (""), or <paramref name="s"/> contains only white space, contains
    /// invalid <see cref="RelativeDuration"/> data, or the format cannot be
    /// determined.</exception>
    public static RelativeDuration ParseExact(string? s, string? format = null)
        => ParseExact(s, format, null);

    /// <summary>
    /// Attempts to convert the specified string representation of a <see cref="RelativeDuration"/> and
    /// returns a value that indicates whether the conversion succeeded.
    /// </summary>
    /// <param name="s">A string containing a duration to convert.</param>
    /// <param name="provider">An object that supplies culture-specific formatting
    /// information.</param>
    /// <param name="result">When this method returns, contains the <see cref="RelativeDuration"/> value
    /// equivalent to the duration contained in <paramref name="s"/>, if the conversion
    /// succeeded, or <see cref="Zero"/> if the conversion failed. The conversion fails if the
    /// <paramref name="s"/> parameter is <see langword="null"/>, or an empty string (""), or
    /// contains only white space, or contains invalid <see cref="RelativeDuration"/> data. This
    /// parameter is passed uninitialized.</param>
    /// <returns><see langword="true"/> if the <paramref name="s"/> parameter was converted
    /// successfully; otherwise, <see langword="false"/>.</returns>
    /// <remarks>
    /// <para>
    /// The <see cref="TryParse(ReadOnlySpan{char}, IFormatProvider, out RelativeDuration)"/> method is
    /// similar to the <see cref="Parse(ReadOnlySpan{char}, IFormatProvider)"/> method, except
    /// that the <see cref="TryParse(ReadOnlySpan{char}, IFormatProvider, out RelativeDuration)"/>
    /// method does not throw an exception if the conversion fails.
    /// </para>
    /// <para>
    /// Only recognized formats can be parsed successfully. Even recognized formats may not
    /// round-trip values correctly, unless one of the formats specifically designed to do so is
    /// chosen <seealso cref="ToString(string, IFormatProvider)"/>
    /// </para>
    /// </remarks>
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out RelativeDuration result)
    {
        if (TryParseRelativeDuration(s, out result))
        {
            return true;
        }
        if (Duration.TryParse(s, provider, out var r))
        {
            result = new RelativeDuration(r);
            return true;
        }
        result = Zero;
        return false;
    }

    /// <summary>
    /// Attempts to convert the specified string representation of a <see cref="RelativeDuration"/> and
    /// returns a value that indicates whether the conversion succeeded.
    /// </summary>
    /// <param name="s">A string containing a duration to convert.</param>
    /// <param name="provider">An object that supplies culture-specific formatting
    /// information.</param>
    /// <param name="result">When this method returns, contains the <see cref="RelativeDuration"/> value
    /// equivalent to the duration contained in <paramref name="s"/>, if the conversion
    /// succeeded, or <see cref="Zero"/> if the conversion failed. The conversion fails if the
    /// <paramref name="s"/> parameter is <see langword="null"/>, or an empty string (""), or
    /// contains only white space, or contains invalid <see cref="RelativeDuration"/> data. This
    /// parameter is passed uninitialized.</param>
    /// <returns><see langword="true"/> if the <paramref name="s"/> parameter was converted
    /// successfully; otherwise, <see langword="false"/>.</returns>
    /// <remarks>
    /// <para>
    /// The <see cref="TryParse(string, IFormatProvider, out RelativeDuration)"/> method is similar to
    /// the <see cref="Parse(string, IFormatProvider)"/> method, except that the <see
    /// cref="TryParse(string, IFormatProvider, out RelativeDuration)"/> method does not throw an
    /// exception if the conversion fails.
    /// </para>
    /// <para>
    /// Only recognized formats can be parsed successfully. Even recognized formats may not
    /// round-trip values correctly, unless one of the formats specifically designed to do so is
    /// chosen <seealso cref="ToString(string, IFormatProvider)"/>
    /// </para>
    /// </remarks>
    public static bool TryParse(string? s, IFormatProvider? provider, out RelativeDuration result)
    {
        if (string.IsNullOrWhiteSpace(s))
        {
            result = Zero;
            return false;
        }
        return TryParse(s.AsSpan(), provider, out result);
    }

    /// <summary>
    /// Attempts to convert the specified string representation of a <see cref="RelativeDuration"/> and
    /// returns a value that indicates whether the conversion succeeded.
    /// </summary>
    /// <param name="s">A string containing a duration to convert.</param>
    /// <param name="result">When this method returns, contains the <see cref="RelativeDuration"/> value
    /// equivalent to the duration contained in <paramref name="s"/>, if the conversion
    /// succeeded, or <see cref="Zero"/> if the conversion failed. The conversion fails if the
    /// <paramref name="s"/> parameter is <see langword="null"/>, or an empty string (""), or
    /// contains only white space, or contains invalid <see cref="RelativeDuration"/> data. This
    /// parameter is passed uninitialized.</param>
    /// <returns><see langword="true"/> if the <paramref name="s"/> parameter was converted
    /// successfully; otherwise, <see langword="false"/>.</returns>
    /// <remarks>
    /// <para>
    /// The <see cref="TryParse(ReadOnlySpan{char}, out RelativeDuration)"/> method is similar to the
    /// <see cref="Parse(ReadOnlySpan{char})"/> method, except that the <see
    /// cref="TryParse(ReadOnlySpan{char}, out RelativeDuration)"/> method does not throw an exception
    /// if the conversion fails.
    /// </para>
    /// <para>
    /// Only recognized formats can be parsed successfully. Even recognized formats may not
    /// round-trip values correctly, unless one of the formats specifically designed to do so is
    /// chosen <seealso cref="ToString(string, IFormatProvider)"/>
    /// </para>
    /// </remarks>
    public static bool TryParse(ReadOnlySpan<char> s, out RelativeDuration result)
        => TryParse(s, null, out result);

    /// <summary>
    /// Attempts to convert the specified string representation of a <see cref="RelativeDuration"/> and
    /// returns a value that indicates whether the conversion succeeded.
    /// </summary>
    /// <param name="s">A string containing a duration to convert.</param>
    /// <param name="result">When this method returns, contains the <see cref="RelativeDuration"/> value
    /// equivalent to the duration contained in <paramref name="s"/>, if the conversion
    /// succeeded, or <see cref="Zero"/> if the conversion failed. The conversion fails if the
    /// <paramref name="s"/> parameter is <see langword="null"/>, or an empty string (""), or
    /// contains only white space, or contains invalid <see cref="RelativeDuration"/> data. This
    /// parameter is passed uninitialized.</param>
    /// <returns><see langword="true"/> if the <paramref name="s"/> parameter was converted
    /// successfully; otherwise, <see langword="false"/>.</returns>
    /// <remarks>
    /// <para>
    /// The <see cref="TryParse(string, out RelativeDuration)"/> method is similar to the <see
    /// cref="Parse(string)"/> method, except that the <see cref="TryParse(string, out
    /// RelativeDuration)"/> method does not throw an exception if the conversion fails.
    /// </para>
    /// <para>
    /// Only recognized formats can be parsed successfully. Even recognized formats may not
    /// round-trip values correctly, unless one of the formats specifically designed to do so is
    /// chosen <seealso cref="ToString(string, IFormatProvider)"/>
    /// </para>
    /// </remarks>
    public static bool TryParse(string? s, out RelativeDuration result)
        => TryParse(s, null, out result);

    /// <summary>
    /// Attempts to convert the specified string representation of a <see
    /// cref="RelativeDuration"/> and returns a value that indicates whether the conversion
    /// succeeded. The format of the string representation must match the specified format
    /// exactly, unless it matches the relative day or year format.
    /// </summary>
    /// <param name="s">A string containing a duration to convert.</param>
    /// <param name="format">The required format of <paramref name="s"/>.</param>
    /// <param name="provider">An object that supplies culture-specific formatting
    /// information.</param>
    /// <param name="result">When this method returns, contains the <see
    /// cref="RelativeDuration"/> value equivalent to the duration contained in <paramref
    /// name="s"/>, if the conversion succeeded, or <see cref="Zero"/> if the conversion failed.
    /// The conversion fails if the
    /// <paramref name="s"/> parameter is <see langword="null"/>, or an empty string (""), or
    /// contains only white space, or contains invalid <see cref="RelativeDuration"/> data. This
    /// parameter is passed uninitialized.</param>
    /// <returns><see langword="true"/> if the <paramref name="s"/> parameter was converted
    /// successfully; otherwise, <see langword="false"/>.</returns>
    /// <remarks>
    /// <para>
    /// The <see cref="TryParseExact(ReadOnlySpan{char}, ReadOnlySpan{char}, IFormatProvider,
    /// out RelativeDuration)"/> method is similar to the <see
    /// cref="ParseExact(ReadOnlySpan{char}, ReadOnlySpan{char}, IFormatProvider)"/> method,
    /// except that the <see cref="TryParseExact(ReadOnlySpan{char}, ReadOnlySpan{char},
    /// IFormatProvider, out RelativeDuration)"/>
    /// method does not throw an exception if the conversion fails.
    /// </para>
    /// <para>
    /// Only recognized formats can be parsed successfully. Even recognized formats may not
    /// round-trip values correctly, unless one of the formats specifically designed to do so is
    /// chosen <seealso cref="ToString(string, IFormatProvider)"/>
    /// </para>
    /// </remarks>
    public static bool TryParseExact(ReadOnlySpan<char> s, ReadOnlySpan<char> format, IFormatProvider? provider, out RelativeDuration result)
    {
        if (TryParseRelativeDuration(s, out result))
        {
            return true;
        }
        if (Duration.TryParseExact(s, format, provider, out var r))
        {
            result = new RelativeDuration(r);
            return true;
        }
        result = Zero;
        return false;
    }

    /// <summary>
    /// Attempts to convert the specified string representation of a <see
    /// cref="RelativeDuration"/> and returns a value that indicates whether the conversion
    /// succeeded. The format of the string representation must match the specified format
    /// exactly, unless it matches the relative day or year format.
    /// </summary>
    /// <param name="s">A string containing a duration to convert.</param>
    /// <param name="format">The required format of <paramref name="s"/>.</param>
    /// <param name="provider">An object that supplies culture-specific formatting
    /// information.</param>
    /// <param name="result">When this method returns, contains the <see
    /// cref="RelativeDuration"/> value equivalent to the duration contained in <paramref
    /// name="s"/>, if the conversion succeeded, or <see cref="Zero"/> if the conversion failed.
    /// The conversion fails if the
    /// <paramref name="s"/> parameter is <see langword="null"/>, or an empty string (""), or
    /// contains only white space, or contains invalid <see cref="RelativeDuration"/> data. This
    /// parameter is passed uninitialized.</param>
    /// <returns><see langword="true"/> if the <paramref name="s"/> parameter was converted
    /// successfully; otherwise, <see langword="false"/>.</returns>
    /// <remarks>
    /// <para>
    /// The <see cref="TryParseExact(string, string, IFormatProvider, out RelativeDuration)"/>
    /// method is similar to the <see cref="ParseExact(string, string, IFormatProvider)"/>
    /// method, except that the <see cref="TryParseExact(string, string, IFormatProvider, out
    /// RelativeDuration)"/>
    /// method does not throw an exception if the conversion fails.
    /// </para>
    /// <para>
    /// Only recognized formats can be parsed successfully. Even recognized formats may not
    /// round-trip values correctly, unless one of the formats specifically designed to do so is
    /// chosen <seealso cref="ToString(string, IFormatProvider)"/>
    /// </para>
    /// </remarks>
    public static bool TryParseExact(string? s, string? format, IFormatProvider? provider, out RelativeDuration result)
    {
        if (string.IsNullOrWhiteSpace(s))
        {
            result = Zero;
            return false;
        }

        return TryParseExact(s.AsSpan(), format is null ? new ReadOnlySpan<char>() : format.AsSpan(), provider, out result);
    }

    /// <summary>
    /// Attempts to convert the specified string representation of a <see
    /// cref="RelativeDuration"/> and returns a value that indicates whether the conversion
    /// succeeded. The format of the string representation must match the specified format
    /// exactly, unless it matches the relative day or year format.
    /// </summary>
    /// <param name="s">A string containing a duration to convert.</param>
    /// <param name="format">The required format of <paramref name="s"/>.</param>
    /// <param name="result">When this method returns, contains the <see
    /// cref="RelativeDuration"/> value equivalent to the duration contained in <paramref
    /// name="s"/>, if the conversion succeeded, or <see cref="Zero"/> if the conversion failed.
    /// The conversion fails if the
    /// <paramref name="s"/> parameter is <see langword="null"/>, or an empty string (""), or
    /// contains only white space, or contains invalid <see cref="RelativeDuration"/> data. This
    /// parameter is passed uninitialized.</param>
    /// <returns><see langword="true"/> if the <paramref name="s"/> parameter was converted
    /// successfully; otherwise, <see langword="false"/>.</returns>
    /// <remarks>
    /// <para>
    /// The <see cref="TryParseExact(ReadOnlySpan{char}, ReadOnlySpan{char}, out
    /// RelativeDuration)"/>
    /// method is similar to the <see cref="ParseExact(ReadOnlySpan{char},
    /// ReadOnlySpan{char})"/> method, except that the <see
    /// cref="TryParseExact(ReadOnlySpan{char}, ReadOnlySpan{char}, out RelativeDuration)"/>
    /// method does not throw an exception if the conversion fails.
    /// </para>
    /// <para>
    /// Only recognized formats can be parsed successfully. Even recognized formats may not
    /// round-trip values correctly, unless one of the formats specifically designed to do so is
    /// chosen <seealso cref="ToString(string, IFormatProvider)"/>
    /// </para>
    /// </remarks>
    public static bool TryParseExact(ReadOnlySpan<char> s, ReadOnlySpan<char> format, out RelativeDuration result)
        => TryParseExact(s, format, null, out result);

    /// <summary>
    /// Attempts to convert the specified string representation of a <see
    /// cref="RelativeDuration"/> and returns a value that indicates whether the conversion
    /// succeeded. The format of the string representation must match the specified format
    /// exactly, unless it matches the relative day or year format.
    /// </summary>
    /// <param name="s">A string containing a duration to convert.</param>
    /// <param name="format">The required format of <paramref name="s"/>.</param>
    /// <param name="result">When this method returns, contains the <see
    /// cref="RelativeDuration"/> value equivalent to the duration contained in <paramref
    /// name="s"/>, if the conversion succeeded, or <see cref="Zero"/> if the conversion failed.
    /// The conversion fails if the
    /// <paramref name="s"/> parameter is <see langword="null"/>, or an empty string (""), or
    /// contains only white space, or contains invalid <see cref="RelativeDuration"/> data. This
    /// parameter is passed uninitialized.</param>
    /// <returns><see langword="true"/> if the <paramref name="s"/> parameter was converted
    /// successfully; otherwise, <see langword="false"/>.</returns>
    /// <remarks>
    /// <para>
    /// The <see cref="TryParseExact(string, string, out RelativeDuration)"/> method is similar
    /// to the <see cref="ParseExact(string, string)"/> method, except that the <see
    /// cref="TryParseExact(string, string, out RelativeDuration)"/> method does not throw an
    /// exception if the conversion fails.
    /// </para>
    /// <para>
    /// Only recognized formats can be parsed successfully. Even recognized formats may not
    /// round-trip values correctly, unless one of the formats specifically designed to do so is
    /// chosen <seealso cref="ToString(string, IFormatProvider)"/>
    /// </para>
    /// </remarks>
    public static bool TryParseExact(string? s, string? format, out RelativeDuration result)
        => TryParseExact(s, format, null, out result);

    private static bool TryParseRelativeDuration(ReadOnlySpan<char> s, out RelativeDuration result)
    {
        result = Zero;
        double proportion;
        if (s.StartsWith(DayString.AsSpan()))
        {
            if (s.Length > 2 && double.TryParse(new string(s[2..].ToArray()), out proportion))
            {
                result = FromProportionOfDay(proportion);
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (s.StartsWith(YearString.AsSpan()))
        {
            if (s.Length > 2 && double.TryParse(new string(s[2..].ToArray()), out proportion))
            {
                result = FromProportionOfYear(proportion);
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
}
