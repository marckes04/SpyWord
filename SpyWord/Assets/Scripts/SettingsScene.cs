using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsScene : MonoBehaviour
{
    public GameLevelData levelData;

    public void clearGameData()
    {
        DataSaver.ClearGameData(levelData);
    }
}
