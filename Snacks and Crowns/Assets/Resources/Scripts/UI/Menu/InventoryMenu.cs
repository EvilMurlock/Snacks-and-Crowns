using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InventoryMenu : Menu
{
    int lastSelectedSlotIndex; // just used to refresh the item description after sale
    ItemInfo itemInfo;
    int inventoryEquipmentDivideIndex;
    int? submitedSlotIndex = null; // submited slot 
    private void Start()
    {
        SubscribeToSlotEvents();
        itemInfo = GetComponentInChildren<ItemInfo>();
        lastSelectedSlotIndex = 0;
        Refresh();
    }
    public void Initialize(GameObject player)
    {
        this.player = player;
        inventoryEquipmentDivideIndex = player.GetComponent<Inventory>().Items.Length;
        // select first button
        player.GetComponent<MenuManager>().SelectObject(GetComponentInChildren<Button>().gameObject);
        AttachToCanvas();
    }
    public override void SlotSelect(MenuSlot slot)
    {
        int index = menuSlots.GetIndex(slot);
        lastSelectedSlotIndex = index;
        // Debug.Log("Index is:" + index);
        itemInfo.LoadNewItem(slot.GetItem());
    }
    public override void SlotSubmit(MenuSlot slot)
    {
        if(submitedSlotIndex == null)
        {
            submitedSlotIndex = menuSlots.GetIndex(slot);
        }

        else
        {
            int newIndex = menuSlots.GetIndex(slot);
            SwapItems((int)submitedSlotIndex, newIndex);
            submitedSlotIndex = null;
        }
        int index = menuSlots.GetIndex(slot);
        lastSelectedSlotIndex = index;
    }
    
    public override void SlotCancel(MenuSlot slot)
    {
        // do nothing
    }
    void SwapItems(int index1, int index2)
    {
        if (index1 == index2) return;
        // swap them and also equip/unequip them
    }
    public override void Refresh()
    {
        ShopSlot selectedSlot = shopSlots[lastSelectedSlotIndex];
        Inventory playerInventory = player.GetComponent<Inventory>();
        // updating buy page
        int index = 0;

        // updating sell page
        foreach (Item item in playerInventory.Items)
        {
            menuSlots[index].AddItem(item);
            index++;
        }

        Item[] menuInventory = shop.GetComponent<Inventory>().Items;
        foreach (Item item in menuInventory)
        {
            shopSlots[index].AddItem(item);
            index++;
        }

        // updaing item info
        itemInfo.LoadNewItem(selectedSlot.GetItem());
    }

}
