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

    private IEnumerator FlyTo(GameObject planet)
    {
        Vector3 startingPos = transform.position;
        Vector3 finalPos = planet.transform.position;
        RotateRocket(planet);

        // Tính chiều dài quãng đường
        float distance = Vector3.Distance(startingPos, finalPos);
        // Đường bay càng xa thì thời gian càng chậm
        // Ví dụ: bay 1km --> 2s, thì 2km phải bay lâu hơn
        // float time = distance / 9f;
        float time = distance / 9f;

        for (float t = 0f; t <= 1f; t += Time.deltaTime / time)
        {
            if (this == null)
                yield break; // Dừng coroutine nếu rocket đã bị hủy bở hàm OnTriggerEnter

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

    public IEnumerator ShakeRocket()
    {
        Vector3 startingPos = transform.position; // vị trí ban đầu
        Quaternion originRot = transform.rotation; // góc quay ban đầu
        Quaternion startingRot = Quaternion.identity; // xoay về góc mặt định để nó lắc cho đẹp chứ hong gì

        float duration = 0.8f; // thời gian lắc
        float initialSpeed = 3f; // tốc độ lắc ban đầu
        float finalSpeed = 9f; // tốc độ lắc cuối cùng
        float positionAmount = 0.06f; // biên độ lắc cho position
        float rotationAmount = 16f; // biên độ lắc cho góc quay
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            // Nội suy giữa initialSpeed và finalSpeed dựa trên elapsedTime
            float currentSpeed = Mathf.Lerp(initialSpeed, finalSpeed, elapsedTime / duration);

            float offsetPos = Mathf.Sin(Time.time * currentSpeed) * positionAmount;
            float offsetRot = Mathf.Sin(Time.time * currentSpeed) * rotationAmount;

            transform.position = new Vector3(startingPos.x + offsetPos, startingPos.y, startingPos.z);
            transform.rotation = Quaternion.Euler(startingRot.eulerAngles.x + offsetRot, startingRot.eulerAngles.y, startingRot.eulerAngles.z);

            // if (elapsedTime > 0.6f)
            // {
                
            //     StartCoroutine(FlyTo(planet));
            // }
            yield return null;
        }

        transform.position = startingPos; // trở về vị trí ban đầu sau khi kết thúc
                transform.rotation = originRot; // trở về góc quay ban đầu sau khi kết thúc
        elapsedTime = 0f; // Reset lại giá trị để dùng cho lần lắc sau
    }

    public IEnumerator ShakeAndFlyTo(GameObject planet)
    {
        yield return StartCoroutine(ShakeRocket());
        StartCoroutine(FlyTo(planet));
    }
}
