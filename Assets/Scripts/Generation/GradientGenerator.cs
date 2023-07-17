using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GradientGenerator
{
    public static float[,] GenerateGradientFalloff(int width, int height)
    {
        float[,] map = new float[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int n = 0; n < height; n++)
            {
                float x = i / (float)width * 2 - 1;
                float y = n / (float)height * 2 - 1;

                float value = Mathf.Max(Mathf.Abs(x), Mathf.Abs(y));
                map[i, n] = Evaluate(value);

            }
        }

        return map;
    }

    static float Evaluate(float value)
    {
        float a = 3;
        float b = 2.2f;

        return Mathf.Pow(value, a) / (Mathf.Pow(value, a) + Mathf.Pow(b - b * value, a)); 
    }
}
