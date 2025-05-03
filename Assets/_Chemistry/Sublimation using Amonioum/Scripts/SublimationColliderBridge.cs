using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SublimationColliderBridge : MonoBehaviour
{
    Sublimation_SimulationManager _listener;
    public void Initialize(Sublimation_SimulationManager l)
    {
        _listener = l;
    }
    void OnCollisionEnter(Collision collision)
    {
        _listener.OnCollisionEnter(collision);
    }
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("hi");
        _listener.OnTriggerEnter(other);
    }
}
