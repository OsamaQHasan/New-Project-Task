using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardGenerator : MonoBehaviour
{
    [SerializeField] GameObject cardPrefab, grid;
    [SerializeField] int x, y;
    // Start is called before the first frame update
    void Start()
    {
        SetUpBoard();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetUpBoard() { 
        
        RectTransform rect = grid.GetComponent<RectTransform>();
        rect.localPosition = new Vector3(rect.localPosition.x, (y - 1) * 50);
        rect.sizeDelta = new Vector2(x * 100, 100);
        for (int i = 0; i < x * y; i++)
        {
            Instantiate(cardPrefab, grid.transform);
        }
    }
}
