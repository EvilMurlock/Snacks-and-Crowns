using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionOnDeleteData : ProjectileComponentData<Explosion>
{
    public float radius;
    public float maxKnockback;
    public float minKnockback;
    public Attack attack;
    public GameObject explosionEffect;
    public override void InicializeComponent(GameObject projectile)
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
