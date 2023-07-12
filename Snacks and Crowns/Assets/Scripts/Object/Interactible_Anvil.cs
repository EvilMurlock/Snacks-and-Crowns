using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

public class Interactible_Anvil : Interactible_Object
{
    public GameObject prefab_anvil_ui;
    public GameObject instance_anvil_ui;
    public GameObject first_selected_button;
    Crafting_Objekt crafting_objekt = Crafting_Objekt.anvil;
    List<Crafting_Recepy> recepies;

    public GameObject item_panel_prefab;
    Item_Slot crafted_item;
    TMPro.TMP_Dropdown dropdown;
    GameObject ingredients_panel;

    GameObject player;
    bool craftable;
    
    public void Start()
    {
        craftable = false;
        Load_Recepies();
    }
    void Load_Recepies()
    {
        recepies = new List<Crafting_Recepy>();
        Crafting_Recepies crafting_recepies = GameObject.Find("Crafting_Recepies").GetComponent<Crafting_Recepies>();
        foreach(Crafting_Recepy crafting_recepy in crafting_recepies.crafting_recepies)
        {
            if(crafting_recepy.crafting_objekt == crafting_objekt)
            {
                recepies.Add(crafting_recepy);
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

        crafted_item.Add_Item(recepies[index].result);

        List<Item> player_items = new List<Item>();
        foreach (Item_Slot item_slot in player.GetComponent<Player_Inventory>().inventory_items)
        {
            if(item_slot.item != null) player_items.Add(item_slot.item);
        }

        foreach (Item item in recepies[index].ingredients)
        {
            
            Item_Slot item_slot = new Item_Slot();
            GameObject item_slot_panel = Instantiate(item_panel_prefab, ingredients_panel.transform);
            item_slot.panel = item_slot_panel;
            item_slot.Add_Item(item);

            int item_index = 0;
            bool item_found = false;
            
            foreach(Item p_item in player_items)
            {
                if (item.item_name == p_item.item_name)
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
            };
        }
    }
    public void Craft()
    {
        if (craftable)
        {
            foreach(Item item in recepies[dropdown.value].ingredients)
            {
                foreach(Item_Slot item_slot in player.GetComponent<Player_Inventory>().inventory_items)
                {
                    if (item_slot.Is_Not_Empty() && item.item_name == item_slot.item.item_name)
                    {
                        item_slot.Remove_Item();
                        break;
                    }
                }
            }
            foreach (Item_Slot item_slot in player.GetComponent<Player_Inventory>().inventory_items)
            {
                if (item_slot.Is_Empty()) item_slot.Add_Item(recepies[dropdown.value].result);
                {
                    item_slot.Add_Item(recepies[dropdown.value].result);
                    break;
                }
            }
        }
        Switch_Recepy();
    }
    public override void Interact(GameObject new_player)
    {
        player = new_player;
        player.GetComponent<Player_State_Manager>().Change_State(Player_State.in_ui_menu);

        Generate_Ui(player);

        Player_Inventory player_inventory = player.GetComponent<Player_Inventory>();
        player_inventory.Add_Interacted_Object(this);


        player_inventory.event_system.GetComponent<MultiplayerEventSystem>().SetSelectedGameObject(first_selected_button);
    }
    public override void Un_Interact(GameObject player)
    {
        Destroy(instance_anvil_ui);
        player.GetComponent<Player_State_Manager>().Change_State(Player_State.normal);
    }

    void Generate_Ui(GameObject player)
    {
        Player_Inventory player_inventory = player.GetComponent<Player_Inventory>();

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
        Load_Recepy(0);
    }
}
