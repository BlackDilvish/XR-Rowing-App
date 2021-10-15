using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    public BoatController boat = null;
    private VideoPlayer m_player;

    void Start()
    {
        m_player = GetComponent<VideoPlayer>();
        RenderSettings.skybox = Resources.Load("SkyboxMaterials/VideoMat", typeof(Material)) as Material;
        //LoadVideoFromResources("Clip1");
        string url = "https://www.dropbox.com/s/o3dck6il7esxlh2/VIDEO_0365.mp4?dl=1";
        string path = "C:/C#/Unity/Images/Road/mov.mp4";
        StartCoroutine(DownloadVideo(url, path));
    }

    void Update()
    {
        if (VideoCanPlay())
        {
            ChangeVideoSpeed(boat.GetSpeedFactor());
            PlayVideo();
        }
        else if (m_player.isPlaying == true && boat.IsMoving() == false)
        {
            PauseVideo();
        }
    }

    public void PauseVideo()
    {
        m_player.Pause();
    }

    public void PlayVideo()
    {
        m_player.Play();
    }

    private void ChangeVideoSpeed(float speedFactor)
    {
        m_player.playbackSpeed = speedFactor;
    }

    private void LoadVideoFromUrl(string path)
    {
        m_player.source = VideoSource.Url;
        m_player.url = path;
        m_player.SetDirectAudioMute(0, true);
        m_player.Prepare();
        m_player.prepareCompleted += M_player_prepareCompleted;
        m_player.loopPointReached += M_player_loopPointReached;
    }

    private void LoadVideoFromResources(string clipName)
    {
        m_player.source = VideoSource.VideoClip;
        m_player.clip = Resources.Load($"Videos/{clipName}", typeof(VideoClip)) as VideoClip;
        m_player.SetDirectAudioMute(0, true);
        m_player.Prepare();
        m_player.prepareCompleted += M_player_prepareCompleted;
        m_player.loopPointReached += M_player_loopPointReached;
    }

    private void M_player_prepareCompleted(VideoPlayer source)
    {
        PlayVideo();
    }

    private void M_player_loopPointReached(VideoPlayer source)
    {
        PauseVideo();
        var finishedLevelMenu = FindObjectOfType<FinishedLevelMenu>(true);
        finishedLevelMenu.StopLevel();
    }

    private bool VideoCanPlay()
    {
        return m_player.isPlaying == false
            && boat.IsMoving() == true
            && PauseMenu.IsPaused == false
            && FinishedLevelMenu.IsStopped == false;
    }

    IEnumerator DownloadVideo(string url, string path)
    {
        var uwr = new UnityWebRequest(url);
        uwr.method = UnityWebRequest.kHttpVerbGET;
        var dh = new DownloadHandlerFile(path);
        dh.removeFileOnAbort = true;
        uwr.downloadHandler = dh;
        Debug.Log("Downloading...");
        yield return uwr.SendWebRequest();
        Debug.Log("Done");

        if (uwr.isNetworkError || uwr.isHttpError)
            Debug.Log(uwr.error);
        else
            Debug.Log("Download saved to: " + uwr.error);

        LoadVideoFromUrl(path);
    }
}
