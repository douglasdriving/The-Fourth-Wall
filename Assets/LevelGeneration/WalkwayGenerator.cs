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
        [SerializeField] float spawnHeight = 1.5f;
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

            if (animationType == AnimationType.MOVE_FROM_ABOVE_TARGET)
            {
                Vector3 spawnPos = targetPos + Vector3.up * spawnHeight;
                bool rotateTowardsPlayer = false;
                SceneRules rules = FindObjectOfType<SceneRules>();
                if (rules && rules.pieceSpawnSpread)
                {
                    //alter the spaw pos!
                    float distanceToPlayer = Vector3.Distance(Camera.main.transform.position, spawnPos);
                    //randomily increase its height
                    float maxUpShift = distanceToPlayer / 5;
                    spawnPos.y += Random.Range(0, maxUpShift);
                    //randomly move it a bit left or right
                    float maxSideShift = distanceToPlayer / 3;
                    spawnPos.x += Random.Range(-maxSideShift, maxSideShift);
                    //rotate it towards the player  
                    rotateTowardsPlayer = true;
                }
                piece = walkwayPieceFactory.SpawnAboveTargetAndMoveIntoPlace(targetPos, targetRot, pieceWord, spawnPos, rotateTowardsPlayer);
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
