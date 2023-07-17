using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public enum DrawMode {NoiseMap, ColorMap, Mesh, Gradient}
    public DrawMode drawMode;

    public bool gradient;
    public int TerrainWidth;
    public int TerrainHeight;
    public float Scale;
    public int Seed;
    public int Octaves;
    public float TerrainAmplitude;
    public AnimationCurve TerrainHeightCurve;

    [Range(0,1)]
    public float Persistance;
    public float Lacunarity;
    public Vector2 Offset;

    public bool autoGen;

    public TerrainType[] regions;

    float[,] gradientMap;
    public VegetationGenerator treeGen;
    public VegetationGenerator grassGen;


    private void Awake()
    {
        gradientMap = GradientGenerator.GenerateGradientFalloff(TerrainWidth, TerrainHeight);
    }


    public void GenerateTerrain()
    {
        treeGen.Clear();
        grassGen.Clear();
        float[,] noiseMap = Noise.GenerateNoiseMap(TerrainWidth, TerrainHeight, Scale, Seed, Octaves, Persistance, Lacunarity, Offset);

        Color[] colorMap = new Color[TerrainWidth * TerrainHeight];
        for (int y = 0; y < TerrainHeight; y++) {
            for (int x = 0; x < TerrainWidth; x++) {
                // Combining noise with gradient falloff
                if (gradient)
                {
                    noiseMap[x, y] = Mathf.Clamp01(noiseMap[x, y] - gradientMap[x, y]);
                }
                // Creating height maps
                float currentHeight = noiseMap[x, y];
                for (int i = 0; i < regions.Length; i++)
                {
                    if(currentHeight <= regions[i].height)
                    {
                        colorMap[y * TerrainWidth + x] = regions[i].color;
                        break;
                    }
                }
            } 
        }

        MapDisplay display = FindObjectOfType<MapDisplay>();
        if (drawMode == DrawMode.NoiseMap)
        {
            display.DrawTextureMap(TextureGenerator.DrawNoiseMap(noiseMap));
        }
        else if (drawMode == DrawMode.ColorMap)
        {
            display.DrawTextureMap(TextureGenerator.DrawColorMap(colorMap, TerrainWidth, TerrainHeight));
        }
        else if (drawMode == DrawMode.Mesh)
        {
            display.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, TerrainAmplitude, TerrainHeightCurve), TextureGenerator.DrawColorMap(colorMap, TerrainWidth, TerrainHeight));
            treeGen.Generate();
            grassGen.Generate();
        }
        else if (drawMode == DrawMode.Gradient)
        {
            display.DrawTextureMap(TextureGenerator.DrawNoiseMap(GradientGenerator.GenerateGradientFalloff(TerrainWidth, TerrainHeight)));
        }

    }


    // Clamping the values in Editor
    private void OnValidate()
    {
        // Terrain Size Clamping
        if (TerrainWidth < 1)
        {
            TerrainWidth = 1;
        }
        if (TerrainHeight < 1)
        {
            TerrainHeight = 1;
        }
        // Noise Scale and Octaves Clamping
        if (Scale < 5)
        {
            Scale = 5;
        }
        if (Octaves < 1)
        {
            Octaves = 1;
        }

        gradientMap = GradientGenerator.GenerateGradientFalloff(TerrainWidth, TerrainHeight);

    }
}


[System.Serializable]
public struct TerrainType
{
    public string name;
    public float height;
    public Color color;
}
