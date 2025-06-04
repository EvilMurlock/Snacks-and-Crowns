using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class MyJoinPlayerFromData : MonoBehaviour
{
    public PlayerInputManager inputManager;
    [SerializeField]
    GameObject RespawnPointOne;
    [SerializeField]
    GameObject RespawnPointTwo;
    List<PlayerInput> players = new List<PlayerInput>();
    // Start is called before the first frame update
    void Start()
    {
        inputManager.playerJoinedEvent.AddListener(PlayerJoined);
        if (StartGameDataHolder.Players.Count != 0)
        {
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
        //Debug.Log("Player device is: " + input.devices[0]);
        //Debug.Log("Player scheme: " + input.currentControlScheme);
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
