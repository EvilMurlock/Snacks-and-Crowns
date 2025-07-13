using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Attack data for a projectile
/// </summary>
public class ProjectileAttackData : ProjectileComponentData<WeaponAttack>
{
    public Attack attack;

    public override void InitializeComponent(GameObject projectile)
    {
        WeaponAttack weaponAttack = projectile.AddComponent<WeaponAttack>();
        weaponAttack.attack = attack;
        projectile.GetComponent<Projectile>().onHit.AddListener(weaponAttack.Attack);
    }
}
