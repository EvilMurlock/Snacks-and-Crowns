using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfStun : ItemComponentData<WeaponKnockback>
{
    public float stunDuration;
    public override void InicializeComponent(GameObject self, Item item)
    {
        if(self.GetComponent<Player_Movement>() != null ) self.GetComponent<Player_Movement>().Stun(stunDuration);
    }
}
