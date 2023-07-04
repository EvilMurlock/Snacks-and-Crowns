using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Crafting_Recepy
{
    public List<Item> ingredients;
    public Item result;
    public Crafting_Objekt crafting_objekt;
}

public enum Crafting_Objekt
{
    workshop,
    furnace,
    anvil
}