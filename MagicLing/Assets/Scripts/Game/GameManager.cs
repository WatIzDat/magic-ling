using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Match match = new(new List<Word>() { new("pater") });
    public List<TMP_Text> wordText;

    private void Awake()
    {
        List<Syllable> syllables = Syllable.Syllabify("abracadabra", SyllableStructure.Parse("(C)V(C)"));

        foreach (Syllable syllable in syllables)
        {
            Debug.Log(syllable.Full);
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
        wordText[index].text = word.Current;
    }
}
