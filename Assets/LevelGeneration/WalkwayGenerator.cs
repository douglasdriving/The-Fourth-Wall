using Player;
using TMPro;
using UnityEngine;

namespace LevelGeneration
{
    /// <summary>
    /// Generates a walkway of pieces to walk on
    /// </summary>
    public class WalkwayGenerator : MonoBehaviour
    {
        LevelPieceMolds levelPieceMolds;
        [SerializeField] float sentanceGapSize = 3f;
        public bool isWordAnimationActive = false;
        public bool isSeparatingSentences = false;

        private void Start()
        {
            levelPieceMolds = FindAnyObjectByType<LevelPieceMolds>();
        }

        public GameObject AddPieceToWalkway(Transform pieceToMoveFrom, string pieceWord)
        {
            Vector3 newPiecePivot = GetNextPiecePivot(pieceToMoveFrom);
            GameObject piece = InstatiatePiece(newPiecePivot, Quaternion.identity, pieceWord);
            return piece;
        }

        private Vector3 GetNextPiecePivot(Transform pieceToMoveFrom)
        {
            Vector3 prevPieceFinalPivot = pieceToMoveFrom.position;
            Vector3 prevPieceFinalScale = pieceToMoveFrom.lossyScale;

            if (isWordAnimationActive)
            {
                LevelPiecePositioner prevPiecePositioner = pieceToMoveFrom.GetComponent<LevelPiecePositioner>();
                prevPieceFinalPivot = prevPiecePositioner.targetPosition;
                prevPieceFinalScale = prevPiecePositioner.targetPieceScale;
            }

            Vector3 finalEndOfPrevPiece = prevPieceFinalPivot + Vector3.forward * prevPieceFinalScale.z;
            float halfWidthOfPrevPiece = prevPieceFinalScale.x / 2;

            Vector3 newPiecePivot = finalEndOfPrevPiece;
            newPiecePivot.x += Random.Range(-halfWidthOfPrevPiece, halfWidthOfPrevPiece);

            if (isSeparatingSentences)
            {
                newPiecePivot = ExtendWithSentenceGapIfLastPieceEndedSentence(pieceToMoveFrom, newPiecePivot);
            }

            return newPiecePivot;
        }

        private Vector3 ExtendWithSentenceGapIfLastPieceEndedSentence(Transform lastPiece, Vector3 piecePivotToExtend)
        {
            bool lastPieceEndedSentence = false;
            if (lastPiece != null)
            {
                lastPieceEndedSentence = IsPieceWordEndingSentence(lastPiece);
            }
            if (lastPieceEndedSentence)
            {
                piecePivotToExtend.z += sentanceGapSize;
            }

            return piecePivotToExtend;
        }

        private static bool IsPieceWordEndingSentence(Transform pieceToMoveFrom)
        {
            bool startsNewSentece = false;
            TMP_Text prevPieceTextElement = pieceToMoveFrom.GetComponentInChildren<TMP_Text>();
            if (prevPieceTextElement != null)
            {
                startsNewSentece = prevPieceTextElement.text.EndsWith(".");
            }

            return startsNewSentece;
        }

        public GameObject InstatiatePiece(Vector3 pivot, Quaternion rot, string pieceWord)
        {
            GameObject piece = levelPieceMolds.CopyNextMold();
            piece.GetComponent<LevelPiecePositioner>().MoveToPosition(pivot, rot, isWordAnimationActive);
            piece.GetComponentInChildren<TMP_Text>().text = pieceWord;
            return piece;
        }
    }
}
