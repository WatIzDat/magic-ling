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
    public List<Damage> Blocks { get; protected set; }

    public delegate void OnDamageTakenEventHandler(Battler hitBattler);
    public delegate void OnDeathEventHandler(Battler battler);
    public delegate void OnBlockChangedHandler(DamageType damageType, float amount);

    public event OnDamageTakenEventHandler OnDamageTaken;
    public event OnDeathEventHandler OnDeath;
    public event OnBlockChangedHandler OnBlockChanged;

    public Battler(List<Word> words, float health = 100f, float attack = 1f, Dictionary<DamageType, float> resistances = null, List<Effect> effects = null, List<Damage> blocks = null)
    {
        Health = health;
        MaxHealth = health;
        Attack = attack;
        Words = words;
        Effects = effects ?? new();
        Blocks = blocks ?? new();

        if (resistances != null)
        {
            foreach (var resistance in resistances)
            {
                Resistances[resistance.Key] = resistance.Value;
            }
        }
    }

    public void TakeDamage(float attack, Damage damage)
    {
        if (damage == null)
            return;

        float calculatedDamage = damage.Amount * attack * (1f - Resistances[damage.DamageType]);

        for (int i = Blocks.Count - 1; i >= 0; i--)
        {
            Debug.Log("Block: " + Blocks[i].Amount + " " + Blocks[i].DamageType);

            if (Blocks[i].DamageType == damage.DamageType)
            {
                float blockedDamage = calculatedDamage - Blocks[i].Amount;

                if (blockedDamage <= 0f)
                {
                    blockedDamage = 0f;

                    Blocks[i] = new Damage(Blocks[i].DamageType, Blocks[i].Amount - calculatedDamage);

                    OnBlockChanged?.Invoke(Blocks[i].DamageType, Blocks[i].Amount);
                }
                else
                {
                    OnBlockChanged?.Invoke(Blocks[i].DamageType, 0f);

                    Blocks.RemoveAt(i);
                }

                calculatedDamage = blockedDamage;

                break;
            }
        }

        Health -= calculatedDamage;

        OnDamageTaken?.Invoke(this);

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

    public void AddBlock(Damage block)
    {
        if (block == null)
            return;

        for (int i = 0; i < Blocks.Count; i++)
        {
            if (Blocks[i].DamageType == block.DamageType)
            {
                Blocks[i] = new Damage(block.DamageType, Blocks[i].Amount + block.Amount);

                OnBlockChanged?.Invoke(block.DamageType, Blocks[i].Amount);

                return;
            }
        }

        Blocks.Add(block);

        OnBlockChanged?.Invoke(block.DamageType, block.Amount);
    }

    public void TickEffects()
    {
        for (int i = Effects.Count - 1; i >= 0; i--)
        {
            Effects[i].EndTurn(this, 1f);

            if (Effects[i].Stacks <= 0)
            {
                Debug.Log("removed");
                Effects.RemoveAt(i);
            }
        }
    }
}
