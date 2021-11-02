using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class WaterManager : MonoBehaviour
{
    private MeshFilter m_meshFilter;

    public float amplitude = 1f;
    public float length = 2f;
    public float speed = 1f;
    public float offset = 0f;

    void Start()
    {
        m_meshFilter = GetComponent<MeshFilter>();
    }

    void Update()
    {
        offset += Time.deltaTime + speed;

        Vector3[] vertices = m_meshFilter.mesh.vertices;
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i].y = GetWaveHeight(vertices[i].x);
        }

        m_meshFilter.mesh.vertices = vertices;
        m_meshFilter.mesh.RecalculateNormals();
    }

    public float GetWaveHeight(float x)
    {
        return amplitude * Mathf.Sin(x / length + offset);
    }
}
