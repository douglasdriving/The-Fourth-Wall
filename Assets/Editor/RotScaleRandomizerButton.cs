using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RotScaleRandomizerButton))]
public class RotScaleRandomizerButton : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MyObjectScript myObjectScript = (MyObjectScript)target;
        if (GUILayout.Button("Run My Function"))
        {
            myObjectScript.MyFunction();
        }

    }
}