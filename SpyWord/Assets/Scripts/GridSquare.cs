using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSquare : MonoBehaviour
{
    public int SquareIndex{ get; set;}

    private AlphabetData.LetterData _normalLetterData;
    private AlphabetData.LetterData _selectedLetterData;
    private AlphabetData.LetterData _correctLetterData;

    private SpriteRenderer _displayedImage;


    void Start()
    {
        _displayedImage = GetComponent<SpriteRenderer>();
    }

    public void SetSprite(AlphabetData.LetterData normalLetterData,
        AlphabetData.LetterData selectedLetterData, AlphabetData.LetterData correctedLetterData)
    {
        _normalLetterData = normalLetterData;
        _selectedLetterData = selectedLetterData;
        _correctLetterData = correctedLetterData;

        GetComponent<SpriteRenderer>().sprite = _normalLetterData.image;
    }
}
