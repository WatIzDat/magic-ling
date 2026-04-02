using System.Text.RegularExpressions;

public readonly struct NaturalClass
{
    public static NaturalClass Consonant = new("[^aeiou]", isBroad: true);
    public static NaturalClass Vowel = new("[aeiou]", true, true);
    public static NaturalClass Plosive = new("[bdgkpt]");
    public static NaturalClass Fricative = new("[fhsvz]");
    public static NaturalClass Liquid = new("[lɹ]");
    public static NaturalClass Semivowel = new("[wy]");
    public static NaturalClass Nasal = new("[mn]");
    public static NaturalClass Affricate = new("[ʦʣ]");
    public static NaturalClass Trill = new("[r]");
    public static NaturalClass Tap = new("[ɾ]");
    //Close

    public Regex Regex { get; }
    public bool IsVowelClass { get; }
    public bool IsBroad { get; }

    public NaturalClass(string regex, bool isVowelClass = false, bool isBroad = false)
    {
        Regex = new Regex(regex);
        IsVowelClass = isVowelClass;
        IsBroad = isBroad;
    }
}
