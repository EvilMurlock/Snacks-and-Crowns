using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ItemInfo : MonoBehaviour
{
    [SerializeField]
    Image itemIcon;
    [SerializeField]
    TextMeshProUGUI itemName;
    [SerializeField]
    TextMeshProUGUI itemDescription;
    private void Awake()
    {
        //itemSlot.panel = this.transform.Find("Item_Icon").gameObject;
        itemIcon.sprite = null;
    }
    public void LoadNewItem(Item item)
    {
        if (item != null) 
        {
            itemName.text = item.name;
            itemDescription.text = item.description;
            itemIcon.sprite = item.icon;
        }
        else
        {
            itemName.text = "";
            itemDescription.text = "";
            itemIcon.sprite = null;
        }
    }
}
