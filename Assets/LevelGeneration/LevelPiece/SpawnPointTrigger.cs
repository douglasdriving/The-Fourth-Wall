using Player;
using UnityEngine;

namespace LevelGeneration
{
    /// <summary>
    /// sets the spawn point for the player when they enter the trigger
    /// </summary>
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
}

