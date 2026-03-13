using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class MatchManager : MonoBehaviour
{
    public Match match;
    public SyllableStructure syllableStructure = SyllableStructure.Parse("CV(C)");

    private Player player = new(new List<Word>() { new("pater") }, 100f, 2f);

    private List<Spell> spells;

    [SerializeField]
    private List<TMP_Text> wordText;

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
        List<Battler> battlers = new() { new(new List<Word>() { new("enemy") }, 10f, 1f, new Dictionary<DamageType, float>() { { DamageType.Fire, 0.5f } }) };

        match = new(player, battlers);

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EndTurn();
        }
    }

    private void OnMatchWordUpdated(int index, Word word)
    {
        //wordText[index].text = word.Current;

        spells = GetSpells(word.Current);

        string newText = "";

        foreach (Spell spell in spells)
        {
            newText += $"<color=#{spell.Color.ToHexString()}>{word.Current[spell.StartIndex..spell.EndIndex]}";
        }

        wordText[index].text = newText;
    }

    private void EndTurn()
    {
        match.CastSpellsOnOpponents(spells);
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
