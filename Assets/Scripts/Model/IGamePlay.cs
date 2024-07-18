using UnityEngine;
public abstract class IGamePlay : MonoBehaviour
{
     [HideInInspector] public CameraShake cameraShake;
     [HideInInspector] public CardController cardController;
     [HideInInspector] public HealthManager healthManager;
     [HideInInspector] public ScoreManager scoreManager;
     public abstract void Play();
}