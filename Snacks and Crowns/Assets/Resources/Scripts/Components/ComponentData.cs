using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ComponentData<T> : ComponentDataGeneric
{
    // T is a component data type that says wich component to generate - only maters for some components, i think
    //public T componentType;
    public ComponentData()
    {
        SetComponentName();
    }
    public override void SetComponentName() => name = GetType().Name;
    public override void Ping()
    {
        Debug.Log("I am inicialized");
    }

    public override void InicializeComponent(GameObject gameObject, Item item)
    {
    }
}
[System.Serializable]
public class ComponentDataGeneric
{
    [SerializeField, HideInInspector] protected string name; //Name doesnt matter!! Must be a string and the first variable in class!!!

    [HideInInspector]
    public bool activateAtUse = false;

    public virtual void SetComponentName() { }
    public virtual void Ping() { }
    public virtual void InicializeComponent(GameObject gameObject) { }

    public virtual void InicializeComponent(GameObject player, GameObject listener) { }

    public virtual void InicializeComponent(GameObject gameObject, Item item) { }
}