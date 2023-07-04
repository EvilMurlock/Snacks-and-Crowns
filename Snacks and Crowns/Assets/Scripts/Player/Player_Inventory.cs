using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
public class Player_Inventory : MonoBehaviour
{
    public Canvas canvas;
    public GameObject event_system;
    //public GameObject charakter_sheet_prefab;
    public GameObject inventory_panel_prefab;
    public GameObject equipment_panel_prefab;
    public GameObject item_info_panel_prefab;

    //GameObject charakter_sheet;
    GameObject inventory_panel;
    GameObject equipment_panel;
    GameObject item_info_panel;

    [SerializeField]
    GameObject left_hand;
    [SerializeField]
    GameObject right_hand;
    [SerializeField]
    GameObject body;

    public Item_Slot[] equipment_items = new Item_Slot[5];
    //1 = left hand
    //2 = right hand
    //3 = body
    //4 = miscelanious1
    //5 = miscelanious2
     
    public Item_Slot[] inventory_items = new Item_Slot[9];

    public List<int> menu_widths; //used for looping the menus
    public List<GameObject> active_menus;
    public List<Menu_Slot[]> menu_items;

    Vector2 cursor;//first value is menu index (equipment/inventory), second value is slot in menu
    Vector2 cursor_last_move;
    Vector2 selected_item;

    public List<Interactible_Object> interacted_objects = new List<Interactible_Object>();
    void Start()
    {
        menu_items = new List<Menu_Slot[]>();
        menu_items.Add(equipment_items);
        menu_items.Add(inventory_items);

        cursor = new Vector2(0, 0);
        cursor_last_move = Vector2.zero;
        selected_item = cursor;
        Find_Slots();

        left_hand = transform.Find("left_hand").gameObject;
        right_hand = transform.Find("right_hand").gameObject;
        body = transform.Find("body").gameObject;
        //Change_Colour(selected_item, Color.blue);

        Set_Menus_Charakter_Sheet();
    }
    public void Add_Interacted_Object(Interactible_Object new_object)
    {
        interacted_objects.Add(new_object);
    }
    public void Remove_Interacted_Object()
    {
        foreach (Interactible_Object i_o in interacted_objects)
        {
            i_o.Un_Interact(this.gameObject);
        }
        interacted_objects = new List<Interactible_Object>();
    }

    public void Set_Basic_Menus()
    {
        active_menus = new List<GameObject>() { inventory_panel, item_info_panel }; //only maters for activation, order irelevant
        menu_widths = new List<int>() { 3 }; //helps with up,down scroling and looping, order must be same as menu_items
        menu_items = new List<Menu_Slot[]>() { inventory_items }; //holds every item and item_slot for colouring
    }
    public void Set_Menus_Charakter_Sheet()
    {
        Set_Basic_Menus();
        active_menus.Add(equipment_panel);
        menu_widths.Add(2);
        menu_items.Add(equipment_items);
    }
    public void Set_UI_With_Inventory(GameObject instance_ui, Menu_Slot[] item_slot_list, int menu_width)
    {
        Set_Basic_Menus();
        active_menus.Add(instance_ui);
        menu_widths.Add(menu_width);
        menu_items.Add(item_slot_list);
    }
    public void Activate_Menus()
    {
        foreach (GameObject g in active_menus)
        {
            g.SetActive(true);
            GetComponent<Player_Movement>().Move_Stop(); //stops player walking "momentum"
        }
        GetComponent<Player_State_Manager>().Change_State(Player_State.in_menu);
    }
    public void Deactivate_Menus()
    {
        foreach (GameObject g in active_menus)
        {
            g.SetActive(false);
        }
        GetComponent<Player_State_Manager>().Change_State(Player_State.normal);
        Remove_Interacted_Object();
    }

