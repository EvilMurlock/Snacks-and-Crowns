using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem.UI;

public class Shop : Interactible_Object
{
    public GameObject shopUiPrefab;
    public GameObject shopUiInstance;
    public GameObject firstSelectedButton;
    GameObject player;

    public ShopSlot[] shopInventory;
    public void Start()
    {
        Generate_Ui();
        firstSelectedButton = shopInventory[0].button;
        //Testing items
        Item axe = (Item)Resources.Load("Items/Equipment/Axe");
        for (int i = 0; i<8; i++)
        {
            shopInventory[i].AddItem(axe);
        }
    }
    public override void Interact(GameObject newPlayer)
    {
        player = newPlayer;
        player.GetComponent<Player_State_Manager>().Change_State(Player_State.in_ui_menu);

        shopUiInstance.transform.SetParent(player.GetComponent<Player_Inventory>().canvas.transform, false);
        shopUiInstance.SetActive(true);

        Player_Inventory player_inventory = player.GetComponent<Player_Inventory>();
        player_inventory.Add_Interacted_Object(this);


        player_inventory.event_system.GetComponent<MultiplayerEventSystem>().SetSelectedGameObject(firstSelectedButton);

        foreach(ShopSlot shopSlot in shopInventory)
        {
            shopSlot.button.GetComponent<Button>().onClick.AddListener(delegate { shopSlot.BuyItem(player, shopSlot); });
        }
    }
    public override void Un_Interact(GameObject player)
    {
        foreach(ShopSlot shopSlot in shopInventory)
        {
            shopSlot.button.GetComponent<Button>().onClick.RemoveAllListeners();
        }
        shopUiInstance.SetActive(false);
        player.GetComponent<Player_State_Manager>().Change_State(Player_State.normal);
    }

    void Generate_Ui()
    {
        shopUiInstance = Instantiate(shopUiPrefab);
        shopInventory = shopUiInstance.GetComponentsInChildren<ShopSlot>();
        shopUiInstance.SetActive(false);
    }
}
