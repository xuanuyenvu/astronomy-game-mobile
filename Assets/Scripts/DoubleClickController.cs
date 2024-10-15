using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DoubleClickController : MonoBehaviour, IPointerClickHandler
{
    private CardController cardController;
    private IGamePlay iGamePlay;

    private float lastClickTime = 0f;
    private float doubleClickThreshold = 0.8f;

    void Start()
    {
        if (iGamePlay == null)
        {
            iGamePlay = FindObjectOfType<IGamePlay>();
        }
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
                iGamePlay.HandleConfirmButton(selectedPlanet.name, selectedPlanet.transform.position);
            }

            lastClickTime = Time.time;
        }
    }
}
