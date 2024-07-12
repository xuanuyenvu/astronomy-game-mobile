using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketController : AstronomicalObject
{    
    public void RotateRocket(GameObject planet)
    {
        // Tính vector từ rocket tới planet
        Vector3 direction = planet.transform.position - transform.position;

        // Tính góc quay để hướng rocket về phía planet
        Quaternion rotation = Quaternion.LookRotation(direction);

        // Xoay rocket theo hướng của vector
        Debug.Log(rotation);
        Debug.Log("rotation1 " + transform.rotation);
        transform.rotation = rotation;
        Debug.Log("rotation2 " + transform.rotation);
    }

    public IEnumerator FlyTo(GameObject planet, float time = 2f)
    {
        Vector3 startingPos = transform.position;
        Vector3 finalPos = planet.transform.position;
        RotateRocket(planet);
        for (float t = 0f; t <= time; t += Time.deltaTime / time)
        {
            transform.position = Vector3.Lerp(startingPos, finalPos, t);

            yield return null;
        }
    }
}
