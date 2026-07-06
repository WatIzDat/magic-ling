using System.Collections.Generic;
using TMPro;
using Tools.UI.Card;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuParent;
    
    [SerializeField] private GameObject cardSelectMenuParent;
    [SerializeField] private GameObject cardSelectParent;
    [SerializeField] private TMP_Text protoWordText;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private float cardGap;
    
    public void OnPlayButtonClicked()
    {
        mainMenuParent.SetActive(false);
        cardSelectMenuParent.SetActive(true);
        
        List<GameCard> cardSelectOptions = RunInfo.NewRun();

        protoWordText.text = RunInfo.ProtoWords[0];

        float initialX = -(cardGap * cardSelectOptions.Count) / 2;

        for (var i = 0; i < cardSelectOptions.Count; i++)
        {
            GameCard cardType = cardSelectOptions[i];
            
            UiCardUtils.InstantiateDisplayCard(cardPrefab, cardSelectParent.transform, 0, Vector3.right * (initialX + i * cardGap),
                cardType);
        }

        // SceneManager.LoadScene("SampleScene");
    }
}
