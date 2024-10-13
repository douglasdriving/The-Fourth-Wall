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
        [SerializeField] float maxSideShift = 0.6f;
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

        public GameObject AddPieceToWalkway(Vector3 entryPoint, string pieceWord, string lastPieceWord)
        {
            Vector3 newPiecePivot = GetNextPieceFinalPos(entryPoint, lastPieceWord);
            GameObject piece = InstatiatePiece(newPiecePivot, Quaternion.identity, pieceWord);
            return piece;
        }

        private Vector3 GetNextPieceFinalPos(Vector3 endTopOfLastPiece, string lastPieceWord)
        {
            Vector3 newPiecePivot = endTopOfLastPiece;

            //adjust down to align top
            float pieceHeight = walkwayPieceFactory.GetPieceHeight();
            newPiecePivot.y -= pieceHeight / 2;

            //adjust in sides at random
            newPiecePivot.x += Random.Range(-maxSideShift, maxSideShift);

            //add sentence gap if needed
            if (isSeparatingSentences && lastPieceWord.EndsWith("."))
            {
                newPiecePivot.z += sentanceGapSize;
            }

            return newPiecePivot;
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
