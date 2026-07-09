using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public static class RunInfo
{   
    public static List<string> ProtoWords { get; private set; }
    public static string SyllableStructureNotation { get; private set; } = "CV(C)";
    public static List<GameCard> Cards { get; private set; } = new();
    public static int MaxHandSize { get; private set; } = 6;
    public static int Floor { get; set; } = 1;

    private const int InitialCardsSize = 3;
    private const int CardSelectOptionsSize = 5;

    public static List<GameCard> InitializeNewRun()
    {
        Cards.Clear();

        List<GameCard> cardSelectOptions = new();

        ProtoWords = new() { Word.RandomWord(SyllableStructure.Parse(SyllableStructureNotation), 1) };

        foreach (char c in ProtoWords[0])
        {
            string letter = c.ToString();

            NaturalClass naturalClass = NaturalClass.Consonant;

            if (NaturalClass.Vowel.Regex.IsMatch(letter.ToString()))
            {
                naturalClass = NaturalClass.Vowel;
            }

            cardSelectOptions.Add(new RuleCard(letter, Rule.RandomLetterOfNaturalClass(naturalClass)));
        }

        int remainingCardsCount = CardSelectOptionsSize - cardSelectOptions.Count;

        for (int i = 0; i < remainingCardsCount; i++)
        {
            string letter = ProtoWords[0][Random.Range(0, ProtoWords[0].Length)].ToString();

            NaturalClass naturalClass = NaturalClass.Consonant;

            if (NaturalClass.Vowel.Regex.IsMatch(letter.ToString()))
            {
                naturalClass = NaturalClass.Vowel;
            }

            cardSelectOptions.Add(new RuleCard(letter, Rule.RandomLetterOfNaturalClass(naturalClass)));
            
            // NaturalClass naturalClass = NaturalClass.Consonant;
            //
            // if (Random.value <= 0.5f)
            // {
            //     naturalClass = NaturalClass.Vowel;
            // }
            //
            // Cards.Add(new RuleCard(Rule.RandomLetterOfNaturalClass(naturalClass), Rule.RandomLetterOfNaturalClass(naturalClass)));
        }

        return cardSelectOptions;
    }

    public static void StartRun(List<GameCard> cards)
    {
        Cards = cards;
        
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

    public static List<Opponent> GetRandomOpponents()
    {
        switch (Floor)
        {
            case 1:
                return new List<Opponent> { Opponent.CreateBasic(1, 1, 1, 1, 3f, 3f, new Regex("[e]")) };
            case 2:
                return new List<Opponent> { Opponent.CreateBasic(1, 1, 1, 1, 3f, 3f, NaturalClass.Vowel.Regex) };
            case 3:
                return new List<Opponent> { Opponent.CreateBasic(2, 2, 1, 1, 3f, 3f, NaturalClass.Vowel.Regex) };
            default:
                return null;
        }
    }
}
