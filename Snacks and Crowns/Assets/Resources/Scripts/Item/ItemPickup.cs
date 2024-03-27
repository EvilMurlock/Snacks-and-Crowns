using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;
public class ItemPickup : Interactible
{
    public override bool LockMove { get { return false; } }
    public Item item;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = item.icon;
        item.prefab = (GameObject)Resources.Load("Prefabs/Items/Item");
        if (!gameObject.GetComponent<TagSystem>())
        {
            gameObject.AddComponent<TagSystem>();
        }
        gameObject.GetComponent<TagSystem>().AddTags(item.Tags);
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
}
