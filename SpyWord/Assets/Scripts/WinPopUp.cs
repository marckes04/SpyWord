using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinPopUp : MonoBehaviour
{
    public GameObject winPopUp;


    // Start is called before the first frame update
    void Start()
    {
        winPopUp.SetActive(false);
    }

    private void OnEnable()
    {
        GameEvents.OnBoardCompleted += ShowWinPopup;
        
    }

    private void OnDisable()
    {
        GameEvents.OnBoardCompleted -= ShowWinPopup;
        
    }

    private void ShowWinPopup()
    {
    
        winPopUp.SetActive(true);
    }

    public void LoadNextLevel()
    {
      
        GameEvents.LoadNextLevelMethod();
    }
}

