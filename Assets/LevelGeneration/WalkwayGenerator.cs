using Player;
using TMPro;
using UnityEngine;

namespace LevelGeneration
{
    /// <summary>
    /// Generates a walkway of pieces to walk on
    /// </summary>
    [RequireComponent(typeof(WalkwayPieceFactory))]
    public class WalkwayGenerator : MonoBehaviour
    {
        WalkwayPieceFactory walkwayPieceFactory;
        [SerializeField] float sentanceGapSize = 3f;
        public enum AnimationType
        {
            NONE,
            MOVE_FROM_SUBTITLE,
            MOVE_FROM_ABOVE_TARGET,
        }
        [SerializeField] AnimationType animationType = AnimationType.NONE;
        public bool isSeparatingSentences = false;
        public bool isDissapearing = false;

        private void Start()
        {
            walkwayPieceFactory = GetComponent<WalkwayPieceFactory>();
        }

        public GameObject AddPieceToWalkway(Vector3 endOfLastPiece, string pieceWord, string lastPieceWord)
        {
            Vector3 newPiecePivot = GetNextPieceFinalPos(endOfLastPiece, lastPieceWord);
            GameObject piece = InstatiatePiece(newPiecePivot, Quaternion.identity, pieceWord);
            return piece;
        }

        private Vector3 GetNextPieceFinalPos(Vector3 endOfLastPiece, string lastPieceWord)
        {
            Vector3 newPiecePivot = endOfLastPiece;
            newPiecePivot.x += Random.Range(-0.3f, 0.3f); //magic numba

            if (isSeparatingSentences && lastPieceWord.EndsWith("."))
            {
                newPiecePivot.z += sentanceGapSize;
            }

            return newPiecePivot;
        }

        // private Vector3 ExtendWithSentenceGapIfLastPieceEndedSentence(Transform lastPiece, Vector3 piecePivotToExtend)
        // {
        //     bool lastPieceEndedSentence = false;
        //     if (lastPiece != null)
        //     {
        //         lastPieceEndedSentence = IsPieceWordEndingSentence(lastPiece);
        //     }
        //     if (lastPieceEndedSentence)
        //     {
        //         piecePivotToExtend.z += sentanceGapSize;
        //     }

        //     return piecePivotToExtend;
        // }

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

        public GameObject InstatiatePiece(Vector3 targetPos, Quaternion targetRot, string pieceWord)
        {

            GameObject piece = null;

            if (animationType == AnimationType.MOVE_FROM_ABOVE_TARGET)
            {
                piece = walkwayPieceFactory.SpawnAboveTargetAndMoveIntoPlace(targetPos, targetRot, pieceWord, 1.5f); //magic numba!
            }
            else if (animationType == AnimationType.MOVE_FROM_SUBTITLE)
            {
                piece = walkwayPieceFactory.InstantiateInFrontOfCameraAnMoveIntoPlace(pieceWord, targetPos, targetRot);
            }
            else
            {
                piece = walkwayPieceFactory.InstantiateAtFinalPosition(targetPos, targetRot, pieceWord);
            }


            if (isDissapearing)
            {
                piece.GetComponent<LevelPieceDestroyTimer>().startDestroyTimerWhenPositioned = true;
            }

            return piece;
        }
    }
}
