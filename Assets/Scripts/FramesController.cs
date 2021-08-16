using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FramesController : MonoBehaviour
{
    public BoatController boat = null;
    private List<Material> m_frameMaterials = new List<Material>();
    private int m_currentFrame = 0;
    private bool m_nextFramePrepared = false;

    void Start()
    {
        m_frameMaterials.Add(Resources.Load("SkyboxMaterials/SkyMat1", typeof(Material)) as Material);
        m_frameMaterials.Add(Resources.Load("SkyboxMaterials/SkyMat2", typeof(Material)) as Material);

        RenderSettings.skybox = m_frameMaterials[m_currentFrame++];
    }

    void Update()
    {
        if (boat.IsMoving() == true)
        {
            m_nextFramePrepared = true;
        }
        else if (boat.IsMoving() == false && m_nextFramePrepared == true)
        {
            RenderNextFrame();
            m_nextFramePrepared = false;
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
