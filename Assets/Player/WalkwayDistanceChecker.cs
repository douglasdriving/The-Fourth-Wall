using System.Collections;
using System.Collections.Generic;
using LevelGeneration;
using UnityEngine;

namespace Player
{
    public class WalkwayDistanceChecker : MonoBehaviour
    {
        LevelGenerator levelGenerator;
        [SerializeField] float distanceToEdgeThatCountsAsClose = 15f;

        void Awake()
        {
            levelGenerator = FindObjectOfType<LevelGenerator>();
        }

        public bool IsPlayerCloseToEdge()
        {
            float distance = GetDistanceToWalkwayEnd();
            return distance < distanceToEdgeThatCountsAsClose;
        }

        private float GetDistanceToWalkwayEnd()
        {
            Vector3 endPiece = levelGenerator.GetLastPieceFinalWalkoffPoint();
            Vector3 playerPosition = transform.position;
            float distance = Vector3.Distance(playerPosition, endPiece);
            return distance;
        }
    }
}

