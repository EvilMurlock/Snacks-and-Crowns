using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

public class Crafter : InteractibleInMenu
{
    [SerializeField]
    GameObject prefabCrafterlUi;
    [SerializeField]
    CraftingObject craftingObjekt;
    public CraftingObject CraftingObjekt { get => craftingObjekt; }    
    //ItemSlot craftedItem;

    GameObject player;
    
    public void Start()
    {
    }

    public override void Interact(GameObject new_player)
    {
        player = new_player;

        GenerateUi(player);
    }
    public override void UnInteract(GameObject player)
    {
        DeleteUi(player);
    }

    void GenerateUi(GameObject player)
    {
        CrafterMenu crafterMenu = Instantiate(prefabCrafterlUi, player.GetComponent<MenuManager>().canvas.transform).GetComponent<CrafterMenu>();
        crafterMenu.Initialize(this, player);
        menus.Add(crafterMenu);
    }
}
