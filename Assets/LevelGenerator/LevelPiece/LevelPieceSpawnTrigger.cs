using UnityEngine;

public class LevelPieceSpawnTrigger : MonoBehaviour
{
    bool hasBeenTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasBeenTriggered) return;
        if (!other.CompareTag("Player")) return;
        Debug.LogWarning("this is not implemented");
        hasBeenTriggered = true;
    }
}
