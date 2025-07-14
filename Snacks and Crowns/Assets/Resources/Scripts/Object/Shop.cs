using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem.UI;
using GOAP;


/// <summary>
/// Manages the shop and loads up the interaction UI
/// </summary>
public class Shop : InteractibleInMenu
{
    public GameObject shopUiPrefab;
    [SerializeField]
    Inventory inventory;
    int maxCapacity = 16;
    public int MaxCapacity { get { return maxCapacity; } }
    public void Start()
    {
        World.AddInventory(GetComponent<Inventory>());
        menus = new List<Menu>();
        inventory = GetComponent<Inventory>();
        inventory.SetCapacity(maxCapacity);
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
    public void TryToBuyItem(GameObject player, int itemIndex)
    {
        // PLAYER tries to BUY an item
        Inventory playerInventory = player.GetComponent<Inventory>();
        GoldTracker playerGoldTracker = player.GetComponent<GoldTracker>();
        Item itemToBeSold = inventory.GetItem(itemIndex);

        if (itemToBeSold == null) return;
        if (playerInventory.HasEmptySpace(1) && playerGoldTracker.HasGold(itemToBeSold.cost))
        {
            playerInventory.AddItem(itemToBeSold);
            playerGoldTracker.AddGold(-itemToBeSold.cost);
            inventory.RemoveItem(itemIndex);
        }

        // don't forget to call refresh on all menus so they show the correct available items
        RefreshMenus();
    }
    public void TryToSellItem(GameObject player, int itemIndex)
    {
        // PLAYER tries to SELL an item
        Inventory playerInventory = player.GetComponent<Inventory>();
        GoldTracker playerGoldTracker = player.GetComponent<GoldTracker>();
        Item itemToBeSold = playerInventory.GetItem(itemIndex);

        if (itemToBeSold == null) return;
        if (inventory.HasEmptySpace(1))
        {
            inventory.AddItem(itemToBeSold);
            playerGoldTracker.AddGold(itemToBeSold.cost);
            playerInventory.RemoveItem(itemToBeSold);
        }

        // don't forget to call refresh on all menus so they show the correct available items
        RefreshMenus();
    }
}
