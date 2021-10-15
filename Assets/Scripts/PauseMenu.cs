using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject m_playerPosition = null;
    [SerializeField] VideoController videoController = null;
    public static bool IsPaused = false;
    public Vector3 menuOffset = new Vector3(25, 5, 0);

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
        gameObject.transform.position = m_playerPosition.transform.position + menuOffset;
        Time.timeScale = 0f;
        videoController?.PauseVideo();
        IsPaused = true;
        InputManager.SwitchPointersState(true);
        gameObject.SetActive(true);
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        IsPaused = false;
        InputManager.SwitchPointersState(false);
        gameObject.SetActive(false);
    }

    public void LoadMainMenu()
    {
        Resume();
        SceneManager.LoadScene("MainMenu");
    }
}
