public abstract class GameCard
{
    public abstract string Title { get; }
    public abstract string Description { get; }

    public virtual void UpdateMatch(Match match)
    {
        match.RemoveCardFromHand(this);
    }
}
