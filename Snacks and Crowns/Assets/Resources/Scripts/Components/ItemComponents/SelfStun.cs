using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewKnockbackData", menuName = "ComponentData/Weapon/Knockback")]

public class SelfStun : ComponentData<WeaponKnockback>
{
    public float stunDuration;
    public override void InicializeComponent(GameObject self)
    {
        if(self.GetComponent<Player_Movement>() != null ) self.GetComponent<Player_Movement>().Stun(stunDuration);
    }
}
