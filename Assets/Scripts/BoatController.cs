using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatController : MonoBehaviour
{
    Rigidbody m_rigidbody;
    public float MIN_VEL = 0.1f;

    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {

    }

    public void MoveOneFrame()
    {
        m_rigidbody.AddForce(new Vector3(10, 0, 0), ForceMode.Impulse);
    }

    public bool IsMoving()
    {
        return m_rigidbody.velocity.x > MIN_VEL;
    }
}
