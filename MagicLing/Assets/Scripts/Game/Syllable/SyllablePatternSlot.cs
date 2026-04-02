using System.Text.RegularExpressions;

public class SyllablePatternSlot
{
    //public NaturalClass Type { get; }
    public Regex Regex { get; }
    public bool IsOptional { get; }

    public SyllablePatternSlot(Regex regex, bool isOptional)
    {
        Regex = regex;
        IsOptional = isOptional;
    }
}
