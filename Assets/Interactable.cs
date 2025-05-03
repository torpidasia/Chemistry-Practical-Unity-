using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    private Vector3 startingPosition;
    public float maxHeight = 2f; // Maximum height above the starting Y position

    void Start()
    {
        startingPosition = transform.position;
    }

    void FixedUpdate()
    {
        if (transform.hasChanged)
        {
            Vector3 currentPosition = transform.position;

            // Lock Z-axis movement
            currentPosition.z = startingPosition.z;

            // Restrict Y-axis movement
            currentPosition.y = Mathf.Clamp(currentPosition.y, startingPosition.y, startingPosition.y + maxHeight);

            transform.position = currentPosition;
            transform.hasChanged = false;
        }
    }
}
