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
    [HideInInspector]
    public GameObject prefab;
    public float useDuration;
    public bool singleUse = false;
    [SerializeReference] private List<ComponentDataGeneric> componentDataUse;
    [ContextMenu(itemName: "Add Heal")]
    private void AddHeal()
    {
        componentDataUse.Add(new IncreaseHealthData());
    }
    [ContextMenu(itemName: "Add Stun")]
    private void AddStun()
    {
        componentDataUse.Add(new SelfStun());
    }

    public virtual void Use(GameObject player)
    {
        Debug.Log("Using " + item_name);
        foreach (ComponentDataGeneric comData in componentDataUse)
        {
            comData.InicializeComponent(player);
        }
    }
}
