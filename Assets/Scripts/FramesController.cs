using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;

public class FramesController : MonoBehaviour
{
    public BoatController boat = null;
    public float distanceTravelledOffset = 20f;
    private Vector3 nextFramePosition = new Vector3();
    private List<Material> m_frameMaterials = new List<Material>();
    private int m_currentFrame = 0;

    void Start()
    {
        for (int i = 1; i < 15; i++)
        {
            m_frameMaterials.Add(Resources.Load($"SkyboxMaterials/Road/Road {i}", typeof(Material)) as Material);
        }
        //StartCoroutine(LoadImageFromStorage());
        RenderSettings.skybox = m_frameMaterials[m_currentFrame++];
        nextFramePosition = boat.transform.position + Vector3.right * distanceTravelledOffset;
    }

    void Update()
    {
        if (Vector3.Distance(boat.transform.position, nextFramePosition) < 0.5f)
        {
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

    private IEnumerator LoadImageFromStorage()
    {
        string path = @"C:/C%23/Unity/Images/Road/1.jpg";
        Debug.Log(new FileInfo(path).Exists);
        UnityWebRequest www = UnityWebRequestTexture.GetTexture("file:///" + path);
        Debug.Log(www.result);
        yield return www.SendWebRequest();

        Debug.Log(www.result);
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(www.error);
        }
        else
        {
            Texture tex = ((DownloadHandlerTexture)www.downloadHandler).texture;
            AssetDatabase.CreateAsset(tex, "Assets/test.asset");
            AssetDatabase.SaveAssets();
            TextureImporter importer = (TextureImporter)TextureImporter.GetAtPath("Assets/test.asset");
            if (tex.dimension != UnityEngine.Rendering.TextureDimension.Cube)
            {
                importer.textureShape = TextureImporterShape.TextureCube;
                importer.SaveAndReimport();
            }
            Material material = new Material(Shader.Find("Skybox/Cubemap"));
            material.mainTexture = tex;
            AssetDatabase.CreateAsset(material, "Assets/test.mat");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
