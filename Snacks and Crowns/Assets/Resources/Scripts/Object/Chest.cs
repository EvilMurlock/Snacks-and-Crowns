using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chest : Interactible
{
    public override bool LockMove { get { return true; } }
    public GameObject prefab_chest_ui;
    public GameObject instance_chest_ui;
    public ItemSlot[] chest_inventory;
    public void Awake()
    {
        int inventory_size = (prefab_chest_ui.GetComponentsInChildren<Image>().Length -1)/2;
        chest_inventory = new ItemSlot[inventory_size];
        for (int i = 0; i< inventory_size; i++)
        {
            chest_inventory[i] = new ItemSlot();
        }
    }
    public bool AddItem(Item item)
    {
        foreach (ItemSlot slot in chest_inventory)
        {
            if (slot.IsEmpty())
            { 
                slot.AddItem(item);
                return true;
            }
        }
        return false;
    }
    public bool RemoveItem(Item item)
    {
        for(int i = 0; i < chest_inventory.Length; i++) 
        {
            if(item == chest_inventory[i].GetItem())
            {
                chest_inventory[i].RemoveItem();
                return true;
            }
        }
        return false;
    }

    public override void Interact(GameObject player)
    {
        GenerateUi(player);
    }
    public override void UnInteract(GameObject player)
    {
        DeleteUi(player);
    }

    void GenerateUi(GameObject player)
    {
        
    }

    void DeleteUi(GameObject player)
    {

    }
}
