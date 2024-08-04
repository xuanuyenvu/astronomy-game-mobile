using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManger : MonoBehaviour
{
    public IGamePlay[] gamePlays;
    public Player player;

    private int id  = 1;
    // Start is called before the first frame update
    void Start()
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
        if (id == 1)
        {
            player.gameObject.SetActive(true);
        }
        else
        {
            iGamePlay.cardController.idGamePlay = id;
        }
        iGamePlay.Play();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
