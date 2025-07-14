using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Manages the crafter menu and interactions with it
/// </summary>
public class CrafterMenu : Menu
{
    int recipeIndex; // just used to refresh the item description after sale

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
        recipeIndex = 0;
        
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

        LoadRecipes();
        foreach (CraftingRecipe recipe in recipes)
        {
            Item result = recipe.result;
            dropdown.options.Add(new TMPro.TMP_Dropdown.OptionData(result.name,result.icon));
        }
        dropdown.onValueChanged.AddListener(delegate{ SwitchRecipe(); });
        LoadRecipe(0);
    }
    void LoadRecipes()
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

    public void SwitchRecipe()
    {
        EraseRecipe();
        LoadRecipe(dropdown.value);
    }
    void EraseRecipe()
    {
        foreach (Transform child in ingredientsPanel.transform)
        {
            Destroy(child.gameObject);
        }
    }
    void LoadRecipe(int index)
    {
        recipeIndex = index;
        craftable = true;

        craftedItem.AddItem(recipes[recipeIndex].result);
        itemInfo.LoadNewItem(recipes[recipeIndex].result);
        
        List<Item> playerItems = new List<Item>();
        foreach (Item item in player.GetComponent<Inventory>().Items)
        {
            if(item != null) playerItems.Add(item);
        }

        foreach (Item item in recipes[recipeIndex].ingredients)
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
        itemInfo.LoadNewItem(recipes[recipeIndex].result);
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
        SwitchRecipe();
    }

}
