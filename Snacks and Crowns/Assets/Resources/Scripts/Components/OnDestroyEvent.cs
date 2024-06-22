using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class onDestroy : MonoBehaviour
{
    public UnityEvent destroyEvent = new UnityEvent();

    private void OnDestroy()
    {
        destroyEvent.Invoke();//parametr is vielder
    }
}
