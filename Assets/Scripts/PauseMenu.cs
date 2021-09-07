using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject m_playerPosition;
    [SerializeField] VideoController videoController;
    public static bool IsPaused = false;
    void Update()
    {
        
    }

    public void ChangeState()
    {
        if (gameObject.activeInHierarchy)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    public void Pause()
    {
        gameObject.transform.position = m_playerPosition.transform.position + Vector3.right * 20f;
        Time.timeScale = 0f;
        videoController.PauseVideo();
        IsPaused = true;
        gameObject.SetActive(true);
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        IsPaused = false;
        gameObject.SetActive(false);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
