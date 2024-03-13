using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour
{
    public Canvas canvas;
    public MultiplayerEventSystem eventSystem;



    void Start()
    {

    }
    public void Innitialize(Canvas canvas, MultiplayerEventSystem eventSystem)
    {
        this.canvas = canvas;
        this.eventSystem = eventSystem;
    }
    public void SetSelectedGameObject(GameObject gameObject)
    {
        //eventSystem.SetSelectedGameObject()
    }
}