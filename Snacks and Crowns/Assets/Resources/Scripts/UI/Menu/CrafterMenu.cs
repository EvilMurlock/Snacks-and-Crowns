using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CrafterMenu : Menu
{
    int lastSelectedSlotIndex; // just used to refresh the item description after sale
    ItemInfo itemInfo;
    int inventoryChestDivideIndex;
    int? submitedSlotIndex = null; // submited slot 
    int lastSubmitedSlotIndex = 0; // submited slot 
    Chest chest;
    //[SerializeField]
    Color submitedButtonColour = new Color(0,0,1);
         
    [SerializeField]
    GameObject itemDropPrefab;
    private void Start()
    {
        SubscribeToSlotEvents();
        itemInfo = GetComponentInChildren<ItemInfo>();
        lastSelectedSlotIndex = 0;
        Refresh();
    }
    public void Initialize(Chest chest, GameObject player)
    {
        this.chest = chest;
        this.player = player;
        inventoryChestDivideIndex = player.GetComponent<Inventory>().Items.Length;
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
        Refresh();
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
        Refresh();
    }
    
    public override void SlotCancel(MenuSlot slot)
    {
        int index = menuSlots.GetIndex(slot);
        Item item;
        // removing the item from self
        if (index >= inventoryChestDivideIndex)
        {
            Inventory chestInventory = chest.GetComponent<Inventory>();
            item = chestInventory.Items[index - inventoryChestDivideIndex];
            chestInventory.RemoveItem(index - inventoryChestDivideIndex);
        }
        else
        {
            Inventory inventory = player.GetComponent<Inventory>();
            item = inventory.Items[index];
            inventory.RemoveItem(index);
        }

        if (item == null) return;

        // creation of item pickup
        GameObject drop = Instantiate(itemDropPrefab);
        drop.transform.position = transform.position;
        drop.GetComponent<ItemPickup>().item = item;
        Refresh();
    }
    void SwapItems(int index1, int index2)
    {
        Item item1 = menuSlots[index1].GetItem();
        Item item2 = menuSlots[index2].GetItem();
        Inventory chestInventory = chest.GetComponent<Inventory>();
        Inventory playerInventory = player.GetComponent<Inventory>();
        // swap legality checks

        if (index1 == index2) return;
        // swap is legal
        if (index1 >= inventoryChestDivideIndex)
            chestInventory.AddItem(item2, index1 - inventoryChestDivideIndex);
        else
            playerInventory.AddItem(item2, index1);

        if (index2 >= inventoryChestDivideIndex)
            chestInventory.AddItem(item1, index2 - inventoryChestDivideIndex);
        else
            playerInventory.AddItem(item1, index2);
    }
    public override void Refresh()
    {
        MenuSlot selectedSlot = menuSlots[lastSelectedSlotIndex];

        Inventory playerInventory = player.GetComponent<Inventory>();
        Inventory chestInventory = chest.GetComponent<Inventory>();
        // updating buy page
        int index = 0;

        // updating sell page
        foreach (Item item in playerInventory.Items)
        {
            menuSlots[index].AddItem(item);
            index++;
        }
        foreach (Item item in chestInventory.Items)
        {
            menuSlots[index].AddItem(item);
            index++;
        }

        // updaing item info
        itemInfo.LoadNewItem(selectedSlot.GetItem());

        // change colour of submited slot

        if (submitedSlotIndex != null)
        {
            MenuSlot submitedSlot = menuSlots[(int)submitedSlotIndex];
            submitedSlot.ChangeColour(submitedButtonColour);
            lastSubmitedSlotIndex = (int)submitedSlotIndex;
        }
        else menuSlots[lastSubmitedSlotIndex].ChangeColour(Color.white);
    }

}
