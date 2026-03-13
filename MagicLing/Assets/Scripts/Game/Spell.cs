using UnityEngine;

public record Spell
{
    public int StartIndex { get; }
    public int EndIndex { get; }
    public Color Color { get; }
    public Damage Damage { get; }

    public Spell(int startIndex, int endIndex, Color color, Damage damage)
    {
        StartIndex = startIndex;
        EndIndex = endIndex;
        Color = color;
        Damage = damage;
    }

    public static Spell CreateSpellOfSyllable(int wordPos, Syllable syllable)
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
        else
        {
            return CreatePhysicalSpell(wordPos, wordPos + syllable.Full.Length);
        }
    }

    public static Spell CreateFireSpell(int startIndex, int endIndex)
    {
        return new(startIndex, endIndex, Color.red, new Damage(DamageType.Fire));
    }

    public static Spell CreateGrassSpell(int startIndex, int endIndex)
    {
        return new(startIndex, endIndex, Color.green, new Damage(DamageType.Grass));
    }

    public static Spell CreateWaterSpell(int startIndex, int endIndex)
    {
        return new(startIndex, endIndex, Color.blue, new Damage(DamageType.Water));
    }

    public static Spell CreatePhysicalSpell(int startIndex, int endIndex)
    {
        return new(startIndex, endIndex, Color.black, new Damage(DamageType.Physical));
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
}
