using System.Collections.Generic;

public record EnemyAction
{
    public List<Spell> Spells { get; }

    public EnemyAction(List<Spell> spells)
    {
        Spells = spells;
    }
}
