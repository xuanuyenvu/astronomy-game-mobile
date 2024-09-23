using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public IGamePlay[] gamePlays;
    private int el;

    public void Initialize(int levelId, int[] planets)
    {
        el = levelId - 1;
        OnStartGame(planets);
    }

    private void OnStartGame(int[] planets)
    {
        var current = Instantiate(gamePlays[el]);
        IGamePlay iGamePlay = current.GetComponent<IGamePlay>();

        iGamePlay.planets = planets;

        // object rỗng để chứa các spawnedObject trong game
        GameObject planetsGroupObject = GameObject.Find("spawnedPlanetsGroup");
        iGamePlay.planetsGroupTransform = planetsGroupObject.transform;

        GameObject effectsGroupObject = GameObject.Find("spawnedEffectsGroup");
        iGamePlay.effectsGroupTransform = effectsGroupObject.transform;

        // thành phần game
        iGamePlay.cameraShake = FindObjectOfType<CameraShake>();
        iGamePlay.cardController = FindObjectOfType<CardController>();
        iGamePlay.healthManager = FindObjectOfType<HealthManager>();
        iGamePlay.scoreManager = FindObjectOfType<ScoreManager>();

        if (el == 1 || el == 2)
        {
            iGamePlay.cardController.idGamePlay = 1;
        }
        iGamePlay.Play();
    }
}