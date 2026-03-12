using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class MatchManager : MonoBehaviour
{
    public Match match;
    public SyllableStructure syllableStructure = SyllableStructure.Parse("CV(C)");

    public List<TMP_Text> wordText;

    //private void Awake()
    //{
    //    List<Syllable> syllables = Syllable.Syllabify("abracadabra", SyllableStructure.Parse("(C)V(C)"));

    //    foreach (Syllable syllable in syllables)
    //    {
    //        Debug.Log(syllable.Full);
    //    }
    //}

    private void Awake()
    {
        match = new(new List<Word>() { new("pater") });

        for (int i = 0; i < match.Words.Count; i++)
        {
            OnMatchWordUpdated(i, match.Words[i]);
        }
    }

    private void OnEnable()
    {
        match.OnWordUpdated += OnMatchWordUpdated;
    }

    private void OnDisable()
    {
        match.OnWordUpdated -= OnMatchWordUpdated;
    }

    private void OnMatchWordUpdated(int index, Word word)
    {
        //wordText[index].text = word.Current;

        List<Spell> spells = GetSpells(word.Current);

        string newText = "";

        foreach (Spell spell in spells)
        {
            newText += $"<color=#{spell.Color.ToHexString()}>{word.Current[spell.StartIndex..spell.EndIndex]}";
        }

        wordText[index].text = newText;
    }

    private List<Spell> GetSpells(string word)
    {
        List<Syllable> syllables = Syllable.Syllabify(word, syllableStructure);

        int wordPos = 0;

        List<Spell> result = new();

        foreach (Syllable syllable in syllables)
        {
            result.Add(Spell.CreateSpellOfSyllable(wordPos, syllable));

            wordPos += syllable.Full.Length;
        }

        return result;
    }
}
