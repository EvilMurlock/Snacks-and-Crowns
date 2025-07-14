using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


/// <summary>
/// parent to button event handlers
/// </summary>
public abstract class ButtonHandler : MonoBehaviour
{
    public delegate void MyEventDelegate(MenuSlot menuSlot);

    protected UnityEvent<MenuSlot> buttonEvent;
    // Start is called before the first frame update
    void Awake()
    {
        buttonEvent = new UnityEvent<MenuSlot>();
    }
    public abstract void Register(Menu menu);
}
