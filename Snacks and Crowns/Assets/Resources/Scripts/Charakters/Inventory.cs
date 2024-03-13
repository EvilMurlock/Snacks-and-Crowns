using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;
using UnityEngine.Events;
public class Inventory : MonoBehaviour
{
    public int capacity = 9;
    [SerializeField]
    List<Item> items = new List<Item>();
    public List<Item> Items { get { return items; } }
    [HideInInspector]
    public UnityEvent<List<Item>> onChangeInventory;
    public void Start()
    {
        onChangeInventory.Invoke(items);
    }
    public bool AddItem(Item item)
    {
        if (items.Count >= capacity) return false;
        items.Add(item);
        onChangeInventory.Invoke(items);
        return true;
    }
    public bool RemoveItem(Item item)
    {
        bool result = items.Remove(item);
        onChangeInventory.Invoke(items);
        return result;
    }
    public Item GetItem(int index)
    {
        return items[index];
    }
    public void UseItem(int index)
    {
        if (items[index] != null)
        {
            items[index].Use(this.gameObject);
            if (items[index].singleUse)
            {
                RemoveItem(items[index]);
            }
        }
    }

    public bool HasEmptySpace(int i)
    {
        // return true if we have atleast i empty space
        int emptySpaceCount = capacity - items.Count;
        return i <= emptySpaceCount;
    }
}
