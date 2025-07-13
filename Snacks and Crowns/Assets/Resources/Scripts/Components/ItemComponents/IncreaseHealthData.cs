using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Increases user health
/// </summary>
public class IncreaseHealthData : ItemComponentData<IncreaseHealthData>
{
    public float healAmount;
    public override void InicializeComponent(GameObject self, Item item)
    {
        self.GetComponent<Damageable>().Heal(healAmount);
    }
}
