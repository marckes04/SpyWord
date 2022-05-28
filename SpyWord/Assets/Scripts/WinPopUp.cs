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
        AdsManager.OnIntersitialAdsClosed += IntersitialAdCompleted;
        
    }

    private void OnDisable()
    {
        GameEvents.OnBoardCompleted -= ShowWinPopup;
        AdsManager.OnIntersitialAdsClosed -= IntersitialAdCompleted;
    }

    private void IntersitialAdCompleted()
    {

    }

    private void ShowWinPopup()
    {
        AdsManager.Instance.HideBanner();
        winPopUp.SetActive(true);
    }

    public void LoadNextLevel()
    {
        AdsManager.Instance.ShowInterstitialAd();
        GameEvents.LoadNextLevelMethod();
    }
}

