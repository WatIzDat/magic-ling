using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MatchManager : MonoBehaviour
{
    public Match match;
    public SyllableStructure syllableStructure = SyllableStructure.Parse("CV(C)");

    private Player player = new(new List<Word>() { new("ʦaʦ") }, 100f, 1f);

    private List<Spell> spells;

    [SerializeField]
    private List<TMP_Text> wordText;

    [SerializeField]
    private RectTransform opponentPrefab;

    [SerializeField]
    private Canvas canvas;

    private readonly Dictionary<Battler, OpponentInfo> opponents = new();

    [SerializeField]
    private GameObject opponentsParent;

    [SerializeField]
    private List<EffectTypeToIconMapping> effectIcons;

    private Dictionary<EffectType, GameObject> effectIconsDictionary;

    [System.Serializable]
    private class EffectTypeToIconMapping
    {
        public EffectType type;
        public GameObject icon;
    }

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
        effectIconsDictionary = new(effectIcons.Select(x => new KeyValuePair<EffectType, GameObject>(x.type, x.icon)));

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
                    Effect.CreateRuptureEffect(3, player),
                    Effect.CreateBurnEffect(2, player)
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
                }),
            new(
                new List<Word>() 
                { 
                    new("abc") 
                },
                20f,
                1f,
                new Dictionary<DamageType, float>() 
                { 
                    { DamageType.Grass, 0.5f } 
                }),
            new(
                new List<Word>() 
                { 
                    new("def") 
                },
                20f,
                1f,
                new Dictionary<DamageType, float>() 
                { 
                    { DamageType.Grass, 0.5f } 
                },
                new List<Effect>()
                {
                    Effect.CreateSinkingEffect(3, player)
                }),
        };

        match = new(player, battlers);

        for (int i = 0; i < match.Words.Count; i++)
        {
            OnMatchWordUpdated(i, match.Words[i]);
        }

        List<Slider> healthSliders = new();

        for (int i = 0; i < battlers.Count; i++)
        {
            GameObject opponentObj = Instantiate(opponentPrefab.gameObject, opponentsParent.transform);

            //Debug.Log(healthBarPrefab.rect.height);

            TMP_Text text = opponentObj.GetComponentInChildren<TMP_Text>();
            text.text = battlers[i].Words[0].Current;

            RectTransform rectTransform = opponentObj.GetComponent<RectTransform>();

            // note: only works for battlers length 4 or less
            if (i == 1)
            {
                rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                rectTransform.anchorMax = new Vector2(1f, 1f);
            }
            else if (i == 2)
            {
                rectTransform.anchorMin = new Vector2(0f, 0f);
                rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            }
            else if (i == 3)
            {
                rectTransform.anchorMin = new Vector2(0.5f, 0f);
                rectTransform.anchorMax = new Vector2(1f, 0.5f);
            }

            //rectTransform.anchoredPosition = new Vector3(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y - (i * healthBarPrefab.rect.height), 0f);

            //healthSliders.Add(opponentObj.GetComponentInChildren<Slider>());

            opponents[battlers[i]] = new OpponentInfo(opponentObj, opponentObj.GetComponentInChildren<Slider>());
            opponents[battlers[i]].EffectIcons = InstantiateEffectIcons(battlers[i].Effects, opponents[battlers[i]]);
        }

        //opponentHealthSliders = battlers.Zip(healthSliders, (k, v) => (k, v)).ToDictionary(x => x.k, x => x.v);
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
            EndTurn();
        }
    }

    private void EndTurn()
    {
        match.EndTurn(spells);

        foreach (Battler battler in match.Battlers)
        {
            opponents[battler].EffectIcons = InstantiateEffectIcons(battler.Effects, opponents[battler]);
        }
    }

    private Dictionary<Effect, GameObject> InstantiateEffectIcons(List<Effect> effects, OpponentInfo opponentInfo)
    {
        Dictionary<Effect, GameObject> effectIcons = new();

        foreach ((Effect _, GameObject obj) in opponentInfo.EffectIcons)
        {
            Destroy(obj);
        }

        int i = 0;

        foreach (Effect effect in effects)
        {
            //Debug.Log(effect);
            //if (opponentInfo.EffectIcons.ContainsKey(effect))
            //{
            //    Destroy(opponentInfo.EffectIcons[effect]);

            //    if (!effects.Contains(effect))
            //}

            GameObject newIcon = Instantiate(effectIconsDictionary[effect.EffectType], opponentInfo.Object.transform);

            RectTransform rectTransform = newIcon.GetComponent<RectTransform>();

            rectTransform.anchoredPosition = new Vector3(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y - (i * rectTransform.rect.height), 0f);

            newIcon.GetComponentInChildren<TMP_Text>().text = effect.Stacks.ToString();

            effectIcons[effect] = newIcon;

            i++;
        }

        return effectIcons;
    }

    private void OnOpponentDamageTaken(Battler hitBattler, Battler attackBattler, Damage damage)
    {
        opponents[hitBattler].HealthSlider.value = hitBattler.Health / hitBattler.MaxHealth;
    }

    private void OnMatchWordUpdated(int index, Word word)
    {
        //wordText[index].text = word.Current;

        spells = GetSpells(word.Current);

        string newText = "";

        foreach (Spell spell in spells)
        {
            Debug.Log(spell.Color);
            newText += $"<color=#{ColorUtility.ToHtmlStringRGB(spell.Color)}>{word.Current[spell.StartIndex..spell.EndIndex]}";
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
            result.Add(Spell.CreateSpellOfSyllable(wordPos, syllable, player));

            wordPos += syllable.Full.Length;
        }

        return result;
    }
}
