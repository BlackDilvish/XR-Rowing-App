using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class InputManager : MonoBehaviour
{
    public Text m_targetText;
    public InputActionReference m_HMDPositionReference = null;
    public InputActionReference m_HMDRotationReference = null;

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
    }

    void Update()
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
