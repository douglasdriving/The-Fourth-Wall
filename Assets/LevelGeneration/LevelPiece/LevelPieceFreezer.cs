using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelPiece
{
    public class Freezer : MonoBehaviour
    {
        [SerializeField] Positioner levelPiecePositioner;
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

        public void Freeze()
        {
            levelPiecePositioner.isFrozen = true;
            GetComponentInParent<ColorSetter>().SetFrozenMaterial();
        }

        public void Unfreeze()
        {
            if (!levelPiecePositioner.isFrozen) return;
            levelPiecePositioner.isFrozen = false;
            GetComponentInParent<ColorSetter>().SetColored();
            if (deleteGameObjectWhenUnfrozen)
            {
                Destroy(gameObject);
            }
        }
    }
}
