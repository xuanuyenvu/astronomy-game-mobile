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
    private GameObject planetSelectionInstance;

    [HideInInspector]
    public CardWrapper selectedCard = null;

    [Header("Asset in Scene")]
    public GameObject darkMask;
    public Button confirmButton;

    [Header("Constraints")]
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
    private RectTransform rectTransform;
    private bool isListChanging = false;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    void Start()
    {
        darkMask.SetActive(false);
        confirmButton.gameObject.SetActive(false);
        InitCards();
    }

    private void InitCards()
    {
        ShuffleCards();
        DisplayCards();
        SetUpCards();
        SetCardsAnchor();
    }

    private void ShuffleCards()
    {
        for (int i = allCards.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
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

    private void SetCardsAnchor()
    {
        foreach (CardWrapper child in allCardInstances)
        {
            child.SetAnchor(new Vector2(0, 0.5f), new Vector2(0, 0.5f));
        }
    }

    void Update()
    {
        UpdateCards();
    }

    private void UpdateCards()
    {
        if (transform.childCount != allCardInstances.Count && isListChanging)
        {
            InitCards();
        }

        if (allCardInstances.Count == 0)
        {
            return;
        }

        UpdateCardSizeAndProperties(allCardInstances.Count);

        SetCardsPosition();
        SetCardsRotation();
        SetCardsUILayers();
    }

    private void UpdateCardSizeAndProperties(int cardCount)
    {
        Vector2 targetSize;

        switch (cardCount)
        {
            case 7:
                targetSize = new Vector2(870, rectTransform.sizeDelta.y);
                break;
            case 6:
                targetSize = new Vector2(770, rectTransform.sizeDelta.y);
                break;
            case 5:
                targetSize = new Vector2(670, rectTransform.sizeDelta.y);
                break;
            case 4:
                targetSize = new Vector2(600, rectTransform.sizeDelta.y);
                maxHeightDisplacement = 0;
                SetNoRotationForAllCards(true);
                break;
            default:
                return;
        }

        StartCoroutine(SmoothResize(targetSize, 0.1f));
    }

    private void SetNoRotationForAllCards(bool noRotation)
    {
        foreach (CardWrapper card in allCardInstances)
        {
            card.NoRotation = noRotation;
        }
    }

    private IEnumerator SmoothResize(Vector2 targetSize, float duration)
    {
        Vector2 initialSize = rectTransform.sizeDelta;
        float timeElapsed = 0;

        while (timeElapsed < duration)
        {
            rectTransform.sizeDelta = Vector2.Lerp(initialSize, targetSize, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        rectTransform.sizeDelta = targetSize;
    }


    private void SetCardsUILayers()
    {
        for (var i = 0; i < allCardInstances.Count; i++)
        {
            allCardInstances[i].uiLayer = 1 + i;
        }
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
        if (allCardInstances.Count < 3) return 0;
        else
            return -maxCardRotation * (index - (allCardInstances.Count - 1) / 2f) / ((allCardInstances.Count - 1) / 2f);
    }

    private float GetCardVerticalDisplacement(int index)
    {
        if (allCardInstances.Count < 3) return 0;
        else
            return maxHeightDisplacement * (1 - Mathf.Pow(index - (allCardInstances.Count - 1) / 2f, 2) / Mathf.Pow((allCardInstances.Count - 1) / 2f, 2));
    }

    private GameObject GetPlanetFromCard(CardWrapper card)
    {

        int idPlanetInList = card.name[0] - '0';

        if (idPlanetInList >= 0 && idPlanetInList < allPlanetSelection.Count)
        {
            return allPlanetSelection[idPlanetInList];
        }

        return null;
    }


    public void OnCardDisplayPlanetSelection(CardWrapper card)
    {
        if (char.IsDigit(card.name[0]))
        {
            // Lấy ký tự đầu tiên của object
            GameObject planet = GetPlanetFromCard(card);

            // Hiển thị planet được chọn
            Vector3 positionPlanet = new Vector3(0, 0, -9);
            planetSelectionInstance = Instantiate(planet, positionPlanet, Quaternion.identity);

            // Bật lớp phủ
            darkMask.SetActive(true);
            confirmButton.gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("The first character is not a digit.");
        }
    }

    public void DestroyPlanetSelection()
    {
        if (planetSelectionInstance != null)
        {
            Destroy(planetSelectionInstance);
            darkMask.SetActive(false);
            confirmButton.gameObject.SetActive(false); ;
        }
    }

    public void ChangeSelectedCard(CardWrapper card)
    {
        // Nếu đã có card được chọn từ trước thì gọi hàm Update() 
        // để reset lại các giá trị Rotation, Position, Sorting
        if (selectedCard != null)
        {
            selectedCard.ResetAllValues();
        }

        // Gán thẻ được chọn vào biến selectedCard
        selectedCard = card;
    }

    public string GetSelectedPlanetName()
    {
        GameObject planet = GetPlanetFromCard(selectedCard);
        if (planet != null)
        {
            DestroyPlanetSelection();
        }
        if (selectedCard != null)
        {
            RemoveAndDestroyCardInstance(selectedCard);
            selectedCard = null;
        }
        return planet.name;
    }

    private void RemoveAndDestroyCardInstance(CardWrapper card)
    {
        isListChanging = true;
        if (card != null)
        {
            allCardInstances.Remove(card);
            Destroy(card.gameObject);

        }
        isListChanging = false;
        StartCoroutine(SmoothUpdateCards());
    }

    private IEnumerator SmoothUpdateCards()
    {
        yield return new WaitForEndOfFrame();
        UpdateCards();
    }
}
