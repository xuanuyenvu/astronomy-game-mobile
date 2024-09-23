using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Thuộc tính singleton
    // public static GameManager Instance { get; private set; }

    public IGamePlay[] gamePlays;

    [HideInInspector] public int el;

    public void Initialize(int levelId, int[] planets)
    {
        switch (levelId)
        {
            case 1:
                el = levelId - 1;
                break;
            case 2:
            case 3:
                el = 1;
                break;
            default:
                el = levelId - 2;
                break;
        }

        OnStartGame(planets);
    }

    private void OnStartGame(int[] planets)
    {
        var current = Instantiate(gamePlays[el]);
        IGamePlay iGamePlay = current.GetComponent<IGamePlay>();

        iGamePlay.planets = planets;

        if (iGamePlay.cameraShake == null)
        {
            iGamePlay.cameraShake = FindObjectOfType<CameraShake>();
        }
        if (iGamePlay.cardController == null)
        {
            iGamePlay.cardController = FindObjectOfType<CardController>();
        }
        if (iGamePlay.healthManager == null)
        {
            iGamePlay.healthManager = FindObjectOfType<HealthManager>();
        }
        if (iGamePlay.scoreManager == null)
        {
            iGamePlay.scoreManager = FindObjectOfType<ScoreManager>();
        }

        if (el != 1)
        {
            iGamePlay.cardController.idGamePlay = (el);
        }
        iGamePlay.Play();
    }
}