using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Generates an explosion
/// </summary>
public class Explosion : MonoBehaviour
{
    public float radius;
    public float maxKnockback;
    public float minKnockback;
    public Attack attack;
    public GameObject explosionEffect;
    public void Explode(GameObject parent)
    {
        //Debug.Log("EXPLODING");
        CircleCollider2D cc = this.gameObject.AddComponent<CircleCollider2D>();
        cc.isTrigger = true;
        cc.radius = radius;
        GameObject explosion = Instantiate(explosionEffect, parent.transform, false);
        explosion.transform.SetParent(null);
        Destroy(explosion, explosion.GetComponent<ParticleSystem>().main.duration);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Damageable>())
        {
            collision.GetComponent<Damageable>().TakeDamage(attack);
        }
        if (collision.GetComponent<Rigidbody2D>())
        {
            float forceRatio = 1-((collision.transform.position - this.transform.position).magnitude / radius);
            float force = (maxKnockback - minKnockback) * forceRatio;
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(force* collision.transform.position - this.transform.position);
        }
        if (collision.GetComponent<Movement>())
        {
            float forceRatio = 1 - ((collision.transform.position - this.transform.position).magnitude / radius);
            float force = (maxKnockback - minKnockback) * forceRatio;
            collision.GetComponent<Movement>().Stun(force / 200);
        }

    }
}
