using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManger : MonoBehaviour
{
    public IGamePlay[] gamePlays;
    // Start is called before the first frame update
    void Start()
    {
        // var current = Instantiate(gamePlays[0]);
        var current = Instantiate(gamePlays[1]);
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
        // iGamePlay.cardController.idGamePlay = 0;
        iGamePlay.Play();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
