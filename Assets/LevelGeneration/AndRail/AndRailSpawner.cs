using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelGeneration
{
    /// <summary>
    /// spawns a rail between 2 points
    /// </summary>
    public class AndRailSpawner : MonoBehaviour
    {
        [SerializeField] GameObject railPrefab;

        void Awake()
        {
            SpawnRail(new Vector3(0, 0, 5), new Vector3(0, 0, 10));
        }

        public void SpawnRail(Vector3 start, Vector3 end)
        {
            GameObject rail = Instantiate(railPrefab);
            AndRailPositioner positioner = rail.GetComponent<AndRailPositioner>();
            positioner.SetupRailPositioning(start, end);
        }
    }
}
