using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New_Item", menuName = "Inventory/Equipment")]

public class Equipment : Item
{
    [SerializeReference] private List<ComponentDataGeneric> componentDataEquipment;
    public Equipment_Slot equipment_slot;
    public GameObject prefab_eq;
    [HideInInspector]
    public GameObject instance;
    public void UseEquipment()
    {
        instance.GetComponent<Hand_Item_Controler>().Use();
    }
    public virtual void Instantiate_Eq(Transform parent)
    {
        instance = Instantiate(prefab_eq, parent);
        instance.GetComponent<SpriteRenderer>().sprite = icon;
        foreach(ComponentDataGeneric comData in componentDataEquipment)
        {
            comData.InicializeComponent(instance);
        }
    }
    public virtual void Destroy_Eq()
    {
        Destroy(instance);
        instance = null;
    }
    public void AddDataEquipment(ComponentDataGeneric data)
    {
        componentDataEquipment.Add(data);
    }

}
public enum Equipment_Slot {none, hand, body, miscelanious}