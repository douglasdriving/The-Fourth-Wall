using TMPro;
using UnityEngine;

public class FontSetter : MonoBehaviour
{
    void Awake()
    {
        var sceneFontHolder = FindObjectOfType<SceneFontHolder>();
        if (sceneFontHolder == null) return;
        TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();
        if (text == null)
        {
            Debug.LogWarning("FontSetter: TextMeshProUGUI component not found on GameObject");
            return;
        }
        text.font = sceneFontHolder.sceneFont;
    }
}
