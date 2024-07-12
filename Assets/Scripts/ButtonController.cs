using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public CardController cardController;
    public PlanetSelectionSpawner planetSelectionSpawner;

    void Start()
    {
        if (cardController == null)
        {
            cardController = FindObjectOfType<CardController>();
        }

        if (planetSelectionSpawner == null)
        {
            planetSelectionSpawner = FindObjectOfType<PlanetSelectionSpawner>();
        }
    }
    public void CallPlanetSelectionSpawner()
    {
        string planetName = cardController.GetSelectedPlanetName();
        planetSelectionSpawner.HandleConfirmButton(planetName);
    }
}
