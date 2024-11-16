using UnityEngine;

public class StarScroller : MonoBehaviour
{
    public GameObject starPS1;
    public GameObject starPS2; 
    public float speed = -0.06f; 
    
    private Transform starPS1Transform; 
    private Transform starPS2Transform; 
    private Vector3 size;

    void Start()
    {
        starPS1Transform = starPS1.transform;
        starPS2Transform = starPS2.transform;
    }

    void Update()
    {
        starPS1Transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
        starPS2Transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
        
        if (starPS1Transform.position.x <= -22)
        {
            starPS1Transform.position = new Vector3(starPS2Transform.position.x + size.x, starPS1Transform.position.y, starPS1Transform.position.z);
        }
        if (starPS2Transform.position.x <= -22)
        {
            starPS2Transform.position = new Vector3(starPS1Transform.position.x + size.x, starPS2Transform.position.y, starPS2Transform.position.z);
        }
    }
}