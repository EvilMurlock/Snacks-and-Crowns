using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ItemInfo : MonoBehaviour
{
    ItemSlot itemSlot;
    TextMeshProUGUI itemName;
    TextMeshProUGUI itemDescription;
    private void Awake()
    {
        itemSlot = new ItemSlot();
        //itemSlot.panel = this.transform.Find("Item_Icon").gameObject;
        itemName = this.transform.Find("Name").gameObject.GetComponent<TextMeshProUGUI>();
        itemDescription = this.transform.Find("Description").gameObject.GetComponent<TextMeshProUGUI>();

    }
    public void LoadNewItem(Item item)
    {
        if (item != null) 
        {
            itemSlot.AddItem(item);
            itemName.text = item.name;
            itemDescription.text = item.description; 
        }
        else
        {
            itemSlot.RemoveItem();
            itemName.text = "";
            itemDescription.text = "";

        }
    }
}
