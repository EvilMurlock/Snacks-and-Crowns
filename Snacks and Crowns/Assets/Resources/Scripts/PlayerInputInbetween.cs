using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


/// <summary>
/// Intermediery class for input deteciton, simply passed detected inputs  into the PlayerStateManager class
/// </summary>
public class PlayerInputInbetween : MonoBehaviour
{
    public
    PlayerStateManager playerStateManager;
    // Start is called before the first frame update
    void Start()
    {
        Initialize();   
    }
    public void Initialize()
    {
        PlayerStateManager[] managers = GetComponentsInChildren<PlayerStateManager>();
        playerStateManager = managers[managers.Length-1];
    }

    public void Movement(InputAction.CallbackContext context)
    {
        playerStateManager.Movement(context);
    }
    public void LeftHand(InputAction.CallbackContext context)
    {
        playerStateManager.LeftHand(context);
    }
    public void RightHand(InputAction.CallbackContext context)
    {
        playerStateManager.RightHand(context);
    }
    public void UseItem(InputAction.CallbackContext context)
    {
        playerStateManager.UseItem(context);
    }
    public void Interact(InputAction.CallbackContext context)
    {
        playerStateManager.Interact(context);
    }
    public void Scroll(InputAction.CallbackContext context)
    {
        playerStateManager.Scroll(context);
    }
    public void Inventory(InputAction.CallbackContext context)
    {
        playerStateManager.Inventory(context);
    }
}
