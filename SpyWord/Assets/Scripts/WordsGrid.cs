using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordsGrid : MonoBehaviour
{

    public GameData currentGameData;
    public GameObject greedSquarePrefab;
    public AlphabetData alphabetData;

    public float squareOffset = 0.0f;
    public float topPosition;

    private List<GameObject> _squareList = new List<GameObject>();


    void Start()
    {
        SpawnGridSquares();
        setSquaresPosition();
    }

    private void setSquaresPosition()
    {
        var SquareRect = _squareList[0].GetComponent<SpriteRenderer>().sprite.rect;
        var SquareTransform = _squareList[0].GetComponent<Transform>();

        var offset = new Vector2
        {
            x = (SquareRect.width * SquareTransform.localScale.x + squareOffset) * 0.01f,
            y = (SquareRect.height * SquareTransform.localScale.y + squareOffset) * 0.01f
        };

        var startPosition = GetFirstSquarePosition();
        int ColumnNumber = 0;
        int RowNumber = 0;

        foreach (var square in _squareList)
        {
            if (RowNumber + 1 > currentGameData.selectedBoardData.Rows)
            {
                ColumnNumber++;
                RowNumber = 0;
            }
            var positionX = startPosition.x + offset.x * ColumnNumber;
            var positionY = startPosition.y - offset.y * RowNumber;

            square.GetComponent<Transform>().position = new Vector2(positionX, positionY);
            RowNumber++;
        }
    }


    private Vector2 GetFirstSquarePosition()
    {
        var StartPosition = new Vector2(0f, transform.position.y);
        var squareRect = _squareList[0].GetComponent<SpriteRenderer>().sprite.rect;
        var squareTransform = _squareList[0].GetComponent<Transform>();
        var squareSize = new Vector2(0f, 0f);


        squareSize.x = squareRect.width * squareTransform.localScale.x;
        squareSize.y = squareRect.height * squareTransform.localScale.y;

        var miwidthPosition = (((currentGameData.selectedBoardData.Columns - 1) * squareSize.x) / 2) * 0.01f;
        var midWidthHeight = (((currentGameData.selectedBoardData.Rows - 1) * squareSize.y) / 2) * 0.01f;

        StartPosition.x = (miwidthPosition != 0) ? miwidthPosition * -1 : miwidthPosition;
        StartPosition.y += midWidthHeight;


        return StartPosition;
    }




    private void SpawnGridSquares()
    {


        if (currentGameData != null)
        {
            var SquareScale = GetSquareScale(new Vector3(1.5f, 1.5f, 0.1f));
            foreach (var squares in currentGameData.selectedBoardData.Board)
            {
                foreach (var squareLetter in squares.Row)
                {
                    var normalLetterData = alphabetData.AlphabetNormal.Find(data => data.letter == squareLetter);
                    var selectedLetterData = alphabetData.AlphabetHighlighted.Find(data => data.letter == squareLetter);
                    var correctedLetterData = alphabetData.AlphabetWrong.Find(data => data.letter == squareLetter);


                    if (normalLetterData.image == null || selectedLetterData.image == null)
                    {
                        Debug.LogError(
                            "All Fields in your Array should have some letters. Press Fill up the random button in your board data to add random letter.Letter: " + squareLetter);

#if UNITY_EDITOR
                        if (UnityEditor.EditorApplication.isPlaying)
                        {
                            UnityEditor.EditorApplication.isPlaying = false;
                        }

#endif
                    }

                    else
                    {
                        _squareList.Add(Instantiate(greedSquarePrefab));
                        _squareList[_squareList.Count - 1].GetComponent<GridSquare>().SetSprite(normalLetterData, correctedLetterData, selectedLetterData);
                        _squareList[_squareList.Count - 1].transform.SetParent(this.transform);
                        _squareList[_squareList.Count - 1].GetComponent<Transform>().position = new Vector3(0f, 0f, 0f);
                        _squareList[_squareList.Count - 1].transform.localScale = SquareScale;
                        _squareList[_squareList.Count - 1].GetComponent<GridSquare>().SetIndex(_squareList.Count - 1);
                    }
                }
            }
        }
    }

    private Vector3 GetSquareScale(Vector3 defaultScale)
    {
        var finalScale = defaultScale;
        var adjusment = 0.01f;

        while (ShouldScaleDown(finalScale))
        {
            finalScale.x -= adjusment;
            finalScale.y -= adjusment;

            if (finalScale.x <= 0 || finalScale.y <= 0)
            {
                finalScale.x = adjusment;
                finalScale.y = adjusment;
                return finalScale;
            }
        }

        return finalScale;
    }

    private bool ShouldScaleDown(Vector3 targetScale)
    {
        var SquareRect = greedSquarePrefab.GetComponent<SpriteRenderer>().sprite.rect;
        var SquareSize = new Vector2(0f, 0f);
        var StartPosition = new Vector2(0f, 0f);

        SquareSize.x = (SquareRect.width * targetScale.x) + squareOffset;
        SquareSize.y = (SquareRect.height * targetScale.y) + squareOffset;

        var midWidthPosition = ((currentGameData.selectedBoardData.Columns * SquareSize.x) / 2) * 0.01f;
        var midHeightPosition = ((currentGameData.selectedBoardData.Rows * SquareSize.y) / 2) * 0.01f;

        StartPosition.x = (midWidthPosition != 0) ? midWidthPosition * -1 : midWidthPosition;
        StartPosition.y = midHeightPosition;

        return StartPosition.x < GetHalfScreenWidth() * -1 || StartPosition.y > topPosition;
    }

    private float GetHalfScreenWidth()
    {
        float height = Camera.main.orthographicSize * 2;
        float width = (1.7f * height) * Screen.width / Screen.height;
        return width / 2;
    }
}
