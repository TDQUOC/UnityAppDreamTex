using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonBase <T>: MonoBehaviour
{
    public static T Instance { get; private set; }
    
    protected virtual void Awake()
    {
        if (Instance == null)
        {
            Instance = gameObject.GetComponent<T>();
        }
        else
        {
            Debug.LogWarning("Multiple instances of " + gameObject.name + " found. Destroying instance.");
            Destroy(gameObject);
        }
    }
}
