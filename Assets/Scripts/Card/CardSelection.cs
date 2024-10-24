using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CardSelection : MonoBehaviour
{
    public AstronomicalObject planet; 
    public string planetName;
    
    private static float time = 0.36f;
    private static float startPos = 1f;
    
    private Vector3 originalPosition;  
    private Vector3 originalScale;     

    void Start()
    {
        originalPosition = planet.transform.position;
        originalScale = planet.transform.localScale;

        planet.transform.localScale = originalScale * 0.5f;
        planet.transform.position = new Vector3(originalPosition.x, originalPosition.y - startPos, originalPosition.z);

        planet.transform.DOMove(originalPosition, time).SetEase(Ease.OutQuad);
        planet.transform.DOScale(originalScale, time).SetEase(Ease.OutQuad);
    }
}
