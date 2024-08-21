using System.Collections;
using UnityEngine;

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
        GameObject portalLevelPiece = FindObjectOfType<LevelGenerator>().SpawnNextPiece("", 0);
        Vector3 portalPos = portalLevelPiece.transform.position + Vector3.up * portalHeightAbovePlatform;
        Instantiate(exitPortalPrefab, portalPos, portalLevelPiece.transform.rotation);
    }
}
