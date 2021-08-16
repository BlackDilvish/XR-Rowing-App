using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatController : MonoBehaviour
{
    Rigidbody m_rigidbody;
    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        
    }

    public void MoveOneFrame()
    {
        Debug.Log("test");
        m_rigidbody.AddForce(new Vector3(0, 0, -10), ForceMode.Impulse);
    }
}
