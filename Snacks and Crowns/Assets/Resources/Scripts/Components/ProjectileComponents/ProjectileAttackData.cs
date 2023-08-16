using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ProjectileAttackData : ProjectileComponentData<WeaponAttack>
{
    public Attack attack;

    public override void InicializeComponent(GameObject projectile)
    {
        WeaponAttack weaponAttack = projectile.AddComponent<WeaponAttack>();
        weaponAttack.attack = attack;
        projectile.GetComponent<Projectile>().onHit.AddListener(weaponAttack.Attack);
    }
}
