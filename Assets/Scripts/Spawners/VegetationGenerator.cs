using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class VegetationGenerator : MonoBehaviour
{
    [SerializeField]
    GameObject prefab;

    [Header("Raycast Settings")]
    [SerializeField] int density;

    [Space]

    [SerializeField] float minHegiht;
    [SerializeField] float maxHeight;
    [SerializeField] Vector2 xRange;
    [SerializeField] Vector2 zRange;

    [Header("Prefab Variation Settings")]
    [SerializeField] float minScale;
    [SerializeField] float maxScale;

#if UNITY_EDITOR

    public void Generate()
    {
        Clear();

        for(int i = 0; i < density; i++)
        {
            float sampleX = Random.Range(xRange.x, xRange.y);
            float sampleY = Random.Range(zRange.x, zRange.y);
            Vector3 rayStart = new Vector3(sampleX, maxHeight, sampleY);

            if (!Physics.Raycast(rayStart, Vector3.down, out RaycastHit hit, Mathf.Infinity))
                continue;

            if (hit.point.y < minHegiht)
                continue;

            GameObject instantiatedPrefab = (GameObject)PrefabUtility.InstantiatePrefab(this.prefab, transform);
            instantiatedPrefab.transform.position = hit.point;

            float randomScale = Random.Range(minScale, maxScale);
            float randomRotation = Random.Range(0, 180);
            instantiatedPrefab.transform.Rotate(0, randomRotation, 0);
            instantiatedPrefab.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
        }
    }

    public void Clear()
    {
        while(transform.childCount != 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
    }
#endif
}
