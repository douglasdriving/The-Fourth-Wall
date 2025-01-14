using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceColorCreator : MonoBehaviour
{
    [SerializeField] Color commonWordColor;
    [SerializeField] Color rareWordColor;

    public Color GetColorForWord(string word)
    {
        float popularityFactor = WordPopularityCounter.GetPopularityForWordNormalized(word);
        Color color = Color.Lerp(rareWordColor, commonWordColor, popularityFactor);
        return color;
    }

    public Color GetCommonWordColor()
    {
        return commonWordColor;
    }
}
