using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public static class Rule
{
    public static readonly string[] LetterInventory = new string[]
    {
        "a", "e", "i", "o", "u",
        "b", "d", "g", "k", "p", "t",
        "f", "h", "s", "v", "z",
        "l", "ɹ",
        "w", "y",
        "m", "n",
        "ʦ", "ʣ",
        "r",
        "ɾ",
        "i", "u"
    };

    //public static readonly Dictionary<NaturalClass, Regex> NaturalClassToRegex = new()
    //{
    //    { NaturalClass.Consonant, new("[^aeiou]") },
    //    { NaturalClass.Vowel, new("[aeiou]") },
    //    { NaturalClass.Plosive, new("[bdgkpt]") },
    //    { NaturalClass.Fricative, new("[fhsvz]") },
    //    { NaturalClass.Liquid, new("[lɹ]") },
    //    { NaturalClass.Semivowel, new("[wy]") },
    //    { NaturalClass.Nasal, new("[mn]") },
    //    { NaturalClass.Affricate, new("[ʦʣ]") },
    //    { NaturalClass.Trill, new("[r]") },
    //    { NaturalClass.Tap, new("[ɾ]") },
    //    { NaturalClass.Close, new("[iu]") }
    //};

    public static readonly NaturalClass[] NaturalClasses = new NaturalClass[]
    {
        NaturalClass.Consonant,
        NaturalClass.Vowel,
        NaturalClass.Plosive,
        NaturalClass.Fricative,
        NaturalClass.Liquid,
        NaturalClass.Semivowel,
        NaturalClass.Nasal,
        NaturalClass.Affricate,
        NaturalClass.Trill,
        NaturalClass.Tap,
    };

    public static List<string> GetLettersOfRegex(Regex regex)
    {
        List<string> letters = new();

        foreach (string letter in LetterInventory)
        {
            if (regex.IsMatch(letter))
            {
                letters.Add(letter);
            }
        }

        return letters;
    }

    public static string RandomLetterOfNaturalClass(NaturalClass naturalClass)
    {
        List<string> letters = GetLettersOfRegex(naturalClass.Regex);

        return letters[Random.Range(0, letters.Count)];
    }

    public static string RandomLetterOfRegex(Regex regex)
    {
        List<string> letters = GetLettersOfRegex(regex);

        return letters[Random.Range(0, letters.Count)];
    }

    public static NaturalClass[] RandomNaturalClasses(int count, bool distinct = true, bool includeBroad = false)
    {
        List<NaturalClass> naturalClasses = NaturalClasses.ToList();

        if (!includeBroad)
        {
            naturalClasses = NaturalClasses.Where(c => !c.IsBroad).ToList();
        }

        NaturalClass[] result = new NaturalClass[count];

        for (int i = 0; i < count; i++)
        {
            int classIndex = Random.Range(0, naturalClasses.Count);

            result[i] = naturalClasses[classIndex];

            if (distinct)
            {
                naturalClasses.RemoveAt(classIndex);
            }
        }

        return result;
    }

    public static string ApplyRule(string pattern, string replacement, string word)
    {
        return word.Replace(pattern, replacement);
    }

    //public static Regex GetRegex(string rule)
    //{
    //    string regexStr = rule;

    //    foreach (var naturalClassRegexPair in NaturalClassToRegex)
    //    {
    //        regexStr = regexStr.Replace(naturalClassRegexPair.Key, naturalClassRegexPair.Value);
    //    }

    //    return new Regex(regexStr);
    //}
}
