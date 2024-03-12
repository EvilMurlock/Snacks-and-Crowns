using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ItemInfo : MonoBehaviour
{
    MenuSlot itemDisplaySlot;
    TextMeshProUGUI itemName;
    TextMeshProUGUI itemDescription;
    private void Awake()
    {
        //itemSlot.panel = this.transform.Find("Item_Icon").gameObject;
        itemName = this.transform.Find("Name").gameObject.GetComponent<TextMeshProUGUI>();
        itemDescription = this.transform.Find("Description").gameObject.GetComponent<TextMeshProUGUI>();

    }
    public void LoadNewItem(Item item)
    {
        if (item != null) 
        {
            itemDisplaySlot.AddItem(item);
            itemName.text = item.name;
            itemDescription.text = item.description; 
        }
        else
        {
            itemDisplaySlot.RemoveItem();
            itemName.text = "";
            itemDescription.text = "";

        }
    }
}
