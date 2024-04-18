using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

public class Crafter : InteractibleInMenu
{
    [SerializeField]
    GameObject prefabAnvilUi;
    [SerializeField]
    GameObject firstSelectedButton;
    [SerializeField]
    CraftingObjekt craftingObjekt;

    List<CraftingRecepy> recepies;

    public GameObject itemPanelPrefab;
    
    ItemSlot craftedItem;
    TMPro.TMP_Dropdown dropdown;
    GameObject ingredientsPanel;
    ItemInfo itemInfo;

    GameObject player;
    bool craftable;
    
    public void Start()
    {
        if (GetComponent<TagSystem>() == null) gameObject.AddComponent<TagSystem>();
        GetComponent<TagSystem>().AddTag(craftingObjekt.ToString());
        craftable = false;
        Load_Recepies();
    }
    void Load_Recepies()
    {
        recepies = new List<CraftingRecepy>();
        CraftingRecepies craftingRecepies = GameObject.Find("Crafting Recepies").GetComponent<CraftingRecepies>();
        foreach(CraftingRecepy craftingRecepy in craftingRecepies.craftingRecepies)
        {
            if(craftingRecepy.craftingObjekt == craftingObjekt)
            {
                recepies.Add(craftingRecepy);
            }
        }
    }
    public void Switch_Recepy()
    {
        Erase_Recepy();
        Load_Recepy(dropdown.value);
    }
    void Erase_Recepy()
    {
        foreach (Transform child in ingredients_panel.transform) 
        {
            Destroy(child.gameObject);
        }
    }
    void Load_Recepy(int index)
    {
        craftable = true;

        crafted_item.AddItem(recepies[index].result);

        itemInfo = instanceAnvilUi.transform.Find("Item_Info").GetComponent<ItemInfo>();
        itemInfo.LoadNewItem(recepies[index].result);

        List<Item> player_items = new List<Item>();
        /*REWERITE THIS
        foreach (Item_Slot item_slot in player.GetComponent<Inventory>().items)
        {
            if(item_slot.item != null) player_items.Add(item_slot.item);
        }*/

        foreach (Item item in recepies[index].ingredients)
        {
            /* REWERITE REWERITE THIS
            Item_Slot item_slot = new Item_Slot();
            GameObject item_slot_panel = Instantiate(item_panel_prefab, ingredients_panel.transform);
            item_slot.panel = item_slot_panel;
            item_slot.Add_Item(item);
            
            int item_index = 0;
            bool item_found = false;
            
            foreach(Item p_item in player_items)
            {
                if (item.itemName == p_item.itemName)
                {
                    item_found = true;
                    break;
                }
                item_index++;
            }
            if (item_found && player_items.Count > 0)
            {
                Debug.Log(item_index);
                player_items.RemoveAt(item_index);
                item_slot.Change_Colour(Color.green);
            }
            else
            {
                item_slot.Change_Colour(Color.red);
                craftable = false;
            };*/
        }
    }
    public void Craft()
    {
        if (craftable)
        {
            foreach(Item item in recepies[dropdown.value].ingredients)
            {/* REWERITE REWERITE THIS
                foreach(Item_Slot item_slot in player.GetComponent<Player_Inventory>().inventory_items)
                {
                    if (item_slot.Is_Not_Empty() && item.itemName == item_slot.item.itemName)
                    {
                        item_slot.Remove_Item();
                        break;
                    }
                }*/
            }/* REWERITE REWERITE THIS
            player.GetComponent<Player_Inventory>().AddItemToInventory(recepies[dropdown.value].result);*/
            /*
            foreach (Item_Slot item_slot in player.GetComponent<Player_Inventory>().inventory_items)
            {
                if (item_slot.Is_Empty()) item_slot.Add_Item(recepies[dropdown.value].result);
                {
                    item_slot.Add_Item(recepies[dropdown.value].result);
                    break;
                }
            }
            */
        }
        Switch_Recepy();
    }
    public override void Interact(GameObject new_player)
    {
        player = new_player;

        Generate_Ui(player);
    }
    public override void UnInteract(GameObject player)
    {
        Destroy(instanceAnvilUi);
    }

    void Generate_Ui(GameObject player)
    {
        /* REWERITE REWERITE THIS
        Inventory player_inventory = player.GetComponent<Inventory>();

        instance_anvil_ui = Instantiate(prefab_anvil_ui);
        instance_anvil_ui.transform.SetParent(player_inventory.canvas.transform, false);
        first_selected_button = instance_anvil_ui.GetComponentsInChildren<Transform>()[1].gameObject;

        dropdown = instance_anvil_ui.transform.Find("Dropdown").gameObject.GetComponent<TMPro.TMP_Dropdown>();
        foreach(Crafting_Recepy recepy in recepies)
        {
            Item result = recepy.result;
            dropdown.options.Add(new TMPro.TMP_Dropdown.OptionData(result.name,result.icon));
        }
        dropdown.onValueChanged.AddListener(delegate{ Switch_Recepy(); });

        ingredients_panel = instance_anvil_ui.transform.Find("Ingrediences").gameObject;
        crafted_item = new Item_Slot();
        crafted_item.panel = instance_anvil_ui.transform.Find("Craft_Item").gameObject;
        instance_anvil_ui.transform.Find("Craft_Button").gameObject.GetComponent<Button>().onClick.AddListener(delegate { Craft(); });

        Load_Recepy(0);*/
    }
}
