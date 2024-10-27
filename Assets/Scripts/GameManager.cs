using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public IGamePlay[] gamePlays;
    private int el;
    private IGamePlay currentGamePlay;

    public IGamePlay CurrentGamePlay
    {
        get { return currentGamePlay; }
        private set { currentGamePlay = value; } 
    }


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Initialize(int levelId, int[] planets, int cardsDisplayed)
    {
        el = levelId - 1;
        OnStartGame(planets, cardsDisplayed);
    }

    private void OnStartGame(int[] planets, int cardsDisplayed)
    {
        if (currentGamePlay != null)
        {
            DestroyCurrentGamePlay();
        }

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

        // Player _player = FindObjectOfType<Player>();
        // _player.SetPlayer(currentGamePlay);

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
}