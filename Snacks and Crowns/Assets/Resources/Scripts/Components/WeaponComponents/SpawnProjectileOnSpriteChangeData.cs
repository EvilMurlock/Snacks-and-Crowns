using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// Spawns projectile when SpriteChange event from an animation fires
/// </summary>
public class SpawnProjectileOnSpriteChangeData : EquipmentComponentData<SpawnProjectileOnSpriteChange>
{
    public GameObject projectilePrefab;
    public float speed;
    public float extraAngle;
    public Vector2 offset;

    public override void InicializeComponent(GameObject weapon, Item item)
    {
        SpawnProjectileOnSpriteChange weaponAttack = weapon.AddComponent<SpawnProjectileOnSpriteChange>();
        weaponAttack.projectilePrefab = projectilePrefab;
        weaponAttack.speed = speed;
        weaponAttack.extraAngle = extraAngle;
        weaponAttack.offset = offset;

        weapon.GetComponent<HandItemControler>().spriteChangeEvent.AddListener(weaponAttack.SpawnProjectile);
    }
}
