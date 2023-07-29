using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewKnockbackData", menuName = "ComponentData/Weapon/Knockback")]

public class KnockbackComponentData : ComponentData<WeaponKnockback>
{
    public float knockback;
    public float stunTime;

    public override void InicializeComponent(GameObject weapon)
    {
        WeaponKnockback weaponKnockback = weapon.AddComponent<WeaponKnockback>();
        weaponKnockback.knockback = knockback;
        weaponKnockback.stunTime = stunTime;
        weapon.GetComponent<Hand_Item_Controler>().onHitEvent.AddListener(weaponKnockback.Knockback);
    }
}
