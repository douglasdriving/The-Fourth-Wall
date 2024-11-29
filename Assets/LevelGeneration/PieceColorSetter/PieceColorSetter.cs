//sets the color of the piece based on the number of letter is it.
using UnityEngine;

public class PieceColorSetter : MonoBehaviour
{
    [SerializeField] Color shortWordColor;
    [SerializeField] Color longWordColor;
    MeshRenderer[] meshesToColor;
    [SerializeField] TMPro.TextMeshProUGUI textToCheck;
    [SerializeField] Material baseMaterial;
    Material transparentMaterial;
    Material originalMaterial;

    void Awake()
    {
        meshesToColor = GetComponentsInChildren<MeshRenderer>();
        if (meshesToColor.Length == 0)
        {
            Debug.LogWarning("PieceColorSetter: No MeshRenderer components found on GameObject or children");
        }
    }

    void Start()
    {
        CreateMaterialsBasedOnWordLength();
        SetTransparent(false);
    }

    private void CreateMaterialsBasedOnWordLength()
    {
        if (textToCheck == null)
        {
            Debug.LogWarning("PieceColorSetter: TextMeshPro component not found on GameObject");
            return;
        }
        int wordLength = textToCheck.text.Length;
        print("Word length: " + wordLength);
        Color materialColor;
        if (wordLength <= 1)
        {
            materialColor = shortWordColor;
        }
        else if (wordLength >= 10)
        {
            materialColor = longWordColor;
        }
        else
        {
            float t = (wordLength - 1) / 9f;
            materialColor = Color.Lerp(shortWordColor, longWordColor, t);
        }
        originalMaterial = new Material(baseMaterial);
        originalMaterial.color = materialColor;
        transparentMaterial = new Material(baseMaterial);
        transparentMaterial.color = new Color(materialColor.r, materialColor.g, materialColor.b, 0.2f);
    }

    public void SetTransparent(bool transparent)
    {
        foreach (MeshRenderer meshToColor in meshesToColor)
        {
            meshToColor.material = transparent ? transparentMaterial : originalMaterial;
        }
    }
}
