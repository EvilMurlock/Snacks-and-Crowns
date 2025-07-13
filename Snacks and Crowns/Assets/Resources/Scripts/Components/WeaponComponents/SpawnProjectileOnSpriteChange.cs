using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawns projectile when event SpriteChange from an animation fires
/// </summary>
public class SpawnProjectileOnSpriteChange : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float speed;
    public float extraAngle;//for diagonal/differently rotated projectile sprites
    public Vector2 offset; 
    public void SpawnProjectile(GameObject g)
    {

        GameObject projectile = Instantiate(projectilePrefab, (g.transform.position + (Vector3)offset), g.transform.parent.parent.rotation);
        Projectile p = projectile.GetComponent<Projectile>();
        p.speed = speed;
        Transform t = projectile.transform;
        p.direction = t.transform.up;
        projectile.transform.Rotate(new Vector3(0, 0, extraAngle));

    }
}
