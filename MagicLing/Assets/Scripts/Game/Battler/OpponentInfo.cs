using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpponentInfo
{
    public GameObject Object { get; set; }
    public Slider HealthSlider { get; set; }
    public Dictionary<Effect, GameObject> EffectIcons { get; set; }

    public OpponentInfo(GameObject gameObject, Slider healthSlider, Dictionary<Effect, GameObject> effectIcons = null)
    {
        Object = gameObject;
        HealthSlider = healthSlider;
        EffectIcons = effectIcons ?? new();
    }
}
