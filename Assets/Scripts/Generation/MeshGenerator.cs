using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshGenerator
{
    public static MeshData GenerateTerrainMesh(float[,] heightmap, float amplitude, AnimationCurve curve)
    {
        int width = heightmap.GetLength(0);
        int height = heightmap.GetLength(1);
        float topLeftX = (width - 1) / -2f;
        float topLeftZ = (height - 1) / 2f;

        MeshData meshData = new MeshData(width, height);
        int vertIndex = 0;

        for(int y = 0; y < height; y++)
        {
            for(int x = 0; x < width; x++)
            {
                meshData.vertices[vertIndex] = new Vector3(topLeftX + x, curve.Evaluate(heightmap[x, y]) * amplitude, topLeftZ - y);
                meshData.uvs[vertIndex] = new Vector2(x / (float)width, y / (float)height);

                if (x < width -1 && y < height - 1)
                {
                    meshData.AddTriangle(vertIndex, vertIndex + width + 1, vertIndex + width);
                    meshData.AddTriangle(vertIndex + width + 1, vertIndex, vertIndex + 1);
                }
                
                vertIndex++;
            }
        }


        return meshData;
    }
}

public class MeshData
{
    public Vector3[] vertices;
    public int[] triangles;
    public Vector2[] uvs;

    int trisIndex;

    public MeshData(int meshWidth, int meshHeight)
    {
        vertices = new Vector3[meshWidth * meshHeight];
        uvs = new Vector2[meshWidth * meshHeight];
        triangles = new int[(meshWidth - 1) * (meshHeight - 1) * 6];
    }

    public void AddTriangle(int a, int b, int c)
    {
        triangles[trisIndex] = a;
        triangles[trisIndex + 1] = b;
        triangles[trisIndex + 2] = c;
        trisIndex += 3;
    }

    public Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
        return mesh;
    }

}
