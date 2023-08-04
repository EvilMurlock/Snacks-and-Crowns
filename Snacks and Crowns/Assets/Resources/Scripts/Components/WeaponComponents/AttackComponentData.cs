using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackComponentData : EquipmentComponentData<WeaponAttack>
{
    public Attack attack;
    
    public override void InicializeComponent(GameObject weapon)
    {
        WeaponAttack weaponAttack = weapon.AddComponent<WeaponAttack>();
        weaponAttack.attack = attack;
        weapon.GetComponent<Hand_Item_Controler>().onHitEvent.AddListener(weaponAttack.Attack);
    }
}
