using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


/// <summary>
/// Creates players in the level, based on character data generated in the character selection scene
/// If no such data exists generates 2 players for debugging
/// </summary>
public class MyJoinPlayerFromData : MonoBehaviour
{
    public PlayerInputManager inputManager;
    [SerializeField]
    GameObject RespawnPointOne;
    [SerializeField]
    GameObject RespawnPointTwo;
    List<PlayerInput> players = new List<PlayerInput>();
    void Start()
    {
        inputManager.playerJoinedEvent.AddListener(PlayerJoined);
        if (StartGameDataHolder.Players.Count != 0)
        {
            StartGameDataHolder.Players.Sort((x, y) => { return x.index.CompareTo(y.index); }) ;
            foreach (PlayerData data in StartGameDataHolder.Players)
            {
                PlayerInput player = inputManager.JoinPlayer(-1, // player index
                                    -1, // split-screen index
                                    controlScheme: data.controlScheme, // control scheme
                                    data.deviceType); // pairWithDevice
                player.gameObject.GetComponentInChildren<FactionMembership>().Faction = data.faction;
                player.gameObject.GetComponentInChildren<CharakterSheet>().SetRace(data.race, data.face);
                GameObject spawnPoint;
                if (data.faction == Factions.One)
                    spawnPoint = RespawnPointOne;
                else
                    spawnPoint = RespawnPointTwo;
                player.transform.position = spawnPoint.transform.position;

                player.GetComponent<RespawnPlayer>().Initialize(data, spawnPoint);

            }
        }
        else
        {
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
    }

    void PlayerJoined(PlayerInput input)
    {
        players.Add(input);
        SetCameras();

    }

    /// <summary>
    /// manages camera shapes for 3 players, so that the third players camera stretches over 2 quadrants 
    /// </summary>
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
public List<PlayerInput> GetPlayers()
    {
        return players;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
