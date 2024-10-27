using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DoubleClickController : MonoBehaviour, IPointerClickHandler
{
    private CardController cardController;

    private float lastClickTime = 0f;
    private float doubleClickThreshold = 0.8f;

    void Start()
    {
        if (cardController == null)
        {
            cardController = FindObjectOfType<CardController>();
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Double click - Card Controller: " + cardController.gamePlayId);
        if (cardController.gamePlayId != 1 && cardController.gamePlayId != 2)
        {
            float timeSinceLastClick = Time.time - lastClickTime;

            if (timeSinceLastClick <= doubleClickThreshold)
            {
                GameObject selectedPlanet = cardController.GetSelectedPlanet();
                GameManager.Instance.CurrentGamePlay.HandleConfirmButton(selectedPlanet.name, selectedPlanet.transform.position);
            }

            lastClickTime = Time.time;
        }
    }
}
