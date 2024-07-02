using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour
{
    [Header("List of Cards")]
    public List<CardWrapper> allCards;
    private RectTransform rectTransform;
    private bool forceFitContainer;

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
    }

    private void DisplayCards()
    {
        Transform canvasTransform = this.transform;
        foreach (CardWrapper card in allCards)
        {
            CardWrapper cardInstance = Instantiate(card, this.transform);
        }
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

    void Update()
    {
        UpdateCards();
    }

    private void UpdateCards()
    {
        if (transform.childCount != allCards.Count)
        {
            InitCards();
        }

        if (allCards.Count == 0)
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
        var cardsTotalWidth = allCards.Sum(card => card.width * card.transform.lossyScale.x);
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
        var distanceBetweenChildren = (width - childrenTotalWidth) / (allCards.Count - 1);
        // Set all children's positions to be evenly spaced out
        var currentX = transform.position.x - width / 2;
        foreach (CardWrapper child in allCards)
        {
            var adjustedChildWidth = child.width * child.transform.lossyScale.x;
            child.targetPosition = new Vector2(currentX + adjustedChildWidth / 2, transform.position.y);
            currentX += adjustedChildWidth + distanceBetweenChildren;
        }
    }

    private void DistributeChildrenWithoutOverlap(float childrenTotalWidth)
    {
        var currentPosition = GetAnchorPositionByAlignment(childrenTotalWidth);
        foreach (CardWrapper child in allCards)
        {
            var adjustedChildWidth = child.width * child.transform.lossyScale.x;
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

    }

}
