using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordsGrid : MonoBehaviour
{
    public GameData currentGameData;
    public GameObject gridSquarePrefab;
    public AlphabetData alphabetData;

    public float squareOffset = 0.0f;
    public float topPosition;

    private List<GameObject> _squareList = new List<GameObject>();


    void Start()
    {

    }

    private void SpawnGridSquares()
    {
        if(currentGameData != null)
        {
            var squareScale = GetSquareScale(new Vector3(1.5f, 1.5f, 0.1f));
            foreach(var squares in currentGameData.selectedBoardData.Board)
            {
                foreach (var squareletter in squares.Row)
                {
                    var normalLetterData = alphabetData.AlphabetNormal.Find(data => data.letter == squareletter);
                    var selectedLetterData = alphabetData.AlphabetHighlighted.Find(data => data.letter == squareletter);
                    var correctLetterData = alphabetData.AlphabetWrong.Find(data => data.letter == squareletter);

                    if(normalLetterData.image == null || selectedLetterData.image == null)
                    {
                        Debug.LogError("All fields in your array should have some letters. Press Fill up with random button " +
                            "in your board data to add random letter. Letter: " + squareletter);
                        #if UNITY_EDITOR
                        if (UnityEditor.EditorApplication.isPlaying)
                        {
                            UnityEditor.EditorApplication.isPlaying = false;
                        }
                        #endif
                    }
                    else
                    {
                        _squareList.Add(Instantiate(gridSquarePrefab));
                    }
                }
            }
        }
    }

    private Vector3 GetSquareScale(Vector3 defaultScale)
    {
        var finalScale = defaultScale;
        var adjustment = 0.01f;

        while (shouldScaleDown(finalScale))
        {
            finalScale.x -= adjustment;
            finalScale.y -= adjustment;

            if(finalScale.x <= 0 || finalScale.y <=0)
            {
                finalScale.x = adjustment;
                finalScale.y = adjustment;
                return finalScale;
            }
        }
        return finalScale;
    }

    private bool shouldScaleDown(Vector3 targetScale)
    {
        var squareRect = gridSquarePrefab.GetComponent<SpriteRenderer>().sprite.rect;
        var squareSize = new Vector2(0f,0f);
        var startPosition = new Vector2(0f, 0f);

        squareSize.x = (squareRect.width * targetScale.x) + squareOffset;
        squareSize.y = (squareRect.height * targetScale.y) + squareOffset;

        var midWidthPosition = ((currentGameData.selectedBoardData.Columns * squareSize.x) / 2)*0.01f;
        var midWidthHeight = ((currentGameData.selectedBoardData.Rows * squareSize.y) / 2) * 0.01f;

        startPosition.x = (midWidthPosition != 0) ? midWidthPosition * -1 : midWidthPosition;
        startPosition.y = midWidthHeight;

        return startPosition.x < GethalfScreenWidht() * -1 || startPosition.y > topPosition;
    }

    private float GethalfScreenWidht()
    {
        float height = Camera.main.orthographicSize * 2;
        float widht = (1.7f * height) * Screen.width / Screen.height;

        return widht / 2;
    }
}
