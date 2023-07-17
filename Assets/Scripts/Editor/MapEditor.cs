using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(TerrainGenerator))]
public class MapEditor : Editor
{
    public override void OnInspectorGUI()
    {
        TerrainGenerator generator = (TerrainGenerator)target;

        if (DrawDefaultInspector())
        {
            if (generator.autoGen)
            {
                generator.GenerateTerrain();
            }
        }
        if (GUILayout.Button ("Generate Noise"))
        {
            generator.GenerateTerrain();
        }

    }
}
