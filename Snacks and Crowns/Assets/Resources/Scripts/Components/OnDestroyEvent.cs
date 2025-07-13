using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



/// <summary>
/// Manages an event that is called on GameObject destruction
/// </summary>
public class onDestroy : MonoBehaviour
{
    public UnityEvent destroyEvent = new UnityEvent();

    private void OnDestroy()
    {
        destroyEvent.Invoke();
    }
}
