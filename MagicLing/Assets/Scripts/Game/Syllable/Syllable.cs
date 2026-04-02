using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

public readonly struct Syllable
{
    public string Onset { get; }
    public string Nucleus { get; }
    public string Coda { get; }
    public string Full => Onset + Nucleus + Coda;

    public Syllable(string onset, string nucleus, string coda)
    {
        Onset = onset;
        Nucleus = nucleus;
        Coda = coda;
    }

    public static Syllable FromMatch(string word, int start, int length, SyllableStructure structure)
    {
        string text = word.Substring(start, length);
        int nucleusStart = -1;
        int nucleusEnd = -1;

        for (int i = 0; i < text.Length; i++)
        {
            if (NaturalClass.Vowel.Regex.IsMatch(text[i].ToString()))
            {
                if (nucleusStart < 0)
                {
                    nucleusStart = i;
                }

                nucleusEnd = i;
            }
        }

        if (nucleusStart < 0)
        {
            return new Syllable(text, "", "");
        }

        return new Syllable(text[..nucleusStart], text[nucleusStart..(nucleusEnd + 1)], text[(nucleusEnd + 1)..]);
    }

    public static List<Syllable> Syllabify(string word, SyllableStructure structure)
    {
        word = word.ToLower();
        return TrySyllabify(word, 0, structure) ?? new List<Syllable>();
    }

    public static List<Syllable> TrySyllabify(string word, int pos, SyllableStructure structure)
    {
        if (pos == word.Length)
        {
            return new List<Syllable>();
        }

        List<int> endPositions = structure
            .AllMatches(word, pos, 0)
            .Distinct()
            .Where(e => e > pos)
            .OrderBy(e => e)
            .ToList();

        foreach (int endPos in endPositions)
        {
            List<Syllable> rest = TrySyllabify(word, endPos, structure);

            if (rest != null)
            {
                Syllable syllable = FromMatch(word, pos, endPos - pos, structure);

                rest.Insert(0, syllable);

                return rest;
            }
        }

        return null;
    }

    //public bool AreAllConsonantsOfNaturalClass(NaturalClass naturalClass)
    //{
    //    Regex regex = new($"^{Rule.NaturalClassToRegex[naturalClass]}*$");

    //    return regex.IsMatch(Onset) && regex.IsMatch(Coda);
    //}

    private Regex NaturalClassesToRegex(NaturalClass[] naturalClasses)
    {
        string regexStr = "";

        for (int i = 0; i < naturalClasses.Length; i++)
        {
            NaturalClass naturalClass = naturalClasses[i];

            regexStr += naturalClass.Regex;

            if (i < naturalClasses.Length - 1)
            {
                regexStr += "|";
            }
        }

        Regex regex = new($"^(?:{regexStr})*$");

        return regex;
    }

    public bool AreAllConsonantsOfNaturalClasses(params NaturalClass[] naturalClasses)
    {
        Regex regex = NaturalClassesToRegex(naturalClasses);

        return regex.IsMatch(Onset) && regex.IsMatch(Coda);
    }

    public bool AreAllVowelsOfNaturalClasses(params NaturalClass[] naturalClasses)
    {
        Regex regex = NaturalClassesToRegex(naturalClasses);

        return regex.IsMatch(Nucleus);
    }

    public bool AreAllVowels(params string[] vowels)
    {
        Regex regex = new($"^[{string.Join("", vowels)}]*$");

        return regex.IsMatch(Nucleus);
    }

    //public static List<Syllable> Syllabify(string word, SyllableStructure structure)
    //{
    //    word = word.ToLower();
    //    List<Syllable> syllables = new();
    //    int pos = 0;

    //    while (pos < word.Length)
    //    {
    //        bool matched = false;

    //        int endPos = structure.AllMatches(word, pos, 0);

    //        if (endPos > pos)
    //        {
    //            syllables.Add(FromMatch(word, pos, endPos - pos, structure));

    //            pos = endPos;
    //            matched = true;
    //        }

    //        if (!matched)
    //        {
    //            pos++;
    //        }
    //    }

    //    return syllables;
    //}
}
