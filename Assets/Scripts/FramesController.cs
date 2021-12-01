using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;

public class FramesController : MonoBehaviour
{
    [SerializeField] private BoatController boat = null;
    private Vector3 nextFramePosition = new Vector3();
    private List<Material> m_frameMaterials = new List<Material>();
    private int m_currentFrame = 0;
    private const float distanceTravelledOffset = 20f;
    private const float m_minimalCloseDistance = 0.5f;

    void Start()
    {
        for (int i = 1; i < 15; i++)
        {
            m_frameMaterials.Add(Resources.Load($"SkyboxMaterials/DefaultFrames/River {i}", typeof(Material)) as Material);
        }
        RenderSettings.skybox = m_frameMaterials[m_currentFrame];
        nextFramePosition = boat.transform.position + Vector3.right * distanceTravelledOffset;
        InputManager.SwitchPointersState(false);
    }

    void Update()
    {
        if (Vector3.Distance(boat.transform.position, nextFramePosition) < m_minimalCloseDistance)
        {
            nextFramePosition = boat.transform.position + Vector3.right * distanceTravelledOffset;
            RenderNextFrame();
        }
    }

    public void RenderNextFrame()
    {
        m_currentFrame += 1;
        if (m_currentFrame < m_frameMaterials.Count)
        {
            Material nextFrame = m_frameMaterials[m_currentFrame];
            RenderSettings.skybox = nextFrame;
        }
        else
        {
            var finishedLevelMenu = FindObjectOfType<FinishedLevelMenu>(true);
            finishedLevelMenu.StopLevel();
        }
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
