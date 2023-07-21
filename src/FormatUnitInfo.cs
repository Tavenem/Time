namespace Tavenem.Time;

internal readonly struct FormatUnitInfo
{
    internal char FormatChar { get; }
    internal char[]? FormatChars { get; }
    internal FormatUnit Unit { get; }

    private bool MultipleFormatChars { get; }

    public FormatUnitInfo(FormatUnit unit, char formatChar)
    {
        Unit = unit;
        FormatChar = formatChar;
        FormatChars = null;
        MultipleFormatChars = false;
    }

    public FormatUnitInfo(FormatUnit unit, char[] formatChars)
    {
        Unit = unit;
        FormatChar = char.MinValue;
        FormatChars = formatChars;
        MultipleFormatChars = true;
    }

    internal bool Match(char ch) => MultipleFormatChars
        ? Array.IndexOf(FormatChars!, ch) != -1
        : FormatChar == ch;
}
