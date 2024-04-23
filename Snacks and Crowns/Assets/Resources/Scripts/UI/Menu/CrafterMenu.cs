using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CrafterMenu : Menu
{
    int lastSelectedSlotIndex; // just used to refresh the item description after sale

    List<CraftingRecepy> recepies;
    bool craftable;

    ItemInfo itemInfo;
    [SerializeField]
    GameObject prefabMenuSlot;
    Crafter crafter;
    [SerializeField]
    MenuSlot craftedItem;
    [SerializeField]
    TMPro.TMP_Dropdown dropdown;
    GameObject ingredientsPanel;
    private void Start()
    {
        itemInfo = GetComponentInChildren<ItemInfo>();
        lastSelectedSlotIndex = 0;
        
        Refresh();
    }
    public void Initialize(Crafter crafter, GameObject player)
    {
        this.crafter = crafter;
        this.player = player;
        // select first button
        player.GetComponent<MenuManager>().SelectObject(GetComponentInChildren<Button>().gameObject);
        AttachToCanvas();


        
        Inventory playerInventory = player.GetComponent<Inventory>();
        dropdown = gameObject.GetComponentInChildren<TMPro.TMP_Dropdown>();

        LoadRecepies();
        foreach (CraftingRecepy recepy in recepies)
        {
            Item result = recepy.result;
            dropdown.options.Add(new TMPro.TMP_Dropdown.OptionData(result.name,result.icon));
        }
        dropdown.onValueChanged.AddListener(delegate{ SwitchRecepy(); });
        LoadRecepy(0);
    }
    void LoadRecepies()
    {
        recepies = new List<CraftingRecepy>();
        CraftingRecepies craftingRecepies = GameObject.Find("Crafting Recepies").GetComponent<CraftingRecepies>();
        foreach (CraftingRecepy craftingRecepy in craftingRecepies.craftingRecepies)
        {
            if (craftingRecepy.craftingObjekt == crafter.CraftingObjekt)
            {
                recepies.Add(craftingRecepy);
            }
        }
    }

    public void SwitchRecepy()
    {
        EraseRecepy();
        LoadRecepy(dropdown.value);
    }
    void EraseRecepy()
    {
        foreach (Transform child in ingredientsPanel.transform)
        {
            Destroy(child.gameObject);
        }
    }
    void LoadRecepy(int index)
    {
        craftable = true;

        craftedItem.AddItem(recepies[index].result);
                itemInfo.LoadNewItem(recepies[index].result);
        
        List<Item> playerItems = new List<Item>();
        foreach (Item item in player.GetComponent<Inventory>().Items)
        {
            if(item != null) playerItems.Add(item);
        }

        foreach (Item item in recepies[index].ingredients)
        {
            MenuSlot menuSlot = Instantiate(prefabMenuSlot, ingredientsPanel.transform).GetComponent<MenuSlot>();
            
            int itemIndex = 0;
            bool itemFound = false;
            
            foreach(Item playerItem in playerItems)
            {
                if (item.itemName == playerItem.itemName)
                {
                    itemFound = true;
                    break;
                }
                itemIndex++;
            }
            if (itemFound && playerItems.Count > 0)
            {
                playerItems.RemoveAt(itemIndex);
                menuSlot.ChangeColour(Color.green);
            }
            else
            {
                menuSlot.ChangeColour(Color.red);
                craftable = false;
            };
        }
    }
    public void Craft()
    {
        if (craftable)
        {
            Inventory playerInventory = player.GetComponent<Inventory>();
            foreach (Item item in recepies[dropdown.value].ingredients)
                playerInventory.RemoveItem(item);

            playerInventory.AddItem(recepies[dropdown.value].result);

        }
        SwitchRecepy();
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
    }
    
    public override void SlotCancel(MenuSlot slot)
    {

    }
    public override void Refresh()
    {
        MenuSlot selectedSlot = menuSlots[lastSelectedSlotIndex];

        // updaing item info
        itemInfo.LoadNewItem(selectedSlot.GetItem());

    }

}
