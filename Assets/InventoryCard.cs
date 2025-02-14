using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class InventoryCard : MonoBehaviour, IPointerClickHandler
{
    public int itemID;
    public Sprite planet;
    public TextMeshProUGUI text;
    public GameObject border;
    public UnityEvent onCardClicked;

    private Image imageComponent;
    private bool isDisplayed = false;

    void Awake()
    {
        imageComponent = GetComponent<Image>();
        if (itemID == 0)
        {
            border.SetActive(true);
        }
        else
        {
            border.SetActive(false);
        }
    }

    private void HidePlanet()
    {
        imageComponent.color = new Color(0.58f, 0.58f, 0.58f, 0.78f); // (149/255, 149/255, 149/255, 200/255)
        text.gameObject.SetActive(true);
    }

    public void DisplayPlanet()
    {
        imageComponent.sprite = planet;
        imageComponent.color = Color.white;
        text.gameObject.SetActive(false);
        isDisplayed = true;
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        onCardClicked.Invoke();
    }
    
    public bool SelectCard()
    {
        if (isDisplayed)
        {
            border.SetActive(true);
            return true;
        }
        return false;
    }

    public void UnselectCard()
    {
        border.SetActive(false);
    }

}