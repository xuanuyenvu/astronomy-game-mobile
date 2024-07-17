using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManger : MonoBehaviour
{
    public IGamePlay [] gamePlays;
    // Start is called before the first frame update
    void Start()
    {
        var current  = Instantiate(gamePlays[0]);
        //gan vo
        current.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
