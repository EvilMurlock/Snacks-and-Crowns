using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;

/// <summary>
/// Pickup object
/// </summary>
public class ItemPickup : Interactible
{
    public override bool LockMove { get { return false; } }
    public Item item;
    // Start is called before the first frame update
    void Start()
    {
        World.AddPickup(this);
        GetComponent<SpriteRenderer>().sprite = item.icon;
        item.prefab = (GameObject)Resources.Load("Prefabs/Items/Item");
        if (!gameObject.GetComponent<TagSystem>())
        {
            gameObject.AddComponent<TagSystem>();
        }
    }
    public override void Interact(GameObject player)
    {
        Inventory inventory = player.GetComponent<Inventory>();
        if (inventory.HasEmptySpace(1))
        {
            inventory.AddItem(item);
            Destroy(gameObject);
        }
    }
    public static ItemPickup GetItemPickupWithItem(Item item)
    {
        foreach (ItemPickup itemPickup in GameObject.FindObjectsByType<ItemPickup>(FindObjectsSortMode.None))
        {
            if (itemPickup.item == item)
                return itemPickup;
        }
        return null;
    }
}
