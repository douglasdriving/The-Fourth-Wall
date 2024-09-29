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
        [SerializeField] float railLength = 5;

        public GameObject SpawnRail(Vector3 start)
        {
            //calculate end point
            Vector3 end = start + Vector3.forward * railLength;

            //spawn rail
            GameObject rail = Instantiate(railPrefab);

            //position rail
            AndRailPositioner positioner = rail.GetComponent<AndRailPositioner>();
            positioner.SetupRailPositioning(start, end);
            return rail;
        }
    }
}
