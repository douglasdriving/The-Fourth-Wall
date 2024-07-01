using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("something entered");
        if (other.CompareTag("Player"))
        {
            Debug.Log("that something was a player");
            other.GetComponent<FallReset>().SetSpawnPoint(transform.position);
        }
    }
}
