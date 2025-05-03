using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BpNapthaleneColliderBridge : MonoBehaviour
{
    BpNapthalene_SimulationManager _listener;
    public void Initialize(BpNapthalene_SimulationManager l)
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
