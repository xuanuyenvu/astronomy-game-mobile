using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationController : MonoBehaviour
{
    public float tiltAngle = 15f; 
    public float rotationSpeed = 12f; 
    
    void Start()
    {
        // Tilt the planet to the left by 15 degrees around the Z-axis
        transform.Rotate(Vector3.forward, tiltAngle);
    }

    void Update()
    {
        Vector3 tiltedAxis = transform.up; 
        // Create a quaternion representing the new rotation around the tilted axis
        Quaternion rotation = Quaternion.AngleAxis(rotationSpeed * Time.deltaTime, tiltedAxis); 
        transform.rotation = rotation * transform.rotation;
    }
}
