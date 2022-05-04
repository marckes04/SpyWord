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


    void Start()
    {
        var button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
        button.interactable = true;
    }

    
    void Update()
    {
        
    }

    private void OnButtonClick()
    {
        gameData.selectedCategoryName = gameObject.name;
        SceneManager.LoadScene(gameSceneName);
    }



}
