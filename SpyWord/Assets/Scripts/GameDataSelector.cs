using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataSelector : MonoBehaviour
{
    public GameData currentGameData;
    public GameLevelData levelData;

    void Awake()
    {
        SelectSequentialBoardData();
    }

    private void SelectSequentialBoardData()
    {
        foreach(var data  in levelData.data)
        {
             if(data.CategoryName == currentGameData.selectedCategoryName)
             {
                var boardIndex = DataSaver.ReadCategoryCurrentIndexValues(currentGameData.selectedCategoryName);

                if(boardIndex < data.boardData.Count)
                {
                    currentGameData.selectedBoardData = data.boardData[boardIndex];
                }
                else
                {
                    var RandomIndex = Random.Range(0,data.boardData.Count);
                    currentGameData.selectedBoardData = data.boardData[RandomIndex];
                }
          
             }
        }
    }
}
