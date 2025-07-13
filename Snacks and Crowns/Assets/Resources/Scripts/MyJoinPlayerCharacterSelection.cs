using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
public class MyJoinPlayerCharecterSelection : MonoBehaviour
{
    public PlayerInputManager inputManager;
    List<PlayerInput> players = new List<PlayerInput>();


    /// <summary>
    /// force joins 2 players, then helps to initialize other players
    /// </summary>
    void Start()
    {
        inputManager.playerJoinedEvent.AddListener(PlayerJoined);
        
        PlayerInput player2 = inputManager.JoinPlayer(-1, // player index
                                -1, // split-screen index
                                controlScheme: "Player2", // control scheme
                                Keyboard.current); // pairWithDevice
        PlayerInput player1 = inputManager.JoinPlayer(-1, // player index
                                -1, // split-screen index
                                controlScheme: "Player1", // control scheme
                                Keyboard.current); // pairWithDevice
        inputManager.joinBehavior = PlayerJoinBehavior.JoinPlayersWhenButtonIsPressed;
    }
    /// <summary>
    /// initializes the joined player
    /// </summary>
    /// <param name="input">Joined player</param>
    void PlayerJoined(PlayerInput input)
    {
        FindFirstObjectByType<LoadLevelWhenReady>().AddPlayer();
        input.transform.parent.GetComponentInChildren<ReadySelect>().deviceType = input.devices[0];
        input.transform.parent.GetComponentInChildren<ReadySelect>().controlScheme = input.currentControlScheme;
        input.transform.parent.GetComponentInChildren<ReadySelect>().SetIndex(players.Count);
        players.Add(input);
        SetCameras();
    }
    void SetCameras()
    {
        int index = 0;
        foreach (var player in players)
        {
            if (players.Count == 3)
            {
                if (index == 0)
                {
                    player.camera.rect = new Rect(0, 0.5f, 0.5f, 0.5f);
                }
                if (index == 1)
                {
                    player.camera.rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
                }
                if (index == 2)
                {
                    player.camera.rect = new Rect(0, 0, 1f, 0.5f);
                }

            }
            index++;
        }
    }
}
