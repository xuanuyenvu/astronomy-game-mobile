using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public InventoryCard[] listOfCards;
    public int openedCardCount;
    private int currentlySelectedCard = 0;

    void Start()
    {
        openedCardCount = Mathf.Clamp(openedCardCount, 0, listOfCards.Length);

        for (int i = 0; i < openedCardCount; i++)
        {
            if (listOfCards[i] != null)
            {
                listOfCards[i].DisplayPlanet();
            }
        }
    }

    public void DisplayPlanet(int cardID)
    {
        bool isCardSelected = listOfCards[cardID].SelectCard();

        if (isCardSelected)
        {
            listOfCards[currentlySelectedCard].UnselectCard();
            currentlySelectedCard = cardID;
        }
    }
}