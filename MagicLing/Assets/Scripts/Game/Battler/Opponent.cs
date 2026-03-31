using System.Collections.Generic;

public class Opponent : Battler
{
    public Opponent(IEnemyBehavior behavior, float health = 100f, float attack = 1f, Dictionary<DamageType, float> resistances = null, List<Effect> effects = null) : base(health, attack, resistances, effects)
    {
        Behavior = behavior;
    }

    public IEnemyBehavior Behavior { get; protected set; }
}
