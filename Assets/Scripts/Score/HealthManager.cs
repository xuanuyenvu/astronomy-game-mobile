using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HealthManager : MonoBehaviour
{
    private int health = 5;
    public int Health => health;
    
    // public Sprite heart;
    public List<Image> hearts;
    public Image star;
    public ParticleSystem psPrefab;
    public Canvas uiCanvas;
    public EnergyManager energyManager;

    void Start()
    {
        ShowUI();
    }
    
    public void SetUp(int _health)
    {
        health = _health;
        ShowUI();
    }

    public void HideUI()
    {
        foreach (var heart in hearts)
        {
            heart.enabled = false;
        }
    }
    
    public void ShowUI()
    {
        foreach (var heart in hearts)
        {
            heart.enabled = false;
        }
        for (int i = 0; i < health; i++)
        {
            hearts[i].enabled = true;
        }
    }

    public IEnumerator StartAddToEnergy()
    {
        // Tìm các trái tim có enabled = true
        for (int i = health - 1; i >= 0; i--)
        {
            if (hearts[i].enabled)
            {
                // Spawn ParticleSystem tại vị trí của trái tim trong không gian UI
                ParticleSystem particle = Instantiate(psPrefab, uiCanvas.transform);

                // Đặt vị trí ParticleSystem trong UI
                RectTransform particleRect = particle.GetComponent<RectTransform>();
                RectTransform heartRect = hearts[i].GetComponent<RectTransform>();
                RectTransform starRect = star.GetComponent<RectTransform>();

                particleRect.position = heartRect.position; 

                Vector3[] path = new Vector3[]
                {
                heartRect.position,
                (heartRect.position + starRect.position) / 2 + Vector3.up * 30,
                starRect.position
                };

                health--;
                ShowUI();
                particle.Play();

                // Di chuyển ParticleSystem theo path
                particleRect.DOPath(path, 0.6f, PathType.CatmullRom)
                    .SetEase(Ease.InOutQuint)
                    .OnComplete(() =>
                    {
                        particle.Stop();
                        Destroy(particle.gameObject, particle.main.duration);
                        // energyManager.ChangeEnergy(10);
                    });
                yield return new WaitForSeconds(0.5f);
            }
        }
    }
    
    public void ReduceHealth()
    {
        Debug.Log("bi tru roi" + health);
        health--;
        
        if (health < 0) health = 0;
        ShowUI();
    }


}
