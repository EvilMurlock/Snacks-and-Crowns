using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GOAP;


/// <summary>
/// Manages the chest
/// </summary>
public class Chest : InteractibleInMenu
{
    [SerializeField]
    GameObject chestUiPrefab;
    Inventory inventory;
    int maxCapacity = 16;
    public void Start()
    {
        World.AddInventory(GetComponent<Inventory>());
        menus = new List<Menu>();
        inventory = GetComponent<Inventory>();
        inventory.SetCapacity(maxCapacity);
    }
    public override void Interact(GameObject player)
    {
        GenerateUi(player);
    }
    public override void UnInteract(GameObject player)
    {
        DeleteUi(player);
    }

    void GenerateUi(GameObject player)
    {
        ChestMenu menuUi = Instantiate(chestUiPrefab).GetComponent<ChestMenu>();
        menuUi.Initialize(this, player);
        menus.Add(menuUi);
    }
}
