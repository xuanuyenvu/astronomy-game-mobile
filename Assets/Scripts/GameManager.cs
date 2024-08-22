using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public IGamePlay[] gamePlays;

    [HideInInspector] public int id;

    public void Initialize(int levelId)
    {
        id = levelId;
        Debug.Log("id : " + id);
        OnStartGame();
    }

    private void OnStartGame()
    {
        var current = Instantiate(gamePlays[id]);
        IGamePlay iGamePlay = current.GetComponent<IGamePlay>();


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


        if (id != 1)
        {
            iGamePlay.cardController.idGamePlay = id;
        }
        iGamePlay.Play();
    }
}
