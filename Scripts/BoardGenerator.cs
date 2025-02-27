using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardGenerator : MonoBehaviour
{
    [SerializeField] GameObject cardPrefab, grid;
    
    [SerializeField] float scaleDifference;
    [SerializeField] int normalY;
    [SerializeField] string[] cardPossibilities;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetUpBoard(int x, int y) {
        foreach(Transform cardGO in grid.transform)
        {
            Destroy(cardGO.gameObject);
        }
        int cardCount = x * y; //get the number of cards
        if(cardCount > cardPossibilities.Length * 2 || cardCount < 4 || cardCount % 2 != 0)//don't allow more cards than possible card values * 2 or less than 4 or non-even card counts
        {
            Debug.LogError("Invalid card number");
            return;
        }
        float scaleRatio = ((float)normalY /(float)y) * scaleDifference;//how much to change the scale of the grid, based on the number of rows
        RectTransform rect = grid.GetComponent<RectTransform>();
        rect.localPosition = new Vector3(rect.localPosition.x, (y - 1) * 50 * scaleRatio);//place the grid in the center of the screen
        rect.sizeDelta = new Vector2(x * 100, 100);//change the grid size to match the ration
        rect.localScale = new Vector2(scaleRatio, scaleRatio);//scale up or down the grid to display all the cards
        SupportScripts.ShuffleArray(cardPossibilities);
        string[] neededArray = new string[cardCount];
        for(int i = 0; i < neededArray.Length; i++)
        {
            neededArray[i] = cardPossibilities[i % neededArray.Length/2];
        }
        SupportScripts.ShuffleArray(neededArray);
        for (int i = 0; i < cardCount; i++)//create the cards
        {
            GameObject newCard = Instantiate(cardPrefab, grid.transform);
            newCard.GetComponent<Card>().UpdateCardValue(neededArray[i]);
        }
    }
}
