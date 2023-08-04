using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ProjectileAttackData : ProjectileComponentData<WeaponAttack>
{
    public Attack attack;

    public override void InicializeComponent(GameObject weapon)
    {
        WeaponAttack weaponAttack = weapon.AddComponent<WeaponAttack>();
        weaponAttack.attack = attack;
        weapon.GetComponent<Projectile>().onHit.AddListener(weaponAttack.Attack);
    }
}
