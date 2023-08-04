using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteOnHitData : ProjectileComponentData<DeleteOnHit>
{
    public override void InicializeComponent(GameObject weapon)
    {
        DeleteOnHit deleteSelf = weapon.AddComponent<DeleteOnHit>();
        deleteSelf.GetComponent<Projectile>().onHit.AddListener(deleteSelf.Die);
    }
}
