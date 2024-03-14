using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class PlayerStateManager : MonoBehaviour
{
    [SerializeField]
    Inventory inventory;

    [SerializeField]
    EquipmentManager equipmentManager;
    [SerializeField]
    Movement movement;
    [SerializeField]
    PlayerInteractManager interactManager;
    public CharakterState playerState;
    // Start is called before the first frame update
    CharakterState preStunState;
    void Start()
    {
        interactManager = GetComponent<PlayerInteractManager>();
        playerState = CharakterState.normal;
        movement.stun.AddListener(ManageStun);
    }
    public void ManageStun(bool value)
    {
        if (value)
        {
            preStunState = playerState;
            ChangeState(CharakterState.stun);
        }
        else ChangeState(preStunState);
    }

    public void Movement(InputAction.CallbackContext context)
    {
        switch (playerState)
        {
            case CharakterState.normal:
                movement.OnMove(context);
                break;
            case CharakterState.inMenu:
                break;
            case CharakterState.stun:
                movement.OnMove(context);
                break;


        }
    }
    public void LeftHand(InputAction.CallbackContext context)
    {
        switch (playerState)
        {
            case CharakterState.normal:
                //equipmentManager.UseLeftHand();
                break;
            case CharakterState.inMenu:
                break;
            case CharakterState.stun:
                break;

        }
    }
    public void RightHand(InputAction.CallbackContext context)
    {
        switch (playerState)
        {
            case CharakterState.normal:
                //equipmentManager.UseRightHand();
                break;
            case CharakterState.inMenu:
                break;
            case CharakterState.stun:
                break;


        }
    }
    public void UseItem(InputAction.CallbackContext context)
    {
        switch (playerState)
        {
            case CharakterState.normal:
                //inventory.UseItem(0);//change this
                break;
            case CharakterState.inMenu:
                break;
            case CharakterState.stun:
                break;


        }
    }
    public void Interact(InputAction.CallbackContext context)
    {
        // Debug.Log("Interacting");
        switch (playerState)
        {
            case CharakterState.normal:
                interactManager.Interact(context);
                break;
            case CharakterState.inMenu:
                interactManager.UnInteract(context);
                break;
            case CharakterState.stun:
                break;


        }
    }
    public void Scroll(InputAction.CallbackContext context)
    {
        switch (playerState)
        {
            case CharakterState.normal:
                //inventory.ScrollHotbar(context);
                break;
            case CharakterState.inMenu:
                break;
            case CharakterState.stun:
                break;


        }
    }
    public void Inventory(InputAction.CallbackContext context)
    {
        switch (playerState)
        {
            case CharakterState.normal:
                //inventory.TogleInventory(context);
                break;
            case CharakterState.inMenu:
                //inventory.TogleInventory(context);
                break;
            case CharakterState.stun:
                break;

        }
    }

    public void ChangeState(CharakterState newState)
    {
        switch (newState)
        {
            case CharakterState.normal:
                break;
            case CharakterState.inMenu:
                movement.MoveStop();
                break;
            case CharakterState.stun:
                break;
        }

        playerState = newState;
    }
}
