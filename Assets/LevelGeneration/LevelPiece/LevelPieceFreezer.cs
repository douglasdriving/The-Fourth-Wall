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
            if (shouldFreeze)
            {
                Freeze();
            }
            if (!shouldFreeze && deleteGameObjectWhenUnfrozen)
            {
                Destroy(gameObject);
            }
        }

        private void Freeze()
        {
            levelPiecePositioner.isFrozen = true;
            GetComponentInParent<PieceColorSetter>().SetFrozenMaterial();
        }

        public void Unfreeze()
        {
            if (!levelPiecePositioner.isFrozen) return;
            levelPiecePositioner.isFrozen = false;
            GetComponentInParent<PieceColorSetter>().SetPieceMaterial();
            if (deleteGameObjectWhenUnfrozen)
            {
                Destroy(gameObject);
            }
        }
    }
}
