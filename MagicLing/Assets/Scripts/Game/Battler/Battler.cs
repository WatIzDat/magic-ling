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

    public delegate void OnDamageTakenEventHandler(Battler hitBattler, Battler attackBattler, Damage damage);

    public event OnDamageTakenEventHandler OnDamageTaken;

    public Battler(List<Word> words, float health = 100f, float attack = 1f, Dictionary<DamageType, float> resistances = null)
    {
        Health = health;
        MaxHealth = health;
        Attack = attack;
        Words = words;

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
        Health -= damage.Amount * battler.Attack * (1f - Resistances[damage.DamageType]);

        OnDamageTaken?.Invoke(this, battler, damage);

        Debug.Log(Health);
    }
}
