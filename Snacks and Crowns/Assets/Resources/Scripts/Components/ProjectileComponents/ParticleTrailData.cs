using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Generates particles generator for a given projectile
/// </summary>
public class ParticleTrailData : ProjectileComponentData<ParticleTrailData>
{
    public GameObject particleEffect;
    public override void InitializeComponent(GameObject projectile)
    {
        GameObject.Instantiate(particleEffect, projectile.transform, false);
    }

}
