using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    List<Equipment> equipmenInstances = new List<Equipment>();
    List<GameObject> equipmentLocations = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        equipmentLocations.Add(transform.Find("RightHand").gameObject);
        equipmentLocations.Add(transform.Find("LeftHand").gameObject);
        equipmentLocations.Add(transform.Find("Body").gameObject);
        equipmentLocations.Add(transform.Find("Miscelanious").gameObject);

        GetComponent<Inventory>().onChangeInventory.AddListener(InventoryUpdateUnequipCheck);
    }
    void InventoryUpdateUnequipCheck(Inventory inventory)
    {
        foreach(Equipment eq in equipmenInstances)
        {
            if (!inventory.HasItem(eq))
            {
                UnEquipItem(eq);
            }
        }
    }
    
    public bool EquipItem(Item item)
    {
        Equipment eq = (Equipment)item;
        bool succes = false;
        if (eq == null || eq.instance != null) return false; //item doesnt exist, or already equiped
        if (eq.equipment_slot == Equipment_Slot.body && equipmentLocations[2].transform.childCount == 0) {eq.Instantiate_Eq(equipmentLocations[2].transform); succes= true; }
        if (eq.equipment_slot == Equipment_Slot.hand && equipmentLocations[1].transform.childCount == 0) { eq.Instantiate_Eq(equipmentLocations[1].transform); succes= true; }
        if (eq.equipment_slot == Equipment_Slot.hand && equipmentLocations[0].transform.childCount == 0 && eq.instance == null) { eq.Instantiate_Eq(equipmentLocations[0].transform); succes= true; }
        if (eq.equipment_slot == Equipment_Slot.miscelanious && equipmentLocations[3].transform.childCount < 2) { eq.Instantiate_Eq(equipmentLocations[3].transform); succes= true; }
        if (succes) equipmenInstances.Add(eq);
        return succes;
    }
    public void UnEquipItem(Item item)
    {
        Equipment eq = (Equipment)item;
        equipmenInstances.Remove(eq);
        eq.Destroy_Eq();
    }
    public void UseLeftHand()
    {

    }
    public void UseRightHand()
    {

    }

}
