using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class IButtonHandler : MonoBehaviour
{
    public string type;
    public abstract void UpdateState(int _state);
}
