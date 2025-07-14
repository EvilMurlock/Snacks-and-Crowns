using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// UI helper class for graphical management
/// </summary>
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
