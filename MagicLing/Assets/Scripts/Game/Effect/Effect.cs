using System;
using UnityEngine;

public class Effect
{
    public int Stacks { get; private set; }
    public int MaxStacks { get; private set; }
    public EffectType EffectType { get; private set; }
    private Action<Battler> DealDamage { get; set; }

    //private Effect(int stacks, EffectType effectType, Action<Battler> dealDamage)
    //{
    //    Stacks = stacks;
    //    MaxStacks = stacks;
    //    EffectType = effectType;
    //    DealDamage = dealDamage;
    //}

    public Effect()
    {
    }

    public Effect(Effect original)
    {
        Stacks = original.Stacks;
        MaxStacks = original.MaxStacks;
        EffectType = original.EffectType;
        DealDamage = original.DealDamage;
    }

    public void EndTurn(Battler battler)
    {
        DealDamage(battler);

        if (Stacks <= 0)
        {
            Debug.LogError("This effect has 0 stacks, it should be removed");
        }

        Stacks--;
    }

    public void AddStacks(int stacks, int maxStacks)
    {
        Stacks = Mathf.Clamp(Stacks + stacks, 0, Mathf.Max(MaxStacks, maxStacks));
    }

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

        effect.DealDamage = battler => battler.TakeDamage(otherBattler, new Damage(DamageType.Grass, effect.Stacks));

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

        effect.DealDamage = battler => battler.TakeDamage(otherBattler, new Damage(DamageType.Water, effect.MaxStacks - effect.Stacks + 1));

        return effect;
    }
}
