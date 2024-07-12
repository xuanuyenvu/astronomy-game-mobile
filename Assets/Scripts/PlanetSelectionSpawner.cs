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

    public RocketController rocketPrefab;

    [HideInInspector]
    public RocketController rocket;
    public GameObject target;

    private int screenWidth;
    private int screenHeight;
    private Vector2 screenCenter;
    private bool isLeft;

    private bool animationTime = false;
    private bool playing = false;

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
        // var id = 5;
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
        float spawnY = screenHeight / 2;
        float spawnX = GetSpawnX(isLeftPart);

        Vector3 spawnPosition = Camera.main.ScreenToWorldPoint(new Vector3(spawnX, spawnY, Camera.main.transform.position.z));
        spawnPosition.z = -3;

        AstronomicalObject clonedPlanet = Instantiate(origin, spawnPosition, Quaternion.identity);
        clonedPlanet.name = clonedPlanet.name.Replace("(Clone)", "");

        if (clonedPlanet.name == "07_saturn")
        {
            Vector3 newRotation = clonedPlanet.transform.rotation.eulerAngles;
            newRotation.x = -10f;
            clonedPlanet.transform.rotation = Quaternion.Euler(newRotation);
        }

        return clonedPlanet;
    }

    private float GetSpawnX(bool isLeftPart = true)
    {
        float spawnX = 0;
        float padding = screenWidth / 6;

        if (isLeftPart)
        {
            spawnX = Random.Range(0 + padding * 1.2f, screenWidth / 2 - padding * 1.5f);
        }
        else
        {
            spawnX = Random.Range(screenWidth / 2 + padding * 2, screenWidth - padding * 1.2f);
        }

        return spawnX;
    }

    public void FindMeanAndSetRocket()
    {
        var d = Vector3.Distance(planet1.transform.position, planet2.transform.position);
        var d2 = d / (1 + Math.Sqrt(planet1.Mass / planet2.Mass));
        var direction = (planet1.transform.position - planet2.transform.position).normalized;

        var answer = planet2.transform.position + direction * ((float)d2);
        
        rocket = Instantiate(rocketPrefab, answer, Quaternion.identity);
        rocket.RotateRocket(planet1.gameObject);
        rocket.gameObject.SetActive(true);
    }

    IEnumerator SetPositionBeforePlaying(float time)
    {
        animationTime = true;
        var center = GetCenterPoint();
        var planet1Pos = planet1.transform.position;
        // var placeHolderPos = PlaceHolder.transform.position;
        var rocketPos = rocket.transform.position;
        planet1.transform.position = center;
        // PlaceHolder.transform.position = center;
        rocket.transform.position = center;
        for (float t = 0f; t <= 1; t += Time.deltaTime / time)
        {
            planet1.transform.position = Vector3.Lerp(center, planet1Pos, t); ;
            // PlaceHolder.transform.position = Vector3.Lerp(center, placeHolderPos, t); ;
            rocket.transform.position = Vector3.Lerp(center, rocketPos, t); ;

            yield return null;
        }
        planet1.transform.position = planet1Pos;
        // PlaceHolder.transform.position = placeHolderPos;
        rocket.transform.position = rocketPos;

        animationTime = false;
        playing = true;
    }

    virtual protected Vector3 GetCenterPoint()
    {
        var spawnScreen = new Vector2(Screen.width / 2, Screen.height / 2);
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(spawnScreen);

        if (Physics.Raycast(ray, out hit, 100))
        {
            return hit.point;
        }
        return Vector3.zero;
    }

    public void Play()
    {
        RandomizePosition();
        FindMeanAndSetRocket();
        StartCoroutine(SetPositionBeforePlaying(0.5f));
    }

    public void HandleConfirmButton(string planetName)
    {
        AstronomicalObject planetAnswer = DisplaySelectedPlanet(planetName);
        RocketFlyAnimation(planetAnswer);
        if (planetName == planet2.name)
        {
            Debug.Log("dung");
        }
        else
        {
            Debug.Log("sai");
            // rocket lao vao

                // StartCoroutine(MoveRocket());
            // no cai bum
        }
    }

    private AstronomicalObject DisplaySelectedPlanet(string planetName)
    {
        AstronomicalObject selectedPlanet = allPlanets.Find(planet => planet.name == planetName);

        if (selectedPlanet != null)
        {
            return Instantiate(selectedPlanet, planet2.transform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Planet with name " + planetName + " not found in allPlanets list.");
            return null;
        }
        // target.SetActive(false);
    }

    private void RocketFlyAnimation(AstronomicalObject planetAnswer) 
    {
        var attractiveForce1 = planet1.GetAttractiveForce(rocket);
        var attractiveForceAnswer = planetAnswer.GetAttractiveForce(rocket);
        if (attractiveForce1 > attractiveForceAnswer)
        {
            Debug.Log("1");
            StartCoroutine(rocket.FlyTo(planet1.gameObject));
        }
        else if (attractiveForce1 < attractiveForceAnswer)
        {
            Debug.Log("2");
            StartCoroutine(rocket.FlyTo(planetAnswer.gameObject));
        }
        else
        {
            // rocket.transform.rotation = 
        }
    }
}
