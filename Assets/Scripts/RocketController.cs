using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketController : AstronomicalObject
{
    private PlanetSelectionSpawner planetSelectionSpawner;
    private bool turnOnCollider = false;

    private Collider rocketCollider;
    public global::System.Boolean TurnOnCollider { get => turnOnCollider; set => turnOnCollider = value; }
    void Start()
    {
        if (planetSelectionSpawner == null)
        {
            planetSelectionSpawner = FindObjectOfType<PlanetSelectionSpawner>();
        }
        rocketCollider = GetComponent<Collider>();
    }

    void Update()
    {
        if (turnOnCollider)
        {
            rocketCollider.enabled = true;
        }
        else
        {
            rocketCollider.enabled = false;
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

    public IEnumerator FlyTo(GameObject planet)
    {
        Vector3 startingPos = transform.position;
        Vector3 finalPos = planet.transform.position;
        RotateRocket(planet);

        // Tính chiều dài quãng đường
        float distance = Vector3.Distance(startingPos, finalPos);
        // Đường bay càng xa thì thời gian càng chậm
        // Ví dụ: bay 1km --> 2s, thì 2km phải bay lâu hơn
        float time = distance / 6f;

        for (float t = 0f; t <= 1f; t += Time.deltaTime / time)
        {
            if (this == null) 
                yield break; // Dừng coroutine nếu đối tượng đã bị hủy bở hàm OnTriggerEnter

            transform.position = Vector3.Lerp(startingPos, finalPos, t);
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider planet)
    {
        if (planet.gameObject.CompareTag("Planet"))
        {
            planet.gameObject.SetActive(false);
            StartCoroutine(planetSelectionSpawner.PlayBoomAndShake());
            Destroy(this.gameObject);
        }
    }
}
