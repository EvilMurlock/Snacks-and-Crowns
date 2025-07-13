using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;



/// <summary>
/// Drops the given items on death 
/// </summary>
public class DropItemsOnDeath : MonoBehaviour
{
    public List<Item> itemsToDrop;
    void Start()
    {
        GetComponent<Damageable>().death.AddListener(DropItems);
        if (!gameObject.GetComponent<TagSystem>())
        {
            gameObject.AddComponent<TagSystem>();
        }
        foreach(Item i in itemsToDrop)
        {
            gameObject.GetComponent<TagSystem>().AddTag(i.name+"Drop");
        }
    }
    void DropItems()
    {
        foreach(Item item in itemsToDrop)
        {
            Item.DropItem(item, transform.position);
        }
    }

}
