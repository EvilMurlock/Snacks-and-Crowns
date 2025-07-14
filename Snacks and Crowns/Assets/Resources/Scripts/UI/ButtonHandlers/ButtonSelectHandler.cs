using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;



/// <summary>
/// Handles select events of a button
/// </summary>
public class ButtonSelectHandler : ButtonHandler, ISelectHandler
{
    // this event fires when this button is selected with the cursor (not pointer cursor, but index "cursor")
    public void OnSelect(BaseEventData eventData)
    {
        buttonEvent.Invoke(gameObject.GetComponentInParent<MenuSlot>());
    }
    public override void Register(Menu menu)
    {
        buttonEvent.AddListener(menu.SlotSelect);
    }

}
