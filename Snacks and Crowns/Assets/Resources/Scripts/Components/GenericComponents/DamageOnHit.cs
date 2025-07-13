using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collision2D_Proxy))]
public class DamageOnHit : MonoBehaviour
{
    [SerializeField]
    bool onlyOnce = true;
    [SerializeField]
    Attack attack;

    List<GameObject> pastCollisions = new List<GameObject>();
    void Start()
    {
        GetComponent<Collision2D_Proxy>().OnTriggerEnter2D_Action += Attack;
    }
    void Attack(Collider2D collider)
    {
        if (onlyOnce)
        {
            if (pastCollisions.Contains(collider.gameObject)) 
                return;
            else
                pastCollisions.Add(collider.gameObject);
        }
        Damageable damagable = collider.gameObject.GetComponent<Damageable>();
        if (damagable != null)
        {
            damagable.TakeDamage(attack);
        }
    }
}
