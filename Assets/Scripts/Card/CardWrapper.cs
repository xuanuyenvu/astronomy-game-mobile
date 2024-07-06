using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using config;

public class CardWrapper : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [HideInInspector]
    public Vector2 targetPosition;
    [HideInInspector]
    public float targetRotation;
    [HideInInspector]
    public float targetVerticalDisplacement;
    public AnimationSpeedConfig animationSpeedConfig;
    private const float EPS = 0.01f;
    private RectTransform rectTransform;
    private Canvas canvas;
    private bool isSelected = false;
    public CardController cardContainer;

    private float width;

    public global::System.Single Width { get => width; set => width = value; }

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Start()
    {
        Width = rectTransform.rect.width * rectTransform.localScale.x;
        // Debug.Log("width: " + Width);
        canvas = GetComponent<Canvas>();
    }

    public void SetAnchor(Vector2 min, Vector2 max)
    {
        rectTransform.anchorMin = min;
        rectTransform.anchorMax = max;
    }

    void Update()
    {
        UpdateRotation();
        UpdatePosition();
    }

    private void UpdateRotation()
    {
        var crtAngle = rectTransform.rotation.eulerAngles.z;
        // If the angle is negative, add 360 to it to get the positive equivalent
        crtAngle = crtAngle < 0 ? crtAngle + 360 : crtAngle;
        // If the card is hovered and the rotation should be reset, set the target rotation to 0
        // var tempTargetRotation = (isHovered || isDragged) && zoomConfig.resetRotationOnZoom
        //     ? 0
        //     : targetRotation;
        var tempTargetRotation = targetRotation;
        tempTargetRotation = tempTargetRotation < 0 ? tempTargetRotation + 360 : tempTargetRotation;
        var deltaAngle = Mathf.Abs(crtAngle - tempTargetRotation);
        if (!(deltaAngle > EPS)) return;

        // Adjust the current angle and target angle so that the rotation is done in the shortest direction
        var adjustedCurrent = deltaAngle > 180 && crtAngle < tempTargetRotation ? crtAngle + 360 : crtAngle;
        var adjustedTarget = deltaAngle > 180 && crtAngle > tempTargetRotation
            ? tempTargetRotation + 360
            : tempTargetRotation;
        var newDelta = Mathf.Abs(adjustedCurrent - adjustedTarget);

        var nextRotation = Mathf.Lerp(adjustedCurrent, adjustedTarget,
            animationSpeedConfig.rotation / newDelta * Time.deltaTime);
        rectTransform.rotation = Quaternion.Euler(0, 0, nextRotation);
    }

    private void UpdatePosition()
    {
        var target = new Vector2(targetPosition.x, targetPosition.y + targetVerticalDisplacement);
        if (isSelected)
        {
            target = new Vector2(target.x, -1);
        }

        var distance = Vector2.Distance(rectTransform.position, target);
        var repositionSpeed = rectTransform.position.y > target.y || rectTransform.position.y < 0
            ? animationSpeedConfig.releasePosition
            : animationSpeedConfig.position;
        rectTransform.position = Vector2.Lerp(rectTransform.position, target,
            repositionSpeed / distance * Time.deltaTime);
    }

    private void UpdateScale()
    {

    }

    private void UpdateUILayer()
    {

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isSelected = true;
        // Gọi hiển thị prefab
        cardContainer.OnCardDisplayPlanetSelection(this);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        
    }
}
