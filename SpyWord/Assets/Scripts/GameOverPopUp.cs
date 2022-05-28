using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPopUp : MonoBehaviour
{
    public GameObject gameoverpopup;
    public GameObject continueAfterAdsButton;


    void Start()
    {
        continueAfterAdsButton.GetComponent<Button>().interactable = false;
        gameoverpopup.SetActive(false);

        GameEvents.OnGameOver += ShowGameOverPopUp;
    }

    private void OnDisable()
    {
        GameEvents.OnGameOver -= ShowGameOverPopUp;
    }

    private void ShowGameOverPopUp()
    {
        AdsManager.Instance.HideBanner();
        gameoverpopup.SetActive(true);
        continueAfterAdsButton.GetComponent<Button>().interactable = false;

    }
}
