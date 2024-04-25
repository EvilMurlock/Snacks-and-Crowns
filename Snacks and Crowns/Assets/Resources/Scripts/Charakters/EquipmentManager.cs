using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    Equipment[] equipments = new Equipment[5];
    public Equipment[] Equipments { get { return equipments; } }
    EquipmentLocation[] equipmentLocations = new EquipmentLocation[5];
    // Start is called before the first frame update
    void Start()
    {
        equipmentLocations = GetComponentsInChildren<EquipmentLocation>();
        //GetComponent<Inventory>().onChangeInventory.AddListener(InventoryUpdateUnequipCheck);
    }
    /*
    void InventoryUpdateUnequipCheck(Inventory inventory)
    {
        // i dont know what this function is suposed to do
        throw new System.Exception("FUCNTIO  NOT IMPLEMENTED IN EQUIPMENT MANAGER");
    }*/
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
    public void EquipItem(Item item, int index)
    {
        if (!CanEquipItem(item, index)) throw new System.Exception("Failed to equip item: " + item);
        if(item is Equipment equipment)
        {
            equipment.InstantiateEquipment(equipmentLocations[index].transform);
            equipments[index] = equipment;
            //Debug.Log("Item " + equipment.name + " equiped");
        }
        return;
    }
    public void UnEquipItem(int index)
    {
        if (equipments[index] == null) return;

        Equipment equipment = equipments[index];
        equipments[index] = null;
        equipment.DestroyEquipmentInstance();
    }
    public void UseLeftHand()
    {
        equipments[0].UseEquipment();
    }
    public void UseRightHand()
    {
        equipments[1].UseEquipment();
    }

}
