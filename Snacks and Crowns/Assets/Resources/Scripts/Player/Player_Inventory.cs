using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
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
    public GameObject hotbarPrefab;

    public GameObject moneyPrefab;
    public GameObject healthPrefab;


    //GameObject charakter_sheet;
    GameObject inventory_panel;
    GameObject equipment_panel;
    GameObject item_info_panel;
    GameObject hotbar;
    int hotbarCursor = 0;

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
    public Item_Slot[] hotbarItems = new Item_Slot[9];

    public List<int> menu_widths; //used for looping the menus
    public List<GameObject> active_menus;
    public List<Menu_Slot[]> menu_items;

    public int gold = 0;
    public UnityEvent<int> moneyChange = new UnityEvent<int>();

    Vector2 cursor;//first value is menu index (equipment/inventory), second value is slot in menu
    Vector2 cursor_last_move;
    Vector2 selected_item;

    public List<Interactible_Object> interacted_objects = new List<Interactible_Object>();

    ItemInfo itemInfo;
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
        Change_Cursor(cursor);
        ChangeHotbarCursor(hotbarCursor);
        //ChangeGold(10);
    }
    public void ChangeGold(int goldChange)
    {
        gold = gold + goldChange;
        moneyChange.Invoke(gold);
    }
    private void UpdateHotbar()
    {
        for(int i = 0; i < 9; i++)
        {
            hotbarItems[i].Add_Item(inventory_items[i].item);
        }
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
        UpdateItemInfo();
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
        hotbar = Instantiate(hotbarPrefab);
        GameObject money = Instantiate(moneyPrefab);
        GameObject health = Instantiate(healthPrefab);

        inventory_panel.transform.SetParent(canvas.transform, false);
        equipment_panel.transform.SetParent(canvas.transform, false);
        item_info_panel.transform.SetParent(canvas.transform, false);
        hotbar.transform.SetParent(canvas.transform, false);

        money.transform.SetParent(canvas.transform, false);
        money.GetComponentInChildren<MoneyTracker>().CoupleToPlayer(gameObject, gold);
        health.transform.SetParent(canvas.transform, false);
        health.GetComponent<HealthTracker>().CoupleToPlayer(gameObject);

        //charakter_sheet = Instantiate(charakter_sheet_prefab);
        //charakter_sheet.transform.SetParent(GameObject.Find("Canvas").transform, false);

        int index = 0;
        foreach (Transform child in inventory_panel.transform)
        {
            if (index >= inventory_items.Length) break;
            inventory_items[index].panel = child.gameObject;
            inventory_items[index].Add_Item(inventory_items[index].item);
            index++;
        }
        index = 0;
        foreach (Transform child in hotbar.transform)
        {
            hotbarItems[index].panel = child.gameObject;
            index++;
        }
        index = 0;
        foreach (Transform child in equipment_panel.transform)
        {
            equipment_items[index].panel = child.gameObject;
            if (index == 0 || index == 1) equipment_items[index].equipment_slot = Equipment_Slot.hand;
            if (index == 2) equipment_items[index].equipment_slot = Equipment_Slot.body;
            if (index == 3 || index == 4) equipment_items[index].equipment_slot = Equipment_Slot.miscelanious;
            equipment_items[index].Add_Item(equipment_items[index].item);
            index++;
        }
        itemInfo = inventory_panel.transform.Find("Item_Info").gameObject.GetComponent<ItemInfo>();
        itemInfo.LoadNewItem(null);

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
    public void ScrollHotbar(InputAction.CallbackContext context)
    {
        //depending on positive Q/E value go to other menu
        int scroll = (int)context.ReadValue<float>();
        if (context.started)
        {
            int newHotbarCursor;
            newHotbarCursor = (hotbarCursor + hotbarItems.Length + scroll) % hotbarItems.Length;
            ChangeHotbarCursor(newHotbarCursor);
        }
    }
    public void Use_Left_Hand(InputAction.CallbackContext context)
    {
        if (context.started && equipment_items[0].item != null)
        {
            Equipment eq = (Equipment)equipment_items[0].item;
            if (eq.instance != null) eq.UseEquipment();
        }
    }
    public void Use_Right_Hand(InputAction.CallbackContext context)
    {
        if (context.started && equipment_items[1].item != null)
        {
            Equipment eq = (Equipment)equipment_items[1].item;
            if (eq.instance != null) eq.UseEquipment();
        }
    }
    public void Use_Item(InputAction.CallbackContext context)
    {
        if (context.started && hotbarItems[hotbarCursor].item != null)
        {
            hotbarItems[hotbarCursor].item.Use(this.gameObject);
            if (hotbarItems[hotbarCursor].item.singleUse)
            {
                Remove_Item(new Vector2(0, hotbarCursor));
            }
        }
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
            if (cursor_slot.item.GetType() == typeof(Equipment))
            {
                Equipment equipment = (Equipment)cursor_slot.item;
                equipment.Destroy_Eq();
            }
            GameObject item_object = Instantiate((GameObject)Resources.Load("Prefabs/Items/Item"), transform.position, transform.rotation);
            item_object.GetComponent<Item_Controler>().item = cursor_slot.item;
            item_object.GetComponent<Rigidbody2D>().AddForce(this.transform.rotation * Vector2.up*500);
            item_object.transform.rotation = this.transform.rotation;
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
        UpdateItemInfo();
    }
    void UpdateItemInfo()
    {
        if (((Item_Slot)menu_items[(int)cursor.x][(int)cursor.y]) != null)
            itemInfo.LoadNewItem(((Item_Slot)menu_items[(int)cursor.x][(int)cursor.y]).item);
        else itemInfo.LoadNewItem(null);
    }
    void ChangeHotbarCursor(int new_cursor)
    {
        ChangeHotbarColour(hotbarCursor, Color.white);
        hotbarCursor = new_cursor;
        ChangeHotbarColour(hotbarCursor, Color.red);
    }

    private void ChangeHotbarColour(int index, Color color)
    {
        hotbarItems[index].Change_Colour(color);
    }
    void Change_Colour(Vector2 index, Color color)
    {
        menu_items[(int)index.x][(int)index.y].Change_Colour(color);
    }
    public bool FindEmptyInventorySlot()
    {
        for (int i = 0; i < 9; i++)
        {
            if (inventory_items[i].item == null) return true;
        }
        return false;
    }

    public void AddItemToInventory(Item item)
    {
        for (int i = 0; i < 9; i++)
        {
            if (inventory_items[i].item == null)
            {
                Add_Item(item, new Vector2(0, i));
                UpdateHotbar();
                return;
            }
        }
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
        UpdateHotbar();
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
            if(source_slot.item.GetType() == typeof(Equipment))
            {
                Equipment to_equip = (Equipment)source_slot.item;
                //Debug.Log("Am here: " + to_equip.item_name + " : " + (to_equip.equipment_slot != destination_slot.equipment_slot));
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
        UpdateHotbar();
    }
    void Remove_Item(Vector2 index)
    {
        Item_Slot index_slot = (Item_Slot)menu_items[(int)index.x][(int)index.y];
        index_slot.Remove_Item();
        UpdateHotbar();
    }
} 
