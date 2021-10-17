using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

public enum UsedVideoType
{
    Local,
    Downloaded
}

public class VideoManager : MonoBehaviour
{
    public Text downloadingInfo;

    public static UsedVideoType videoType;
    private static string videoName;
    private static string basePath = "C:/C#/Unity/Images/Road/";
    private static string videoDataPath = "Assets/Resources/Data/video_data.txt";
    private static Dictionary<string, string> videoData = new Dictionary<string, string>();

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        InitVideoData();
    }

    public static string GetCurrentVideoPath()
    {
        return basePath + videoName;
    }

    public void StartDownloadingVideo()
    {
        var selectedVideo = EventSystem.current.currentSelectedGameObject.transform.parent.gameObject.name;
        Debug.Log(selectedVideo);
        string url = videoData[selectedVideo];
        string path = basePath + selectedVideo + ".mp4";
        StartCoroutine(DownloadVideo(url, path));
    }

    public void SetCurrentVideo()
    {
        var selectedVideo = EventSystem.current.currentSelectedGameObject.transform.parent.gameObject.name;
        videoType = selectedVideo == "Clip1" ? UsedVideoType.Local : UsedVideoType.Downloaded;
        videoName = selectedVideo + ".mp4";
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

        if (uwr.result == UnityWebRequest.Result.ConnectionError || uwr.result == UnityWebRequest.Result.ProtocolError)
            Debug.Log(uwr.error);
        else
            Debug.Log("Download saved to: " + uwr.error);

        downloadingInfo.gameObject.SetActive(false);
    }

    private void InitVideoData()
    {
        foreach (string line in File.ReadAllLines(videoDataPath))
        {
            string[] splitted = line.Split();
            string videoName = splitted[0];
            string videoUrl = splitted[1];
            videoData.Add(videoName, videoUrl);
        }
    }
}
