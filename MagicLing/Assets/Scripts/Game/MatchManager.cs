using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MatchManager : MonoBehaviour
{
    public Match match;
    public SyllableStructure syllableStructure = SyllableStructure.Parse("CV(C)");

    private Player player = new(new List<Word>() { new("pater") }, 100f, 1f);

    private List<Spell> spells;

    [SerializeField]
    private List<TMP_Text> wordText;

    [SerializeField]
    private RectTransform healthBarPrefab;

    [SerializeField]
    private Canvas canvas;

    private Dictionary<Battler, Slider> opponentHealthSliders;

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
        List<Battler> battlers = new() 
        {
            new(
                new List<Word>() 
                {
                    new("enemy") 
                },
                20f,
                1f,
                new Dictionary<DamageType, float>()
                {
                    { DamageType.Fire, 0.5f } 
                },
                new List<Effect>() 
                {
                    Effect.CreateRuptureEffect(3, player) 
                }),
            new(
                new List<Word>() 
                { 
                    new("test") 
                },
                20f,
                1f,
                new Dictionary<DamageType, float>() 
                { 
                    { DamageType.Grass, 0.5f } 
                })
        };

        match = new(player, battlers);

        for (int i = 0; i < match.Words.Count; i++)
        {
            OnMatchWordUpdated(i, match.Words[i]);
        }

        List<Slider> healthSliders = new();

        for (int i = 0; i < battlers.Count; i++)
        {
            GameObject healthSlider = Instantiate(healthBarPrefab.gameObject, canvas.transform);

            Debug.Log(healthBarPrefab.rect.height);

            RectTransform rectTransform = healthSlider.GetComponent<RectTransform>();

            rectTransform.anchoredPosition = new Vector3(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y - (i * healthBarPrefab.rect.height), 0f);

            healthSliders.Add(healthSlider.GetComponent<Slider>());
        }

        opponentHealthSliders = battlers.Zip(healthSliders, (k, v) => (k, v)).ToDictionary(x => x.k, x => x.v);
    }

    private void OnEnable()
    {
        match.OnWordUpdated += OnMatchWordUpdated;

        foreach (Battler battler in match.Battlers)
        {
            battler.OnDamageTaken += OnOpponentDamageTaken;
        }
    }

    private void OnDisable()
    {
        match.OnWordUpdated -= OnMatchWordUpdated;

        foreach (Battler battler in match.Battlers)
        {
            battler.OnDamageTaken -= OnOpponentDamageTaken;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            match.EndTurn(spells);
        }
    }

    private void OnOpponentDamageTaken(Battler hitBattler, Battler attackBattler, Damage damage)
    {
        opponentHealthSliders[hitBattler].value = hitBattler.Health / hitBattler.MaxHealth;
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
