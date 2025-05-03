using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderBridge : MonoBehaviour
{
    SimulationManager _listener;
    public void Initialize(SimulationManager l)
    {
        _listener = l;
    }
    void OnCollisionEnter(Collision collision)
    {
        _listener.OnCollisionEnter(collision);
    }
    void OnTriggerEnter(Collider other)
    {

        Debug.Log("hi " + other.gameObject.name);
        _listener.OnTriggerEnter(other);
    }
}