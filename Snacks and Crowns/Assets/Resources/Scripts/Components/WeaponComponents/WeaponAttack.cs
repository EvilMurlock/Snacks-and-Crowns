using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Attack data
/// </summary>
public class WeaponAttack : MonoBehaviour
{
    public Attack attack;
    public void Attack(GameObject g)
    {
        
        Damageable damagable = g.GetComponent<Damageable>();
        if(damagable != null)
        {
            damagable.TakeDamage(attack);
        }
    }
}
