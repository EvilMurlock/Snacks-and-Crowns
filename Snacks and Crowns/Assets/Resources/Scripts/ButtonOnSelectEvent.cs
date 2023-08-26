using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ButtonOnSelectEvent : MonoBehaviour, ISelectHandler
{
    public UnityEvent onSelect;
    private void Start()
    {
        //onSelect = new UnityEvent();
    }
    public void OnSelect(BaseEventData eventData)
    {
        onSelect.Invoke();
    }
}
