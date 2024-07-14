using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketController : AstronomicalObject
{    
    private CameraShake cameraShake;
    private bool turnOnCollider = false;

    public global::System.Boolean TurnOnCollider { get => turnOnCollider; set => turnOnCollider = value; }

    private Collider objectCollider;
    void Start()
    {
        if (cameraShake == null)
        {
            cameraShake = FindObjectOfType<CameraShake>();
        }
        objectCollider = GetComponent<Collider>();
    }

    void Update()
    {
        if (turnOnCollider)
        {
            objectCollider.enabled = true;
        }
        else
        {
            objectCollider.enabled = false;
        }
    }
    
    public void RotateRocket(GameObject planet)
    {
        // Tính vector từ rocket tới planet
        Vector3 direction = planet.transform.position - transform.position;

        // Tính góc quay để hướng rocket về phía planet
        Quaternion rotation = Quaternion.LookRotation(direction);

        // Xoay rocket theo hướng của vector
        transform.rotation = rotation;
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

    private void OnTriggerEnter(Collider planet)
    {
        // Kiểm tra xem đối tượng nào đã vào vùng trigger
        if (planet.gameObject.CompareTag("Planet"))
        {
            Debug.Log("Đã vào vùng trigger của " + planet.gameObject.name);
            cameraShake.ShakeCamera();
        }
    }
}
