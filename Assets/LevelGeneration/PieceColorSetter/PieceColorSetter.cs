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

        int wordPopularity = WordPopularityCounter.GetPopularityForWord(textToCheck.text);
        int mostPopularWordCount = WordPopularityCounter.mostPopularWord.Item2;

        Debug.Log("wordPopularity: " + wordPopularity);
        Debug.Log("mostPopularWordCount: " + mostPopularWordCount);

        if (wordPopularity == 0)
        {
            Debug.LogWarning("PieceColorSetter: Word not found in WordPopularityCounter: " + textToCheck.text);
        }
        if (mostPopularWordCount == 0)
        {
            Debug.LogError("mostPopularWordCount is zero, cannot divide by zero.");
            return;
        }

        float popularityFactor = (float)wordPopularity / (float)mostPopularWordCount;
        Debug.Log("popularityFactor: " + popularityFactor);

        Color materialColor = Color.Lerp(rareWordColor, commonWordColor, popularityFactor);
        Debug.Log("materialColor: " + materialColor);

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
