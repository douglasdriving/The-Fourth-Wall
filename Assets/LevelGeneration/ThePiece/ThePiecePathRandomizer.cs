using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace LevelGeneration.ThePiece
{
    /// <summary>
    /// Randomizes the correct fork in the path of the piece.
    /// </summary>
    public class ThePiecePathRandomizer : MonoBehaviour
    {

        [SerializeField] Transform forks;
        [SerializeField] Transform walkOffPoint;


        void Awake()
        {
            RandomizeCorrectFork();
        }

        private void RandomizeCorrectFork()
        {
            int correctForkId = Random.Range(0, forks.childCount);
            for (int i = 0; i < forks.childCount; i++)
            {
                Transform fork = forks.GetChild(i);
                if (i == correctForkId)
                {
                    SetTheFork(fork, true);
                }
                else
                {
                    SetTheFork(fork, false);
                }
            }
        }

        private void SetTheFork(Transform fork, bool shouldBeTheFork)
        {
            if (!shouldBeTheFork)
            {
                Destroy(fork.GetComponentInChildren<Collider>());
            }

            string textOnFork = shouldBeTheFork ? "the" : "a";
            fork.GetComponentInChildren<TMP_Text>().text = textOnFork;

            if (shouldBeTheFork)
            {
                walkOffPoint.transform.localPosition = new Vector3(fork.localPosition.x, walkOffPoint.localPosition.y, walkOffPoint.localPosition.z);
            }
        }
    }
}

