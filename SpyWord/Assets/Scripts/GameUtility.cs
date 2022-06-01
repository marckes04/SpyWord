using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUtility : MonoBehaviour
{
   public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ExitAplication()
    {
        Application.Quit();
    }

    public void HideBanerAds()
    {
        AdsManager.Instance.HideBanner();
    }

    public void MuteToggleBackgroundMusic()
    {
        SoundManager.instance.ToggleBackgroundMusic();
    }

    public void MuteToggleSoundFX()
    {
        SoundManager.instance.ToggleSoundFx();
    }
}
