using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

struct MoveData
{
    public bool preparedForMove;
    public bool move;
    public Vector3 nextPosition;
}

public class PlayerController : MonoBehaviour
{
    public Text accelerationInfo;
    public Image gaugeBarTop;
    public Image gaugeBarBottom;
    public GameObject framesManager;
    public Vector3 mockAcc = new Vector3(0, 0, 0); //for computer debug purposes
    private Rigidbody m_body;
    [SerializeField] private float m_minimalPrepareForce = 0.2f;
    [SerializeField] private float m_minimalMoveForce = 0.6f;
    private MoveData m_moveData = new MoveData();
    private const float M_STEP = 10.0f;
    private const float M_ESPILON = 0.1f;

    void Start()
    {
        m_body = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Vector3 accelerationVector = Input.acceleration;
        float accelerationMoveAxisValue = accelerationVector.z;

        FillGaugeBars(accelerationMoveAxisValue);

        accelerationInfo.text = GetAccelerationInfoString(accelerationVector);
        UpdateMove(accelerationMoveAxisValue);
    }

    public void Reset()
    {
        this.transform.position = new Vector3(0, 1, 0);
        this.m_body.velocity = new Vector3(0, 0, 0);
        m_moveData.move = false;
        m_moveData.preparedForMove = false;
        framesManager.GetComponent<FramesController>().SetFirstFrame();
    }

    private string GetAccelerationInfoString(Vector3 acceleration)
    {
        string x = acceleration.x.ToString();
        string y = acceleration.y.ToString();
        string z = acceleration.z.ToString();

        return $"Acceleration: x:{x.Substring(0, 4)}, y:{y.Substring(0, 4)}, z:{z.Substring(0, 4)}";
    }

    private void FillGaugeBars(float accelerationValue)
    {
        ResizeGaugeBars(accelerationValue);
        ColorGaugeBars(accelerationValue);
    }

    private void ResizeGaugeBars(float accelerationValue)
    {
        if (accelerationValue > 0)
        {
            gaugeBarTop.fillAmount = accelerationValue;
        }
        else
        {
            gaugeBarBottom.fillAmount = -accelerationValue;
        }
    }

    private void ColorGaugeBars(float accelerationValue)
    {
        if (Mathf.Abs(accelerationValue) < m_minimalPrepareForce)
        {
            gaugeBarTop.color = Color.green;
            gaugeBarBottom.color = Color.green;
        }
        else if (Mathf.Abs(accelerationValue) < m_minimalMoveForce)
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

    private void UpdateMove(float accelerationValue)
    {
        if (IsMoving())
        {
            ApplyMove();
        }
        else
        {
            PrepareMove(accelerationValue);
        }
    }

    private void ApplyMove()
    {
        transform.position = Vector3.Lerp(transform.position, m_moveData.nextPosition, Time.deltaTime);

        if (Vector3.Distance(transform.position, m_moveData.nextPosition) < M_ESPILON)
        {
            m_moveData.move = false;
            framesManager.GetComponent<FramesController>().RenderNextFrame();
        }
    }

    private void PrepareMove(float accelerationValue)
    {
        if (accelerationValue > m_minimalPrepareForce && m_moveData.preparedForMove == false)
        {
            m_moveData.preparedForMove = true;
        }

        if (accelerationValue < -m_minimalMoveForce && m_moveData.preparedForMove == true)
        {
            m_moveData.preparedForMove = false;
            m_moveData.move = true;
            m_moveData.nextPosition = transform.position - Vector3.forward * M_STEP;
        }
    }

    private bool IsMoving()
    {
        return m_moveData.move;
    }
}
