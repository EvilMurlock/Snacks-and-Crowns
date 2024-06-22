using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CraftingRecipe
{
    
    public List<Item> ingredients;
    [HideInInspector]
    public Item result;
    public CraftingObjekt craftingObjekt;

    public bool CanCraftFrom(List<Item> givenItems)
    {
        List<Item> availibleItems = new List<Item>(givenItems);
        foreach(Item item in ingredients)
        {
            if (availibleItems.Contains(item))
            {
                availibleItems.Remove(item);
            }
            else return false;
        }
        return true;
    }
}