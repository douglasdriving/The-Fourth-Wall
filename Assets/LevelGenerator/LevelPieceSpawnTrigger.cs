using UnityEngine;

public class LevelPieceSpawnTrigger : MonoBehaviour
{
    public LevelPieceType nextPieceType = LevelPieceType.WALKWAY; //change this when subtitle hits given time!
    bool hasBeenTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasBeenTriggered) return;
        if (!other.CompareTag("Player")) return;
        FindAnyObjectByType<LevelGenerator>().SpawnNextLevelPiece(nextPieceType);
        hasBeenTriggered = true;
    }
}
