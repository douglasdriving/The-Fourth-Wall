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
        return Color.Lerp(rareWordColor, commonWordColor, popularityFactor);
    }

    public Color GetCommonWordColor()
    {
        return commonWordColor;
    }
}
