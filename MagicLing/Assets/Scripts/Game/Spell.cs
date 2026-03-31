using UnityEngine;

public record Spell
{
    public int StartIndex { get; }
    public int EndIndex { get; }
    public Color Color { get; }
    public Damage Damage { get; }
    public Effect Effect { get; }

    public int NucleusStartIndex { get; }
    public int NucleusEndIndex { get; }
    public Color NucleusColor { get; }
    public Damage Block { get; }

    public float Heal { get; }

    //public Spell(int startIndex, int endIndex, Color color, Effect effect, int nucleusStartIndex, int nucleusEndIndex, Color nucleusColor, Damage block = null)
    //    : this(startIndex, endIndex, color, null, effect, nucleusStartIndex, nucleusEndIndex, nucleusColor, block)
    //{
    //    //StartIndex = startIndex;
    //    //EndIndex = endIndex;
    //    //Color = color;
    //    //Effect = effect;
    //    //Block = block;
    //}

    //public Spell(int startIndex, int endIndex, Color color, Damage damage, int nucleusStartIndex, int nucleusEndIndex, Color nucleusColor, Damage block = null)
    //    : this(startIndex, endIndex, color, damage, null, nucleusStartIndex, nucleusEndIndex, nucleusColor, block)
    //{
    //    //StartIndex = startIndex;
    //    //EndIndex = endIndex;
    //    //Color = color;
    //    //Damage = damage;
    //    //Block = block;
    //}

    public Spell(int startIndex, int endIndex, Color color, Damage damage, Effect effect, int nucleusStartIndex, int nucleusEndIndex, Color nucleusColor, Damage block, float heal)
    {
        StartIndex = startIndex;
        EndIndex = endIndex;
        Color = color;
        Damage = damage;
        Effect = effect;
        NucleusStartIndex = nucleusStartIndex;
        NucleusEndIndex = nucleusEndIndex;
        NucleusColor = nucleusColor;
        Block = block;
        Heal = heal;
    }

    public void CastSpell(Battler hitBattler, Battler attackBattler)
    {
        if (Effect != null)
            hitBattler.AddEffect(new Effect(Effect));

        hitBattler.TakeDamage(attackBattler.Attack, Damage);
    }

    public static Spell CreateSpellOfSyllable(int wordPos, Syllable syllable)
    {
        int startIndex = wordPos;
        int endIndex = wordPos + syllable.Full.Length;
        int nucleusStartIndex = wordPos + syllable.Onset.Length;
        int nucleusEndIndex = nucleusStartIndex + syllable.Nucleus.Length;

        Color color = Color.black;
        Damage damage = new(DamageType.Physical);
        Effect effect = null;

        if (IsFireSyllable(syllable))
        {
            color = Color.red;
            damage = new(DamageType.Fire);
        }
        else if (IsGrassSyllable(syllable))
        {
            color = Color.green;
            damage = new(DamageType.Grass);
        }
        else if (IsWaterSyllable(syllable))
        {
            color = Color.blue;
            damage = new(DamageType.Water);
        }
        else if (IsBurnSyllable(syllable))
        {
            color = new Color(1f, 0.647f, 0f);
            damage = null;
            effect = Effect.CreateBurnEffect(3);
        }
        else if (IsRuptureSyllable(syllable))
        {
            color = new Color(0.024f, 0.251f, 0.169f);
            damage = null;
            effect = Effect.CreateRuptureEffect(3);
        }
        else if (IsSinkingSyllable(syllable))
        {
            color = new Color(0f, 1f, 1f);
            damage = null;
            effect = Effect.CreateSinkingEffect(3);
        }

        Damage block = null;
        Color nucleusColor = color;
        float heal = 0f;

        if (syllable.AreAllVowels("i"))
        {
            block = new Damage(DamageType.Fire);
            nucleusColor = Color.red;
        }
        else if (syllable.AreAllVowels("o"))
        {
            block = new Damage(DamageType.Grass);
            nucleusColor = Color.green;
        }
        else if (syllable.AreAllVowels("u"))
        {
            block = new Damage(DamageType.Water);
            nucleusColor = Color.blue;
        }
        else if (syllable.AreAllVowels("e"))
        {
            block = new Damage(DamageType.Physical);
            nucleusColor = Color.black;
        }
        else if (syllable.AreAllVowels("a"))
        {
            heal = 1f;
            nucleusColor = Color.yellow;
        }

        return new Spell(startIndex, endIndex, color, damage, effect, nucleusStartIndex, nucleusEndIndex, nucleusColor, block, heal);
    }

    //public static Spell CreateFireSpell(int startIndex = 0, int endIndex = 0, int nucleusStartIndex = 0, int nucleusEndIndex = 0, Damage block = null)
    //{
    //    return new(startIndex, endIndex, Color.red, new Damage(DamageType.Fire), nucleusStartIndex, nucleusEndIndex,  block);
    //}

    //public static Spell CreateGrassSpell(int startIndex = 0, int endIndex = 0, Damage block = null)
    //{
    //    return new(startIndex, endIndex, Color.green, new Damage(DamageType.Grass), block);
    //}

    //public static Spell CreateWaterSpell(int startIndex = 0, int endIndex = 0, Damage block = null)
    //{
    //    return new(startIndex, endIndex, Color.blue, new Damage(DamageType.Water), block);
    //}

    //public static Spell CreatePhysicalSpell(int startIndex = 0, int endIndex = 0, Damage block = null)
    //{
    //    return new(startIndex, endIndex, Color.black, new Damage(DamageType.Physical), block);
    //}

    //public static Spell CreateBurnSpell(int startIndex = 0, int endIndex = 0, Damage block = null)
    //{
    //    return new(startIndex, endIndex, new Color(1f, 0.647f, 0f), Effect.CreateBurnEffect(3), block);
    //}

    //public static Spell CreateRuptureSpell(int startIndex = 0, int endIndex = 0, Damage block = null)
    //{
    //    return new(startIndex, endIndex, new Color(0.024f, 0.251f, 0.169f), Effect.CreateRuptureEffect(3), block);
    //}

    //public static Spell CreateSinkingSpell(int startIndex = 0, int endIndex = 0, Damage block = null)
    //{
    //    return new(startIndex, endIndex, new Color(0f, 1f, 1f), Effect.CreateSinkingEffect(3), block);
    //}

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

    public static bool IsBurnSyllable(Syllable syllable)
    {
        return syllable.AreAllConsonantsOfNaturalClasses(NaturalClass.Trill, NaturalClass.Nasal);
    }

    public static bool IsRuptureSyllable(Syllable syllable)
    {
        return syllable.AreAllConsonantsOfNaturalClasses(NaturalClass.Affricate, NaturalClass.Nasal);
    }

    public static bool IsSinkingSyllable(Syllable syllable)
    {
        return syllable.AreAllConsonantsOfNaturalClasses(NaturalClass.Tap, NaturalClass.Nasal);
    }

    //public static bool IsBlockSyllable(Syllable syllable)
    //{
    //    return syllable.AreAllVowelsOfNaturalClasses(NaturalClass.Close);
    //}
}
