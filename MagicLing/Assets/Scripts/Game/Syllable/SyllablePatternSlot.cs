using System.Collections.Generic;
using System.Text.RegularExpressions;

public class SyllablePatternSlot
{
    //public NaturalClass Type { get; }
    public List<Regex> Patterns { get; }
    public bool IsOptional { get; }

    public SyllablePatternSlot(List<Regex> patterns, bool isOptional)
    {
        Patterns = patterns;
        IsOptional = isOptional;
    }

    public SyllablePatternSlot(Regex pattern, bool isOptional) 
        : this(new List<Regex> { pattern }, isOptional)
    {
    }
}
