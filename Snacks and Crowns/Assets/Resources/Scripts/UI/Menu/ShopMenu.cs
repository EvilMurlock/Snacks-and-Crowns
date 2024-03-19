using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShopMenu : Menu
{
    Shop shop;
    int lastSelectedSlotIndex; // just used to refresh the item description after sale
    ItemInfo itemInfo;
    int buySellDivideIndex;
    private void Start()
    {
        SubscribeToSlotEvents();
        itemInfo = GetComponentInChildren<ItemInfo>();
        lastSelectedSlotIndex = 0;
        Refresh();
    }
    public void Initialize(Shop shop, GameObject player)
    {
        this.shop = shop;
        this.player = player;
        buySellDivideIndex = shop.MaxCapacity;
        // select first button
        player.GetComponent<MenuManager>().eventSystem.SetSelectedGameObject(GetComponentInChildren<Button>().gameObject);
        AttachToCanvas();
    }
    public override void SlotSelect(MenuSlot slot)
    {
        int index = shopSlots.GetIndex(slot);
        lastSelectedSlotIndex = index;
        // Debug.Log("Index is:" + index);
        itemInfo.LoadNewItem(slot.GetItem());
    }
    public override void SlotSubmit(MenuSlot slot)
    {
        // try to buy item

        int index = shopSlots.GetIndex(slot);
        lastSelectedSlotIndex = index;
        Debug.Log("Index of shop slot: " + index);
        Debug.Log("BuySellDivide: " + buySellDivideIndex);
        if (index < buySellDivideIndex)
            shop.TryToBuyItem(player, index);
        else
            shop.TryToSellItem(player, index-buySellDivideIndex);
    }
    
    public override void SlotCancel(MenuSlot slot)
    {
        // do nothing
    }

    public override void Refresh()
    {
        ShopSlot selectedSlot = shopSlots[lastSelectedSlotIndex];
        Inventory playerInventory = player.GetComponent<Inventory>();
        // updating buy page
        int index = 0;
        Item[] menuInventory = shop.GetComponent<Inventory>().Items;
        foreach(Item item in menuInventory)
        {
            shopSlots[index].AddItem(item);
            index++;
        }

        // updating sell page
        foreach (Item item in playerInventory.Items)
        {
            shopSlots[index].AddItem(item);
            index++;
        }

        // updaing item info
        itemInfo.LoadNewItem(selectedSlot.GetItem());
    }

}
