using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardGenerator : MonoBehaviour
{
    [SerializeField] GameObject cardPrefab, grid;
    [SerializeField] int x, y;
    [SerializeField] float scaleDifference;
    [SerializeField] int normalY;
    public float scaleRatio;
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
        scaleRatio = ((float)normalY /(float)y) * scaleDifference;
        RectTransform rect = grid.GetComponent<RectTransform>();
        rect.localPosition = new Vector3(rect.localPosition.x, (y - 1) * 50 * scaleRatio);
        rect.sizeDelta = new Vector2(x * 100, 100);
        rect.localScale = new Vector2(rect.localScale.x * scaleRatio, rect.localScale.y * scaleRatio);
        for (int i = 0; i < x * y; i++)
        {
            Instantiate(cardPrefab, grid.transform);
        }
    }
}
