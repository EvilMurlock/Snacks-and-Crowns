using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ItemInfo : MonoBehaviour
{
    Item_Slot itemSlot;
    TextMeshProUGUI itemName;
    TextMeshProUGUI itemDescription;
    private void Awake()
    {
        itemSlot = new Item_Slot();
        itemSlot.panel = this.transform.Find("Item_Icon").gameObject;
        Debug.Log(itemSlot);
        itemName = this.transform.Find("Name").gameObject.GetComponent<TextMeshProUGUI>();
        itemDescription = this.transform.Find("Description").gameObject.GetComponent<TextMeshProUGUI>();

    }
    public void LoadNewItem(Item item)
    {
        if (item != null) 
        {
            itemSlot.Add_Item(item);
            itemName.text = item.name;
            itemDescription.text = item.description; 
        }
        else
        {
            itemSlot.Remove_Item();
            itemName.text = "";
            itemDescription.text = "";

        }
    }
}
