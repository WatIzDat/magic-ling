public class RuleCard : ICard
{
    public string Pattern { get; }
    public string Replacement { get; }
    public string Title => $"{Pattern} → {Replacement}";

    public RuleCard(string pattern, string replacement)
    {
        Pattern = pattern;
        Replacement = replacement;
    }
}
