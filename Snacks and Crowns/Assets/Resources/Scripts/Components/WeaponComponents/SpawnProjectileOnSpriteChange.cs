using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnProjectileOnSpriteChange : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float speed;
    public float extraAngle;//forr diagonal/direfently rotated projectile sprites
    public Vector2 offset; 
    public void SpawnProjectile(GameObject g)
    {

        GameObject projectile = Instantiate(projectilePrefab, (g.transform.position + (Vector3)offset), g.transform.parent.parent.rotation);
        Projectile p = projectile.GetComponent<Projectile>();
        p.speed = speed;
        Transform t = projectile.transform;
        //t.transform.Rotate(new Vector3(0, 0, extraAngle));
        p.direction = t.transform.up;
        //t.transform.Rotate(new Vector3(0, 0, -extraAngle));
        projectile.transform.Rotate(new Vector3(0, 0, extraAngle));

    }
}
