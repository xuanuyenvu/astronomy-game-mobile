using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardWrapper : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [HideInInspector]
    public Vector2 targetPosition;
    [HideInInspector]
    public float targerVerticalDisplacement;
    private const float EPS = 0.01f;
    private RectTransform rectTransform;
    private Canvas canvas;
    private bool isSelected = false;

    public float width;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        width = rectTransform.rect.width * rectTransform.localScale.x;;
    }

    void Start()
    {
        canvas = GetComponent<Canvas>();
    }

    void Update()
    {

    }

    private void UpdateRotation()
    {

    }

    private void UpdatePosition()
    {
        if (!isSelected)
        {

        }
        else
        {

        }
    }

    private void UpdateScale()
    {

    }

    private void UpdateUILayer()
    {

    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    public void OnPointerUp(PointerEventData eventData)
    {

    }
}
