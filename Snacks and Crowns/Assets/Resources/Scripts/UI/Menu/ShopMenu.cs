using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
public class ShopMenu : Menu
{
    [SerializeField]
    GameObject buyPage;
    [SerializeField]
    GameObject sellPage;
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
        // try to buy item

        int index = menuSlots.GetIndex(slot);
        lastSelectedSlotIndex = index;
        if (index < buySellDivideIndex)
            shop.TryToBuyItem(player, index);
        else
            shop.TryToSellItem(player, index-buySellDivideIndex);
    }
    
    public override void SlotCancel(MenuSlot slot)
    {
        // do nothing
    }
    void UpdateBuyPage()
    {
        // updating buy page
        int index = 0;
        Item[] menuInventory = shop.GetComponent<Inventory>().Items;
        foreach (Item item in menuInventory)
        {
            buyPage.GetComponentsInChildren<ShopSlot>()[index].AddItem(item);
            //menuSlots[index].AddItem(item);
            index++;
        }
    }
    void UpdateSellPage()
    {
        // updating sell page
        int index = 0;
        Inventory playerInventory = player.GetComponent<Inventory>();
        foreach (Item item in playerInventory.Items)
        {
            sellPage.GetComponentsInChildren<ShopSlot>()[index].AddItem(item);
            //menuSlots[index].AddItem(item);
            index++;
        }
    }
    void UpdateItemInfo()
    {
        // updating item info
        ShopSlot selectedSlot = (ShopSlot)menuSlots[lastSelectedSlotIndex];
        itemInfo.LoadNewItem(selectedSlot.GetItem());

    }
    public override void Refresh()
    {

        UpdateBuyPage(); 
        UpdateSellPage();
        UpdateItemInfo();
    }

}
