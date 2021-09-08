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
        LoadVideoFromUrl("C:/C#/Unity/Images/VIDEO_0353.mp4");
    }

    void Update()
    {
        if (m_player.isPlaying == false && boat.IsMoving() == true && PauseMenu.IsPaused == false)
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
    }

    private void M_player_prepareCompleted(VideoPlayer source)
    {
        PlayVideo();
    }
}
