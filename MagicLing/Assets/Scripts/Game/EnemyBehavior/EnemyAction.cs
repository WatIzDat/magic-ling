public record EnemyAction
{
    public Spell Spell { get; }

    public EnemyAction(Spell spell)
    {
        Spell = spell;
    }
}
