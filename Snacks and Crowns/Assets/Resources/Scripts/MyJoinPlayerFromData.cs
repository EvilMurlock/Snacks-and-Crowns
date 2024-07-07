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
    // Start is called before the first frame update
    void Start()
    {
        inputManager.playerJoinedEvent.AddListener(PlayerJoined);
        foreach(PlayerData data in StartGameDataHolder.Players)
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

    void PlayerJoined(PlayerInput input)
    {
        //Debug.Log("Player device is: " + input.devices[0]);
        //Debug.Log("Player scheme: " + input.currentControlScheme);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
