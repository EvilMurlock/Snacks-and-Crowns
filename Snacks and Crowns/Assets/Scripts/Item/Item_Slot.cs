using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Item_Slot : Menu_Slot
{
    public Item item;
    public GameObject panel;
    public Equipment_Slot equipment_slot = Equipment_Slot.none;

    public void Add_Item(Item new_item)
    {
        item = new_item;
        if(Is_Not_Empty()) panel.GetComponentsInChildren<Image>()[1].sprite = item.icon;
        else panel.GetComponentsInChildren<Image>()[1].sprite = null;
    }
    public void Remove_Item()
    {
        item = null;
        panel.GetComponentsInChildren<Image>()[1].sprite = null;
    }
    public override void Change_Colour(Color color)
    {
        panel.GetComponent<Image>().color = color;
    }
    public bool Is_Empty()
    {
        return item == null;
    }
    public bool Is_Not_Empty()
    {
        return item != null;
    }
}
