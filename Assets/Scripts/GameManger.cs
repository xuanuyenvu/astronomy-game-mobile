using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManger : MonoBehaviour
{
    public IGamePlay[] gamePlays;
    // Start is called before the first frame update
    void Start()
    {
        var current = Instantiate(gamePlays[0]);
        PlanetSelectionSpawner planetSelectionSpawner = current.GetComponent<PlanetSelectionSpawner>();
        if (planetSelectionSpawner.cameraShake == null)
        {
            planetSelectionSpawner.cameraShake = FindObjectOfType<CameraShake>();
        }
        if (planetSelectionSpawner.cardController == null)
        {
            planetSelectionSpawner.cardController = FindObjectOfType<CardController>();
        }
        if (planetSelectionSpawner.healthManager == null)
        {
            planetSelectionSpawner.healthManager = FindObjectOfType<HealthManager>();
        }
        if (planetSelectionSpawner.scoreManager == null)
        {
            planetSelectionSpawner.scoreManager = FindObjectOfType<ScoreManager>();
        }
        planetSelectionSpawner.Play();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
