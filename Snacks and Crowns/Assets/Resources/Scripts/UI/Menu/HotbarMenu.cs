using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HotbarMenu : Menu
{
    [SerializeField]
    GameObject Player;
    int selectedSlotIndex = 0;
    Color selectedColor = Color.yellow;
    Color normalColor = Color.white;
    private void Start()
    {
        player = Player;
        SubscribeToSlotEvents();
        player.GetComponent<Inventory>().onChangeInventory.AddListener(UpdateHotbar);
        UpdateColor();
    }

    void UpdateHotbar(Inventory inventory)
    {

        for(int i = 0; i < inventory.Items.Length; i++)
        {
            menuSlots[i].AddItem(inventory.Items[i]);
        }
        UpdateColor();
    }
    public void UseItem()
    {

        Item item = menuSlots[selectedSlotIndex].GetItem();
        item.Use(player);
        if (item.singleUse)
        {
            player.GetComponent<Inventory>().RemoveItem(selectedSlotIndex);
        }
    }
    public void Scroll(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() > 0) ScrollRight();
        else ScrollLeft();
    }
    void ScrollLeft()
    {
        selectedSlotIndex--;
        if (selectedSlotIndex < 0) selectedSlotIndex += player.GetComponent<Inventory>().Items.Length;
        UpdateColor();
    }
    public void ScrollRight()
    {
        selectedSlotIndex++;
        selectedSlotIndex %= player.GetComponent<Inventory>().Items.Length;
        UpdateColor();
    }

    void UpdateColor()
    {
        foreach(MenuSlot slot in menuSlots)
        {
            slot.ChangeColour(normalColor);
        }
        menuSlots[selectedSlotIndex].ChangeColour(selectedColor);
    }
}
