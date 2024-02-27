using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponKnockback : MonoBehaviour
{
    public float knockback;
    public float stunTime;
    public void Knockback(GameObject g)
    {
        Rigidbody2D rb = g.GetComponent<Rigidbody2D>();
        if(rb != null)
        {
            Vector2 direction = -(gameObject.transform.parent.parent.position - g.transform.position).normalized; 
            rb.AddForce(direction*knockback);
            Movement move = g.GetComponent<Movement>();
            if (move != null)
            {
                move.Stun(stunTime);
            }
        }
    }
}
