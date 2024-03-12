using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopMenu : Menu
{
    Shop shop;
    GameObject player;
    int lastSelectedSlotIndex; // just used to refresh the item description after sale
    private void Start()
    {
        SubscribeToSlotEvents();
    }
    public void Initialize(Shop shop, GameObject player)
    {
        this.shop = shop;
        this.player = player;
    }
    public override void SlotSelect(MenuSlot slot)
    {
        int index = slot.transform.GetSiblingIndex();
        lastSelectedSlotIndex = index;
        Debug.Log("Index is:" + index);

        // refresh item description
    }
    public override void SlotSubmit(MenuSlot slot)
    {
        // try to buy item
        int index = slot.transform.GetSiblingIndex();
        lastSelectedSlotIndex = index;
        shop.TryToBuyItem(player, index);
    }
    public override void SlotCancel(MenuSlot slot)
    {
        // do nothing
    }

}
