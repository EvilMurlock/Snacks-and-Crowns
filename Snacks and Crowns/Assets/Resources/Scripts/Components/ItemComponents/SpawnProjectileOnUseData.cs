using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnProjectileOnUseData : ItemComponentData<SpawnProjectileOnUseData>
{
    public GameObject projectilePrefab;
    public float speed;
    public float extraAngle;
    public Vector2 offset;

    public override void InicializeComponent(GameObject g)
    {
        GameObject projectile = Object.Instantiate(projectilePrefab, (g.transform.position), g.transform.rotation);
        Projectile p = projectile.GetComponent<Projectile>();
        p.speed = speed;
        Transform t = projectile.transform;
        //t.transform.Rotate(new Vector3(0, 0, extraAngle));
        p.direction = t.transform.up;
        //t.transform.Rotate(new Vector3(0, 0, -extraAngle));
        projectile.transform.Rotate(new Vector3(0, 0, extraAngle));
        projectile.transform.position += projectile.transform.rotation * (Vector3)offset;
    }
}
