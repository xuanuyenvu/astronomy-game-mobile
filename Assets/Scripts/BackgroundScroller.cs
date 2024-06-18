using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundRotation : MonoBehaviour
{
    void Update()
    {
        transform.position += new Vector3(-0.09f * Time.deltaTime, 0);

        if(transform.position.x < -20.18)
        {
            transform.position = new Vector3(20.18f, transform.position.y);
        }
    }
}
