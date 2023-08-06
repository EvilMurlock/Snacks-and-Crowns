using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class Player_State_Manager : MonoBehaviour
{
    [SerializeField]
    Player_Inventory inventory;
    [SerializeField]
    Player_Movement movement;
    [SerializeField]
    Player_Interact_Manager interact_manager;
    public Player_State player_state;
    // Start is called before the first frame update
    Player_State preStunState;
    void Start()
    {
        player_state = Player_State.normal;
        movement.stun.AddListener(ManageStun);
    }
    public void ManageStun(bool value)
    {
        if (value)
        {
            preStunState = player_state;
            Change_State(Player_State.stun);
        }
        else Change_State(preStunState);
    }

    public void Movement(InputAction.CallbackContext context)
    {
        switch (player_state)
        {
            case Player_State.normal:
                movement.On_Move(context);
                break;
            case Player_State.in_menu:
                inventory.On_Move(context);
                break;
            case Player_State.in_ui_menu:
                break;
            case Player_State.stun:
                movement.On_Move(context);
                break;


        }
    }
    public void Left_Hand(InputAction.CallbackContext context)
    {
        switch (player_state)
        {
            case Player_State.normal:
                inventory.Use_Left_Hand(context);
                break;
            case Player_State.in_menu:
                inventory.Select(context);
                break;
            case Player_State.in_ui_menu:
                break;
            case Player_State.stun:
                break;

        }
    }
    public void Right_Hand(InputAction.CallbackContext context)
    {
        switch (player_state)
        {
            case Player_State.normal:
                inventory.Use_Right_Hand(context);
                break;
            case Player_State.in_menu:
                inventory.Confirm(context);
                break;
            case Player_State.in_ui_menu:
                break;
            case Player_State.stun:
                break;


        }
    }
    public void Use_Item(InputAction.CallbackContext context)
    {
        switch (player_state)
        {
            case Player_State.normal:
                inventory.Use_Item(context);
                break;
            case Player_State.in_menu:
                inventory.Throw_Away(context);
                break;
            case Player_State.in_ui_menu:
                break;
            case Player_State.stun:
                break;


        }
    }
    public void Interact(InputAction.CallbackContext context)
    {
        Debug.Log("Interacting");
        switch (player_state)
        {
            case Player_State.normal:
                interact_manager.Interact(context);
                break;
            case Player_State.in_menu:
                break;
            case Player_State.in_ui_menu:
                interact_manager.UnInteract(context);
                break;
            case Player_State.stun:
                break;


        }
    }
    public void Scroll(InputAction.CallbackContext context)
    {
        switch (player_state)
        {
            case Player_State.normal:
                inventory.ScrollHotbar(context);
                break;
            case Player_State.in_menu:
                inventory.Scroll_Menu(context);
                break;
            case Player_State.in_ui_menu:
                break;
            case Player_State.stun:
                break;


        }
    }
    public void Inventory(InputAction.CallbackContext context)
    {
        switch (player_state)
        {
            case Player_State.normal:
                inventory.Togle_Inventory(context);
                break;
            case Player_State.in_menu:
                inventory.Togle_Inventory(context);
                break;
            case Player_State.in_ui_menu:
                inventory.Remove_Interacted_Object();
                break;
            case Player_State.stun:
                break;

        }
    }

    public void Change_State(Player_State new_state)
    {
        switch (new_state)
        {
            case Player_State.normal:
                break;
            case Player_State.in_menu:
                movement.Move_Stop();
                break;
            case Player_State.in_ui_menu:
                movement.Move_Stop();
                break;
            case Player_State.stun:
                break;
        }

        player_state = new_state;
    }
}
public enum Player_State
{
    normal,
    in_menu,
    in_ui_menu,
    stun
}
