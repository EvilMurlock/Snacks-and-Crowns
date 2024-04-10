using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New_Item", menuName = "Inventory/Equipment")]

public class Equipment : Item
{
    [SerializeReference] private List<ComponentDataGeneric> componentDataEquipment;
    public EquipmentSlot equipmentSlot;
    public GameObject equipmentPrefab;
    [HideInInspector]
    public GameObject instance;
    public void UseEquipment()
    {
        instance.GetComponent<Hand_Item_Controler>().Use();
    }
    public virtual void InstantiateEquipment(Transform parent)
    {
        instance = Instantiate(equipmentPrefab, parent);
        instance.GetComponent<SpriteRenderer>().sprite = icon;
        foreach(ComponentDataGeneric comData in componentDataEquipment)
        {
            comData.InicializeComponent(instance, (Item) this);
        }
    }
    public virtual void DestroyEquipmentInstance()
    {
        Destroy(instance);
        instance = null;
    }
    public void AddDataEquipment(ComponentDataGeneric data)
    {
        componentDataEquipment.Add(data);
    }

}
public enum EquipmentSlot {none, hand, body, miscelanious}