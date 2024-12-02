//sets the color of the piece based on the number of letter is it.
using UnityEngine;

public class PieceColorSetter : MonoBehaviour
{
    [SerializeField] Color commonWordColor;
    [SerializeField] Color rareWordColor;
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
        CreateMaterialsBasedOnWordPopularity();
        SetTransparent(false);
    }

    private void CreateMaterialsBasedOnWordPopularity()
    {

        if (textToCheck == null)
        {
            Debug.LogWarning("PieceColorSetter: TextMeshPro component not found on GameObject");
            return;
        }

        float popularityFactor = WordPopularityCounter.GetPopularityForWordNormalized(textToCheck.text);
        Color materialColor = Color.Lerp(rareWordColor, commonWordColor, popularityFactor);

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
