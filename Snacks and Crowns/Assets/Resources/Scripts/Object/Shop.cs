using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem.UI;

public class Shop : Interactible
{
    public GameObject shopUiPrefab;
    List<Menu> menus;
    [SerializeField]
    Inventory inventory;
    int maxCapacity = 16;
    public void Start()
    {
        menus = new List<Menu>();
        inventory = GetComponent<Inventory>();
        inventory.SetCapacity(maxCapacity);

        //Testing items
        Item axe = (Item)Resources.Load("Resources/Items/Equipment/Axe");
        Item hpPotion = (Item)Resources.Load("Resources/Items/Potions/Health Potion");
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
        // instantiate with this shop and player
        GenerateUi(player);
    }
    void GenerateUi(GameObject player)
    {
        ShopMenu menuUi = Instantiate(shopUiPrefab).GetComponent<ShopMenu>();
        menuUi.Initialize(this, player);
        menus.Add(menuUi);
    }

    public override void UnInteract(GameObject player)
    {
        DeleteUi(player);
    }
    void DeleteUi(GameObject player)
    {
        foreach(Menu menu in menus)
        {
            Debug.Log("yep deleting ui");

            menu.DeleteSelf(player);
        }
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
