using UnityEngine;

public record Spell
{
    public int StartIndex { get; }
    public int EndIndex { get; }
    public Color Color { get; }
    public Damage Damage { get; }
    public Effect Effect { get; }

    public Spell(int startIndex, int endIndex, Color color, Effect effect)
    {
        StartIndex = startIndex;
        EndIndex = endIndex;
        Color = color;
        Effect = effect;
    }

    public Spell(int startIndex, int endIndex, Color color, Damage damage)
    {
        StartIndex = startIndex;
        EndIndex = endIndex;
        Color = color;
        Damage = damage;
    }

    public void CastSpell(Battler hitBattler, Battler attackBattler)
    {
        if (Effect != null)
            hitBattler.AddEffect(new Effect(Effect));

        hitBattler.TakeDamage(attackBattler, Damage);
    }

    public static Spell CreateSpellOfSyllable(int wordPos, Syllable syllable, Player player)
    {
        if (IsFireSyllable(syllable))
        {
            return CreateFireSpell(wordPos, wordPos + syllable.Full.Length);
        }
        else if (IsGrassSyllable(syllable))
        {
            return CreateGrassSpell(wordPos, wordPos + syllable.Full.Length);
        }
        else if (IsWaterSyllable(syllable))
        {
            return CreateWaterSpell(wordPos, wordPos + syllable.Full.Length);
        }
        else if (IsRuptureSyllable(syllable))
        {
            return CreateRuptureSpell(player, wordPos, wordPos + syllable.Full.Length);
        }
        else
        {
            return CreatePhysicalSpell(wordPos, wordPos + syllable.Full.Length);
        }
    }

    public static Spell CreateFireSpell(int startIndex = 0, int endIndex = 0)
    {
        return new(startIndex, endIndex, Color.red, new Damage(DamageType.Fire));
    }

    public static Spell CreateGrassSpell(int startIndex = 0, int endIndex = 0)
    {
        return new(startIndex, endIndex, Color.green, new Damage(DamageType.Grass));
    }

    public static Spell CreateWaterSpell(int startIndex = 0, int endIndex = 0)
    {
        return new(startIndex, endIndex, Color.blue, new Damage(DamageType.Water));
    }

    public static Spell CreatePhysicalSpell(int startIndex = 0, int endIndex = 0)
    {
        return new(startIndex, endIndex, Color.black, new Damage(DamageType.Physical));
    }

    public static Spell CreateRuptureSpell(Player player, int startIndex = 0, int endIndex = 0)
    {
        return new(startIndex, endIndex, new Color(0.024f, 0.251f, 0.169f), Effect.CreateRuptureEffect(3, player));
    }

    public static bool IsFireSyllable(Syllable syllable)
    {
        return syllable.AreAllConsonantsOfNaturalClasses(NaturalClass.Plosive, NaturalClass.Nasal);
    }

    public static bool IsGrassSyllable(Syllable syllable)
    {
        return syllable.AreAllConsonantsOfNaturalClasses(NaturalClass.Fricative, NaturalClass.Nasal);
    }

    public static bool IsWaterSyllable(Syllable syllable)
    {
        return syllable.AreAllConsonantsOfNaturalClasses(NaturalClass.Liquid, NaturalClass.Semivowel, NaturalClass.Nasal);
    }

    public static bool IsRuptureSyllable(Syllable syllable)
    {
        return syllable.AreAllConsonantsOfNaturalClasses(NaturalClass.Affricate, NaturalClass.Nasal);
    }
}
