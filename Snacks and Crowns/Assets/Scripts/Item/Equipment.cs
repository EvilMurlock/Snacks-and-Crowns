using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New_Item", menuName = "Inventory/Equipment")]

public class Equipment : Item
{
    public Equipment_Slot equipment_slot;
    public GameObject prefab_eq;
    public GameObject instance;
    public override void Use()
    {
        base.Use();
    }
    public virtual void Instantiate_Eq(Transform parent)
    {

    }
    public virtual void Destroy_Eq()
    {
        Destroy(instance);
        instance = null;
        Debug.Log("EQ destroyed");
    }
}
public enum Equipment_Slot {none, hand, body, miscelanious}