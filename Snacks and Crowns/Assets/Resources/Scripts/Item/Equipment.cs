using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New_Item", menuName = "Inventory/Equipment")]

public class Equipment : Item
{
    [SerializeReference] private List<ComponentDataGeneric> componentData;
    public Equipment_Slot equipment_slot;
    public GameObject prefab_eq;
    [HideInInspector]
    public GameObject instance;

    [ContextMenu(itemName: "Add Knockback")]
    private void AddKnockback()
    {
        componentData.Add(new KnockbackComponentData());
    }
    [ContextMenu(itemName: "Add Attack")]
    private void AddAttack()
    {
        componentData.Add(new AttackComponentData());
    }

    public override void Use()
    {
        instance.GetComponent<Hand_Item_Controler>().Use();
    }
    public virtual void Instantiate_Eq(Transform parent)
    {
        instance = Instantiate(prefab_eq, parent);
        instance.GetComponent<SpriteRenderer>().sprite = icon;

        Debug.Log("Instantiating...");
        foreach(ComponentDataGeneric comData in componentData)
        {
            comData.InicializeComponent(instance);
            Debug.Log("Component instantiated");

        }
    }
    public virtual void Destroy_Eq()
    {
        Destroy(instance);
        instance = null;
        Debug.Log("EQ destroyed");
    }
}
public enum Equipment_Slot {none, hand, body, miscelanious}