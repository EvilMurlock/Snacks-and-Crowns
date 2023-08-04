using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentData<T> : ComponentDataGeneric
{
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

    public override void InicializeComponent(GameObject gameObject)
    {
    }
}
[System.Serializable]
public class ComponentDataGeneric
{
    [SerializeField, HideInInspector] protected string name;
    public virtual void SetComponentName() { }
    public virtual void Ping() { }
    public virtual void InicializeComponent(GameObject gameObject) { }
}