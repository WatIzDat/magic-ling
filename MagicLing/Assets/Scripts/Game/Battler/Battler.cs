using System;
using System.Collections.Generic;
using UnityEngine;

public class Battler
{
    public float Health { get; protected set; }
    public float MaxHealth { get; protected set; }
    public float Attack { get; protected set; }
    public Dictionary<DamageType, float> Resistances { get; protected set; } = new()
    {
        { DamageType.Physical, 0f },
        { DamageType.Fire, 0f },
        { DamageType.Grass, 0f },
        { DamageType.Water, 0f }
    };
    public List<Word> Words { get; protected set; }
    public List<Effect> Effects { get; protected set; }

    public delegate void OnDamageTakenEventHandler(Battler hitBattler, Battler attackBattler, Damage damage);
    public delegate void OnDeathEventHandler(Battler battler);

    public event OnDamageTakenEventHandler OnDamageTaken;
    public event OnDeathEventHandler OnDeath;

    public Battler(List<Word> words, float health = 100f, float attack = 1f, Dictionary<DamageType, float> resistances = null, List<Effect> effects = null)
    {
        Health = health;
        MaxHealth = health;
        Attack = attack;
        Words = words;
        Effects = effects ?? new();

        if (resistances != null)
        {
            foreach (var resistance in resistances)
            {
                Resistances[resistance.Key] = resistance.Value;
            }
        }
    }

    public void TakeDamage(Battler battler, Damage damage)
    {
        if (damage == null)
            return;

        Health -= damage.Amount * battler.Attack * (1f - Resistances[damage.DamageType]);

        OnDamageTaken?.Invoke(this, battler, damage);

        if (Health <= 0f)
        {
            OnDeath?.Invoke(this);
        }

        Debug.Log(Health);
    }

    public void AddEffect(Effect effect)
    {
        if (effect == null)
            return;

        for (int i = 0; i < Effects.Count; i++)
        {
            if (Effects[i].EffectType == effect.EffectType)
            {
                Effects[i].AddStacks(effect.Stacks, effect.MaxStacks);

                return;
            }
        }

        Effects.Add(effect);
    }

    public void EndTurn()
    {
        for (int i = Effects.Count - 1; i >= 0; i--)
        {
            Effects[i].EndTurn(this);

            if (Effects[i].Stacks <= 0)
            {
                Debug.Log("removed");
                Effects.RemoveAt(i);
            }
        }
    }
}
