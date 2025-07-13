using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;



/// <summary>
/// Manages the player state and interprets player inputs according to the state
/// </summary>
public class PlayerStateManager : MonoBehaviour
{
    bool debug = false;

    [SerializeField]
    Inventory inventory;

    [SerializeField]
    EquipmentManager equipmentManager;
    [SerializeField]
    Movement movement;
    [SerializeField]
    PlayerInteractManager interactManager;
    MenuManager menuManager;
    [SerializeField]
    HotbarMenu hotbarMenu;
    public CharacterState playerState;
    // Start is called before the first frame update
    CharacterState preStunState;
    void Start()
    {
        interactManager = GetComponent<PlayerInteractManager>();
        menuManager = GetComponent<MenuManager>();
        playerState = CharacterState.normal;
        movement.stun.AddListener(ManageStun);
        hotbarMenu = transform.parent.GetComponentInChildren<HotbarMenu>();
    }
    public void ManageStun(bool value)
    {
        if (value)
        {
            preStunState = playerState;
            ChangeState(CharacterState.stun);
        }
        else ChangeState(preStunState);
    }

    public void Movement(InputAction.CallbackContext context)
    {
        if(debug) Debug.Log("MOVIGN");
        switch (playerState)
        {
            case CharacterState.normal:
                movement.OnMove(context);
                break;
            case CharacterState.inMenu:
                break;
            case CharacterState.stun:
                movement.OnMove(context);
                break;


        }
    }
    public void LeftHand(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (debug) Debug.Log("Left hand");
        switch (playerState)
        {
            case CharacterState.normal:
                equipmentManager.UseLeftHand();
                break;
            case CharacterState.inMenu:
                break;
            case CharacterState.stun:
                break;

        }
    }
    public void RightHand(InputAction.CallbackContext context)
    {
        //if (!context.started) return;
        if (!context.started) return;
        if (debug) Debug.Log("Right hand");
        switch (playerState)
        {
            case CharacterState.normal:
                equipmentManager.UseRightHand();
                break;
            case CharacterState.inMenu:
                break;
            case CharacterState.stun:
                break;


        }
    }
    public void UseItem(InputAction.CallbackContext context)
    {
        
        if (!context.started) return;

        if (debug) Debug.Log("Use item");
        switch (playerState)
        {
            case CharacterState.normal:
                hotbarMenu.UseItem();
                break;
            case CharacterState.inMenu:
                break;
            case CharacterState.stun:
                break;


        }
    }
    public void Interact(InputAction.CallbackContext context)
    {
        if (debug) Debug.Log("Interacting");
        switch (playerState)
        {
            case CharacterState.normal:
                interactManager.Interact(context);
                break;
            case CharacterState.inMenu:
                interactManager.UnInteract(context);
                break;
            case CharacterState.stun:
                break;


        }
    }
    public void Scroll(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        if (debug) Debug.Log("Scroll");
        switch (playerState)
        {
            case CharacterState.normal:
                hotbarMenu.Scroll(context);
                break;
            case CharacterState.inMenu:
                break;
            case CharacterState.stun:
                break;


        }
    }
    public void Inventory(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (debug) Debug.Log("Inventory");
        switch (playerState)
        {
            case CharacterState.normal:
                menuManager.OpenInventory();
                break;
            case CharacterState.inMenu:
                menuManager.CloseInventory();
                break;
            case CharacterState.stun:
                break;

        }
    }

    public void ChangeState(CharacterState newState)
    {
        switch (newState)
        {
            case CharacterState.normal:
                break;
            case CharacterState.inMenu:
                movement.MoveStop();
                break;
            case CharacterState.stun:
                break;
        }

        playerState = newState;
    }
}
