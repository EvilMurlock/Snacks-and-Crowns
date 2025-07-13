using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseHealthData : ItemComponentData<IncreaseHealthData>
{
    public float healAmount;
    public override void InicializeComponent(GameObject self, Item item)
    {
        self.GetComponent<Damageable>().Heal(healAmount);
    }
}
