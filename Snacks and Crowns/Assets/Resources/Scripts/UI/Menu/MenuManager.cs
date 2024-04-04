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
    [SerializeField]
    GameObject inventoryMenuPrefab;
    GameObject inventoryMenu;

    void Start()
    {
    }

    public void OpenInventory()
    {
        inventoryMenu = Instantiate(inventoryMenuPrefab);
        inventoryMenu.GetComponent<InventoryMenu>().Initialize(this.gameObject);
        //inventoryMenu.transform.SetParent(canvas.transform);
        GetComponent<PlayerStateManager>().ChangeState(CharakterState.inMenu);
    }
    public void CloseInventory()
    {
        Destroy(inventoryMenu);
        GetComponent<PlayerStateManager>().ChangeState(CharakterState.normal);
    }
    public void SelectObject(GameObject gameObjecct)
    {
        multiplayerEventSystem.SetSelectedGameObject(gameObjecct);
    }
}