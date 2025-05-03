using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sulphateColliderBridge : MonoBehaviour
{
    sulphate_SimulationManager _listener;
    public void Initialize(sulphate_SimulationManager l)
    {
        _listener = l;
    }
    void OnCollisionEnter(Collision collision)
    {
        _listener.OnCollisionEnter(collision);
    }
    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("hi");
        _listener.OnTriggerEnter(other);
    }
}
