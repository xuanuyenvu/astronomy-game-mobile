using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class PlanetSelectionSpawner : MonoBehaviour
{
    [Header("List of Planets")]
    public List<AstronomicalObject> allPlanets;

    [HideInInspector]
    public AstronomicalObject planet1;
    [HideInInspector]
    public AstronomicalObject planet2;

    public AstronomicalObject rocket;
    public GameObject target;

    private int screenWidth;
    private int screenHeight;
    private Vector2 screenCenter;
    private bool isLeft;


    void Awake()
    {
        screenWidth = Screen.width;
        screenHeight = Screen.height;

        screenCenter = new Vector2(screenWidth / 2, screenHeight / 2);
    }

    void Start()
    {
        Play();
    }

    public void RandomizePosition()
    {
        var id = UnityEngine.Random.Range(0, allPlanets.Count);
        // var id = 0;
        planet1 = allPlanets[id];

        if (id < 4)
        {
            planet2 = allPlanets[Random.Range(0, 4)];
        }
        else
        {
            planet2 = allPlanets[Random.Range(4, allPlanets.Count)];
        }

        isLeft = Random.Range(0, 2) == 0 ? true : false;
        planet1 = Clone(planet1, isLeft);
        planet1.gameObject.SetActive(true);

        planet2 = Clone(planet2, !isLeft);
        planet2.gameObject.SetActive(false);

        Instantiate(target, planet2.transform.position, Quaternion.identity);
    }

    private AstronomicalObject Clone(AstronomicalObject origin, bool isLeftPart = true)
    {
        float spawnY = screenHeight/2;
        float spawnX = GetSpawnX(isLeftPart);
 
        Vector3 spawnPosition = Camera.main.ScreenToWorldPoint(new Vector3(spawnX, spawnY, Camera.main.transform.position.z));
        spawnPosition.z = -3;

        AstronomicalObject clonedPlanet = Instantiate(origin, spawnPosition, Quaternion.identity);

        if(clonedPlanet.tag == "07_saturn")
        {
            Vector3 newRotation = clonedPlanet.transform.rotation.eulerAngles;
            newRotation.x = -36f;
            clonedPlanet.transform.rotation = Quaternion.Euler(newRotation);
        }
        
        return clonedPlanet;
    }

    private float GetSpawnX(bool isLeftPart = true)
    {
        float spawnX = 0;
        float padding = screenWidth / 6;

        if (isLeftPart) {
            spawnX = Random.Range(0 + padding * 1.2f, screenWidth / 2 - padding * 1.5f);
        }
        else {
            spawnX = Random.Range(screenWidth / 2 + padding * 2, screenWidth - padding * 1.2f);
        }

        return spawnX;
    }

    public void FindMeanAndSetRocket()
    {
        var d = Vector3.Distance(planet1.transform.position, planet2.transform.position);
        var d2 = d /(1 + Math.Sqrt(planet1.Mass / planet2.Mass));
        var direction = (planet1.transform.position - planet2.transform.position).normalized;
        
        var answer = planet2.transform.position + direction * ((float)d2);

        if(isLeft) {
            rocket = Instantiate(rocket, answer, Quaternion.Euler(0, 90, 0));
        }
        else {
            rocket = Instantiate(rocket, answer, Quaternion.Euler(0, -90, 0));
        }
        rocket.gameObject.SetActive(true);
    }

    public void Play()
    {
        RandomizePosition();
        FindMeanAndSetRocket();
    }
}
