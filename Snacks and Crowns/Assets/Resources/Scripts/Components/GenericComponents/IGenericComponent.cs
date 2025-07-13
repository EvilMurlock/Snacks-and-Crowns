using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// Generic components that can be used in any component list
/// </summary>
public interface IGenericComponent
{
    public abstract void InitializeComponent(GameObject gameObject);
}
