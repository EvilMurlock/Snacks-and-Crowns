using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// Attack data
/// </summary>
public class AttackComponentData : EquipmentComponentData<WeaponAttack>
{
    public Attack attack;
    
    public override void InicializeComponent(GameObject weapon, Item item)
    {
        WeaponAttack weaponAttack = weapon.AddComponent<WeaponAttack>();
        weaponAttack.attack = attack;
        weapon.GetComponent<HandItemController>().onHitEvent.AddListener(weaponAttack.Attack);
    }
}
