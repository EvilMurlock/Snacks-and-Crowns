using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



/// <summary>
/// Used for debug spawning without needing to set up player data during character selection
/// Is lo longer used, as we implemented the same thing in the MyJoinPlayerFromData class
/// </summary>
public class MyJoinPlayer : MonoBehaviour
{
    public PlayerInputManager inputManager;
    // Start is called before the first frame update
    void Start()
    {
        inputManager.playerJoinedEvent.AddListener(PlayerJoined);
        inputManager.playerLeftEvent.AddListener(PlayerLeft);
        
        PlayerInput player2 = inputManager.JoinPlayer(-1, // player index
                                -1, // split-screen index
                                controlScheme: "Player2", // control scheme
                                Keyboard.current); // pairWithDevice
        player2.gameObject.GetComponentInChildren<FactionMembership>().Faction = Factions.Two;

        PlayerInput player1 = inputManager.JoinPlayer(-1, // player index
                                -1, // split-screen index
                                controlScheme: "Player1", // control scheme
                                Keyboard.current); // pairWithDevice
        player1.gameObject.GetComponentInChildren<FactionMembership>().Faction = Factions.One;
    }

    void PlayerJoined(PlayerInput input)
    {
        Debug.Log("Player device "+ input.devices[0] + " scheme: " + input.currentControlScheme);
    }
    void PlayerLeft(PlayerInput input)
    {
        Debug.Log("Disconected player with device "+ input.devices[0] +" and scheme: " + input.currentControlScheme);
    }
}
