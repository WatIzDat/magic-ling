using UnityEngine;

public class CardSelectClickable : MonoBehaviour, IClickable
{
    [HideInInspector] public MainMenuManager mainMenuManager;
    
    public void OnClick()
    {
        mainMenuManager.SelectCard(GetComponent<UiCardDisplay>());
    }
}
