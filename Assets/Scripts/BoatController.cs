using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoatController : MonoBehaviour
{
    public InputManager inputManager;
    public Image gaugeBarTop;
    public Image gaugeBarBottom;
    public Text HUDText;

    private Rigidbody m_rigidbody;
    private bool m_moveBackReady = false;

    private float MIN_VEL = 0.1f;
    private float MIN_BACK_FORCE = 0.2f;
    private float MIN_FORWARD_FORCE = 0.3f;

    private float maxBackForce = 0f;
    private float maxForwardForce = 0f;
    private const float baseSpeed = 20f;

    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        MIN_BACK_FORCE = MovementOptions.minimalBackwardDistance;
        MIN_FORWARD_FORCE = MovementOptions.minimalForwardDistance;
    }

    void Update()
    {
        if (PauseMenu.IsPaused == false)
        {
            Vector3 hmdPosition = inputManager.GetHMDPositionVector();

            UpdateGaugeBars(hmdPosition.z);
            UpdateMove(hmdPosition.z);
            RotatePaddles(hmdPosition.z);
            UpdateHUDInfo();
        }
    }

    public void MoveOneFrame()
    {
        m_rigidbody.AddForce(new Vector3(baseSpeed * GetSpeedFactor(), 0, 0), ForceMode.Impulse);
    }

    public bool IsMoving()
    {
        return m_rigidbody.velocity.x > MIN_VEL;
    }

    public float GetSpeedFactor()
    {
        return (Mathf.Abs(maxBackForce) + maxForwardForce) / (MIN_BACK_FORCE + MIN_FORWARD_FORCE);
    }

    public void UpdateMove(float positionValue)
    {
        if (positionValue <= -MIN_BACK_FORCE)
        {
            if (m_moveBackReady == false)
            {
                m_moveBackReady = true;
            }

            maxBackForce = Mathf.Min(maxBackForce, positionValue);
        }

        if (m_moveBackReady == true && positionValue >= MIN_FORWARD_FORCE)
        {
            maxForwardForce = positionValue;
            MoveOneFrame();
            m_moveBackReady = false;
        }
    }

    private void RotatePaddles(float positionValue)
    {
        Transform rightPaddle = transform.GetChild(1);
        Transform leftPaddle = transform.GetChild(2);
        float rotationFactor = positionValue * 100f;

        rightPaddle.rotation = Quaternion.Euler(-rotationFactor, rightPaddle.rotation.eulerAngles.y, rightPaddle.rotation.eulerAngles.z);
        leftPaddle.rotation = Quaternion.Euler(rotationFactor, leftPaddle.rotation.eulerAngles.y, leftPaddle.rotation.eulerAngles.z);
    }

    private void UpdateHUDInfo()
    {
        HUDText.text = $"Predkosc lodzi: {m_rigidbody.velocity.x}\n";
        HUDText.text += $"Pokonana odleglosc: {m_rigidbody.position.x}\n";
    }

    private void UpdateGaugeBars(float positionValue)
    {
        ResizeGaugeBars(positionValue);
        ColorGaugeBars(positionValue);
    }

    private void ResizeGaugeBars(float positionValue)
    {
        if (positionValue > 0)
        {
            gaugeBarTop.fillAmount = positionValue;
        }
        else
        {
            gaugeBarBottom.fillAmount = -positionValue;
        }
    }

    private void ColorGaugeBars(float positionValue)
    {
        if (positionValue > MIN_FORWARD_FORCE*2)
        {
            gaugeBarTop.color = Color.red;
        }
        else if (positionValue > MIN_FORWARD_FORCE && positionValue < MIN_FORWARD_FORCE * 2)
        {
            gaugeBarTop.color = Color.yellow;
        }
        else if (positionValue < MIN_FORWARD_FORCE && positionValue > -MIN_BACK_FORCE)
        {
            gaugeBarTop.color = Color.green;
            gaugeBarBottom.color = Color.green;
        }
        else if (positionValue < -MIN_BACK_FORCE && positionValue > -MIN_BACK_FORCE * 2)
        {
            gaugeBarBottom.color = Color.yellow;
        }
        if (positionValue < -MIN_BACK_FORCE * 2)
        {
            gaugeBarBottom.color = Color.red;
        }
    }
}
