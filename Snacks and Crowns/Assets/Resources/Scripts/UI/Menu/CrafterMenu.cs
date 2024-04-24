using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CrafterMenu : Menu
{
    int recepyIndex; // just used to refresh the item description after sale

    List<CraftingRecipe> recipes;
    bool craftable;
    [SerializeField]
    ItemInfo itemInfo;
    [SerializeField]
    GameObject prefabMenuSlot;
    Crafter crafter;
    [SerializeField]
    MenuSlot craftedItem;
    [SerializeField]
    TMPro.TMP_Dropdown dropdown;
    [SerializeField]
    GameObject ingredientsPanel;
    private void Start()
    {
        recepyIndex = 0;
        
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
        foreach (CraftingRecipe recipy in recipes)
        {
            Item result = recipy.result;
            dropdown.options.Add(new TMPro.TMP_Dropdown.OptionData(result.name,result.icon));
        }
        dropdown.onValueChanged.AddListener(delegate{ SwitchRecepy(); });
        LoadRecepy(0);
    }
    void LoadRecepies()
    {
        recipes = new List<CraftingRecipe>();
        CraftingRecipes craftingRecepies = GameObject.Find("Crafting Recipes").GetComponent<CraftingRecipes>();
        foreach (CraftingRecipe craftingRecepy in craftingRecepies.craftingRecipes)
        {
            if (craftingRecepy.craftingObjekt == crafter.CraftingObjekt)
            {
                recipes.Add(craftingRecepy);
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
        recepyIndex = index;
        craftable = true;

        craftedItem.AddItem(recipes[recepyIndex].result);
        itemInfo.LoadNewItem(recipes[recepyIndex].result);
        
        List<Item> playerItems = new List<Item>();
        foreach (Item item in player.GetComponent<Inventory>().Items)
        {
            if(item != null) playerItems.Add(item);
        }

        foreach (Item item in recipes[recepyIndex].ingredients)
        {
            MenuSlot menuSlot = Instantiate(prefabMenuSlot, ingredientsPanel.transform).GetComponent<MenuSlot>();
            menuSlot.AddItem(item);
            int itemIndex = 0;
            bool itemFound = false;
            
            foreach(Item playerItem in playerItems)
            {
                if (item == playerItem)
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
        itemInfo.LoadNewItem(recipes[recepyIndex].result);
    }
    public void Craft()
    {
        if (craftable)
        {
            Inventory playerInventory = player.GetComponent<Inventory>();
            foreach (Item item in recipes[dropdown.value].ingredients)
                playerInventory.RemoveItem(item);

            playerInventory.AddItem(recipes[dropdown.value].result);

        }
        SwitchRecepy();
    }

}
