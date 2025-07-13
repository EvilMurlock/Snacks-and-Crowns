using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Spawns given objects
/// </summary>
public class SpawnObject : ComponentDataGeneric, IGenericComponent
{
    [SerializeField]
    GameObject template;
    [SerializeField]
    Vector2 offset;
    public override void InitializeComponent(GameObject self)
    {
        GameObject spawned = GameObject.Instantiate(template, self.transform);
        spawned.transform.SetLocalPositionAndRotation(offset, Quaternion.Euler(0, 0, 0));
    }
}
