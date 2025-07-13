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
    public Dictionary<GameObject,GameObject> instances = new Dictionary<GameObject, GameObject>();
    public void Innit(Equipment equipment)
    {
        Innit((Item)equipment);
        componentDataEquipment = equipment.componentDataEquipment;
        equipmentSlot = equipment.equipmentSlot;
        equipmentPrefab = equipment.equipmentPrefab;
        instances = equipment.instances;
    }
    public void UseEquipment(GameObject key)
    {
        instances[key].GetComponent<HandItemControler>().Use();
    }
    public virtual void InstantiateEquipment(Transform parent, GameObject key)
    {
        instances[key] = Instantiate(equipmentPrefab, parent);
        instances[key].GetComponent<SpriteRenderer>().sprite = icon;
        foreach(ComponentDataGeneric comData in componentDataEquipment)
        {
            comData.InicializeComponent(instances[key], (Item) this);
        }
    }
    public GameObject GetInstance(GameObject key)
    {
        return instances[key];
    }
    public virtual void DestroyEquipmentInstance(GameObject key)
    {
        Destroy(instances[key]);
        instances.Remove(key);
    }
    public void AddDataEquipment(ComponentDataGeneric data)
    {
        componentDataEquipment.Add(data);
    }

}
public enum EquipmentSlot {none, hand, body, miscelanious}