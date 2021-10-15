using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    public BoatController boat = null;
    private VideoPlayer m_player;

    void Start()
    {
        m_player = GetComponent<VideoPlayer>();
        RenderSettings.skybox = Resources.Load("SkyboxMaterials/VideoMat", typeof(Material)) as Material;
        LoadVideo();
        InputManager.SwitchPointersState(false);
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

    private void LoadVideo()
    {
        if (VideoManager.videoType == UsedVideoType.Local)
        {
            LoadVideoFromResources("Clip1");
        }
        else
        {
            LoadVideoFromUrl(VideoManager.GetCurrentVideoPath());
        }
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
}
