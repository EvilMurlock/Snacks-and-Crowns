using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


/// <summary>
/// Displays currently selected item
/// </summary>
public class ItemInfo : MonoBehaviour
{
    [SerializeField]
    Image icon;
    [SerializeField]
    TextMeshProUGUI itemName;
    [SerializeField]
    TextMeshProUGUI itemDescription;
    private void Awake()
    {
        //itemSlot.panel = this.transform.Find("Item_Icon").gameObject;
        icon.sprite = null;
    }
    public void LoadNewItem(Item item)
    {
        if (item != null) 
        {
            itemName.text = item.name;
            itemDescription.text = item.description;
            icon.color = new Color(255, 255, 255, 255);
            icon.sprite = item.icon;
        }
        else
        {
            itemName.text = "";
            itemDescription.text = "";
            icon.color = new Color(255,255,255,0);
                 
            icon.sprite = null;
        }
    }
}
