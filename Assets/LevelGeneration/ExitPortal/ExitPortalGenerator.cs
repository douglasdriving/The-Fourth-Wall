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

        public IEnumerator GenerateExitPortalAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            GeneratePieceWithPortal();
        }

        private void GeneratePieceWithPortal()
        {
            GameObject portalLevelPiece = FindObjectOfType<LevelGenerator>().SpawnNextPiece("");
            LevelPiecePositioner piecePositioner = portalLevelPiece.GetComponent<LevelPiecePositioner>();
            Vector3 portalPos = piecePositioner.targetPosition + Vector3.up * portalHeightAbovePlatform;
            Instantiate(exitPortalPrefab, portalPos, piecePositioner.targetRotation);
        }
    }
}

