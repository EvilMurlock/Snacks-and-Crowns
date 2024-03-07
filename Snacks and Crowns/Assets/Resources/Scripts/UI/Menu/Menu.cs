using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }
    protected void SubscribeToSlotEvents()
    {
        Debug.Log("Subbing to slots");
        MenuSlot[] slots = GetComponentsInChildren<MenuSlot>();
        foreach(MenuSlot slot in slots)
        {
            ButtonHandler[] handlers = slot.GetComponentsInChildren<ButtonHandler>();
            foreach(var handler in handlers)
            {
                handler.Register(this);
            }
        }
        Debug.Log("Subscribed to " + slots.Length + " slots");
    }
    public virtual void SlotSelect(MenuSlot slot)
    {
        Debug.Log("Slot selected: " + slot.ToString());
    }
    public virtual void SlotSubmit(MenuSlot slot)
    {
        Debug.Log("Slot submited: " + slot.ToString());
    }
    public virtual void SlotCancel(MenuSlot slot)
    {
        Debug.Log("Slot canceled: " + slot.ToString());
    }

}
