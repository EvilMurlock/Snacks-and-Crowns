using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Damanges and kncokbacks and creates partciles representing an explosion
/// </summary>
public class ExplosionOnDeleteData : ProjectileComponentData<Explosion>
{
    public float radius;
    public float maxKnockback;
    public float minKnockback;
    public Attack attack;
    public GameObject explosionEffect;
    public override void InitializeComponent(GameObject projectile)
    {
        Explosion explosion = projectile.AddComponent<Explosion>();
        explosion.radius = radius;
        explosion.maxKnockback = maxKnockback;
        explosion.minKnockback = minKnockback;
        explosion.attack = attack;
        explosion.explosionEffect = explosionEffect;

        projectile.GetComponent<Projectile>().onDestroy.AddListener(explosion.Explode);
    }

}
