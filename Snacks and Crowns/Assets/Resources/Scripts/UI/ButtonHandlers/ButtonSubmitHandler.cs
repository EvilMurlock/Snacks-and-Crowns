using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ButtonSubmitHandler : ButtonHandler, ISubmitHandler
{
    // this event fires when the Submit key is pressed on this button
    public void OnSubmit(BaseEventData eventData)
    {
        buttonEvent.Invoke(gameObject.GetComponentInParent<MenuSlot>());
    }
    public override void Register(Menu menu)
    {
        buttonEvent.AddListener(menu.SlotSubmit);
    }
}