    void Find_Slots()
    {
        inventory_panel = Instantiate(inventory_panel_prefab);
        equipment_panel = Instantiate(equipment_panel_prefab);
        item_info_panel = Instantiate(item_info_panel_prefab);

        inventory_panel.transform.SetParent(canvas.transform, false);
        equipment_panel.transform.SetParent(canvas.transform, false);
        item_info_panel.transform.SetParent(canvas.transform, false);
        //charakter_sheet = Instantiate(charakter_sheet_prefab);
        //charakter_sheet.transform.SetParent(GameObject.Find("Canvas").transform, false);

        int index = 0;
        foreach (Transform child in inventory_panel.transform)
        {
            inventory_items[index].panel = child.gameObject;
            index++;
        }
        index = 0;
        foreach (Transform child in equipment_panel.transform)
        {
            equipment_items[index].panel = child.gameObject;
            if (index == 0 || index == 1) equipment_items[index].equipment_slot = Equipment_Slot.hand;
            if (index == 2) equipment_items[index].equipment_slot = Equipment_Slot.body;
            if (index == 3 || index == 4) equipment_items[index].equipment_slot = Equipment_Slot.miscelanious;

            index++;
        }
        //charakter_sheet.SetActive(false);
        inventory_panel.SetActive(false);
        equipment_panel.SetActive(false);
        item_info_panel.SetActive(false);

    }
    public void Togle_Inventory(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (active_menus[0].activeSelf)
            {
                Deactivate_Menus();
            }
            else
            {
                Set_Menus_Charakter_Sheet();
                Activate_Menus();
            }
            cursor_last_move = Vector2.zero;
        }
    }
    public void Use_Left_Hand(InputAction.CallbackContext context)
    {
        if (equipment_items[0].item != null)
        {
            Equipment eq = (Equipment)equipment_items[0].item;
            if (eq.instance != null) eq.Use();
        }
    }
    public void Scroll_Menu(InputAction.CallbackContext context) 
    {
        //depending on positive Q/E value go to other menu
        int scroll = (int)context.ReadValue<float>();
        if (context.started)
        {
            Vector2 new_cursor;
            new_cursor.x = (cursor.x + menu_items.Count + scroll) % menu_items.Count;
            new_cursor.y = cursor.y % menu_items[(int)new_cursor.x].Length;
            Change_Cursor(new_cursor);
        }
    }
    public void Use_Right_Hand(InputAction.CallbackContext context)
    {
        if (equipment_items[1].item != null)
        {
            Equipment eq = (Equipment)equipment_items[1].item;
            if (eq.instance != null) eq.Use();
        }
    }
    public void Use_Item(InputAction.CallbackContext context)
    {
        //Item in invetory.use
        //eq.Use();
    }
    public void On_Move(InputAction.CallbackContext context)
    {
        Vector2 move = context.ReadValue<Vector2>();
        move.x = Mathf.RoundToInt(move.x);
        move.y = Mathf.RoundToInt(move.y);
        Vector2 new_cursor = new Vector2();
        new_cursor = cursor;

        
        float move_magnitude = Mathf.Abs(move.x) + Mathf.Abs(move.y);
        float move_magnitude_prev = Mathf.Abs(cursor_last_move.x) + Mathf.Abs(cursor_last_move.y);
        cursor_last_move = move;
        if (move_magnitude_prev < move_magnitude)
        {
            new_cursor.y = (cursor.y + move.x + menu_items[(int)cursor.x].Length) % menu_items[(int)cursor.x].Length;
            Change_Cursor(new_cursor);
            new_cursor.y = (cursor.y - move.y * menu_widths[(int)cursor.x] + menu_items[(int)cursor.x].Length) % menu_items[(int)cursor.x].Length;
            Change_Cursor(new_cursor);
        }
    }
