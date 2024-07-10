using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public CardController cardController;
    public PlanetSelectionSpawner planetSelectionSpawner;
    public void CallPlanetSelectionSpawner()
    {
        var planetName = cardController.GetSelectedPlanetName();
        planetSelectionSpawner.HandleConfirmButton(planetName);
    }
}
