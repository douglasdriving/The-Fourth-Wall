using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnWalkwayTrigger : MonoBehaviour
{
    bool hasBeenTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasBeenTriggered) return;
        if (!other.CompareTag("Player")) return;
        FindObjectOfType<WalkwayGenerator>().GenerateNextSection();
        hasBeenTriggered = true;
    }
}
