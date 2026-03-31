using System.Collections.Generic;
using UnityEngine;

public record Word
{
    public string Proto { get; }
    public string Current { get; }

    public Word(string protoWord)
    {
        Proto = protoWord;
        Current = protoWord;
    }

    public Word(string protoWord, string currentWord)
    {
        Proto = protoWord;
        Current = currentWord;
    }

    public static string RandomWord(SyllableStructure syllableStructure, int numSyllables)
    {
        string word = "";

        for (int i = 0; i < numSyllables; i++)
        {
            foreach (SyllablePatternSlot slot in syllableStructure.Slots)
            {
                if (slot.IsOptional && Random.value <= 0.5f)
                    continue;

                word += Rule.RandomLetterOfNaturalClass(slot.Type);
            }
        }

        return word;
    }

    //public List<Spell> GetSpells()
    //{

    //}

    //private string[] SplitSyllables()
    //{

    //}
}
