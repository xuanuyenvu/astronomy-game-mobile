using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketController : AstronomicalObject
{
    private bool turnOnCollider = false;

    private Collider rocketCollider;
    public global::System.Boolean TurnOnCollider { get => turnOnCollider; set => turnOnCollider = value; }
    void Start()
    {
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

    public void RotateRocket(Vector3 planetPosition)
    {
        // Tính vector từ rocket tới planet
        Vector3 direction = planetPosition - this.transform.position;

        // Tính góc quay để hướng rocket về phía planet
        Quaternion rotation = Quaternion.LookRotation(direction);

        // Xoay rocket theo hướng của vector
        transform.rotation = rotation;
    }

    private IEnumerator FlyTo(Vector3 planetPosition)
    {
        Vector3 startingPos = transform.position;
        Vector3 finalPos = planetPosition;
        RotateRocket(planetPosition);

        // Tính chiều dài quãng đường
        float distance = Vector3.Distance(startingPos, finalPos);
        // Đường bay càng xa thì thời gian càng chậm
        // Ví dụ: bay 1km --> 2s, thì 2km phải bay lâu hơn
        float time = distance / 9f;

        for (float t = 0f; t <= 1f; t += Time.deltaTime / time)
        {
            if (this == null)
                yield break; // Dừng coroutine nếu rocket đã bị hủy bở hàm OnTriggerEnter

            transform.position = Vector3.Lerp(startingPos, finalPos, t);
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider obj)
    {
        if (obj.gameObject.CompareTag("Planet") || obj.gameObject.CompareTag("Rocket"))
        {
            obj.gameObject.SetActive(false);
            GameManager.Instance.CurrentGamePlay.ExecuteAfterCollision(obj.gameObject.GetComponent<AstronomicalObject>());
            Destroy(this.gameObject);
        }
        else if (obj.gameObject.CompareTag("Bound"))
        {
            GameManager.Instance.CurrentGamePlay.ExecuteAfterCollision(null);
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

            yield return null;
        }

        transform.position = startingPos; // trở về vị trí ban đầu sau khi kết thúc
        transform.rotation = originRot; // trở về góc quay ban đầu sau khi kết thúc
        elapsedTime = 0f; // Reset lại giá trị để dùng cho lần lắc sau
    }

    public IEnumerator ShakeAndFlyTo(Vector3 planetPosition)
    {
        yield return StartCoroutine(ShakeRocket());
        StartCoroutine(FlyTo(planetPosition));
    }
}
