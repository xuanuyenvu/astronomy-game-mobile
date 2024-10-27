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
    private bool areEventsRegistered = false;

    void Start()
    {
        // RegisterInputEvents();

        if (cardController == null)
        {
            cardController = FindObjectOfType<CardController>();
        }
    }

    private void OnDestroy()
    {
        UnregisterInputEvents();
    }

    public void SetPlayer(IGamePlay _iGamePlay)
    {
        // iGamePlay = _iGamePlay;
    }

    public void RegisterInputEvents()
    {
        if (areEventsRegistered) return; 

        // Đăng ký các sự kiện đầu vào
        Debug.Log("Rregister Input Events");
        inputReader.OnPointerClicked += OnDragStart;
        inputReader.OnPointerClickedRelease += OnDragEnd;
        inputReader.OnPointerDrag += OnDrag;
        areEventsRegistered = true;
    }

    public void UnregisterInputEvents()
    {
        if (!areEventsRegistered) return;

        // Hủy đăng ký các sự kiện đầu vào
        Debug.Log("Unregister Input Events");
        inputReader.OnPointerClicked -= OnDragStart;
        inputReader.OnPointerClickedRelease -= OnDragEnd;
        inputReader.OnPointerDrag -= OnDrag;
        areEventsRegistered = false;
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

            Ray ray = Camera.main.ScreenPointToRay(new Vector3(pointerPos.x, pointerPos.y, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Kiểm tra va chạm và tag của đối tượng
                if (hit.collider != null && hit.transform.CompareTag("PlanetSelection") && !isSelected)
                {
                    AstronomicalObject selectedPlanetCard = hit.transform.GetComponent<AstronomicalObject>();
                    if (selectedPlanetCard != null && !isSelected)
                    {
                        draggedPlanet = Instantiate(selectedPlanetCard, worldPosition, Quaternion.identity);
                        draggedPlanet.name = draggedPlanet.name.Replace("(Clone)", "");
                        if (draggedPlanet.name == "07_saturn")
                        {
                            Vector3 newRotation = draggedPlanet.transform.rotation.eulerAngles;
                            newRotation.x = -10f;
                            draggedPlanet.transform.rotation = Quaternion.Euler(newRotation);
                        }
                    }
                    selectedPlanetCard = null;
                    isSelected = true;
                }

                if (draggedPlanet != null && isSelected)
                {
                    draggedPlanet.transform.position = new Vector3(worldPosition.x, worldPosition.y, -10);
                    GameManager.Instance.CurrentGamePlay.CheckDragPosition(draggedPlanet.transform.position, draggedPlanet.name);
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
        isDragging = false;
        isSelected = false;
        if (draggedPlanet != null)
        {
            Debug.Log("Dragged planet: " + draggedPlanet.name);
            GameManager.Instance.CurrentGamePlay.HandleConfirmButton(draggedPlanet.name, draggedPlanet.transform.position);
            Destroy(draggedPlanet.gameObject);
            draggedPlanet = null;
            hasExecuted = false;
        }
    }
    
    public void ResetPlayer()
    {
        cardController = null;
        // iGamePlay = null;
    }
}
