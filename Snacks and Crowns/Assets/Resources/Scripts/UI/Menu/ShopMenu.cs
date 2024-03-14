using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShopMenu : Menu
{
    Shop shop;
    int lastSelectedSlotIndex; // just used to refresh the item description after sale
    ItemInfo itemInfo;
    private void Start()
    {
        SubscribeToSlotEvents();
        itemInfo = GetComponentInChildren<ItemInfo>();
        lastSelectedSlotIndex = 0;
    }
    public void Initialize(Shop shop, GameObject player)
    {
        this.shop = shop;
        this.player = player;
        // select first button
        player.GetComponent<MenuManager>().eventSystem.SetSelectedGameObject(GetComponentInChildren<Button>().gameObject);
        AttachToCanvas();
    }
    public override void SlotSelect(MenuSlot slot)
    {
        int index = slot.transform.GetSiblingIndex();
        lastSelectedSlotIndex = index;
        // Debug.Log("Index is:" + index);
        itemInfo.LoadNewItem(slot.GetItem());
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

    public override void Refresh()
    {
        ShopSlot[] shopSlots = GetComponentsInChildren<ShopSlot>();
        ShopSlot selectedSlot = shopSlots[lastSelectedSlotIndex];
        itemInfo.LoadNewItem(selectedSlot.GetItem());

        int index = 0;
        List<Item> menuInventory = shop.GetComponent<Inventory>().Items;
        foreach(Item item in menuInventory)
        {
            shopSlots[index].AddItem(item);

            index++;
        }
        for(int i = index; i <  shopSlots.Length; i++)
        {
            shopSlots[i].RemoveItem();
        }
    }

}
