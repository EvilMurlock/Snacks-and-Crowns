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

    public Item_Slot[] shopInventory;
    public void Start()
    {
        int inventory_size = (shopUiInstance.GetComponentsInChildren<Image>().Length -1)/2;
        shopInventory = new Item_Slot[inventory_size];
        for (int i = 0; i< inventory_size; i++)
        {
            shopInventory[i] = new Item_Slot();
        }
    }
    public override void Interact(GameObject newPlayer)
    {
        player = newPlayer;
        player.GetComponent<Player_State_Manager>().Change_State(Player_State.in_ui_menu);

        Generate_Ui(player);

        Player_Inventory player_inventory = player.GetComponent<Player_Inventory>();
        player_inventory.Add_Interacted_Object(this);


        player_inventory.event_system.GetComponent<MultiplayerEventSystem>().SetSelectedGameObject(firstSelectedButton);
    }
    public override void Un_Interact(GameObject player)
    {
        Delete_Ui();
    }

    void Generate_Ui(GameObject player)
    {
        Player_Inventory p_inventory = player.GetComponent<Player_Inventory>();
        shopUiInstance = Instantiate(shopUiInstance);

        shopUiInstance.transform.SetParent(p_inventory.canvas.transform, false);

        int index = 0;
        foreach (Transform child in shopUiInstance.transform)
        {
            Item item = shopInventory[index].item;
            shopInventory[index] = new Item_Slot();
            shopInventory[index].panel = child.gameObject;
            shopInventory[index].Add_Item(item);
            index++;
        }
        shopUiInstance.SetActive(false);
    }

    void Delete_Ui()
    {
        Destroy(shopUiInstance);
    }
}
