using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField] float speed, startWait;
    [SerializeField] GameObject cardFront, cardBack, cardValue;
    public string cardValueString;
    bool cardShown = false;
    bool isFlipping = false;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        ShowHideCard(true);//card starts as flipped face up
        yield return new WaitForSeconds(startWait);
        StartFlipCard();
    }
    void Update()
    {

    }
    public void CardMatched(float wait = 0)
    {
        StartCoroutine(MatchCard(wait));
    }
    public void SelectCard()
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().SelectCard(this);
        
    }
    public void StartFlipCard(float wait = 0)
    {
        StartCoroutine(FlipCard(wait));
    }
    public void UpdateCardValue(string value)
    {
        cardValue.GetComponent<TMP_Text>().text = value;
        cardValueString = value;
    }
    void ShowHideCard(bool show = true)
    {
        cardShown = show;
        cardFront.SetActive(show);//changes the active status of the card front
        cardBack.SetActive(!cardFront.activeSelf);//if the front is active, hide the back, otherwise show it
        cardValue.SetActive(cardFront.activeSelf);//the value is shown with the card front

    }
    // Update is called once per frame

    IEnumerator MatchCard(float wait)
    {
        yield return new WaitForSeconds(wait);
        cardFront.SetActive(false);
        cardBack.SetActive(false);
        cardValue.SetActive(false);
    }
    IEnumerator FlipCard(float wait)
    {
        while(isFlipping)
        {
            yield return null;
        }
        yield return new WaitForSeconds(wait);
        isFlipping = true;
        bool displayedCard = false;
        while (transform.eulerAngles.y < 180f)//stop once card is fully flipped
        {
            
            transform.Rotate(0, speed, 0);//rotate by speed

            
            if (transform.eulerAngles.y >= 90 && !displayedCard)//the first time when the rotation becomes more than 90 degrees
            {
                displayedCard = true;
                ShowHideCard(!cardShown);//changes the card shown status
                if(cardShown)
                {
                    cardValue.transform.localRotation = Quaternion.Euler(0, 180, 0);//flip value while rotating card
                }
            }

            yield return new WaitForFixedUpdate();//wait for one FixedUpdate
        }

        transform.rotation = Quaternion.Euler(0, 0, 0);//reset rotation to 0
        cardValue.transform.localRotation = Quaternion.Euler(0, 0, 0);//flip value back
        isFlipping = false;
    }
}
