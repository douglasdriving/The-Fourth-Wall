//sets the color of the piece based on the number of letter is it.
using UnityEngine;

namespace LevelPiece
{
    public class ColorSetter : MonoBehaviour
    {
        MeshRenderer[] meshesToColor;
        [SerializeField] Material baseMaterial;
        [SerializeField] Material frozenMaterial;
        Material pieceMaterial;

        void Awake()
        {
            meshesToColor = GetComponentsInChildren<MeshRenderer>();
            SceneRules rules = FindObjectOfType<SceneRules>();
            if (rules && rules.setPieceColors && rules.freezePiecesOnSpawn)
            {
                SetFrozenMaterial();
            }
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

        public void UpdatePieceMaterialByWord(string word)
        {
            PieceColorCreator pieceColorCreator = FindObjectOfType<PieceColorCreator>();
            if (!pieceColorCreator) return;
            {
                Debug.LogWarning("PieceColorSetter: No PieceColorCreator found in scene");
            }
            Color materialColor;
            if (word == null || word == "")
            {
                materialColor = pieceColorCreator.GetCommonWordColor();
            }
            else
            {
                materialColor = pieceColorCreator.GetColorForWord(word);
            }
            pieceMaterial = new Material(baseMaterial);
            pieceMaterial.color = materialColor;
            bool isFrozen = GetComponent<Positioner>().isFrozen;
            if (!isFrozen)
            {
                SetPieceMaterial();
            }
        }

        public void SetFrozenMaterial()
        {
            ApplyMaterial(frozenMaterial);
        }

        public void SetPieceMaterial()
        {
            ApplyMaterial(pieceMaterial);
        }

        public void SetBaseMaterial()
        {
            ApplyMaterial(baseMaterial);
        }

    }
}