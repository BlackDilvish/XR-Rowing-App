using System.Collections;
using System.Collections.Generic;
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

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    public static string GetCurrentVideoPath()
    {
        return basePath + videoName;
    }

    public void StartDownloadingVideo()
    {
        var optionName = EventSystem.current.currentSelectedGameObject.transform.parent.gameObject.name;
        Debug.Log(optionName);
        string url = "https://www.dropbox.com/s/o3dck6il7esxlh2/VIDEO_0365.mp4?dl=1";
        string videoName = optionName + ".mp4";
        string path = basePath + videoName;
        StartCoroutine(DownloadVideo(url, path));
    }

    public void SetCurrentVideo()
    {
        var optionName = EventSystem.current.currentSelectedGameObject.transform.parent.gameObject.name;
        videoType = optionName == "Clip1" ? UsedVideoType.Local : UsedVideoType.Downloaded;
        videoName = optionName + ".mp4";
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
}
