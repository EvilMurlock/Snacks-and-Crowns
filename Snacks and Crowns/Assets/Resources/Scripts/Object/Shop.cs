using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem.UI;

public class Shop : InteractibleObject
{
    public GameObject shopUiPrefab;
    public GameObject shopUiInstance;
    public GameObject firstSelectedButton;
    GameObject player;
    ItemInfo itemInfo;

    // list<Menu> menus --- call update when inventory changes, they hold their own player to which they belong

    public ShopSlot[] shopInventory;
    public void Start()
    {
        Generate_Ui();
        //firstSelectedButton = shopInventory[0].SelectThisButton();
        //Testing items
        Item axe = (Item)Resources.Load("Items/Equipment/Axe");
        Item hpPotion = (Item)Resources.Load("Items/Potions/Health Potion");
        foreach(ShopSlot shopSlot in shopInventory)
        {
            shopSlot.UpdateItem();
        }
        for (int i = 0; i<8; i++)
        {
            shopInventory[i].AddItem(axe);
        }
        for (int i = 8; i < 14; i++)
        {
            shopInventory[i].AddItem(hpPotion);
        }
    }
    public override void Interact(GameObject newPlayer)
    {
        player = newPlayer;
        player.GetComponent<PlayerStateManager>().Change_State(CharakterState.in_menu);

        //shopUiInstance.transform.SetParent(player.GetComponent<Inventory>().canvas.transform, false);
        shopUiInstance.SetActive(true);

        //player_inventory.event_system.GetComponent<MultiplayerEventSystem>().SetSelectedGameObject(firstSelectedButton);

        foreach(ShopSlot shopSlot in shopInventory)
        {/*
            shopSlot.button.GetComponent<ButtonOnSelectEvent>().onSelect.AddListener(delegate { UpdateItemInfo(shopSlot.item); });
            shopSlot.button.GetComponent<Button>().onClick.AddListener(delegate { shopSlot.BuyItem(player, shopSlot); });
            shopSlot.button.GetComponent<Button>().onClick.AddListener(delegate { UpdateItemInfo(shopSlot.item); });*/

        }
    }
    public override void UnInteract(GameObject player)
    {
        foreach(ShopSlot shopSlot in shopInventory)
        {/*
            shopSlot.button.GetComponent<Button>().onClick.RemoveAllListeners();*/
        }
        shopUiInstance.SetActive(false);
        player.GetComponent<PlayerStateManager>().Change_State(CharakterState.normal);
    }

    void Generate_Ui()
    {
        shopUiInstance = Instantiate(shopUiPrefab);
        shopInventory = shopUiInstance.GetComponentsInChildren<ShopSlot>();
        shopUiInstance.SetActive(false);
        itemInfo = shopUiInstance.transform.Find("Item_Info").GetComponent<ItemInfo>();
    }
    public void UpdateItemInfo(Item item)
    {
        if (item != null)
            itemInfo.LoadNewItem(item);
        else itemInfo.LoadNewItem(null);
    }
}
