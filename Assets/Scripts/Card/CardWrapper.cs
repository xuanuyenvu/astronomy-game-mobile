using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using config;
using UnityEngine.UI;

public class CardWrapper : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [HideInInspector] public Vector2 targetPosition;
    [HideInInspector] public float targetRotation;
    [HideInInspector] public float targetVerticalDisplacement;
    public string planetName;
    public Image border;
    
    private GameObject cardAnimation;
    private const float EPS = 0.01f;
    private RectTransform rectTransform;
    private Canvas canvas;
    private bool isSelected = false;
    public static CardController cardController;

    public AnimationSpeedConfig animationSpeedConfig;
    private float overrideYPosition = 9;
    private int zoomedSortOrder = 100;
    public int uiLayer;

    private float width;

    private bool noRotation = false;
    [HideInInspector] public bool turnOnPointerDownAndUp = true;

    private bool isAnimation = false;

    public global::System.Single Width { get => width; set => width = value; }
    public global::System.Boolean NoRotation { get => noRotation; set => noRotation = value; }
    public global::System.Boolean IsAnimation { get => isAnimation; set => isAnimation = value; }
    public global::System.Boolean IsSelected { get => isSelected; set => isSelected = value; }

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Start()
    {
        Width = rectTransform.rect.width * rectTransform.localScale.x;
        canvas = GetComponent<Canvas>();
        cardAnimation = GameObject.Find(planetName + "CA");
        border.gameObject.SetActive(false);
    }

    public void SetAnchor(Vector2 min, Vector2 max)
    {
        rectTransform.anchorMin = min;
        rectTransform.anchorMax = max;
    }

    void Update()
    {
        if (!IsAnimation)
        {
            UpdateRotation();
            UpdatePosition();
            UpdateUILayer();
        }
    }

    private void UpdateUILayer()
    {
        if (!isSelected)
        {
            canvas.sortingOrder = uiLayer;
        }
    }

    private void UpdateRotation()
    {
        var crtAngle = rectTransform.rotation.eulerAngles.z;
        // If the angle is negative, add 360 to it to get the positive equivalent
        crtAngle = crtAngle < 0 ? crtAngle + 360 : crtAngle;
        // If the card is hovered and the rotation should be reset, set the target rotation to 0
        var tempTargetRotation = (isSelected || noRotation)
            ? 0
            : targetRotation;
        // var tempTargetRotation = targetRotation;
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

        if (noRotation)
        {
            target = new Vector2(target.x, target.y + cardController.MaxHeight);
        }
        if (isSelected)
        {
            target = new Vector2(target.x, target.y + overrideYPosition);
        }

        var distance = Vector2.Distance(rectTransform.position, target);
        var repositionSpeed = rectTransform.position.y > target.y || rectTransform.position.y < 0
            ? animationSpeedConfig.releasePosition
            : animationSpeedConfig.position;
        rectTransform.position = Vector2.Lerp(rectTransform.position, target,
            repositionSpeed / distance * Time.deltaTime);
    }

    public void OnPointerDown(PointerEventData eventData) { /*do nothing*/ }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isSelected && turnOnPointerDownAndUp)
        {
            Debug.Log("onPointerUp" + this.name);
            isSelected = true;
            border.gameObject.SetActive(true);

            cardController.ChangeSelectedCard(this);
            // Chạy hiệu ứng thẻ bài
            DisplayCardWithAnimation();
            // Đặt độ ưu tiên = 100
            canvas.sortingOrder = zoomedSortOrder;

        }
        else
        {
            border.gameObject.SetActive(false);
            
            if (isSelected)
            {
                ConcealCardWithAnimation();
            }

            cardController.ChangeSelectedCard(null);
        }
    }

    private void DisplayCardWithAnimation()
    {
        // Set the card to the correct position
        Vector3 position = Camera.main.ScreenToWorldPoint(new Vector3(this.rectTransform.position.x, this.rectTransform.position.y, Camera.main.nearClipPlane));
        position.z = -7;
        cardAnimation.transform.position = position;

        // Set animation for the card
        cardAnimation.transform.DOMove(new Vector3(0f, -1.5f, cardAnimation.transform.position.z), 0.16f)
            .SetEase(Ease.OutQuad);
        
        cardAnimation.transform.DORotate(new Vector3(70f, 0f, 0f), 0.16f, RotateMode.FastBeyond360)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                // Call the function to display the planet selection
                cardController.OnCardDisplayPlanetSelection(this);
                cardAnimation.transform.position = new Vector3(0, 0, 1);
            });
    }

    public void ConcealCardWithAnimation()
    {
        // Set the card to the correct position
        Vector3 startPos = new Vector3(0f, -1.5f, -7);
        cardAnimation.transform.position = startPos;

        Vector3 desPos = Camera.main.ScreenToWorldPoint(new Vector3(this.rectTransform.position.x, this.rectTransform.position.y, Camera.main.nearClipPlane));
        desPos.z = -7;
        // Set animation for the card
        cardAnimation.transform.DOMove(desPos, 0.16f)
            .SetEase(Ease.OutQuad);

        cardAnimation.transform.DORotate(new Vector3(0f, 0f, 0f), 0.16f, RotateMode.FastBeyond360)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                cardAnimation.transform.position = new Vector3(0, 0, 1);
            });
    }

    public void ResetAllValues()
    {
        border.gameObject.SetActive(false);
        isSelected = false;
        UpdateRotation();
        UpdatePosition();
        UpdateUILayer();
    }

    public void AsignValueRecTransformAndSetCanvas(CardWrapper card2)
    {
        this.rectTransform.position = card2.rectTransform.position;
        this.rectTransform.rotation = Quaternion.identity;
        // this.canvas.sortingOrder = card2.canvas.sortingOrder - 1;
    }


    public IEnumerator CoroutineCardAnimation(CardWrapper card2)
    {
        yield return StartCoroutine(MoveSelectedCard(card2));
        StartCoroutine(RotateSelectedCard());
    }

    private IEnumerator MoveSelectedCard(CardWrapper card2)
    {
        Vector3 startingPos = this.rectTransform.position;
        Vector3 finalPos = new Vector3(startingPos.x, 0, 0);

        float time = 1f;

        for (float t = 0f; t <= 1f; t += Time.deltaTime / time)
        {
            if (this == null)
                yield break;

            this.rectTransform.position = Vector3.Lerp(startingPos, finalPos, t);
            yield return null;
        }
    }

    private IEnumerator RotateSelectedCard()
    {
        yield return null;
    }
}

