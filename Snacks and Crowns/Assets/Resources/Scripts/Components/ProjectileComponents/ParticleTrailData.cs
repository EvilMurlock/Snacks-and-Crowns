using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleTrailData : ProjectileComponentData<ParticleTrailData>
{
    public GameObject particleEffect;
    public override void InicializeComponent(GameObject projectile)
    {
        GameObject.Instantiate(particleEffect, projectile.transform, false);
    }

}
