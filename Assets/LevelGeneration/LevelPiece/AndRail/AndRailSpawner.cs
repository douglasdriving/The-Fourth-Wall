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
        [SerializeField] float railLength = 10;

        public GameObject SpawnRail(Vector3 start)
        {
            Vector3 end = GetRandomRailEnd(start);

            //spawn rail
            GameObject rail = Instantiate(railPrefab);

            //position rail
            AndRailPositioner positioner = rail.GetComponent<AndRailPositioner>();
            positioner.SetupRailPositioning(start, end);
            return rail;
        }

        private Vector3 GetRandomRailEnd(Vector3 start)
        {
            //calculate end point
            float xRot = Random.Range(-20, 20);
            float yRot = Random.Range(-45, 45);
            Vector3 railForward = (Quaternion.Euler(xRot, yRot, 0) * Vector3.forward).normalized;
            Vector3 end = start + railForward * railLength;
            return end;
        }
    }
}
