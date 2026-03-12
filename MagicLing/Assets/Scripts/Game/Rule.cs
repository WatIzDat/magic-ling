using System.Collections.Generic;
using System.Text.RegularExpressions;

public static class Rule
{
    public static readonly Dictionary<NaturalClass, Regex> NaturalClassToRegex = new()
    {
        { NaturalClass.Consonant, new("[^aeiou]") },
        { NaturalClass.Vowel, new("[aeiou]") },
        { NaturalClass.Plosive, new("[bcdgkpqt]") },
        { NaturalClass.Fricative, new("[fhsvxz]") },
        { NaturalClass.Liquid, new("[lr]") },
        { NaturalClass.Semivowel, new("[wy]") },
        { NaturalClass.Nasal, new("[mn]") }
    };

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
