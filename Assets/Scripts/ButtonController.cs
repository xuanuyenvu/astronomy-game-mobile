using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public CardController cardController;
    public PlanetSelectionSpawner planetSelectionSpawner;
    public void CallPlanetSelectionSpawner()
    {
        string planetName = cardController.GetSelectedPlanetName();
        // Debug.Log(" _" + planetName);
        // planetSelectionSpawner.HandleConfirmButton(planetName);
    }
}
