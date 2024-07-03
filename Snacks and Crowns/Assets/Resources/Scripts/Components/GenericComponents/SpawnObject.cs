using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnObject : ComponentDataGeneric, IGenericComponent
{
    [SerializeField]
    GameObject template;
    [SerializeField]
    Vector2 offset;
    public override void InicializeComponent(GameObject self)
    {
        GameObject spawned = GameObject.Instantiate(template, self.transform);
        spawned.transform.SetLocalPositionAndRotation(offset, Quaternion.Euler(0, 0, 0));// תת self.transform.rotation); ; ; ; ;// = self.transform.position + (Vector3)offset;
        //spawned.transform.rotation = self.transform.rotation;
    }
}
