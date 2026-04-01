using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattlerUIInfo
{
    public GameObject Object { get; set; }
    public Slider HealthSlider { get; set; }
    public TMP_Text Text { get; set; }
    public Dictionary<Effect, GameObject> EffectIcons { get; set; }
    public List<GameObject> ActionIcons { get; set; }
    public Dictionary<DamageType, GameObject> BlockIcons { get; set; }

    public BattlerUIInfo(GameObject gameObject, Slider healthSlider, TMP_Text text = null, Dictionary<Effect, GameObject> effectIcons = null, List<GameObject> actionIcons = null, Dictionary<DamageType, GameObject> blockIcons = null)
    {
        Object = gameObject;
        HealthSlider = healthSlider;
        Text = text;
        EffectIcons = effectIcons ?? new();
        ActionIcons = actionIcons ?? new();
        BlockIcons = blockIcons ?? new();
    }
}
