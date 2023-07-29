using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentData<T> : ComponentDataGeneric
{
    //public T componentType;
    public override void Ping()
    {
        Debug.Log("I am inicialized");
    }

    public override void InicializeComponent(GameObject gameObject)
    {
    }
}
[System.Serializable]
public abstract class ComponentDataGeneric
{
    public abstract void Ping();
    public abstract void InicializeComponent(GameObject gameObject);
}