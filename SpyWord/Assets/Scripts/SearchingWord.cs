using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SearchingWord : MonoBehaviour
{

    public Text displayedText;
    public Image crossLine;

    private string _word;

    void Start()
    {

    }

    private void OnEnable()
    {
        GameEvents.onCorrectWord += CorrectWord;
    }

    private void OnDisable()
    {
        GameEvents.onCorrectWord -= CorrectWord;
    }

    public void Setword(string word)
    {
        _word = word;
        displayedText.text = word;
    }
    private void CorrectWord(string word, List<int> squareIndexes)
    {
        if (word == _word)
        {
            crossLine.gameObject.SetActive(true);
        }
    }
}