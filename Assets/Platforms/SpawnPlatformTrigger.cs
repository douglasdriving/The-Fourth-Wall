using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlatformTrigger : MonoBehaviour
{
    bool hasBeenTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasBeenTriggered) return;
        if (!other.CompareTag("Player")) return;
        FindObjectOfType<PlatformGenerator>().GenerateNextPlatform();
        hasBeenTriggered = true;
    }
}
