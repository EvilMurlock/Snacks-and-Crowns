using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MenuSlot
{
    public void AddItem(Item new_item)
    {
    }
    public void RemoveItem()
    {
        //panel.GetComponent<Image>().color = new Color(0, 0, 0, 0);
    }
    public bool IsEmpty()
    {
        return item == null;
    }
    public bool IsNotEmpty()
    {
        return item != null;
    }

    public Item GetItem()
    {
        return item;
    }
}
