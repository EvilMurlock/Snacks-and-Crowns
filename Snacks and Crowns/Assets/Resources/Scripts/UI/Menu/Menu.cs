using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Menu : MonoBehaviour
{
    protected GameObject player;
    protected MenuSlot[] menuSlots;
    // menu is how a player interacts with an interactible object, its the medium between them
    void Start()
    {

    }
    protected void AttachToCanvas()
    {
        transform.SetParent(player.GetComponent<MenuManager>().canvas.transform, false);
    }
    protected void SubscribeToSlotEvents()
    {
        // Debug.Log("Subbing to slots");
        menuSlots = GetComponentsInChildren<MenuSlot>();
        foreach (MenuSlot slot in menuSlots)
        {
            ButtonHandler[] handlers = slot.GetComponentsInChildren<ButtonHandler>();
            foreach (var handler in handlers)
            {
                handler.Register(this);
            }
        }
    }
    public virtual void SlotSelect(MenuSlot slot)
    {
        Debug.Log("Menu slot select not implemented!");
    }
    public virtual void SlotSubmit(MenuSlot slot)
    {
        Debug.Log("Menu slot submit not implemented!");
    }
    public virtual void SlotCancel(MenuSlot slot)
    {
        Debug.Log("Menu slot cancel not implemented!");
    }
    public virtual void Refresh()
    {
        Debug.Log("Menu refresh not implemented!");
        // read inventory and refresh all shop slots and the item description
    }
    public bool BelongsToPlayer(GameObject playerToCompare) { return this.player == playerToCompare;}

    /*
    public void DeleteSelf(GameObject player)
    {
        Debug.Log("destroying self check");

        if (this.player == player)
        {
            Debug.Log("destroying self ui");
            Destroy(this.gameObject);
        }
    }*/
}
