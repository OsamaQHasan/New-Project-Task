using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private Queue<Card> selectedCards;
    private BoardGenerator boardGenerator;
    [SerializeField] int x, y;
    [SerializeField] float showCardFor;
    int combo = 0,score = 0;
    [SerializeField] TMP_Text scoreText, comboText, levelText;
    [SerializeField] int scorePerMatch;
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
                UpdateCombo(0);
            }
            else
            {
                selectedCard1.CardMatched(showCardFor);
                selectedCard2.CardMatched(showCardFor);
                UpdateScore(score + (combo == 0 ? 1 : combo) * scorePerMatch);
                UpdateCombo(combo + 1);
                
            }
        }
    }
    private void UpdateScore(int score)
    {
        this.score = score;
        scoreText.text = "SCORE: " + score;

    }
    private void UpdateCombo(int combo)
    {
        this.combo = combo;
        comboText.text = "COMBO: " + combo;
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
