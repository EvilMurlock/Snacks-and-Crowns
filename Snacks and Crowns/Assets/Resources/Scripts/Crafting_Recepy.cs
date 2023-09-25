using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Crafting_Recepy
{
    public List<Item> ingredients;
    public Item result;
    public Crafting_Objekt crafting_objekt;

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

public enum Crafting_Objekt
{
    workshop,
    forge,
    anvil
}