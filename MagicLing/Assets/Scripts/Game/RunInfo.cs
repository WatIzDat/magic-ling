using System.Collections.Generic;
using UnityEngine;

public static class RunInfo
{   
    public static List<string> ProtoWords { get; private set; }
    public static string SyllableStructureNotation { get; private set; } = "CV(C)";
    public static List<GameCard> Cards { get; } = new();
    public static int MaxHandSize { get; private set; } = 6;

    private const int InitialCardsSize = 10;

    public static void NewRun()
    {
        Cards.Clear();

        ProtoWords = new() { Word.RandomWord(SyllableStructure.Parse(SyllableStructureNotation), 2) };

        foreach (char c in ProtoWords[0])
        {
            string letter = c.ToString();

            NaturalClass naturalClass = NaturalClass.Consonant;

            if (Rule.NaturalClassToRegex[NaturalClass.Vowel].IsMatch(letter.ToString()))
            {
                naturalClass = NaturalClass.Vowel;
            }

            Cards.Add(new RuleCard(letter, Rule.RandomLetterOfNaturalClass(naturalClass)));
        }

        int remainingCardsCount = InitialCardsSize - Cards.Count;

        for (int i = 0; i < remainingCardsCount; i++)
        {
            NaturalClass naturalClass = NaturalClass.Consonant;

            if (Random.value <= 0.5f)
            {
                naturalClass = NaturalClass.Vowel;
            }

            Cards.Add(new RuleCard(Rule.RandomLetterOfNaturalClass(naturalClass), Rule.RandomLetterOfNaturalClass(naturalClass)));
        }
    }
}
