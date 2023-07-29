using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewKnockbackData", menuName = "ComponentData/Weapon/Knockback")]

public class SelfStun : ComponentData<WeaponKnockback>
{
    public float stunDuration;
    public override void InicializeComponent(GameObject self)
    {
        if(self.GetComponent<Player_State_Manager>() != null ) self.GetComponent<Player_State_Manager>().Stun(stunDuration);
    }
}
