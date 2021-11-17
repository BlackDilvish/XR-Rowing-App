using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject m_playerPosition = null;
    [SerializeField] private VideoController videoController = null;
    public static bool IsPaused = false;
    private Vector3 m_menuOffset = new Vector3(25, 5, 0);

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
        gameObject.transform.position = m_playerPosition.transform.position + m_menuOffset;
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
