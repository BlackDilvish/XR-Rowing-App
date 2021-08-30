using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoatController : MonoBehaviour
{
    public InputManager inputManager;
    public Image gaugeBarTop;
    public Image gaugeBarBottom;

    private Rigidbody m_rigidbody;
    private bool m_moveBackReady = false;

    [SerializeField] private float MIN_VEL = 0.1f;
    [SerializeField] private float MIN_BACK_FORCE = 1f;
    [SerializeField] private float MIN_FORWARD_FORCE = 1f;

    /// <summary>
    public Vector3 mockVector = new Vector3();
    /// </summary>

    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Vector3 hmdPosition = mockVector;// inputManager.GetHMDPositionVector();

        UpdateGaugeBars(hmdPosition.x);
        UpdateMove(hmdPosition.x);
        ApplyRotation();
    }

    public void MoveOneFrame()
    {
        m_rigidbody.AddForce(new Vector3(10, 0, 0), ForceMode.Impulse);
    }

    public bool IsMoving()
    {
        return m_rigidbody.velocity.x > MIN_VEL;
    }

    public void UpdateMove(float positionValue)
    {
        if (m_moveBackReady == false && positionValue < -MIN_BACK_FORCE)
        {
            m_moveBackReady = true;
        }

        if (m_moveBackReady == true && positionValue > MIN_FORWARD_FORCE)
        {
            MoveOneFrame();
            m_moveBackReady = false;
        }
    }

    private void ApplyRotation()
    {
        transform.Rotate(Mathf.Sin(Time.time) / 180, 0, 0);
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
        if (Mathf.Abs(positionValue) < MIN_BACK_FORCE)
        {
            gaugeBarTop.color = Color.green;
            gaugeBarBottom.color = Color.green;
        }
        else if (Mathf.Abs(positionValue) < MIN_FORWARD_FORCE)
        {
            gaugeBarTop.color = Color.yellow;
            gaugeBarBottom.color = Color.yellow;
        }
        else
        {
            gaugeBarTop.color = Color.red;
            gaugeBarBottom.color = Color.red;
        }
    }
}