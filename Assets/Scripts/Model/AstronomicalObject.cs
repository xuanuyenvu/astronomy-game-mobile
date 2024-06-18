using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstronomicalObject : MonoBehaviour
{
    static public double G = 6.67384e-11;
    public string Name;
    public double Mass;

    public float scaleFactor = 1;
    private float scaleDownFactor = 1000; // 1unit: m -> km

    public float Distance(AstronomicalObject obj)
    {
        return Vector3.Distance(transform.position, obj.transform.position) * scaleDownFactor; 
    }

    public double GetAttractiveForce(AstronomicalObject obj)
    {
        return (G * this.Mass * obj.Mass) / Mathf.Pow((Distance(obj)*1000),2f);
    }
}
