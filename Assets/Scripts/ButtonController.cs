using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public CardController cardController;
    // private IGamePlay current;
    private PlanetSelectionSpawner planetSelectionSpawner;

    void Start()
    {  
        if(planetSelectionSpawner == null)
        {
            planetSelectionSpawner = FindObjectOfType<PlanetSelectionSpawner>();
        }
        if (cardController == null)
        {
            cardController = FindObjectOfType<CardController>();
        }
    }

    void Update(){
        if(planetSelectionSpawner == null)
        {
            planetSelectionSpawner = FindObjectOfType<PlanetSelectionSpawner>();
        }
    }
    public void CallPlanetSelectionSpawner()
    {
        GameObject selectedPlanet = cardController.GetSelectedPlanet();
        GameManager.Instance.CurrentGamePlay.HandleConfirmButton(selectedPlanet.name, selectedPlanet.transform.position);
    }
}
