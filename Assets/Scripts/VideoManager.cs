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

    public static UsedVideoType videoType = UsedVideoType.Local;
    private static string currentVideoName = "Clip1";
    private static string basePath = "C:/C#/Unity/Images/Road/";
    private static string videoDataPath = "Assets/Resources/Data/video_data.txt";
    private static Dictionary<string, string> videoData = new Dictionary<string, string>();

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        InitVideoData();
        InstantiateVideoOptions();
    }

    public static string GetCurrentVideoPath()
    {
        return basePath + currentVideoName + ".mp4";
    }

    public void StartDownloadingVideo()
    {
        var selectedVideo = EventSystem.current.currentSelectedGameObject.transform.parent.gameObject.name;
        Debug.Log(selectedVideo);
        string url = videoData[selectedVideo];
        string path = basePath + selectedVideo + ".mp4";
        downloadingInfo.gameObject.SetActive(true);
        StartCoroutine(DownloadVideo(url, path));
    }

    public void SetCurrentVideo()
    {
        var selectedVideo = EventSystem.current.currentSelectedGameObject.transform.parent;
        var selectedVideoText = selectedVideo.GetChild(0).GetComponent<Text>();
        GameObject.Find(currentVideoName).transform.GetChild(0).GetComponent<Text>().color = Color.black;
        selectedVideoText.color = Color.red;
        Debug.Log(selectedVideoText.text);
        videoType = selectedVideoText.text == "Clip1" ? UsedVideoType.Local : UsedVideoType.Downloaded;
        currentVideoName = selectedVideoText.text;
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
            Debug.Log("Download saved to: " + path);

        downloadingInfo.gameObject.SetActive(false);
    }

    private void InitVideoData()
    {
        foreach (string line in File.ReadAllLines(videoDataPath))
        {
            string[] splitted = line.Split();
            string videoName = splitted[0];
            string videoUrl = splitted[1];
            if (!videoData.ContainsKey(videoName))
            {
                videoData.Add(videoName, videoUrl);
            }
        }
    }

    private void InstantiateVideoOptions()
    {
        Vector2 position = new Vector2(0, 50);
        var canvas = GameObject.Find("ScrollableArea");
        var videoOption = Resources.Load("RuntimePrefabs/ClipOption", typeof(GameObject)) as GameObject;

        foreach (string line in File.ReadAllLines(videoDataPath))
        {
            string[] splitted = line.Split();
            string videoName = splitted[0];
            
            GameObject newOption = Instantiate(videoOption, canvas.transform);
            newOption.name = videoName;
            newOption.transform.GetChild(0).GetComponent<Text>().text = videoName;
            newOption.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(StartDownloadingVideo);
            newOption.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(SetCurrentVideo);
            newOption.transform.SetParent(canvas.transform);
            newOption.GetComponent<RectTransform>().anchoredPosition = position;

            position.y -= 30;
        }

        GameObject.Find("LibraryCanvas").SetActive(false);
    }
}
