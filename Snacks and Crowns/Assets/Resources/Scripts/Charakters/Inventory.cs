using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;
using UnityEngine.Events;
public class Inventory : MonoBehaviour
{
    //[SerializeField]
    Item[] items = new Item[9];
    public Item[] Items { get { return items; } }
    [HideInInspector]
    public UnityEvent<Inventory> onChangeInventory;
    public void Start()
    {
        onChangeInventory.Invoke(this);
    }
    public void AddItem(Item item)
    {
        if (item == null) throw new System.Exception("Add Item to inventory failed, item to be added is null");
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null)
            {
                items[i] = item;
                onChangeInventory.Invoke(this);
                return;
            }
        }
        throw new System.Exception("Inventory is full, AddItem failed");
    }
    public void RemoveItem(Item item)
    {
        for (int i = 0; i<items.Length; i++)
        {
            if (items[i] == item)
            {
                items[i] = null;
                onChangeInventory.Invoke(this);
                return;
            }
        }
        throw new System.Exception("Remove item from inventory failed, item not contained in inventory: " + item.name);
    }
    public void RemoveItem(int itemIndex)
    {
        if (items[itemIndex] == null)
        {
            throw new System.Exception("Remove item from inventory failed, item is null on index: " + itemIndex);
        }
        items[itemIndex] = null;
        onChangeInventory.Invoke(this);
    }
    public Item GetItem(int index)
    {
        return items[index];
    }
    public bool HasItem(Item item)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == item) return true;
        }
        return false;
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

    public bool HasEmptySpace(int requiredSpace)
    {
        // return true if we have atleast i empty space
        int emptySpaceCount = 0;
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null) emptySpaceCount++;
        }
        return requiredSpace <= emptySpaceCount;
    }
    public void SetCapacity(int newCapacity)
    {
        items = new Item[newCapacity];
    }
}