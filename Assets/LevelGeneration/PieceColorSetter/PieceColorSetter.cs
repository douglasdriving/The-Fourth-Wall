//sets the color of the piece based on the number of letter is it.
using UnityEngine;

public class PieceColorSetter : MonoBehaviour
{
    MeshRenderer[] meshesToColor;
    [SerializeField] TMPro.TextMeshProUGUI textToCheck;
    [SerializeField] Material baseMaterial;
    [SerializeField] Material frozenMaterial;
    Material pieceMaterial;

    void Awake()
    {
        meshesToColor = GetComponentsInChildren<MeshRenderer>();
        // CreatePieceMaterial();
        //ok, quite simply, this should not be called on awake. rather, it should be called by whaterever sets the word
        SceneRules rules = FindObjectOfType<SceneRules>();
        if (rules && rules.freezePiecesOnSpawn)
        {
            SetFrozenMaterial();
        }
        // else
        // {
        //     SetPieceMaterial();
        // }
    }

    private void ApplyMaterial(Material material)
    {
        if (meshesToColor.Length == 0)
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
        if (!pieceColorCreator)
        {
            Debug.LogWarning("PieceColorSetter: No PieceColorCreator found in scene");
            return;
        }

        Color materialColor;
        if (word == null || word == "")
        {
            materialColor = pieceColorCreator.GetCommonWordColor();
        }
        else
        {
            materialColor = pieceColorCreator.GetColorForWord(word); //word hasnt been added yet...
            //in reality, all of this should be controlled by the factory, which should determine the order of things.
        }
        pieceMaterial = new Material(baseMaterial);
        pieceMaterial.color = materialColor;
    }

    public void SetFrozenMaterial()
    {
        ApplyMaterial(frozenMaterial);
    }

    public void SetPieceMaterial()
    {
        ApplyMaterial(pieceMaterial);
    }

}
