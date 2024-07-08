using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using Card.CardController;

public class Player : MonoBehaviour
{
    public InputReader inputReader;

    private bool isDragging = false;
    private bool isSelected = false;
    private AstronomicalObject draggedPlanet = null;
    private AstronomicalObject selectedPlanetCard = null;

    public CardController cardController;

    void Start()
    {
        RegisterInputEvents();
    }

    private void OnDestroy()
    {
        UnregisterInputEvents();
    }

    private void RegisterInputEvents()
    {
        // Đăng ký các sự kiện đầu vào
        inputReader.OnPointerClicked += OnDragStart;
        inputReader.OnPointerClickedRelease += OnDragEnd;
        inputReader.OnPointerDrag += OnDrag;
    }

    private void UnregisterInputEvents()
    {
        // Hủy đăng ký các sự kiện đầu vào
        inputReader.OnPointerClicked -= OnDragStart;
        inputReader.OnPointerClickedRelease -= OnDragEnd;
        inputReader.OnPointerDrag -= OnDrag;
    }

    private void OnDragStart()
    {
        isDragging = true;
    }

    private void OnDrag(Vector2 pointerPos)
    {
        if (isDragging)
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(pointerPos.x, pointerPos.y, Camera.main.nearClipPlane));
            // Bước 1: Tạo ra một tia từ vị trí của con trỏ trên màn hình
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(pointerPos.x, pointerPos.y, 0));

            // Biến để lưu thông tin va chạm
            RaycastHit hit;

            // Bước 2: Thực hiện raycast 3D
            if (Physics.Raycast(ray, out hit))
            {
                // Bước 3: Kiểm tra va chạm và tag của đối tượng
                if (hit.collider != null && hit.transform.CompareTag("PlanetSelection") && !isSelected)
                {
                    // In ra tên đối tượng và đặt isSelected thành true
                    Debug.Log(hit.transform.name);
                    AstronomicalObject selectedPlanetCard = hit.transform.GetComponent<AstronomicalObject>();
                    Debug.Log("name " + selectedPlanetCard.name);
                    if (selectedPlanetCard != null && !isSelected)
                    {
                        draggedPlanet = Instantiate(selectedPlanetCard, worldPosition, Quaternion.identity);
                    }
                    isSelected = true;

                }

                if (draggedPlanet != null && isSelected)
                {
                    draggedPlanet.transform.position = new Vector3(worldPosition.x, worldPosition.y, -10);
                    cardController.DestroyPlanetSelection();
                }
            }
        }
    }

    private void OnDragEnd()
    {
        isDragging = false;
        isSelected = false;
        DestroyImmediate(draggedPlanet);
    }
}
