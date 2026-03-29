public class CyclingBehavior : IEnemyBehavior
{
    private readonly EnemyAction[] pattern;
    private int index;

    public CyclingBehavior(EnemyAction[] pattern)
    {
        this.pattern = pattern;
    }

    public EnemyAction GetNextAction()
    {
        EnemyAction action = pattern[index];

        index = (index + 1) % pattern.Length;

        return action;
    }
}
