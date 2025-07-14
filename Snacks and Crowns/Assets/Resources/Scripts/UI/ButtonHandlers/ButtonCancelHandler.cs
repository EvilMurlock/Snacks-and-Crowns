using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// Handles button cancel events
/// </summary>
public class ButtonCancelHandler : ButtonHandler, ICancelHandler
{
    // this event fires when the Cancel key is pressed on this button
    public void OnCancel(BaseEventData eventData)
    {
        buttonEvent.Invoke(gameObject.GetComponentInParent<MenuSlot>());
    }
    public override void Register(Menu menu)
    {
        buttonEvent.AddListener(menu.SlotCancel);
    }

}
