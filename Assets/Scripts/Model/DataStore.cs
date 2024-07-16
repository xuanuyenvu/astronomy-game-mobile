using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataStore : MonoBehaviour
{
    public GameObject storedObject { get; private set; }
    public Vector3 position { get; private set; }
    public Quaternion rotation { get; private set; }
    public Vector3 scale { get; private set; }

    // Constructor
    public DataStore(GameObject _object,
                     Vector3 _position,
                     Quaternion _rotation,
                     Vector3 _scale = default(Vector3))
    {
        storedObject = _object;
        position = _position;
        rotation = _rotation;

        if (_scale == default(Vector3) || _scale == Vector3.zero)
        {
            scale = _object.transform.localScale;
        }
        else
        {
            scale = _scale;
        }
    }

    public GameObject SpawnObject()
    {
        GameObject clone = Instantiate(storedObject, position, rotation);
        clone.transform.localScale = scale;
        clone.name = clone.name.Replace("(Clone)", "");
        return clone;
    }
}
