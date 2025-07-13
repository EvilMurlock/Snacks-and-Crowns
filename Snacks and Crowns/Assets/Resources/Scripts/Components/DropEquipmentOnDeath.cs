using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;

/// <summary>
/// Drops equipment when this character dies
/// </summary>
public class DropEquipmentOnDeath : MonoBehaviour
{
    void Start()
    {
        GetComponent<Damageable>().death.AddListener(DropItems);
    }
    void DropItems()
    {
        foreach (Item item in GetComponent<EquipmentManager>().Equipments)
        {
            Item.DropItem(item, transform.position);

        }
        for (int i = 0; i < GetComponent<EquipmentManager>().Equipments.Length; i++)
        { 
            GetComponent<EquipmentManager>().UnEquipItem(i); 
        }

    }

}
