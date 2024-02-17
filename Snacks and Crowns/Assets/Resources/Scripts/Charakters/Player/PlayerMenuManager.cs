using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
public class PlayerMenuManager : MonoBehaviour
{
    public Canvas canvas;
    public GameObject eventSystem;

    public GameObject inventoryMenuPrefab;
    public GameObject inventoryMenu;

    bool firstButtonPress = true;
    void Start()
    {
    }

    public void ButtonPress()
    {
        if (firstButtonPress)
        {
            // select tile
        }
        else
        {
            // swap tiles
        }
    }
}