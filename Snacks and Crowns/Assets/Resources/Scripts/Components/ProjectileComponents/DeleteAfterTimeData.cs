using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteAfterTimeData : ProjectileComponentData<DeleteOnHit>
{
    [SerializeField]
    float time;
    public override void InicializeComponent(GameObject weapon)
    {
        DeleteAfterTime deleteAfterTime = weapon.AddComponent<DeleteAfterTime>();
        deleteAfterTime.Init(time);
    }
}
