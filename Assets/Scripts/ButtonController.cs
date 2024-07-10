using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public CardController cardController;
    // public PlanetSelectionSpwaner planetSelectionSpwaner;
    public void CallPlanetSelectionSpawner()
    {
        var planetName = cardController.GetSelectedPlanetName();
        Debug.Log("name: " + planetName);
    }
}
