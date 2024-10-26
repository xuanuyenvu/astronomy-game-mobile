using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public IGamePlay[] gamePlays;
    // public EndGamePlay endGamePlay;
    private int el;
    private IGamePlay currentGamePlay;

    public void Initialize(int levelId, int[] planets, int cardsDisplayed)
    {
        el = levelId - 1;
        OnStartGame(planets, cardsDisplayed);
    }

    private void OnStartGame(int[] planets, int cardsDisplayed)
    {
        var current = Instantiate(gamePlays[el]);
        currentGamePlay = current.GetComponent<IGamePlay>();

        currentGamePlay.planets = planets;

        // object rỗng để chứa các spawnedObject trong game
        GameObject planetsGroupObject = GameObject.Find("spawnedPlanetsGroup");
        currentGamePlay.planetsGroupTransform = planetsGroupObject.transform;

        GameObject effectsGroupObject = GameObject.Find("spawnedEffectsGroup");
        currentGamePlay.effectsGroupTransform = effectsGroupObject.transform;

        // thành phần game
        currentGamePlay.cameraShake = FindObjectOfType<CameraShake>();
        currentGamePlay.cardController = FindObjectOfType<CardController>();
        currentGamePlay.healthManager = FindObjectOfType<HealthManager>();
        currentGamePlay.universalLevelManager = FindObjectOfType<UniversalLevelManager>();
        currentGamePlay.energyManager = FindObjectOfType<EnergyManager>();
        currentGamePlay.timerManager = FindObjectOfType<TimerManager>();

        currentGamePlay.cardController.cardsDisplayed = cardsDisplayed;
        currentGamePlay.cardController.gamePlayId = el;
        
        currentGamePlay.cameraShake.IsShake = -1;
        
        Player _player = FindObjectOfType<Player>();
        _player.SetPlayer(currentGamePlay);

        if (currentGamePlay.timerManager != null)
        {
            currentGamePlay.timerManager.StartTimer();
        }
        
        currentGamePlay.Play();
    }

    public void DestroyCurrentGamePlay()
    {
        if (currentGamePlay != null)
        {
            Debug.Log("Destroying current game play " + currentGamePlay);
            Destroy(currentGamePlay.gameObject); 
            currentGamePlay = null; 
        }
    }

    // public void UpdateFinalEnergy()
    // {
    //     int health = currentGamePlay.healthManager.health; // * 10
    //     for (int i = 0; i < health; i++)
    //     {
    //         currentGamePlay.energyManager.ChangeEnergy(10);
    //     }

    //     float time = currentGamePlay.timerManager.GetRemainingTimePercentage(); // * 10
    //     currentGamePlay.energyManager.ChangeEnergy(time * 50);
    // }
}