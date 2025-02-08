//sets the color of the piece based on the number of letter is it.
using UnityEngine;

namespace LevelPiece
{
    public class ColorSetter : MonoBehaviour
    {
        MeshRenderer[] meshesToColor;
        [SerializeField] Material baseMaterial;
        [SerializeField] Material frozenMaterial;
        Material coloredMaterial;
        PieceColorCreator pieceColorCreator;
        SceneRules rules;

        void Awake()
        {
            pieceColorCreator = FindObjectOfType<PieceColorCreator>();
            meshesToColor = GetComponentsInChildren<MeshRenderer>();
            rules = FindObjectOfType<SceneRules>();
            UpdateToAverageColor();
            if (rules && rules.colorPieces)
            {
                if (rules.freezePiecesOnSpawn)
                {
                    SetFrozenMaterial();
                }
                else
                {
                    SetColored();
                }
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
                Debug.LogWarning("PieceColorSetter: No MeshRenderer components found on GameObject or children");
                return;
            }
            foreach (MeshRenderer meshToColor in meshesToColor)
            {
                meshToColor.material = material;
            }
        }

        public void UpdateColor(string word)
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
            else
            {
                Debug.LogWarning("PieceColorSetter: SceneRules not found or colorPieces is false. Will not color");
            }
        }

        public void SetFrozenMaterial()
        {
            ApplyMaterial(frozenMaterial);
        }

        public void SetColored()
        {
            ApplyMaterial(coloredMaterial);
        }

        public void SetBaseMaterial()
        {
            ApplyMaterial(baseMaterial);
        }

    }
}