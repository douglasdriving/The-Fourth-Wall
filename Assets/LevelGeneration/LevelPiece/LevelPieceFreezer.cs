using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelGeneration
{
    public class LevelPieceFreezer : MonoBehaviour
    {
        [SerializeField] LevelPiecePositioner levelPiecePositioner;
        [SerializeField] bool deleteGameObjectWhenUnfrozen = false;

        void Awake()
        {
            SceneRules rules = FindObjectOfType<SceneRules>();
            bool shouldFreeze = rules && rules.freezePiecesOnSpawn;
            levelPiecePositioner.isFrozen = shouldFreeze;
            if (!shouldFreeze && deleteGameObjectWhenUnfrozen)
            {
                Destroy(gameObject);
            }
        }

        public void Unfreeze()
        {
            if (!levelPiecePositioner.isFrozen) return;
            levelPiecePositioner.isFrozen = false;
            if (deleteGameObjectWhenUnfrozen)
            {
                Destroy(gameObject);
            }
        }
    }
}
