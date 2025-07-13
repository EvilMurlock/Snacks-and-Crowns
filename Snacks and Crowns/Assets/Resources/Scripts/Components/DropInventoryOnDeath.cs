using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;
public class DropInventoryOnDeath : MonoBehaviour
{
    void Start()
    {
        GetComponent<Damageable>().death.AddListener(DropItems);
    }
    void DropItems()
    {
        foreach(Item item in GetComponent<Inventory>())
        {
            Item.DropItem(item, transform.position);
        }
        GetComponent<Inventory>().Clear();
    }
}
