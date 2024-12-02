//sets the color of the piece based on the number of letter is it.
using UnityEngine;

public class PieceColorSetter : MonoBehaviour
{
    MeshRenderer[] meshesToColor;
    [SerializeField] TMPro.TextMeshProUGUI textToCheck;
    [SerializeField] Material baseMaterial;
    Material coloredMaterial;

    void Start()
    {
        if (FindAnyObjectByType<PieceColorCreator>())
        {
            SetMaterialBasedOnWordPopularity();
        }
    }

    private void SetMaterialBasedOnWordPopularity()
    {

        PieceColorCreator pieceColorCreator = FindObjectOfType<PieceColorCreator>();

        if (textToCheck == null)
        {
            SetColor(pieceColorCreator.GetCommonWordColor());
            return;
        }

        Color materialColor = pieceColorCreator.GetColorForWord(textToCheck.text);
        SetColor(materialColor);
    }

    private void SetColor(Color colorToSet)
    {

        meshesToColor = GetComponentsInChildren<MeshRenderer>();

        if (meshesToColor.Length == 0)
        {
            Debug.LogWarning("PieceColorSetter: No MeshRenderer components found on GameObject or children");
            return;
        }

        coloredMaterial = new Material(baseMaterial);
        coloredMaterial.color = colorToSet;

        foreach (MeshRenderer meshToColor in meshesToColor)
        {
            meshToColor.material = coloredMaterial;
        }
    }

}
