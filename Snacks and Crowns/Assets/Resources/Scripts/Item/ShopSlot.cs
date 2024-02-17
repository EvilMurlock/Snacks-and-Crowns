using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class ShopSlot : MenuItemSlot
{
    public GameObject priceText;
    private void Start()//this firies when cahnging canvases
    {
        UpdateItem();
    }
    public void AddItem(Item newItem)
    {
        item = newItem;
        UpdateItem();
    }
    public void UpdateItem()
    {
        if (item != null)
        {
            image.GetComponent<Image>().color = new Color(255, 255, 255, 255);
            priceText.GetComponent<TextMeshProUGUI>().text = item.cost.ToString();
            image.GetComponent<Image>().sprite = item.icon;
        }
        else
        {
            image.GetComponent<Image>().color = new Color(0, 0, 0, 0);
            priceText.GetComponent<TextMeshProUGUI>().text = "";
            image.GetComponent<Image>().sprite = null;
        }

    }
    public void BuyItem(GameObject player, ShopSlot ShopSlot)
    {
        if (ShopSlot.item == null) return;
        GoldTracker goldTracker = player.GetComponent<GoldTracker>();
        if(goldTracker.Gold >= ShopSlot.item.cost)
        {
            Inventory playerInventory = player.GetComponent<Inventory>();
            if (playerInventory.AddItem(item))
            {
                goldTracker.AddGold(-ShopSlot.item.cost);
                ShopSlot.RemoveItem();
            }
            else
            {
                Debug.Log("Full inventory");
            }
        }
        else
        {
            Debug.Log("Not enough gold to buy the item");
        }
    }
    public void RemoveItem()
    {
        item = null;
        UpdateItem();
    }
}
