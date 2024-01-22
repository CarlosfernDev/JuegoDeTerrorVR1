using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopPhisicsOnExit : MonoBehaviour
{
    public Rigidbody _rb;

    public void StopPhisics()
    {
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
    }
}