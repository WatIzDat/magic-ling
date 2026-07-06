using TMPro;
using UnityEngine;

public class UiCardDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text descriptionText;
    
    private GameCard card;

    public GameCard Card
    {
        get => card;
        set 
        { 
            card = value;

            titleText.text = card.Title;
            descriptionText.text = card.Description;
        }
    }
}
