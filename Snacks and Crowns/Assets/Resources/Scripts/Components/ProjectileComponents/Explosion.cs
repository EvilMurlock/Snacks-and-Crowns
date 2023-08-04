using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float radius;
    public float maxKnockback;
    public float minKnockback;
    public Attack attack;
    public void Explode(GameObject parent)
    {
        Debug.Log("EXPLODING");
        CircleCollider2D cc = this.gameObject.AddComponent<CircleCollider2D>();
        cc.isTrigger = true;
        cc.radius = radius;
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Damagable>())
        {
            collision.GetComponent<Damagable>().TakeDamage(attack);
        }
        if (collision.GetComponent<Rigidbody2D>())
        {
            float forceRatio = 1-((collision.transform.position - this.transform.position).magnitude / radius);
            float force = (maxKnockback - minKnockback) * forceRatio;
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(force* collision.transform.position - this.transform.position);
        }

    }
}
