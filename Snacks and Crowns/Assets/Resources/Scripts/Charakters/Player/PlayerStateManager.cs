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
        playerState = CharakterState.normal;
        movement.stun.AddListener(ManageStun);
    }
    public void ManageStun(bool value)
    {
        if (value)
        {
            preStunState = playerState;
            Change_State(CharakterState.stun);
        }
        else Change_State(preStunState);
    }

    public void Movement(InputAction.CallbackContext context)
    {
        switch (playerState)
        {
            case CharakterState.normal:
                movement.OnMove(context);
                break;
            case CharakterState.in_menu:
                break;
            case CharakterState.stun:
                movement.OnMove(context);
                break;


        }
    }
    public void Left_Hand(InputAction.CallbackContext context)
    {
        switch (playerState)
        {
            case CharakterState.normal:
                equipmentManager.UseLeftHand();
                break;
            case CharakterState.in_menu:
                break;
            case CharakterState.stun:
                break;

        }
    }
    public void Right_Hand(InputAction.CallbackContext context)
    {
        switch (playerState)
        {
            case CharakterState.normal:
                equipmentManager.UseRightHand();
                break;
            case CharakterState.in_menu:
                break;
            case CharakterState.stun:
                break;


        }
    }
    public void Use_Item(InputAction.CallbackContext context)
    {
        switch (playerState)
        {
            case CharakterState.normal:
                inventory.UseItem(0);//change this
                break;
            case CharakterState.in_menu:
                break;
            case CharakterState.stun:
                break;


        }
    }
    public void Interact(InputAction.CallbackContext context)
    {
        Debug.Log("Interacting");
        switch (playerState)
        {
            case CharakterState.normal:
                interactManager.Interact(context);
                break;
            case CharakterState.in_menu:
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
            case CharakterState.in_menu:
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
            case CharakterState.in_menu:
                //inventory.TogleInventory(context);
                break;
            case CharakterState.stun:
                break;

        }
    }

    public void Change_State(CharakterState newState)
    {
        switch (newState)
        {
            case CharakterState.normal:
                break;
            case CharakterState.in_menu:
                movement.MoveStop();
                break;
            case CharakterState.stun:
                break;
        }

        playerState = newState;
    }
}
