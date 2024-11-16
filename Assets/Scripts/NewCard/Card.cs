using System;
using UnityEngine;
using DG.Tweening; 

public class Card : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    [SerializeField] private Sprite faceSprite;
    [SerializeField] private Sprite backSprite;

    private bool isFaceUp;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = backSprite;
        isFaceUp = false;
    }
    
    public void StartAnimation()
    {
        RotateCard();
    }

    private void RotateCard()
    {
        float rotationAngle = isFaceUp ? 180f : 0f;
        Sequence rotationSequence = DOTween.Sequence();
        
        rotationSequence
            .Append(transform.DOScale(0.07f, 0.2f).SetEase(Ease.InOutSine))
            .Append(transform.DORotate(new Vector3(0f, 90f, 0f), 0.15f).SetEase(Ease.InOutSine))
            .AppendCallback(() =>
            {
                spriteRenderer.sprite = isFaceUp ? backSprite : faceSprite;
            });
        
        rotationSequence.Append(transform.DORotate(new Vector3(0f, rotationAngle, 0f), 0.15f).SetEase(Ease.InOutSine))
            .OnComplete(() =>
            {
                isFaceUp = !isFaceUp;
            });

    }
}