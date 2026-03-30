public class RuleCard : GameCard
{
    public string Pattern { get; }
    public string Replacement { get; }
    public override string Title => $"{Pattern} → {Replacement}";
    public override string Description => $"{Pattern} becomes {Replacement}";

    public RuleCard(string pattern, string replacement)
    {
        Pattern = pattern;
        Replacement = replacement;
    }

    public override void UpdateMatch(Match match)
    {
        for (int i = 0; i < match.Words.Count; i++)
        {
            Word newWord = new(
                match.Words[i].Proto,
                Rule.ApplyRule(Pattern, Replacement, match.Words[i].Current));

            match.UpdateWord(i, newWord);
        }

        base.UpdateMatch(match);
    }
}
