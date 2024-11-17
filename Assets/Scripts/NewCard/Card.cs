using System;
using UnityEngine;
using DG.Tweening; 

public class Card : MonoBehaviour
{
    public SpriteRenderer cardSprite;
    public ParticleSystem shinePS;
    public ParticleSystem borderPS;
    
    [SerializeField] private Sprite faceSprite;
    [SerializeField] private Sprite backSprite;
    
    private bool isFaceUp = false;

    private void Start()
    {
        shinePS.Stop();
        borderPS.Stop();
        cardSprite.sprite = backSprite;
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
            .Append(cardSprite.gameObject.transform.DOScale(0.075f, 0.2f).SetEase(Ease.InOutSine))
            .Append(cardSprite.gameObject.transform.DORotate(new Vector3(0f, 90f, 0f), 0.1f).SetEase(Ease.InOutSine))
            .AppendCallback(() =>
            {
                cardSprite.sprite = isFaceUp ? backSprite : faceSprite;
            });
        
        rotationSequence.Append(cardSprite.gameObject.transform.DORotate(new Vector3(0f, rotationAngle, 0f), 0.1f).SetEase(Ease.InOutSine))
            .OnComplete(() =>
            {
                shinePS.Play();
                borderPS.Play();
                isFaceUp = !isFaceUp;
            });

    }
    
    private void OnDestroy()
    {
        DOTween.Kill(cardSprite.gameObject.transform);
    }
}