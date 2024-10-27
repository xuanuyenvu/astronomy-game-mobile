using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IGamePlay : MonoBehaviour
{
     [HideInInspector] public CameraShake cameraShake;
     [HideInInspector] public CardController cardController;
     [HideInInspector] public HealthManager healthManager;
     [HideInInspector] public UniversalLevelManager universalLevelManager;
     // [HideInInspector] public EnergyManager energyManager;
     [HideInInspector] public TimerManager timerManager;

     [HideInInspector] public Transform planetsGroupTransform;
     [HideInInspector] public Transform effectsGroupTransform;


     // dùng để lưu các planet thuộc 1 level 
     [HideInInspector] public int[] planets;

     public abstract void Play();
     public abstract void ExecuteAfterCollision(AstronomicalObject planet);
     public abstract void HandleConfirmButton(string planetName, Vector3 planetPosition);
     public abstract void CheckDragPosition(Vector3 dragPos, string planetName);
     public abstract void OnTimeOver();
     public abstract void OnFullEnergy();
}