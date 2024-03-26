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
    public MultiplayerEventSystem multiplayerEventSystem;



    void Start()
    {
    }
    public void Innitialize(Canvas canvas, MultiplayerEventSystem multiplayerEventSystem)
    {
        this.canvas = canvas;
        this.multiplayerEventSystem = multiplayerEventSystem;
    }
    public void SelectObject(GameObject gameObjecct)
    {
        multiplayerEventSystem.SetSelectedGameObject(gameObjecct);
    }
}