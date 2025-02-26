using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;


public abstract class IButtonHandler : MonoBehaviour
{
    public string type;

    public abstract void UpdateState(string levelState, int chapter);
    public abstract int GetLevel();
}
