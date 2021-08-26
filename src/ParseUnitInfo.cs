namespace Tavenem.Time;

internal readonly struct ParseUnitInfo
{
    internal int Index { get; }
    internal int Length { get; }
    internal char? Seperator { get; }
    internal int SeperatorLength { get; }
    internal FormatUnit Unit { get; }

    public ParseUnitInfo(FormatUnit unit, int index, int length, char? seperator, int seperatorLength)
    {
        Unit = unit;
        Index = index;
        Length = length;
        Seperator = seperator;
        SeperatorLength = seperatorLength;
    }
}
