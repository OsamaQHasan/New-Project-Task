using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private Queue<Card> selectedCards;
    private BoardGenerator boardGenerator;
    [SerializeField] int x, y;
    [SerializeField] float showCardFor;
    // Start is called before the first frame update
    void Start()
    {
        selectedCards = new Queue<Card>();
        boardGenerator = GetComponent<BoardGenerator>();
        boardGenerator.SetUpBoard(x, y);
    }

    // Update is called once per frame
    void Update()
    {
        if(selectedCards.Count > 1) { 
            Card selectedCard1 = selectedCards.Dequeue();
            Card selectedCard2 = selectedCards.Dequeue();

            if (selectedCard1.cardValueString != selectedCard2.cardValueString)
            {
                selectedCard1.StartFlipCard(showCardFor);
                selectedCard2.StartFlipCard(showCardFor);
            }
            else
            {
                selectedCard1.gameObject.SetActive(false);
                selectedCard2.gameObject.SetActive(false);
            }
        }
    }
    public void SelectCard(Card card)
    {
        if (!selectedCards.Contains(card))
        {
            selectedCards.Enqueue(card);
            card.StartFlipCard();
        }
            
    }
}
