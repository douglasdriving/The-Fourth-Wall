//sets the color of the piece based on the number of letter is it.
using System;
using UnityEngine;

namespace LevelPiece
{
    public class ColorSetter : MonoBehaviour
    {

        public enum Mode
        {
            MOVING_LEVEL_PIECE,
            AVERAGE_COLOR,
        }

        MeshRenderer[] meshesToColor;
        [SerializeField] Mode mode = Mode.MOVING_LEVEL_PIECE;
        [SerializeField] Material baseMaterial;
        [SerializeField] Material frozenMaterial;
        [SerializeField] Positioner positioner;
        Material coloredMaterial;
        PieceColorCreator pieceColorCreator;
        SceneRules rules;

        void Awake()
        {

            pieceColorCreator = FindObjectOfType<PieceColorCreator>();
            meshesToColor = GetComponentsInChildren<MeshRenderer>();
            rules = FindObjectOfType<SceneRules>();

            if (rules == null || rules.colorPieces == false)
            {
                return;
            }

            if (mode == Mode.MOVING_LEVEL_PIECE)
            {
                UpdateToAverageColor();
                if (rules.freezePiecesOnSpawn)
                {
                    SetFrozenMaterial();
                }
                else
                {
                    SetColored();
                }
            }
            else if (mode == Mode.AVERAGE_COLOR)
            {
                UpdateToAverageColor();
                SetColored();
            }
        }

        private void UpdateToAverageColor()
        {
            coloredMaterial = new Material(baseMaterial);
            coloredMaterial.color = pieceColorCreator.GetAverageColor();
        }

        private void ApplyMaterial(Material material)
        {
            if (meshesToColor == null || meshesToColor.Length == 0)
            {
                return;
            }
            foreach (MeshRenderer meshToColor in meshesToColor)
            {
                meshToColor.material = material;
            }
        }

        public void OnWordSet(string word)
        {
            UpdateColor(word);
            if (positioner.isFrozen) SetFrozenMaterial();
            else SetColored();
        }

        void UpdateColor(string word)
        {
            if (rules && rules.colorPieces)
            {
                if (!pieceColorCreator)
                {
                    Debug.LogWarning("PieceColorSetter: No PieceColorCreator found in scene");
                    return;
                }

                Color materialColor;
                if (word == null || word == "")
                {
                    materialColor = pieceColorCreator.GetAverageColor();
                }
                else
                {
                    materialColor = pieceColorCreator.GetColorForWord(word);
                }

                coloredMaterial = new Material(baseMaterial);
                coloredMaterial.color = materialColor;
            }
        }

        public void SetFrozenMaterial()
        {
            ApplyMaterial(frozenMaterial);
        }

        public void SetColored()
        {
            if (rules && rules.colorPieces)
            {
                ApplyMaterial(coloredMaterial);
            }
        }

        public void SetBaseMaterial()
        {
            ApplyMaterial(baseMaterial);
        }

    }
}