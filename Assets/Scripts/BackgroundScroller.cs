using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundRotation : MonoBehaviour
{
    public Transform BG1;
    public Transform BG2;
    private Renderer spriteRenderer;
    public float speed = -0.06f;
    Vector3 size;
    // void Start(){
    //     spriteRenderer = BG1.GetComponent<SpriteRenderer>();
    //     size = spriteRenderer.bounds.size;
    // }
    // void Update()
    // {
    //     BG1.position += new Vector3(speed * Time.deltaTime, 0);
    //     BG2.position += new Vector3(speed * Time.deltaTime, 0);
    //     if(BG1.position.x <= -22)
    //     {
    //        BG1.position = new Vector3(BG2.position.x + size.x, BG1.position.y);
    //     }
    //     if(BG2.position.x <= -22)
    //     {
    //        BG2.position = new Vector3(BG1.position.x + size.x, BG2.position.y);
    //     }
    // }

        void Update()
    {
        BG1.position += new Vector3(speed * Time.deltaTime, 0);
        BG2.position += new Vector3(speed * Time.deltaTime, 0);
        
        if (BG1.position.x <= -22)
        {
            BG1.position = new Vector3(BG2.position.x, BG1.position.y);
        }
        if (BG2.position.x <= -22)
        {
            BG2.position = new Vector3(BG1.position.x, BG2.position.y);
        }
    }
}
