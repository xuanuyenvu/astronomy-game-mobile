using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public InventoryCard[] listOfCards;
    public PlanetInfoUIController planetInfoUIController;
    private int openedCardCount;
    private int currentlySelectedCard = 0;

    void Start()
    {
        openedCardCount = DataSaver.Instance.userModel.Card;
        Debug.Log("ononon" + openedCardCount);

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
            Debug.Log("duoc selected");
            AudioManager.Instance.PlaySFX("Click");
            listOfCards[currentlySelectedCard].ShowBorder();
            planetInfoUIController.DisplayPlanetInfo(cardID);
            currentlySelectedCard = cardID;
        }
    }
}