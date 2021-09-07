using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void OpenVideoMode()
    {
        SceneManager.LoadScene("Video360VRScene");
    }

    public void OpenFramesMode()
    {
        SceneManager.LoadScene("Frames360VRScene");
    }

    public void OpenCallibration()
    {
        SceneManager.LoadScene("Callibration");
    }
}
