using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttack : MonoBehaviour
{
    public Attack attack;
    public void Attack(GameObject g)
    {
        
        Damagable damagable = g.GetComponent<Damagable>();
        if(damagable != null)
        {
            damagable.TakeDamage(attack);
        }
    }
}
