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
        public bool isUsingOldAnimation = false;
        public bool isUsingNewAnimation = false;
        public bool isSeparatingSentences = false;
        public bool isDissapearing = false;

        private void Start()
        {
            walkwayPieceFactory = GetComponent<WalkwayPieceFactory>();
        }

        public GameObject AddPieceToWalkway(Transform prevPiece, string pieceWord)
        {
            Vector3 finalPos = GetNextPieceFinalPos(prevPiece);
            GameObject piece = InstatiatePiece(finalPos, Quaternion.identity, pieceWord, prevPiece);
            return piece;
        }

        private Vector3 GetNextPieceFinalPos(Transform pieceToMoveFrom)
        {
            Vector3 prevPieceFinalPivot = pieceToMoveFrom.position;
            Vector3 prevPieceFinalScale = pieceToMoveFrom.lossyScale;

            if (isUsingOldAnimation || isUsingNewAnimation)
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

        public GameObject InstatiatePiece(Vector3 pos, Quaternion rot, string pieceWord, Transform prevPiece)
        {

            GameObject piece = null;

            if (isUsingNewAnimation)
            {
                piece = walkwayPieceFactory.InstantiatePieceAbovePos(pos, rot, prevPiece, pieceWord);
                piece.GetComponent<LevelPiecePositioner>().MoveWithSimpleAnimation(pos, rot);
            }
            else if (isUsingOldAnimation)
            {
                piece = walkwayPieceFactory.InstantiateInFrontOfCamera(pieceWord);
                piece.GetComponent<LevelPiecePositioner>().MoveWithAnimation(pos, rot);
            }
            else
            {
                piece = walkwayPieceFactory.InstantiateAtFinalPosition(pos, rot, pieceWord);
            }


            if (isDissapearing)
            {
                piece.GetComponent<LevelPieceDestroyTimer>().startDestroyTimerWhenPositioned = true;
            }

            return piece;
        }
    }
}