using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using Card.CardController;

public class Player : MonoBehaviour
{
    public InputReader inputReader;

    private bool isDragging = false;
    private bool isSelected = false;
    private bool hasExecuted = false;
    private AstronomicalObject draggedPlanet = null;
    private AstronomicalObject selectedPlanetCard = null;

    public CardController cardController;

    void Start()
    {
        RegisterInputEvents();

        if (cardController == null)
        {
            cardController = FindObjectOfType<CardController>();
        }
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
        Debug.Log("Start drag");
        isDragging = true;
    }

    private void OnDrag(Vector2 pointerPos)
    {
        if (isDragging)
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(pointerPos.x, pointerPos.y, Camera.main.nearClipPlane));

            Ray ray = Camera.main.ScreenPointToRay(new Vector3(pointerPos.x, pointerPos.y, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Kiểm tra va chạm và tag của đối tượng
                if (hit.collider != null && hit.transform.CompareTag("PlanetSelection") && !isSelected)
                {
                    // Debug.Log(hit.transform.name);
                    AstronomicalObject selectedPlanetCard = hit.transform.GetComponent<AstronomicalObject>();
                    // Debug.Log("name " + selectedPlanetCard.name);
                    if (selectedPlanetCard != null && !isSelected)
                    {
                        draggedPlanet = Instantiate(selectedPlanetCard, worldPosition, Quaternion.identity);
                    }
                    isSelected = true;

                }

                if (draggedPlanet != null && isSelected)
                {
                    draggedPlanet.transform.position = new Vector3(worldPosition.x, worldPosition.y, -10);

                    // chỉ thực hiện gọi 1 lần
                    if (!hasExecuted)
                    {
                        cardController.DestroyPlanetSelection();
                        cardController.HideACard();
                        hasExecuted = true;
                    }
                }
            }
        }
    }

    private void OnDragEnd()
    {
        Debug.Log("End drag");
        isDragging = false;
        isSelected = false;
        if (draggedPlanet != null)
        {
            Destroy(draggedPlanet.gameObject);
            draggedPlanet = null;
        }
    }
}
