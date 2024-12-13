using System.Collections;
using UnityEngine;

namespace LevelGeneration
{
    /// <summary>
    /// generates an exit portal at the end of the level after a delay
    /// </summary>
    public class ExitPortalGenerator : MonoBehaviour
    {
        [SerializeField] GameObject exitPortalPrefab;
        [SerializeField] float portalHeightAbovePlatform = 1.5f;
        [SerializeField] float timeBetweenPieceAndPortalSpawn = 0.8f;

        public IEnumerator GenerateExitPortalAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            GameObject portalLevelPiece = FindObjectOfType<LevelGenerator>().SpawnNextPiece("");
            LevelPiecePositioner piecePositioner = portalLevelPiece.GetComponent<LevelPiecePositioner>();
            Vector3 portalPos = piecePositioner.targetPos + Vector3.up * portalHeightAbovePlatform;
            yield return new WaitForSeconds(timeBetweenPieceAndPortalSpawn);
            Instantiate(exitPortalPrefab, portalPos, piecePositioner.targetRot);
        }
    }
}

