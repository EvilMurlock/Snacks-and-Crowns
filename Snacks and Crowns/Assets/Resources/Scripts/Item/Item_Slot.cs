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
        //panel.GetComponent<Image>().color = new Color(255, 255, 255, 255);
        panel.GetComponentsInChildren<Image>()[1].color = new Color(255, 255, 255, 255);
        if (Is_Not_Empty()) panel.GetComponentsInChildren<Image>()[1].sprite = item.icon;//panel.GetComponent<Image>().sprite = item.icon;
        else Remove_Item();//panel.GetComponentsInChildren<Image>()[1].sprite = null;//panel.GetComponent<Image>().color = new Color(0, 0, 0, 0);
    }
    public void Remove_Item()
    {
        item = null;
        panel.GetComponentsInChildren<Image>()[1].sprite = null;
        panel.GetComponentsInChildren<Image>()[1].color = new Color(0, 0, 0, 0);

        //panel.GetComponent<Image>().color = new Color(0, 0, 0, 0);
    }
    public void ChangeBackground(Sprite sprite)
    {
        panel.GetComponent<Image>().sprite = sprite;
        //panel.GetComponentsInChildren<Image>()[1].sprite = sprite;
    }
    public override void Change_Colour(Color color)
    {
        panel.GetComponent<Image>().color = color;
        //panel.GetComponentsInChildren<Image>()[1].color = color;
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
