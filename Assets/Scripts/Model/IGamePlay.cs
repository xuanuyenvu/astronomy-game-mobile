using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IGamePlay : MonoBehaviour
{
     [HideInInspector] public CameraShake cameraShake;
     [HideInInspector] public CardController cardController;
     [HideInInspector] public HealthManager healthManager;
     [HideInInspector] public ScoreManager scoreManager;
     public abstract void Play();

     public abstract IEnumerator PlayBoomAndShake();

     public abstract void HandleConfirmButton(string planetName, Vector3 planetPosition);

     public abstract void CheckDragPosition(Vector3 dragPos, string planetName);
}