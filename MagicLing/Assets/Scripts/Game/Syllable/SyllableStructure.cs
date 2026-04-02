using System.Collections.Generic;
using System.Text.RegularExpressions;

public readonly struct SyllableStructure
{
    //public string Onset { get; }
    //public string Nucleus { get; }
    //public string Coda { get; }

    public string Notation { get; }
    public List<SyllablePatternSlot> Slots { get; }

    //public SyllableStructure(string onset, string nucleus, string coda)
    //{
    //    Onset = onset;
    //    Nucleus = nucleus;
    //    Coda = coda;
    //}

    public SyllableStructure(string notation, List<SyllablePatternSlot> slots)
    {
        Notation = notation;
        Slots = slots;
    }

    public static SyllableStructure Parse(string notation)
    {
        List<SyllablePatternSlot> slots = new();
        bool inOptional = false;

        foreach (char c in notation)
        {
            switch (c)
            {
                case '(':
                    inOptional = true;
                    break;
                case ')':
                    inOptional = false;
                    break;
                case 'C':
                    slots.Add(new SyllablePatternSlot(NaturalClass.Consonant.Regex, inOptional));
                    break;
                case 'V':
                    slots.Add(new SyllablePatternSlot(NaturalClass.Vowel.Regex, inOptional));
                    break;
                case 'P':
                    slots.Add(new SyllablePatternSlot(NaturalClass.Plosive.Regex, inOptional));
                    break;
                default:
                    slots.Add(new SyllablePatternSlot(new Regex(c.ToString()), inOptional));
                    break;
            }
        }

        return new SyllableStructure(notation, slots);
    }

    public List<int> AllMatches(string word, int wordPos, int slotIndex)
    {
        List<int> results = new();

        if (slotIndex >= Slots.Count)
        {
            results.Add(wordPos);
            return results;
        }

        SyllablePatternSlot slot = Slots[slotIndex];

        if (wordPos < word.Length)
        {
            string c = word[wordPos].ToString();
            bool isMatch = slot.Regex.IsMatch(c);

            if (isMatch)
            {
                results.AddRange(AllMatches(word, wordPos + 1, slotIndex + 1));

                //if (result >= 0)
                //{
                //    return result;
                //}
            }
        }

        if (slot.IsOptional)
        {
            results.AddRange(AllMatches(word, wordPos, slotIndex + 1));
        }

        return results;
    }

    //public List<string> SplitSyllables(string word)
    //{
    //    List<string> syllables = new();

    //    string currentSyllable = "";

    //    MatchCollection onsetMatches = Rule.GetRegex(Onset).Matches(word);
    //    MatchCollection nucleusMatches = Rule.GetRegex(Nucleus).Matches(word);
    //    MatchCollection codaMatches = Rule.GetRegex(Coda).Matches(word);

    //    int i = 0;

    //    while (true)
    //    {
    //        currentSyllable += onsetMatches[i];
    //        currentSyllable += nucleusMatches[i];


    //    }
    //}
}
