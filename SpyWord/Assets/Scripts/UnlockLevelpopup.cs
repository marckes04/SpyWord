using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnlockLevelpopup : MonoBehaviour
{
    [System.Serializable]

    public struct CategoryName
    {
        public string  name;
        public Sprite sprite;
    };

    public GameData currenGameData;
    public List<CategoryName> categoryNames;
    public GameObject winPopup;
    public Image categoryNameImage;

    void Start()
    {
        winPopup.SetActive(false);

        GameEvents.OnUnlockNextCategory += OnLuckNextCategory;
      
    }

    private void OnDisable()
    {
        GameEvents.OnUnlockNextCategory -= OnLuckNextCategory;
    }

    private void OnLuckNextCategory()
    {
        bool captureNext = false;

        foreach(var writing in categoryNames)
        {
            if (captureNext)
            {
                categoryNameImage.sprite = writing.sprite;
                captureNext = false;
                break;
            }
            if(writing.name == currenGameData.selectedCategoryName)
            {
                captureNext = true;
            }
        }

        winPopup.SetActive(true);
    }
}
