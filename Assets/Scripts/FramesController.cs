using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FramesController : MonoBehaviour
{
    public BoatController boat = null;
    public float distanceTravelledOffset = 20f;
    private Vector3 nextFramePosition = new Vector3();
    private List<Material> m_frameMaterials = new List<Material>();
    private int m_currentFrame = 0;

    void Start()
    {
        m_frameMaterials.Add(Resources.Load("SkyboxMaterials/SkyMat1", typeof(Material)) as Material);
        m_frameMaterials.Add(Resources.Load("SkyboxMaterials/SkyMat2", typeof(Material)) as Material);

        RenderSettings.skybox = m_frameMaterials[m_currentFrame++];
        nextFramePosition = boat.transform.position + Vector3.right * distanceTravelledOffset;
    }

    void Update()
    {
        if (Vector3.Distance(boat.transform.position, nextFramePosition) < 0.5f)
        {
            Debug.Log("Teees");
            nextFramePosition = boat.transform.position + Vector3.right * distanceTravelledOffset;
            RenderNextFrame();
        }
    }

    public void RenderNextFrame()
    {
        if(m_currentFrame == m_frameMaterials.Count)
        {
            m_currentFrame = 0;
        }

        Material nextFrame = m_frameMaterials[m_currentFrame];
        m_currentFrame += 1;
        RenderSettings.skybox = nextFrame;
    }

    public void SetFirstFrame()
    {
        SetFrame(0);
    }

    private void SetFrame(int frameNumber)
    {
        if (frameNumber < m_frameMaterials.Count)
        {
            RenderSettings.skybox = m_frameMaterials[frameNumber];
        }
        else
        {
            Debug.Log($"Frame number {frameNumber} is bigger than count of stored materials");
        }
    }
}
