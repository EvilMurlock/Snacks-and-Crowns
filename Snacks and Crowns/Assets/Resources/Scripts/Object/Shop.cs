using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem.UI;

public class Shop : InteractibleObject
{
    public GameObject shopUiPrefab;
    List<Menu> menus;
    Inventory inventory;
    public void Start()
    {
        //Testing items
        Item axe = (Item)Resources.Load("Items/Equipment/Axe");
        Item hpPotion = (Item)Resources.Load("Items/Potions/Health Potion");
        for (int i = 0; i<8; i++)
        {
            inventory.AddItem(axe);
        }
        for (int i = 8; i < 14; i++)
        {
            inventory.AddItem(hpPotion);
        }
    }
    public override void Interact(GameObject player)
    {
        GameObject menuUi = Instantiate(shopUiPrefab);
        // set corect canvas parent
        // instantiate with this shop and player

        // THIS SHOULD MAYBE HAPPEN IN PLAYER INTERACT??? PROBABLY NOT, THERE ARE MANY THINGS TO ITNERACT WITH, MANY ARENT MENUS
        player.GetComponent<PlayerStateManager>().Change_State(CharakterState.in_menu);


        //shopUiInstance.transform.SetParent(player.GetComponent<Inventory>().canvas.transform, false);

        //player_inventory.event_system.GetComponent<MultiplayerEventSystem>().SetSelectedGameObject(firstSelectedButton);
        /*
        foreach(ShopSlot shopSlot in shopInventory)
        {
            shopSlot.button.GetComponent<ButtonOnSelectEvent>().onSelect.AddListener(delegate { UpdateItemInfo(shopSlot.item); });
            shopSlot.button.GetComponent<Button>().onClick.AddListener(delegate { shopSlot.BuyItem(player, shopSlot); });
            shopSlot.button.GetComponent<Button>().onClick.AddListener(delegate { UpdateItemInfo(shopSlot.item); });

        }*/
    }
    public override void UnInteract(GameObject player)
    {/*
        foreach(ShopSlot shopSlot in shopInventory)
        {
            shopSlot.button.GetComponent<Button>().onClick.RemoveAllListeners();
        }
        shopUiInstance.SetActive(false);
        player.GetComponent<PlayerStateManager>().Change_State(CharakterState.normal);*/
    }

    void Generate_Ui()
    {
        //shopInventory = shopUiInstance.GetComponentsInChildren<ShopSlot>();
    }
    public void TryToBuyItem(GameObject player, int itemIndex)
    {
        Inventory playerInventory = player.GetComponent<Inventory>();
        GoldTracker playerGoldTracker = player.GetComponent<GoldTracker>();
        Item itemToBeSold = inventory.GetItem(itemIndex);
        if (itemToBeSold == null) return;
        if (playerInventory.HasEmptySpace(1) && playerGoldTracker.HasGold(itemToBeSold.cost))
        {
            playerInventory.AddItem(itemToBeSold);
            playerGoldTracker.AddGold(-itemToBeSold.cost);
            inventory.RemoveItem(itemToBeSold);
        }

        // dont forget to call refresh on all menus so they show the corect avvailible items
        RefreshMenus();
    }
    void RefreshMenus()
    {
        foreach(Menu menu in menus)
        {
            menu.Refresh();
        }
    }
}
