namespace Tavenem.Time;

internal readonly record struct ParseUnitInfo(
    FormatUnit Unit,
    int Index,
    int Length,
    char? Separator,
    int SeparatorLength);
