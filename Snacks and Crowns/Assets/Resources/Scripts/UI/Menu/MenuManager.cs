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
        canvas = transform.parent.GetComponentInChildren<Canvas>();
        multiplayerEventSystem = transform.parent.GetComponentInChildren<MultiplayerEventSystem>();
    }

    public void OpenInventory()
    {
        inventoryMenu = Instantiate(inventoryMenuPrefab, canvas.transform);
        inventoryMenu.GetComponent<InventoryMenu>().Initialize(this.gameObject);
        GetComponent<PlayerStateManager>().ChangeState(CharacterState.inMenu);
    }
    public void CloseInventory()
    {
        if (inventoryMenu == null) return;
        Destroy(inventoryMenu);
        GetComponent<PlayerStateManager>().ChangeState(CharacterState.normal);
    }
    public void SelectObject(GameObject gameObjecct)
    {
        multiplayerEventSystem.SetSelectedGameObject(gameObjecct);
    }
}