using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class InputManager : MonoBehaviour
{
    [SerializeField] private Text m_targetText = null;
    [SerializeField] private InputActionReference m_HMDPositionReference = null;
    [SerializeField] private InputActionReference m_HMDRotationReference = null;
    [SerializeField] private InputActionReference m_controllerMenuButton = null;

    public PauseMenu pauseMenu;

    void Start()
    {
        if (m_HMDPositionReference != null)
        {
            m_HMDPositionReference.action.Enable();
        }

        if (m_HMDRotationReference != null)
        {
            m_HMDRotationReference.action.Enable();
        }

        if (m_controllerMenuButton != null)
        {
            m_controllerMenuButton.action.Enable();
            m_controllerMenuButton.action.performed += (ctx) =>
            {
                if (!FinishedLevelMenu.IsStopped)
                    pauseMenu?.ChangeState();
            };
        }
    }

    public Vector3 GetHMDPositionVector()
    {
        if (m_HMDPositionReference != null && m_HMDPositionReference.action != null)
        {
            return m_HMDPositionReference.action.ReadValue<Vector3>();
        }

        return new Vector3();
    }

    public static void SwitchPointersState(bool enable)
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        var interactorLines = player.GetComponentsInChildren<UnityEngine.XR.Interaction.Toolkit.XRInteractorLineVisual>();
        foreach (var interactorLine in interactorLines)
        {
            interactorLine.enabled = enable;
        }
    }

    private void PrintHMDVectors()
    {
        if (m_HMDPositionReference != null && m_HMDPositionReference.action != null)
        {
            Vector3 value = m_HMDPositionReference.action.ReadValue<Vector3>();
            m_targetText.text = $"Value: {value}";
        }

        if (m_HMDRotationReference != null && m_HMDRotationReference.action != null)
        {
            Quaternion value = m_HMDRotationReference.action.ReadValue<Quaternion>();
            m_targetText.text += $" {value}";
        }
    }
}
