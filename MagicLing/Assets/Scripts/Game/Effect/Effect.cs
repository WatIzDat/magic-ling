using System;

public class Effect
{
    public int Stacks { get; private set; }
    public int MaxStacks { get; private set; }
    public EffectType EffectType { get; private set; }
    public Action<Battler> DealDamage { get; private set; }

    //private Effect(int stacks, EffectType effectType, Action<Battler> dealDamage)
    //{
    //    Stacks = stacks;
    //    MaxStacks = stacks;
    //    EffectType = effectType;
    //    DealDamage = dealDamage;
    //}

    public static Effect CreateBurnEffect(int stacks, Battler otherBattler)
    {
        Effect effect = new()
        {
            Stacks = stacks,
            MaxStacks = stacks,
            EffectType = EffectType.Burn
        };

        effect.DealDamage = battler => battler.TakeDamage(otherBattler, new Damage(DamageType.Fire, effect.Stacks));

        return effect;
    }
    
    public static Effect CreateRuptureEffect(int stacks, Battler otherBattler)
    {
        Effect effect = new()
        {
            Stacks = stacks,
            MaxStacks = stacks,
            EffectType = EffectType.Rupture
        };

        effect.DealDamage = battler =>
        {
            battler.TakeDamage(otherBattler, new Damage(DamageType.Grass, effect.Stacks));
            effect.Stacks--;
        };

        return effect;
    }

    public static Effect CreateSinkingEffect(int stacks, Battler otherBattler)
    {
        Effect effect = new()
        {
            Stacks = stacks,
            MaxStacks = stacks,
            EffectType = EffectType.Sinking
        };

        effect.DealDamage = battler =>
        {
            battler.TakeDamage(otherBattler, new Damage(DamageType.Water, effect.MaxStacks - effect.Stacks + 1));
            effect.Stacks--;
        };

        return effect;
    }
}
