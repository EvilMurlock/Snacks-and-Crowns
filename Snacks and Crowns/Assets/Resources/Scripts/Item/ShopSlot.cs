using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class ShopSlot : MenuSlot
{
    [SerializeField]
    TextMeshProUGUI priceText;
    private void Start()
    {
    }
    public override void AddItem(Item newItem)
    {
        base.AddItem(newItem);
        priceText.text = System.Convert.ToString(item.cost);
    }
    public override void RemoveItem()
    {
        base.RemoveItem();
        priceText.text = "";
    }

}
