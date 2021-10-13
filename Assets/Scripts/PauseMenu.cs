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
        SwitchPointersState();
    }

    public void Pause()
    {
        gameObject.transform.position = m_playerPosition.transform.position + menuOffset;
        Time.timeScale = 0f;
        videoController?.PauseVideo();
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
        Resume();
        SceneManager.LoadScene("MainMenu");
    }

    private void SwitchPointersState()
    {
        var interactorLines = m_playerPosition.GetComponentsInChildren<UnityEngine.XR.Interaction.Toolkit.XRInteractorLineVisual>();
        foreach(var interactorLine in interactorLines)
        {
            interactorLine.enabled = !interactorLine.enabled;
        }
    }
}
