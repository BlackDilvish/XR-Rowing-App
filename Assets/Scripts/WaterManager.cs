using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class WaterManager : MonoBehaviour
{
    private MeshFilter m_meshFilter;

    void Start()
    {
        m_meshFilter = GetComponent<MeshFilter>();
    }

    void Update()
    {
        Vector3[] vertices = m_meshFilter.mesh.vertices;
        for (int i = 0; i<vertices.Length; i++)
        {
            vertices[i].y = WaveManager.instance.GetWaveHeight(transform.position.x + vertices[i].x);
        }

        m_meshFilter.mesh.vertices = vertices;
        m_meshFilter.mesh.RecalculateNormals();
    }
}
