using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Stuns the user
/// </summary>
public class SelfStun : ItemComponentData<WeaponKnockback>
{
    public float stunDuration;
    public override void InicializeComponent(GameObject self, Item item)
    {
        if(self.GetComponent<Movement>() != null ) self.GetComponent<Movement>().Stun(stunDuration);
    }
}
