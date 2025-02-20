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
        TalkingHead talkingHead;

        private void Awake()
        {
            walkwayPieceFactory = GetComponent<WalkwayPieceFactory>();
            talkingHead = FindObjectOfType<TalkingHead>();
        }

        public GameObject AddPieceToWalkway(Vector3 entryPoint, string pieceWord, string lastPieceWord)
        {
            Vector3 newPiecePivot = GetNextPieceFinalPos(entryPoint, lastPieceWord, pieceWord);
            GameObject piece = InstatiatePiece(newPiecePivot, Quaternion.identity, pieceWord);
            return piece;
        }

        private Vector3 GetNextPieceFinalPos(Vector3 endTopOfLastPiece, string lastPieceWord, string pieceWord)
        {
            Vector3 newPiecePivot = endTopOfLastPiece;

            //adjust down to align top
            float pieceHeight = walkwayPieceFactory.GetPieceHeight();
            newPiecePivot.y -= pieceHeight / 2;

            //adjust in sides at random
            newPiecePivot.x += Random.Range(-maxSideShift, maxSideShift);

            //add sentence gap if needed
            if (isSeparatingSentences && lastPieceWord.EndsWith(".") && !string.IsNullOrEmpty(pieceWord))
            {
                newPiecePivot.z += sentanceGapSize;
            }

            return newPiecePivot;
        }

        public GameObject InstatiatePiece(Vector3 targetPos, Quaternion targetRot, string pieceWord)
        {
            GameObject piece = null;

            if (talkingHead != null)
            {
                piece = walkwayPieceFactory.SpawnFromTalkingHead(targetPos, targetRot, pieceWord);
            }
            else if (animationType == AnimationType.MOVE_FROM_ABOVE_TARGET)
            {
                piece = walkwayPieceFactory.SpawnAboveTargetAndMoveIntoPlace(targetPos, targetRot, pieceWord);
            }
            else if (animationType == AnimationType.MOVE_FROM_SUBTITLE)
            {
                piece = walkwayPieceFactory.SpawnInFrontOfCameraAnMoveIntoPlace(pieceWord, targetPos, targetRot);
            }
            else
            {
                piece = walkwayPieceFactory.SpawnAtFinalPosition(targetPos, targetRot, pieceWord);
            }

            if (isDissapearing)
            {
                piece.GetComponent<LevelPiece.DestroyTimer>().startDestroyTimerWhenPositioned = true;
            }

            return piece;
        }
    }
}
