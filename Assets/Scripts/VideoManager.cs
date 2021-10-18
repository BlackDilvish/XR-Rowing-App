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
    private static string basePath;
    private static string videoDataPath = "Assets/Resources/Data/video_data.txt";
    private static Dictionary<string, string> videoData = new Dictionary<string, string>();

    private static string BUILDIN_VIDEO_NAME = "Clip1";

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        InitVideoData();
        InstantiateVideoOptions();
        basePath = Application.persistentDataPath + "/";
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
        DisableOptionButtons();
        StartCoroutine(DownloadVideo(url, path));
    }

    public void SetCurrentVideo()
    {
        var selectedVideo = EventSystem.current.currentSelectedGameObject.transform.parent;
        var selectedVideoText = selectedVideo.GetChild(0).GetComponent<Text>();
        if (File.Exists(basePath + selectedVideoText.text + ".mp4") || selectedVideoText.text.Equals(BUILDIN_VIDEO_NAME))
        {
            GameObject.Find(currentVideoName).transform.GetChild(0).GetComponent<Text>().color = Color.black;
            selectedVideoText.color = Color.red;
            videoType = selectedVideoText.text == BUILDIN_VIDEO_NAME ? UsedVideoType.Local : UsedVideoType.Downloaded;
            currentVideoName = selectedVideoText.text;
            Debug.Log("Current video is: " + currentVideoName);
        }
        else
        {
            Debug.LogError("This video has not been downloaded");
        }
    }

    IEnumerator DownloadVideo(string url, string path)
    {
        var handler = new DownloadHandlerFile(path);
        handler.removeFileOnAbort = true;
        var webRequest = new UnityWebRequest(url);
        webRequest.method = UnityWebRequest.kHttpVerbGET;
        webRequest.downloadHandler = handler;
        Debug.Log("Downloading...");
        yield return webRequest.SendWebRequest();
        Debug.Log("Done");

        if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            Debug.Log(webRequest.error);
        else
            Debug.Log("Download saved to: " + path);

        EnableOptionButtons();
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

    private void DisableOptionButtons()
    {
        downloadingInfo.gameObject.SetActive(true);
        foreach (var button in GameObject.Find("ScrollableArea").GetComponentsInChildren<Button>())
        {
            button.enabled = false;
        }
    }

    private void EnableOptionButtons()
    {
        downloadingInfo.gameObject.SetActive(false);
        foreach (var button in GameObject.Find("ScrollableArea").GetComponentsInChildren<Button>())
        {
            button.enabled = true;
        }
    }
}
