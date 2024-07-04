using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
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
        player2.gameObject.GetComponent<FactionMembership>().Faction = Factions.Two;

        PlayerInput player1 = inputManager.JoinPlayer(-1, // player index
                                -1, // split-screen index
                                controlScheme: "Player1", // control scheme
                                Keyboard.current); // pairWithDevice
        player1.gameObject.GetComponent<FactionMembership>().Faction = Factions.One;

        /*
        inputManager.JoinPlayer(-1, // player index
                                -1, // split-screen index
                                controlScheme: "Controller", // control scheme
                                InputSystem.devices[2]); // pairWithDevice
        */
        //inputManager.joinBehavior = PlayerJoinBehavior.JoinPlayersWhenButtonIsPressed;
        /*
        Debug.Log("Contorlers detected: " + InputSystem.devices.Count);
        Debug.Log("Contorlers detected: " + InputSystem.devices[0].name);
        Debug.Log("Contorlers detected: " + InputSystem.devices[2].name);*/
    }

    void PlayerJoined(PlayerInput input)
    {
        Debug.Log("Player device "+ input.devices[0] + " scheme: " + input.currentControlScheme);
    }
    void PlayerLeft(PlayerInput input)
    {
        Debug.Log("Disconected player with device "+ input.devices[0] +" and scheme: " + input.currentControlScheme);
        
        PlayerInput player = inputManager.JoinPlayer(input.playerIndex, // player index
                                input.splitScreenIndex, // split-screen index
                                controlScheme: input.currentControlScheme, // control scheme
                                input.devices[0]); // pairWithDevice
    }
}
