using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectPuzzleButton : MonoBehaviour
{
    public GameData gameData;
    public GameLevelData levelData;
    public Text categoryText;
    public Image progressBarFilling;

    private string gameSceneName = "GameScene";

    private bool _levelLocked;



    void Start()
    {
        _levelLocked = false;
        var button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
        UpdateButtonInformation();

        if (_levelLocked)
        {
            button.interactable = false;
        }

        else
        {
            button.interactable = true;
        }
       
    }


    private void OnEnable()
    {
        AdsManager.OnIntersitialAdsClosed += IntersitialAdColsed;
    }

    private void OnDisable()
    {
        AdsManager.OnIntersitialAdsClosed -= IntersitialAdColsed;
    }

    private void IntersitialAdColsed()
    {
        
    }


    private void UpdateButtonInformation()
    {
        var currrentIndex = -1;
        var totalboards = 0;

        foreach(var data in levelData.data)
        {
            if(data.CategoryName == gameObject.name)
            {
                currrentIndex = DataSaver.ReadCategoryCurrentIndexValues(gameObject.name);
                totalboards = data.boardData.Count;

                if(levelData.data[0].CategoryName == gameObject.name && currrentIndex < 0)
                {
                    DataSaver.SaveCategoryData(levelData.data[0].CategoryName, 0);
                    currrentIndex = DataSaver.ReadCategoryCurrentIndexValues(gameObject.name);
                    totalboards = data.boardData.Count;
                }
            }
        }
        if(currrentIndex == -1)
          _levelLocked = true;

        categoryText.text = _levelLocked ? string.Empty : (currrentIndex.ToString() + "/" + totalboards.ToString());
        progressBarFilling.fillAmount = (currrentIndex > 0 && totalboards > 0) ? ((float)currrentIndex / (float)totalboards) : 0f;

       
    }

    private void OnButtonClick()
    {
        gameData.selectedCategoryName = gameObject.name;
        AdsManager.Instance.ShowInterstitialAd();
        SceneManager.LoadScene(gameSceneName);
    }
}
