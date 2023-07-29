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
    void Start()
    {
        player_state = Player_State.normal;
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
        }

        player_state = new_state;
    }
    public void Stun(float stunTime)
    {
        StopCoroutine("StunCo");
        StartCoroutine("StunCo",stunTime);
    }
    IEnumerator StunCo(float stunTime)
    {
        movement.ChangeSpeed(0,0);
        yield return new WaitForSeconds(stunTime);
        movement.ResetSpeed();
    }
}
public enum Player_State
{
    normal,
    in_menu,
    in_ui_menu,
}
