public class CyclingBehavior : IEnemyBehavior
{
    private readonly EnemyAction[] pattern;
    private int index;

    public CyclingBehavior(EnemyAction[] pattern)
    {
        this.pattern = pattern;
    }

    public EnemyAction GetCurrentAction()
    {
        return pattern[index];
    }

    public void NextAction()
    {
        index = (index + 1) % pattern.Length;
    }
}
