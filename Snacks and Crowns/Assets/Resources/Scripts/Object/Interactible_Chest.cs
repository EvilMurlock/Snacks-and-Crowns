using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interactible_Chest : Interactible_Object
{
    public GameObject prefab_chest_ui;
    public GameObject instance_chest_ui;
    public Item_Slot[] chest_inventory;
    public void Start()
    {
        int inventory_size = (prefab_chest_ui.GetComponentsInChildren<Image>().Length -1)/2;
        chest_inventory = new Item_Slot[inventory_size];
        for (int i = 0; i< inventory_size; i++)
        {
            chest_inventory[i] = new Item_Slot();
        }
    }
    public bool AddItem(Item item)
    {
        foreach (Item_Slot slot in chest_inventory)
        {
            if (slot.Is_Empty())
            { 
                slot.Add_Item(item);
                return true;
            }
        }
        return false;
    }
    public override void Interact(GameObject player)
    {
        Generate_Ui(player);
        Player_Inventory player_inventory = player.GetComponent<Player_Inventory>();
        player_inventory.Set_UI_With_Inventory(instance_chest_ui, chest_inventory, 4);
        player_inventory.Activate_Menus();
        player_inventory.Add_Interacted_Object(this);
    }
    public override void Un_Interact(GameObject player)
    {
        Delete_Ui();
    }

    void Generate_Ui(GameObject player)
    {
        Player_Inventory p_inventory = player.GetComponent<Player_Inventory>();
        instance_chest_ui = Instantiate(prefab_chest_ui);

        instance_chest_ui.transform.SetParent(p_inventory.canvas.transform, false);

        int index = 0;
        foreach (Transform child in instance_chest_ui.transform)
        {
            Item item = chest_inventory[index].item;
            chest_inventory[index] = new Item_Slot();
            chest_inventory[index].panel = child.gameObject;
            chest_inventory[index].Add_Item(item);
            index++;
        }
        instance_chest_ui.SetActive(false);
    }

    void Delete_Ui()
    {
        Destroy(instance_chest_ui);
    }
}
