using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WordChecker : MonoBehaviour
{
    public GameData currentGameData;
    public GameLevelData gameLevelData;

    private string _word;

    private int _assignedPoints = 0;
    private int _completedWords = 0;
    private Ray _rayUp, _rayDown;
    private Ray _rayLeft, _rayRight;
    private Ray _rayDiagonalLeftUp, _rayDiagonalLeftDown;
    private Ray _rayDiagonalRightUp, _rayDiagonalRightDown;
    private Ray _currentRay = new Ray();
    private Vector3 _rayStartPosition;
    private List<int> _correctSquareList = new List<int>();




    private void OnEnable()
    {
        GameEvents.OnCheckSquare += SquareSelected;
        GameEvents.OnClearSelection += ClearSelection;
        GameEvents.OnLoadNextLevel += LoadNextGameLevel;
    }


    private void OnDisable()
    {
        GameEvents.OnCheckSquare -= SquareSelected;
        GameEvents.OnClearSelection -= ClearSelection;
        GameEvents.OnLoadNextLevel -= LoadNextGameLevel;
    }

    private void LoadNextGameLevel()
    {
        SceneManager.LoadScene("GameScene");
    }

    // Start is called before the first frame update
    void Start()
    {
        _assignedPoints = 0;
        _completedWords = 0;
        AdsManager.Instance.ShowBanner();
    }

    // Update is called once per frame
    void Update()
    {
        if (_assignedPoints > 0 && Application.isEditor)
        {
            Debug.DrawRay(_rayUp.origin, _rayUp.direction * 4);
            Debug.DrawRay(_rayDown.origin, _rayDown.direction * 4);
            Debug.DrawRay(_rayLeft.origin, _rayLeft.direction * 4);
            Debug.DrawRay(_rayRight.origin, _rayRight.direction * 4);
            Debug.DrawRay(_rayDiagonalLeftUp.origin, _rayDiagonalLeftUp.direction * 4);
            Debug.DrawRay(_rayDiagonalLeftDown.origin, _rayDiagonalLeftDown.direction * 4);
            Debug.DrawRay(_rayDiagonalRightUp.origin, _rayDiagonalRightUp.direction * 4);
            Debug.DrawRay(_rayDiagonalRightDown.origin, _rayDiagonalRightDown.direction * 4);
        }
    }

    private void SquareSelected(string letter, Vector3 SquarePosition, int squareIndex)
    {
        if (_assignedPoints == 0)
        {
            _rayStartPosition = SquarePosition;
            _correctSquareList.Add(squareIndex);
            _word += letter;

            _rayUp = new Ray(new Vector2(SquarePosition.x, SquarePosition.y), new Vector2(0f, 1));
            _rayDown = new Ray(new Vector2(SquarePosition.x, SquarePosition.y), new Vector2(0f, -1));
            _rayLeft = new Ray(new Vector2(SquarePosition.x, SquarePosition.y), new Vector2(-1, 0f));
            _rayRight = new Ray(new Vector2(SquarePosition.x, SquarePosition.y), new Vector2(1, 0f));
            _rayDiagonalLeftUp = new Ray(new Vector2(SquarePosition.x, SquarePosition.y), new Vector2(-1, 1));
            _rayDiagonalLeftDown = new Ray(new Vector2(SquarePosition.x, SquarePosition.y), new Vector2(-1, -1));
            _rayDiagonalRightUp = new Ray(new Vector2(SquarePosition.x, SquarePosition.y), new Vector2(1, 1));
            _rayDiagonalRightDown = new Ray(new Vector2(SquarePosition.x, SquarePosition.y), new Vector2(1, -1));
        }

        else if (_assignedPoints == 1)
        {
            _correctSquareList.Add(squareIndex);
            _currentRay = SelectRay(_rayStartPosition, SquarePosition);

            GameEvents.SelectSquareMethod(SquarePosition);

            _word += letter;
            CheckWord();
        }
        else
        {
            if (IsPointOnTheRay(_currentRay, SquarePosition))
            {
                _correctSquareList.Add(squareIndex);
                GameEvents.SelectSquareMethod(SquarePosition);
                _word += letter;
                CheckWord();
            }
        }

        _assignedPoints++;

    }

    private void CheckWord()
    {
        foreach (var searchingWord in currentGameData.selectedBoardData.searchWords)
        {
            if (_word == searchingWord.Word && searchingWord.found == false)
            {
                searchingWord.found = true;
                GameEvents.CorrectWordMethod(_word, _correctSquareList);
                _completedWords++;
                _word = string.Empty;
                _correctSquareList.Clear();
                CheckBoardCompleted();
                return;
            }
        }
    }

    private bool IsPointOnTheRay(Ray currentRay, Vector3 point)
    {
        var hits = Physics.RaycastAll(currentRay, 100.0f);

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].transform.position == point)
                return true;
        }

        return false;
    }

    private Ray SelectRay(Vector2 firstPosition, Vector2 secondPosition)
    {
        var direction = (secondPosition - firstPosition).normalized;
        float tolerance = 0.01f;

        if (Mathf.Abs(direction.x) < tolerance && Mathf.Abs(direction.y - 1f) < tolerance)
        {
            return _rayUp;
        }
        if (Mathf.Abs(direction.x) < tolerance && Mathf.Abs(direction.y - (-1f)) < tolerance)
        {
            return _rayDown;
        }

        if (Mathf.Abs(direction.x - (-1f)) < tolerance && Mathf.Abs(direction.y) < tolerance)
        {
            return _rayLeft;
        }

        if (Mathf.Abs(direction.x - 1f) < tolerance && Mathf.Abs(direction.y) < tolerance)
        {
            return _rayRight;
        }

        if (direction.x < 0f && direction.y > 0f)
        {
            return _rayDiagonalLeftUp;
        }

        if (direction.x < 0f && direction.y < 0f)
        {
            return _rayDiagonalLeftDown;
        }

        if (direction.x > 0f && direction.y > 0f)
        {
            return _rayDiagonalRightUp;
        }

        if (direction.x > 0f && direction.y < 0f)
        {
            return _rayDiagonalRightDown;
        }

        return _rayDown;
    }

    private void ClearSelection()
    {
        _assignedPoints = 0;
        _correctSquareList.Clear();
        _word = string.Empty;
    }

    private void CheckBoardCompleted()
    {
        bool LoadNextCategory = false;

        if(currentGameData.selectedBoardData.searchWords.Count == _completedWords)
        {
            //Save level Progress
            var categoryName = currentGameData.selectedCategoryName;
            var currentBoardIndex = DataSaver.ReadCategoryCurrentIndexValues(categoryName);
            var nextBoardIndex = -1;
            var currentCategoryIndex = 0;
            bool readNextLevelName = false;

            for(int index = 0; index < gameLevelData.data.Count;index++)
            {
                if (readNextLevelName)
                {
                    nextBoardIndex = DataSaver.ReadCategoryCurrentIndexValues(gameLevelData.data[index].CategoryName);
                    readNextLevelName = false;
                }

                if (gameLevelData.data[index].CategoryName == categoryName)
                {
                    readNextLevelName = true;
                    currentCategoryIndex = index;
                }
            }

            var currentLevelSize = gameLevelData.data[currentCategoryIndex].boardData.Count;
            if (currentBoardIndex < currentLevelSize)
                currentBoardIndex++;

            DataSaver.SaveCategoryData(categoryName, currentBoardIndex);

            // Unlock Next Category
            if(currentBoardIndex >= currentLevelSize)
            {
                currentCategoryIndex++;
                if(currentCategoryIndex < gameLevelData.data.Count)// If this is not last category
                {
                    categoryName = gameLevelData.data[currentCategoryIndex].CategoryName;
                    currentBoardIndex = 0;
                    LoadNextCategory = true;

                    if (nextBoardIndex <= 0)
                    {
                        DataSaver.SaveCategoryData(categoryName, currentBoardIndex);
                    }
                }
                else
                {
                    SceneManager.LoadScene("SelectCategory");
                }
            }
            else
            {
                GameEvents.BoardCompletedMethod();
            }

            if (LoadNextCategory)
                GameEvents.UnlockNextCategoryMethod();

        }
    }
}
