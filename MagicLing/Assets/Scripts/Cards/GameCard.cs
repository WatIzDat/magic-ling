public abstract class GameCard
{
    public abstract string Title { get; }
    public virtual void UpdateMatch(Match match)
    {
        match.RemoveCardFromHand(this);
    }
}
