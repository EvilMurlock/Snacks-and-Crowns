using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
public class MyJoinPlayerCharecterSelection : MonoBehaviour
{
    public PlayerInputManager inputManager;
    // Start is called before the first frame update
    List<PlayerInput> players = new List<PlayerInput>();
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
        /*
        inputManager.JoinPlayer(-1, // player index
                                -1, // split-screen index
                                controlScheme: "Controller", // control scheme
                                InputSystem.devices[2]); // pairWithDevice
        */
        inputManager.joinBehavior = PlayerJoinBehavior.JoinPlayersWhenButtonIsPressed;
        /*
        Debug.Log("Contorlers detected: " + InputSystem.devices.Count);
        Debug.Log("Contorlers detected: " + InputSystem.devices[0].name);
        Debug.Log("Contorlers detected: " + InputSystem.devices[2].name);*/
    }

    void PlayerJoined(PlayerInput input)
    {
        //Debug.Log("Player device is: " + input.devices[0]);
        FindFirstObjectByType<LoadLevelWhenReady>().AddPlayer();
        input.transform.parent.GetComponentInChildren<ReadySelect>().deviceType = input.devices[0];
        input.transform.parent.GetComponentInChildren<ReadySelect>().controlScheme = input.currentControlScheme;
        //Debug.Log("Player scheme: " + input.currentControlScheme);
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

    // Update is called once per frame
    void Update()
    {
    }
}
