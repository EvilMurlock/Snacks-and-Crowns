using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewKnockbackData", menuName = "ComponentData/Weapon/Knockback")]

public class IncreaseHealthData : ComponentData<WeaponKnockback>
{
    public float healAmount;
    public override void InicializeComponent(GameObject self)
    {
        self.GetComponent<Damagable>().Heal(healAmount);
    }
}