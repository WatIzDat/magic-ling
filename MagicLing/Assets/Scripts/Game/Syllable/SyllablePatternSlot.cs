public class SyllablePatternSlot
{
    public NaturalClass Type { get; }
    public bool IsOptional { get; }

    public SyllablePatternSlot(NaturalClass type, bool isOptional)
    {
        Type = type;
        IsOptional = isOptional;
    }
}
