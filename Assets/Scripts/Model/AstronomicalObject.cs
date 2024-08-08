using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstronomicalObject : MonoBehaviour
{
    static public double G = 6.67384e-11;
    public string Name;
    public double Mass;

    public float scaleFactor = 1;
    protected float scaleDownFactor = 1000; // 1unit: m -> km

    public float Distance(AstronomicalObject obj)
    {
        return Vector3.Distance(transform.position, obj.transform.position) * scaleDownFactor;
    }

    public double GetAttractiveForce(AstronomicalObject obj)
    {
        return (G * this.Mass * obj.Mass) / Mathf.Pow((Distance(obj) * 1000), 2f);
    }

    public Vector3 GetVectorAttractiveForce(AstronomicalObject obj)
    {
        Vector3 direction = obj.transform.position - this.transform.position;
        float distance = direction.magnitude;
        if (distance != 0)
        {
            float gravitationalForce = (float)(G * obj.Mass * this.Mass) / (distance * distance);
            return direction.normalized * gravitationalForce;
        }
        return Vector3.zero;
    }
}
