using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Knockback data
/// </summary>
public class KnockbackComponentData : EquipmentComponentData<WeaponKnockback>
{
    public float knockback;
    public float stunTime;

    public override void InicializeComponent(GameObject weapon, Item item)
    {
        WeaponKnockback weaponKnockback = weapon.AddComponent<WeaponKnockback>();
        weaponKnockback.knockback = knockback;
        weaponKnockback.stunTime = stunTime;
        weapon.GetComponent<HandItemControler>().onHitEvent.AddListener(weaponKnockback.Knockback);
    }
}
