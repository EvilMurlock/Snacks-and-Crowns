using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// Manages equipment, equiping and swaping items
/// </summary>
public class EquipmentManager : MonoBehaviour
{
    [SerializeField]
    Equipment[] equipments = new Equipment[5];
    public Equipment[] Equipments { get { return equipments; } }
    EquipmentLocation[] equipmentLocations = new EquipmentLocation[5];
    // Start is called before the first frame update
    void Start()
    {
        equipmentLocations = GetComponentsInChildren<EquipmentLocation>();
    }
    public bool HasEquippedItem(List<ItemTags> tags)
    {
        foreach (Equipment equipment in equipments)
        {
            if (equipment == null) continue;

            if (equipment.HasTags(tags))
                return true;
        }
        return false;
    }
    public bool HasEquippedItem(Item item)
    {
        foreach (Equipment equipment in equipments)
        {
            if (equipment == null) continue;

            if (equipment == item)
                return true;
        }
        return false;
    }
    public bool CanEquipItem(Item item, int index)
    {
        if (item == null) return true;
        if (equipments[index] != null) return false;
        if (item is Equipment equipment)
        {
            if (equipment.equipmentSlot != equipmentLocations[index].equipmentSlot) return false;
        }
        else return false;
        return true;
    }
    public Equipment EquipItem(Item item, int index)
    {
        if (!CanEquipItem(item, index)) throw new System.Exception("Failed to equip item: " + item);
        if(item is Equipment equipment)
        {
            equipment.InstantiateEquipment(equipmentLocations[index].transform, gameObject);
            equipments[index] = equipment;
        }
        return equipments[index];
    }
    public void UnEquipItem(int index)
    {
        if (equipments[index] == null) return;

        Equipment equipment = equipments[index];
        equipments[index] = null;
        equipment.DestroyEquipmentInstance(gameObject);
        foreach(Transform child in equipmentLocations[index].transform)
        {
            Destroy(child.gameObject);
        }
    }
    public void UseLeftHand()
    {
        UseHand(0);
    }
    public void UseRightHand()
    {
        UseHand(1);
    }
    public void UseHand(int index)
    {
        if (equipmentLocations[index].equipmentSlot != EquipmentSlot.hand) return;
        if (equipments[index] == null) return;
        equipments[index].UseEquipment(gameObject);
    }
}
