using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TransformRandomizer))]
public class TransformRandomizerScriptEditor : Editor
{
  public override void OnInspectorGUI()
  {

    DrawDefaultInspector();

    TransformRandomizer transformRandomizer = (TransformRandomizer)target;

    if (GUILayout.Button("Randomize"))
    {
      transformRandomizer.Randomize();
    }

    if (GUILayout.Button("Reset"))
    {
      transformRandomizer.Reset();
    }

  }
}
