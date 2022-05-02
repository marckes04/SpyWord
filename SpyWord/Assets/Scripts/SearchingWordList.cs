using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchingWordList : MonoBehaviour
{

    public GameData currentGameData;
    public GameObject searchingWordPrefab;
    public float offset = 0.0f;
    public int maxColumns = 5;
    public int maxRows = 4;

    private int _columns;
    private int _rows;
    private int _wordsNumber;

    private List<GameObject> _words = new List<GameObject>();

    // Start is called before the first frame update
    private void Start()
    {
        _wordsNumber = currentGameData.selectedBoardData.searchWords.Count;

        if (_wordsNumber < _columns)
            _rows = 1;
        else
            CalculateColumnsAndRowsNumber();

        CreateWordsObjects();
        SetWordsPosition();

    }

    private void CalculateColumnsAndRowsNumber()
    {
        do
        {
            _columns++;
            _rows = _wordsNumber / _columns;
        }
        while (_rows >= maxRows);

        if (_columns > maxColumns)
        {
            _columns = maxColumns;
            _rows = _wordsNumber / _columns;
        }
    }

    private bool TryIncreaseColumnNumber()
    {
        _columns++;
        _rows = _wordsNumber / _columns;

        if (_columns > maxColumns)
        {
            _columns = maxColumns;
            _rows = _wordsNumber / _columns;

            return false;
        }

        if (_wordsNumber % _columns > 0)
            _rows++;

        return true;
    }

    private void CreateWordsObjects()
    {
        var SquareScale = GetSquareScale(new Vector3(1f, 1f, 0.1f));

        for (var index = 0; index < _wordsNumber; index++)
        {
            _words.Add(Instantiate(searchingWordPrefab) as GameObject);
            _words[index].transform.SetParent(this.transform);
            _words[index].GetComponent<RectTransform>().localScale = SquareScale;
            _words[index].GetComponent<RectTransform>().localPosition = new Vector3(0f, 0f, 0f);
            _words[index].GetComponent<SearchingWord>().Setword(currentGameData.selectedBoardData.searchWords[index].Word);
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
        var squareRect = searchingWordPrefab.GetComponent<RectTransform>();
        var parentRect = this.GetComponent<RectTransform>();

        var squareSize = new Vector2(0f, 0f);

        squareSize.x = squareRect.rect.width * targetScale.x + offset;
        squareSize.y = squareRect.rect.height * targetScale.y + offset;

        var TotalSquareHeight = squareSize.y * _rows;

        // Make sure all of the square fit in the paren rectangle area

        if (TotalSquareHeight > parentRect.rect.height)
        {
            while (TotalSquareHeight > parentRect.rect.height)
            {
                if (TryIncreaseColumnNumber())
                    TotalSquareHeight = squareSize.y * _rows;
                else
                    return true;
            }
        }

        var totalSquareWidth = squareSize.x * _columns;

        if (totalSquareWidth > parentRect.rect.width)
            return true;

        return false;
    }

    private void SetWordsPosition()
    {
        var squareRect = _words[0].GetComponent<RectTransform>();
        var wordOffset = new Vector2
        {
            x = squareRect.rect.width * squareRect.transform.localScale.x + offset,
            y = squareRect.rect.height * squareRect.transform.localScale.y + offset
        };

        int columnNumber = 0;
        int rowNumber = 0;
        var startPosition = GetFirstSquarePosition();

        foreach (var word in _words)
        {
            if (columnNumber + 1 > _columns)
            {
                columnNumber = 0;
                rowNumber++;
            }

            var positionX = startPosition.x + wordOffset.x * columnNumber;
            var positionY = startPosition.y - wordOffset.y * rowNumber;


            word.GetComponent<RectTransform>().localPosition = new Vector2(positionX, positionY);
            columnNumber++;
        }
    }

    private Vector2 GetFirstSquarePosition()
    {
        var StartPosition = new Vector2(0f, transform.position.y);
        var SquareRect = _words[0].GetComponent<RectTransform>();
        var ParentRect = this.GetComponent<RectTransform>();
        var SquareSize = new Vector2(0f, 0f);

        SquareSize.x = SquareRect.rect.width * SquareRect.transform.localScale.x + offset;
        SquareSize.y = SquareRect.rect.height * SquareRect.transform.localScale.y + offset;

        //Make sure they are in the center area

        var shiftBy = (ParentRect.rect.width - (SquareSize.x * _columns)) / 2;

        StartPosition.x = ((ParentRect.rect.width - SquareSize.x) / 2) * (-1);
        StartPosition.y += shiftBy;
        StartPosition.y = (ParentRect.rect.height - SquareSize.y) / 2;

        return StartPosition;
    }

}