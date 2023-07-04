using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New_Item", menuName = "Inventory/Equipment_Hand")]

public class Equipment_Hand : Equipment
{
    public override void Use()
    {
        base.Use();
        instance.GetComponent<Hand_Item_Controler>().Use();
    }
    public override void Instantiate_Eq(Transform parent)
    {
        instance = Instantiate(prefab_eq, parent);
        instance.GetComponent<SpriteRenderer>().sprite = icon;
    }

}