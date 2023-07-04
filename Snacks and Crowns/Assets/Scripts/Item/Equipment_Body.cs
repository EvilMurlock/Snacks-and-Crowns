using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New_Item", menuName = "Inventory/Equipment_Body")]

public class Equipment_Body : Equipment
{
    public override void Instantiate_Eq(Transform parent)
    {
        instance = Instantiate(prefab_eq, parent);
        instance.GetComponent<SpriteRenderer>().sprite = icon;
    }
}