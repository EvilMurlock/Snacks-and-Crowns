using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class ShopSlot : MenuSlot
{
    [SerializeField]
    TextMeshProUGUI priceText;

    public override void UpdateItem()
    {
        base.UpdateItem();
        if(item == null) priceText.text = "";
        else priceText.text = System.Convert.ToString(item.cost);
    }
}
