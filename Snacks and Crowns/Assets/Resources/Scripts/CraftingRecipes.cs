using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
