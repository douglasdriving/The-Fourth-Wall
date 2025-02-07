using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelPiece
{
    public class WordSetter : MonoBehaviour
    {
        [SerializeField] TMPro.TextMeshProUGUI textToSet;
        [SerializeField] ColorSetter pieceColorSetter;

        public void SetWord(string word)
        {
            textToSet.text = word;
            pieceColorSetter.OnWordSet(word);
        }
    }
}

