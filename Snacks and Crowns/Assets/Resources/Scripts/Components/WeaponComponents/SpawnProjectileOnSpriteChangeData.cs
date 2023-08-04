using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SpawnProjectileOnSpriteChangeData : EquipmentComponentData<SpawnProjectileOnSpriteChange>
{
    public GameObject projectilePrefab;
    public float speed;
    public float extraAngle;
    public Vector2 offset;

    public override void InicializeComponent(GameObject weapon)
    {
        SpawnProjectileOnSpriteChange weaponAttack = weapon.AddComponent<SpawnProjectileOnSpriteChange>();
        weaponAttack.projectilePrefab = projectilePrefab;
        weaponAttack.speed = speed;
        weaponAttack.extraAngle = extraAngle;
        weaponAttack.offset = offset;

        weapon.GetComponent<Hand_Item_Controler>().spriteChangeEvent.AddListener(weaponAttack.SpawnProjectile);
    }
}
