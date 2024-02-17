using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;
using UnityEngine.Events;
public class Inventory : MonoBehaviour
{
    public int capacity = 9;
    [SerializeField]
    List<Item> inventory = new List<Item>();
    public List<Item> GetInventory { get { return inventory; } }
    [HideInInspector]
    public UnityEvent<List<Item>> onChangeInventory;
    public void Start()
    {
        onChangeInventory.Invoke(inventory);
    }
    public bool AddItem(Item item)
    {
        if (inventory.Count >= capacity) return false;
        inventory.Add(item);
        onChangeInventory.Invoke(inventory);
        return true;
    }
    public bool RemoveItem(Item item)
    {
        bool result = inventory.Remove(item);
        onChangeInventory.Invoke(inventory);
        return result;
    }
    public void UseItem(int index)
    {
        if (inventory[index] != null)
        {
            inventory[index].Use(this.gameObject);
            if (inventory[index].singleUse)
            {
                RemoveItem(inventory[index]);
            }
        }
    }

}
