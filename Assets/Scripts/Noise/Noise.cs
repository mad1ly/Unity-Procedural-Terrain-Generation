using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise
{
    public static float[,] GenerateNoiseMap(int width, int height, float scale, int seed, int octaves, float persistance, float lacunarity, Vector2 offset)
    {
        float[,] noiseMap = new float[width, height];

        // Seed are used in order to achieve the same result with given parameter
        // It essentialy scrolls through our noise map
        System.Random prng = new System.Random(seed);
        Vector2[] octavesOffset = new Vector2[octaves];
        for (int i = 0; i < octaves; i++)
        {
            float offsetX = prng.Next(-100000, 100000) + offset.x;
            float offsetY = prng.Next(-100000, 100000) + offset.y;
            octavesOffset[i] = new Vector2(offsetX, offsetY);
        }

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        // This lines allow us to zoom in and out of the centre of the perlin noise
        float halfWidth = width / 2f;
        float halfHeight = height / 2f;


        // Calculating perlin value for each coordinate
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for (int i = 0; i < octaves; i++)
                {
                    float sampleX = (x - halfWidth) / scale * frequency + octavesOffset[i].x;
                    float sampleY = (y - halfHeight) / scale * frequency + octavesOffset[i].y;

                    float value = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;

                    noiseHeight += value * amplitude;

                    amplitude *= persistance;
                    frequency *= lacunarity;
                }


                if (noiseHeight > maxNoiseHeight)
                {
                    maxNoiseHeight = noiseHeight;
                }
                else if (noiseHeight < minNoiseHeight)
                {
                    minNoiseHeight = noiseHeight;
                }

                noiseMap[x, y] = noiseHeight;

            }
        }


        // Normalizing perlin values
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
            }
        }

        return noiseMap;
    }

}
