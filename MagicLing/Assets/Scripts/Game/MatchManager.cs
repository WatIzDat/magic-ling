using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MatchManager : MonoBehaviour
{
    public Match match;
    public SyllableStructure syllableStructure = SyllableStructure.Parse("CV(C)");

    private Player player = new(new List<Word>() { new("ʦaʦ") });

    private List<Spell> spells;

    [SerializeField]
    private List<TMP_Text> wordText;

    [SerializeField]
    private RectTransform opponentPrefab;

    [SerializeField]
    private Canvas canvas;

    private readonly Dictionary<Battler, BattlerUIInfo> opponents = new();

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

    //[SerializeField]
    //private Slider playerHealthSlider;

    [SerializeField]
    private GameObject playerObject;

    private BattlerUIInfo playerInfo;

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

        List<Opponent> battlers = new() 
        {
            new(
                new List<Word>() 
                {
                    new("enemy") 
                },
                new CyclingBehavior(new EnemyAction[] { new(Spell.CreateFireSpell()), new(Spell.CreateRuptureSpell()) }),
                20f,
                1f,
                new Dictionary<DamageType, float>()
                {
                    { DamageType.Fire, 0.5f } 
                },
                new List<Effect>() 
                {
                    Effect.CreateRuptureEffect(3),
                    Effect.CreateBurnEffect(2)
                }),
            //new(
            //    new List<Word>() 
            //    { 
            //        new("test") 
            //    },
            //    20f,
            //    1f,
            //    new Dictionary<DamageType, float>() 
            //    { 
            //        { DamageType.Grass, 0.5f } 
            //    }),
            //new(
            //    new List<Word>() 
            //    { 
            //        new("abc") 
            //    },
            //    20f,
            //    1f,
            //    new Dictionary<DamageType, float>() 
            //    { 
            //        { DamageType.Grass, 0.5f } 
            //    }),
            //new(
            //    new List<Word>() 
            //    { 
            //        new("def") 
            //    },
            //    20f,
            //    1f,
            //    new Dictionary<DamageType, float>() 
            //    { 
            //        { DamageType.Grass, 0.5f } 
            //    },
            //    new List<Effect>()
            //    {
            //        Effect.CreateSinkingEffect(3, player)
            //    }),
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

            opponents[battlers[i]] = new BattlerUIInfo(opponentObj, opponentObj.GetComponentInChildren<Slider>());
            opponents[battlers[i]].EffectIcons = InstantiateEffectIcons(battlers[i].Effects, opponents[battlers[i]]);
        }

        playerInfo = new BattlerUIInfo(playerObject, playerObject.GetComponentInChildren<Slider>());

        //opponentHealthSliders = battlers.Zip(healthSliders, (k, v) => (k, v)).ToDictionary(x => x.k, x => x.v);
    }

    private void OnEnable()
    {
        match.OnWordUpdated += OnMatchWordUpdated;

        foreach (Battler battler in match.Opponents)
        {
            battler.OnDamageTaken += OnOpponentDamageTaken;
            battler.OnDeath += OnOpponentDeath;
        }

        player.OnDamageTaken += OnPlayerDamageTaken;
    }

    private void OnDisable()
    {
        match.OnWordUpdated -= OnMatchWordUpdated;

        foreach (Battler battler in match.Opponents)
        {
            battler.OnDamageTaken -= OnOpponentDamageTaken;
            battler.OnDeath -= OnOpponentDeath;
        }

        player.OnDamageTaken -= OnPlayerDamageTaken;
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

        foreach (Opponent battler in match.Opponents)
        {
            opponents[battler].EffectIcons = InstantiateEffectIcons(battler.Effects, opponents[battler]);
        }

        playerInfo.EffectIcons = InstantiateEffectIcons(player.Effects, playerInfo);
    }

    private Dictionary<Effect, GameObject> InstantiateEffectIcons(List<Effect> effects, BattlerUIInfo uiInfo)
    {
        Dictionary<Effect, GameObject> effectIcons = new();

        foreach ((Effect _, GameObject obj) in uiInfo.EffectIcons)
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

            RectTransform parentTransform = uiInfo.Object.GetComponent<RectTransform>();

            GameObject newIcon = Instantiate(effectIconsDictionary[effect.EffectType], uiInfo.Object.transform);
            //newIcon.transform.position = new(parentTransform.rect.xMax, parentTransform.rect.yMin);

            RectTransform rectTransform = newIcon.GetComponent<RectTransform>();

            rectTransform.anchoredPosition = new Vector3(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y - (i * rectTransform.rect.height), 0f);

            newIcon.GetComponentInChildren<TMP_Text>().text = effect.Stacks.ToString();

            effectIcons[effect] = newIcon;

            i++;
        }

        return effectIcons;
    }

    private void OnOpponentDamageTaken(Battler hitBattler)
    {
        opponents[hitBattler].HealthSlider.value = hitBattler.Health / hitBattler.MaxHealth;
    }

    private void OnOpponentDeath(Battler battler)
    {
        Destroy(opponents[battler].Object);
    }

    private void OnPlayerDamageTaken(Battler hitBattler)
    {
        playerInfo.HealthSlider.value = hitBattler.Health / hitBattler.MaxHealth;
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
