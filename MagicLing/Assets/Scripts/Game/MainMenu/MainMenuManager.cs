using System;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using TMPro;
using Tools.UI.Card;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuParent;
    
    [SerializeField] private GameObject cardSelectMenuParent;
    [SerializeField] private GameObject cardSelectParent;
    [SerializeField] private TMP_Text protoWordText;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private float cardGap;

    [SerializeField] private InputAction click;

    private readonly List<UiCardDisplay> selectedCards = new();

    private int selectedCardsSize = 3;

    private void Awake()
    {
        click.performed += _ =>
        {
            Vector2 screenPos = Mouse.current.position.ReadValue();

            if (Physics.Raycast(Camera.main.ScreenPointToRay(screenPos), out RaycastHit hit))
            {
                hit.collider.GetComponent<IClickable>()?.OnClick();
            }
        };
        
        click.Enable();
    }

    public void OnPlayButtonClicked()
    {
        mainMenuParent.SetActive(false);
        cardSelectMenuParent.SetActive(true);
        
        selectedCards.Clear();
        
        List<GameCard> cardSelectOptions = RunInfo.InitializeNewRun();

        protoWordText.text = RunInfo.ProtoWords[0];

        float initialX = -(cardGap * cardSelectOptions.Count) / 2;

        for (int i = 0; i < cardSelectOptions.Count; i++)
        {
            GameCard cardType = cardSelectOptions[i];

            UiCardDisplay displayCard = UiCardUtils.InstantiateDisplayCard(cardPrefab, cardSelectParent.transform, i, Vector3.right * (initialX + i * cardGap),
                cardType);

            CardSelectClickable clickable = displayCard.AddComponent<CardSelectClickable>();
            clickable.mainMenuManager = this;
        }
    }

    public void OnCardSelectConfirmButtonClicked()
    {
        RunInfo.StartRun(selectedCards.Select(c => c.Card).ToList());

        SceneManager.LoadScene("SampleScene");
    }

    public void SelectCard(UiCardDisplay card)
    {
        if (selectedCards.Contains(card))
        {
            selectedCards.Remove(card);
            
            card.UnhighlightBorder();
            
            return;
        }
        
        if (selectedCards.Count == selectedCardsSize)
            return;
        
        selectedCards.Add(card);
        
        card.HighlightBorder();
        
        Debug.Log($"Selected card: {card.gameObject.name}");
    }
}
