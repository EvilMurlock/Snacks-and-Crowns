using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Delets this GameObject on hit
/// </summary>
public class DeleteOnHitData : ProjectileComponentData<DeleteOnHit>
{
    public override void InitializeComponent(GameObject weapon)
    {
        DeleteOnHit deleteSelf = weapon.AddComponent<DeleteOnHit>();
        deleteSelf.GetComponent<Projectile>().onHit.AddListener(deleteSelf.Die);
    }
}
