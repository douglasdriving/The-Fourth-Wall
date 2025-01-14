using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelPiece //this namespace should be utilized better
{
    public class WordSetter : MonoBehaviour
    {
        [SerializeField] TMPro.TextMeshProUGUI textToSet;
        [SerializeField] PieceColorSetter pieceColorSetter;

        public void SetWord(string word)
        {
            textToSet.text = word;
            pieceColorSetter.UpdatePieceMaterialByWord(word);
        }
    }
}

