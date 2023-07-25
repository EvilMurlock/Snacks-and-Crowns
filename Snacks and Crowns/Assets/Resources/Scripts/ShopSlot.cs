using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class ShopSlot : MonoBehaviour
{
    public GameObject button;
    public GameObject image;
    public GameObject priceText;
    public Item item;
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
            priceText.GetComponent<TextMeshProUGUI>().text = item.cost.ToString();
            image.GetComponent<Image>().sprite = item.icon;
        }
        else
        {
            priceText.GetComponent<TextMeshProUGUI>().text = "";
            image.GetComponent<Image>().sprite = null;
        }

    }
    public void BuyItem(GameObject player, ShopSlot ShopSlot)
    {
        if (ShopSlot.item == null) return;
        Player_Inventory pi = player.GetComponent<Player_Inventory>();
        if(pi.gold >= ShopSlot.item.cost)
        {
            if (pi.FindEmptyInventorySlot())
            {
                pi.ChangeGold(-ShopSlot.item.cost);
                pi.AddItemToInventory(ShopSlot.item);
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
