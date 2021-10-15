using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishedLevelMenu : MonoBehaviour
{
    [SerializeField] GameObject m_playerPosition = null;
    public static bool IsStopped = false;
    public Vector3 menuOffset = new Vector3(25, 5, 0);

    public void StopLevel()
    {
        gameObject.transform.position = m_playerPosition.transform.position + menuOffset;
        Time.timeScale = 0f;
        IsStopped = true;
        InputManager.SwitchPointersState(true);
        gameObject.SetActive(true);
    }

    public void RestartLevel()
    {
        ResetStopValues();
        InputManager.SwitchPointersState(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadMainMenu()
    {
        ResetStopValues();
        InputManager.SwitchPointersState(true);
        SceneManager.LoadScene("MainMenu");
    }

    private void ResetStopValues()
    {
        Time.timeScale = 1f;
        IsStopped = false;
        gameObject.SetActive(false);
    }
}
