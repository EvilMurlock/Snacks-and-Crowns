using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Data for DeleteAfterTime component
/// </summary>
public class DeleteAfterTimeData : ProjectileComponentData<DeleteOnHit>
{
    [SerializeField]
    float time;
    public override void InitializeComponent(GameObject weapon)
    {
        DeleteAfterTime deleteAfterTime = weapon.AddComponent<DeleteAfterTime>();
        deleteAfterTime.Init(time);
    }
}
