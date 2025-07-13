using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// List of all crafting recipes, they are loaded at initialization
/// </summary>
public class CraftingRecipes : MonoBehaviour
{
    public List<CraftingRecipe> craftingRecipes;
    private void Start()
    {
        Item[] items = Resources.LoadAll<Item>("Items");
        foreach(Item item in items)
        {
            if(item.Recipe != null)
            {
                craftingRecipes.Add(item.Recipe);
            }
        }
    }
}
