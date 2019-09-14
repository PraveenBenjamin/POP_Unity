using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SingletonBehaviour<T> : MonoBehaviour where T: class
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            return _instance;
        } 
    }


    protected abstract void InitializeSingleton();
    protected abstract void OnDestroySingleton();

    protected virtual void UpdateSingleton() { }

    private void Awake()
    {
        if (_instance != null)
        {
            Debug.LogWarning("Attempting to create a second instance of a SingletonBehaviour. This is not allowed. Destroying component");
            DestroyImmediate(this);
            return;
        }
        _instance = (T)System.Convert.ChangeType(this,typeof(T));
        InitializeSingleton();
    }

    private void Update()
    {
        UpdateSingleton();
    }

    ~SingletonBehaviour()
    {
        OnDestroySingleton();
    }
}