public void Select(InputAction.CallbackContext context) //selects item
    {
        if (context.started)
        {
            Change_Colour(selected_item, Color.white);
            selected_item = cursor;
            Change_Colour(cursor, Color.blue);
        }
    }
    public void Confirm(InputAction.CallbackContext context) //selects item
    {
        if (context.started)
        {
            if (Swap(selected_item, cursor)) Select(context);
        }
    }
    public void Throw_Away(InputAction.CallbackContext context)
    {
        if (menu_items[(int)cursor.x][(int)cursor.y] != null)
        {
            Item_Slot cursor_slot = (Item_Slot)menu_items[(int)cursor.x][(int)cursor.y];
            GameObject item_object = Instantiate(cursor_slot.item.prefab, transform.position, transform.rotation);
            item_object.GetComponent<Item_Controler>().item = cursor_slot.item;
            Remove_Item(cursor);
            //delete from inventory and instantiate in world
        }
    }
    void Change_Cursor(Vector2 new_cursor)
    {
        Change_Colour(cursor,Color.white);
        cursor = new_cursor;
        Change_Colour(selected_item, Color.blue);
        Change_Colour(cursor, Color.red);
    }
    void Change_Colour(Vector2 index, Color color)
    {
        menu_items[(int)index.x][(int)index.y].Change_Colour(color);
    }
    public void Pick_Up_Item(GameObject new_item)
    {
        for (int i = 0; i < 9; i++)
        {
            if (inventory_items[i].Is_Empty())
            {
                Add_Item(new_item.GetComponent<Item_Controler>().item, new Vector2(0, i));
                Destroy(new_item);
                return;
            }
        }
    }
    bool Swap(Vector2 index1, Vector2 index2)
    {

        if (Equip_Check(index1, index2) == false || Equip_Check(index2, index1) == false) return false;

        Un_Equip_Item(index1);
        Un_Equip_Item(index2);

        Item_Slot index1_slot = (Item_Slot)menu_items[(int)index1.x][(int)index1.y];
        Item_Slot index2_slot = (Item_Slot)menu_items[(int)index2.x][(int)index2.y];

        Item item_data = index1_slot.item;
        index1_slot.Add_Item(index2_slot.item);
        index2_slot.Add_Item(item_data);

        Equip_Equipment();

        return true;
    }
    void Un_Equip_Item(Vector2 index)
    {
        Item_Slot index_slot = (Item_Slot)menu_items[(int)index.x][(int)index.y];
        if (index_slot.equipment_slot != Equipment_Slot.none)
        {
            if (index_slot.Is_Not_Empty())
            {
                Equipment eq = (Equipment)index_slot.item;
                if (eq.instance != null) eq.Destroy_Eq();
            }
        }
    }
    void Equip_Equipment()
    {
        if (equipment_items[0].Is_Not_Empty()) { Equipment l_hand = (Equipment)equipment_items[0].item; if (l_hand.instance == null) l_hand.Instantiate_Eq(left_hand.transform); }
        if (equipment_items[1].Is_Not_Empty()) { Equipment r_hand = (Equipment)equipment_items[1].item; if (r_hand.instance == null) r_hand.Instantiate_Eq(right_hand.transform); }
        if (equipment_items[2].Is_Not_Empty()) { Equipment body_eq = (Equipment)equipment_items[2].item; if (body_eq.instance == null) body_eq.Instantiate_Eq(body.transform); } 
    }
    bool Equip_Check(Vector2 source, Vector2 destination)
    {
        Item_Slot source_slot = (Item_Slot)menu_items[(int)source.x][(int)source.y];
        Item_Slot destination_slot = (Item_Slot)menu_items[(int)destination.x][(int)destination.y];

        if (source_slot.Is_Not_Empty() && destination_slot.equipment_slot != Equipment_Slot.none)
        {
            if (source_slot.item.GetType().IsSubclassOf(typeof(Equipment)))
            {
                Equipment to_equip = (Equipment)source_slot.item;
                Debug.Log("Am here: " + to_equip.item_name + " : " + (to_equip.equipment_slot != destination_slot.equipment_slot));
                if (to_equip.equipment_slot != destination_slot.equipment_slot) return false;
            }
            else return false;
        }
        return true;
    }
    void Add_Item(Item item, Vector2 index)
    {
        Item_Slot index_slot = (Item_Slot)menu_items[(int)index.x][(int)index.y];
        index_slot.Add_Item(item);
    }
    void Remove_Item(Vector2 index)
    {
        Item_Slot index_slot = (Item_Slot)menu_items[(int)index.x][(int)index.y];
        index_slot.Remove_Item();
    }
} 
