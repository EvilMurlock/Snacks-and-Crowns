using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New_Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public Sprite icon;
    public string item_name;
    public string description;
    public int cost;
    public GameObject prefab;

    public virtual void Use()
    {   
        Debug.Log("Using " + item_name);
    }
}
