using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using Random = UnityEngine.Random;

public class Opponent : Battler
{
    public Opponent(IEnemyBehavior behavior, float health = 10f, float attack = 1f, Dictionary<DamageType, float> resistances = null, List<Effect> effects = null) : base(health, attack, resistances, effects)
    {
        Behavior = behavior;
    }

    public IEnemyBehavior Behavior { get; protected set; }

    public static Opponent CreateBasic(int minActions, int maxActions, int minSyllables, int maxSyllables,
        float minHealth, float maxHealth, Regex vowelRegex)
    {
        return Create(minActions, maxActions, minSyllables, maxSyllables, minHealth, maxHealth, numSyllables => Word.RandomSyllables(new List<SyllableStructure>()
            {
                new(null,
                    new List<SyllablePatternSlot>()
                    {
                        new(NaturalClass.Plosive.Regex, false),
                        new(vowelRegex, false),
                        new(NaturalClass.Plosive.Regex, true)
                    }),
                new(null,
                    new List<SyllablePatternSlot>()
                    {
                        new(NaturalClass.Fricative.Regex, false),
                        new(vowelRegex, false),
                        new(NaturalClass.Fricative.Regex, true)
                    }),
                new(null,
                    new List<SyllablePatternSlot>()
                    {
                        new(new List<Regex> { NaturalClass.Liquid.Regex, NaturalClass.Semivowel.Regex }, false),
                        new(vowelRegex, false),
                        new(new List<Regex> { NaturalClass.Liquid.Regex, NaturalClass.Semivowel.Regex }, true)
                    })
            },
            numSyllables));
    }

    public static Opponent CreateBerserker(int minActions, int maxActions, int minSyllables, int maxSyllables, float minHealth, float maxHealth)
    {
        return Create(minActions, maxActions, minSyllables, maxSyllables, minHealth, maxHealth, numSyllables => Word.RandomSyllables(
            () =>
            {
                NaturalClass[] naturalClasses = Rule.RandomNaturalClasses(2);

                string syllable =
                    Rule.RandomLetterOfNaturalClass(naturalClasses[0]) + 
                    Rule.RandomLetterOfNaturalClass(NaturalClass.Vowel) +
                    Rule.RandomLetterOfNaturalClass(naturalClasses[1]);

                Debug.Log("Syllable: " + syllable);

                return syllable;
            },
            SyllableStructure.Parse("CV(C)"),
            numSyllables));
    }

    private static Opponent Create(int minActions, int maxActions, int minSyllables, int maxSyllables, float minHealth,
        float maxHealth, Func<int, List<Syllable>> generateSyllables)
    {
        int numActions = Random.Range(minActions, maxActions + 1);

        EnemyAction[] enemyActions = new EnemyAction[numActions];

        for (int i = 0; i < numActions; i++)
        {
            int numSyllables = Random.Range(minSyllables, maxSyllables + 1);

            List<Syllable> syllables = generateSyllables(numSyllables);

            enemyActions[i] = new EnemyAction(syllables);
        }

        CyclingBehavior behavior = new(enemyActions);

        float health = Random.Range((int)minHealth, (int)maxHealth + 1);

        return new Opponent(behavior, health);
    }

    //public static Opponent CreateElementalist
}
