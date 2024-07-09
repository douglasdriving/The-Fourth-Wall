using UnityEngine;

public class LevelPieceSpawnTrigger : MonoBehaviour
{
    bool hasBeenTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasBeenTriggered) return;
        if (!other.CompareTag("Player")) return;
        FindAnyObjectByType<LevelGenerator>().SpawnNextLevelPiece();
        hasBeenTriggered = true;
    }
}
