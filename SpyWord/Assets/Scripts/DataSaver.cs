using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataSaver : MonoBehaviour
{
    public static int ReadCategoryCurrentIndexValues(string name)
    {
        var value = -1;
        if (PlayerPrefs.HasKey(name))
            value = PlayerPrefs.GetInt(name);

        return value;
    }

    public static void SaveCategoryData(string CategoryName, int currentIndex)
    {
        PlayerPrefs.SetInt(CategoryName, currentIndex);
        PlayerPrefs.Save();
    }

    public static void ClearGameData(GameLevelData levelData)
    {
        foreach(var data in levelData.data)
        {
            PlayerPrefs.SetInt(data.CategoryName, -1);
        }

        //Unlock first level
        PlayerPrefs.SetInt(levelData.data[0].CategoryName,0);
        PlayerPrefs.Save();
    }
}
