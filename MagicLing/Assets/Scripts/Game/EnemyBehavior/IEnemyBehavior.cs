public interface IEnemyBehavior
{
    EnemyAction GetCurrentAction();
    void NextAction();
}
