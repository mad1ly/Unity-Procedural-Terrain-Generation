using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(VegetationGenerator))]
public class VegetationEditor : Editor
{
    public override void OnInspectorGUI()
    {
        VegetationGenerator generator = (VegetationGenerator)target;
        DrawDefaultInspector();
        if (GUILayout.Button("Raycast Prefabs"))
        {
            generator.Generate();
        }
        if (GUILayout.Button("Clear Prefabs"))
        {
            generator.Clear();
        }
    }
}
