using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelGeneration
{
    /// <summary>
    /// spawns a rail between 2
    /// </summary>
    public class AndRailSpawner : MonoBehaviour
    {
        [SerializeField] GameObject railPrefab;

        void Awake()
        {
            SpawnRail(new Vector3(2, 0, 0), new Vector3(10, 0, 10));
        }

        public void SpawnRail(Vector3 start, Vector3 end)
        {
            Rail rail = Instantiate(railPrefab).GetComponent<Rail>();
            rail.SetStartAndEnd(start, end);
        }
    }
}
