using System.Collections;
using Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tools.UI.Card
{
    public class UiCardUtils : MonoBehaviour
    {
        //--------------------------------------------------------------------------------------------------------------

        #region Fields

        private int Count { get; set; }

        [SerializeField] [Tooltip("Prefab of the Card C#")]
        private GameObject cardPrefabCs;

        [SerializeField] [Tooltip("World point where the deck is positioned")]
        private Transform deckPosition;

        [SerializeField] [Tooltip("Game view transform")]
        private Transform gameView;

        private UiCardHand CardHand { get; set; }

        #endregion

        //--------------------------------------------------------------------------------------------------------------

        #region Unitycallbacks

        private void Awake()
        {
            CardHand = transform.parent.GetComponentInChildren<UiCardHand>();
        }

        //private IEnumerator Start()
        //{
            //RuleCard[] cards = new RuleCard[]
            //{
            //    new("p", "f"),
            //    new("t", "th"),
            //    new("a", "e"),
            //    new("e", "a"),
            //    new("r", "f"),
            //    new("t", "l"),
            //};

            ////starting cards
            //for (var i = 0; i < 6; i++)
            //{
            //    yield return new WaitForSeconds(0.2f);
            //    DrawCard(cards[i]);
            //}
        //}

        #endregion

        //--------------------------------------------------------------------------------------------------------------

        #region Operations
        
        public static UiCardDisplay InstantiateDisplayCard(GameObject cardPrefab, Transform parent, int count, Vector3 position, GameCard cardType)
        {
            //TODO: Consider replace Instantiate by an Object Pool Pattern
            var cardGo = Instantiate(cardPrefab, parent);
            cardGo.name = "Card_" + count;
            var card = cardGo.GetComponent<UiCardDisplay>();
            card.transform.position = position;
            card.Card = cardType;

            return card;
        }

        public static IUiCard InstantiateCard(GameObject cardPrefab, Transform parent, int count, Vector3 position, GameCard cardType)
        {
            //TODO: Consider replace Instantiate by an Object Pool Pattern
            // var cardGo = Instantiate(cardPrefab, parent);
            // cardGo.name = "Card_" + count;
            // var card = cardGo.GetComponent<IUiCard>();
            // card.transform.position = position;
            // card.Card = cardType;

            IUiCard card = InstantiateDisplayCard(cardPrefab, parent, count, position, cardType).GetComponent<IUiCard>();
            card.Card = cardType;

            return card;
        }

        [Button]
        public void DrawCard(GameCard cardType)
        {
            IUiCard card = InstantiateCard(cardPrefabCs, gameView, Count, deckPosition.position, cardType);
            Count++;
            CardHand.AddCard(card);
        }

        [Button]
        public void PlayCard()
        {
            if (CardHand.Cards.Count > 0)
            {
                var randomCard = CardHand.Cards.RandomItem();
                CardHand.PlayCard(randomCard);
            }
        }

        //private void Update()
        //{
        //    if (Input.GetKeyDown(KeyCode.Tab)) DrawCard();
        //    if (Input.GetKeyDown(KeyCode.Space)) PlayCard();
        //    if (Input.GetKeyDown(KeyCode.Escape)) Restart();
        //}

        public void Restart()
        {
            SceneManager.LoadScene(0);
        }

        #endregion

        //--------------------------------------------------------------------------------------------------------------
    }
}