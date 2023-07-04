using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Dropdown_Scroller : MonoBehaviour, ISelectHandler
{
    ScrollRect scroll_rekt;
    float scroll_position = 1;
    void Start()
    {
        scroll_rekt = GetComponentInParent<ScrollRect>(true);
        int child_count = scroll_rekt.content.transform.childCount - 1;
        int child_index = transform.GetSiblingIndex();

        child_index = child_index < ((float)child_count / 2) ? child_index - 1 : child_index;
        scroll_position = 1 - ((float)child_index / child_count);
    }

    public void OnSelect(BaseEventData event_data)
    {
        if(scroll_rekt)
        scroll_rekt.verticalScrollbar.value = scroll_position;
    }
}
