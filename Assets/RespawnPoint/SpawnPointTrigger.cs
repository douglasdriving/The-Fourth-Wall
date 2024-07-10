using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<FallReset>().SetSpawnPoint();
        }
    }
}
