using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Intermediate generic class, generic type is the type of the child 
/// </summary>
/// <typeparam name="T"></typeparam>
[System.Serializable]
public class ComponentData<T> : ComponentDataGeneric
{
}


/// <summary>
/// Parent of all component classes
/// </summary>
[System.Serializable]
public class ComponentDataGeneric
{
    [SerializeField, HideInInspector] protected string name; //Name doesn't matter!! Must be a string and the first variable in class!!!

    [HideInInspector]
    public bool activateAtUse = false;
    public void SetComponentName() => name = GetType().Name;
    public ComponentDataGeneric()
    {
        SetComponentName();
    }
    public virtual void Ping() { }
    public virtual void InitializeComponent(GameObject gameObject) { }

    public virtual void InitializeComponent(GameObject player, GameObject listener) { }

    public virtual void InicializeComponent(GameObject gameObject, Item item) { }
}