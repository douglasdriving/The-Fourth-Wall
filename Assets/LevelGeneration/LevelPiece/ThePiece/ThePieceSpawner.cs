using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelGeneration
{
    /// <summary>
    /// spawns "the" pieces onto the path.
    /// </summary>
    public class ThePieceSpawner : MonoBehaviour
    {
        [SerializeField] GameObject thePiecePrefab;

        public GameObject Spawn(Vector3 entryPoint)
        {
            GameObject piece = SpawnAboveFinalPos(entryPoint);
            StartAnimation(entryPoint, piece);
            LevelPiece.ColorSetter colorSetter = piece.GetComponent<LevelPiece.ColorSetter>();
            colorSetter.UpdateColor("the");
            colorSetter.SetColored();
            return piece;
        }

        private static void StartAnimation(Vector3 entryPoint, GameObject piece)
        {
            LevelPiece.Positioner positioner = piece.GetComponent<LevelPiece.Positioner>();
            Vector3 finalPos = entryPoint + Vector3.down * 0.5f;
            positioner.MoveWithSimpleAnimation(finalPos, Quaternion.identity);
        }

        private GameObject SpawnAboveFinalPos(Vector3 entryPoint)
        {
            Vector3 spawnPosition = entryPoint + Vector3.up * 3.5f + Vector3.left * 2.7f + Vector3.forward * 2;
            Quaternion spawnRotation = Quaternion.Euler(0, 90, 270);
            GameObject piece = Instantiate(thePiecePrefab, spawnPosition, spawnRotation);
            return piece;
        }
    }

}
