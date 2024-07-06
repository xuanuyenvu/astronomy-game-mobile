using UnityEngine.UI;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using config;

public class CardController : MonoBehaviour
{
    [Header("List of Cards")]
    public List<CardWrapper> allCards;
    private List<CardWrapper> allCardInstances;

    [Header("List of selected planets")]
    public List<GameObject> allPlanetSelection;
    private RectTransform rectTransform;

    [SerializeField]
    private bool forceFitContainer;

    [Header("Rotation")]
    [SerializeField]
    [Range(0f, 90f)]
    private float maxCardRotation;
    [SerializeField]
    private float maxHeightDisplacement;

    [SerializeField]
    private AnimationSpeedConfig animationSpeedConfig;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    void Start()
    {
        InitCards();
    }

    private void InitCards()
    {
        ShuffleCards();
        DisplayCards();
        SetUpCards();
    }
    
    private void ShuffleCards()
    {
        for (int i = allCards.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1); // Tạo một số ngẫu nhiên từ 0 đến i
            CardWrapper temp = allCards[i];
            allCards[i] = allCards[randomIndex];
            allCards[randomIndex] = temp;
        }
    }

    private void DisplayCards()
    {
        Transform canvasTransform = this.transform;
        foreach (CardWrapper card in allCards)
        {
            CardWrapper cardInstance = Instantiate(card, this.transform);
            cardInstance.name = cardInstance.name.Replace("(Clone)", "");
        }

        allCardInstances = new List<CardWrapper>(GetComponentsInChildren<CardWrapper>());
    }
    
    private void SetUpCards()
    {
        foreach (CardWrapper card in allCardInstances)
        {
            var canvas = card.GetComponent<Canvas>();
            if (canvas == null)
            {
                canvas = card.gameObject.AddComponent<Canvas>();
            }

            canvas.overrideSorting = true;

            if (card.GetComponent<GraphicRaycaster>() == null)
            {
                card.gameObject.AddComponent<GraphicRaycaster>();
            }

            card.animationSpeedConfig = animationSpeedConfig;
            card.cardContainer = this;
        }
    }

    void Update()
    {
        UpdateCards();
        SetCardsAnchor();
    }

    private void SetCardsAnchor() {
        foreach (CardWrapper child in allCardInstances) {
            child.SetAnchor(new Vector2(0, 0.5f), new Vector2(0, 0.5f));
        }
    }

    private void UpdateCards()
    {
        if (transform.childCount != allCardInstances.Count)
        {
            InitCards();
        }

        if (allCardInstances.Count == 0)
        {
            return;
        }

        SetCardsPosition();
        SetCardsRotation();
        // SetCardsUILayers();
        // UpdateCardOrder();
    }

    private void SetCardsPosition()
    {
        // Compute the total width of all the cards in global space
        float cardsTotalWidth = 0;
        foreach (CardWrapper card in allCardInstances)
        {
            cardsTotalWidth += card.Width * (card.transform.lossyScale.x * 9);
        }
        // Compute the width of the container in global space
        var containerWidth = rectTransform.rect.width * transform.lossyScale.x;
        if (forceFitContainer && cardsTotalWidth > containerWidth)
        {
            DistributeChildrenToFitContainer(cardsTotalWidth);
        }
        else
        {
            DistributeChildrenWithoutOverlap(cardsTotalWidth);
        }
    }

    private void DistributeChildrenToFitContainer(float childrenTotalWidth)
    {
        // Get the width of the container
        var width = rectTransform.rect.width * transform.lossyScale.x;
        // Get the distance between each child
        var distanceBetweenChildren = (width - childrenTotalWidth) / (allCardInstances.Count - 1);
        // Set all children's positions to be evenly spaced out
        var currentX = transform.position.x - width / 2;
        foreach (CardWrapper child in allCardInstances)
        {
            var adjustedChildWidth = child.Width * (child.transform.lossyScale.x * 9);
            child.targetPosition = new Vector2(currentX + adjustedChildWidth / 2, transform.position.y);
            currentX += adjustedChildWidth + distanceBetweenChildren;
        }
    }

    private void DistributeChildrenWithoutOverlap(float childrenTotalWidth)
    {
        var currentPosition = GetAnchorPositionByAlignment(childrenTotalWidth);
        foreach (CardWrapper child in allCardInstances)
        {
            var adjustedChildWidth = child.Width * (child.transform.lossyScale.x * 9);
            child.targetPosition = new Vector2(currentPosition + adjustedChildWidth / 2, transform.position.y);
            currentPosition += adjustedChildWidth;
        }
    }

    private float GetAnchorPositionByAlignment(float childrenWidth)
    {
        var containerWidthInGlobalSpace = rectTransform.rect.width * transform.lossyScale.x;
        return transform.position.x - childrenWidth / 2;
    }

    private void SetCardsRotation()
    {
        for (var i = 0; i < allCardInstances.Count; i++)
        {
            allCardInstances[i].targetRotation = GetCardRotation(i);
            allCardInstances[i].targetVerticalDisplacement = GetCardVerticalDisplacement(i);
        }
    }

    private float GetCardRotation(int index)
    {
        if (allCards.Count < 3) return 0;
        else
            return -maxCardRotation * (index - (allCardInstances.Count - 1) / 2f) / ((allCards.Count - 1) / 2f);
    }

    private float GetCardVerticalDisplacement(int index)
    {
        if (allCardInstances.Count < 3) return 0;
        else
            return maxHeightDisplacement * (1 - Mathf.Pow(index - (allCards.Count - 1) / 2f, 2) / Mathf.Pow((allCards.Count - 1) / 2f, 2));
    }


    public void OnCardDisplayPlanetSelection(CardWrapper card)
    {
        var planetSelectionName = card.name.Replace("UI", "Selection");
        Debug.Log("name" + planetSelectionName);
    }
}
