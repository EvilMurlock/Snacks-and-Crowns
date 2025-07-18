using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


/// <summary>
/// Respawns the player
/// </summary>
public class RespawnPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    GameObject playerPrefab;

    Damageable damagable;
    CameraFollowPlayer cameraFollow;
    
    PlayerData playerData;
    GameObject respawnPoint;
    PlayerInputInbetween playerInputInbetween;
    void Start()
    {
        damagable = GetComponentInChildren<Damageable>();
        SetUpDeathListener();
        cameraFollow = GetComponentInChildren<CameraFollowPlayer>();
        playerInputInbetween = GetComponent<PlayerInputInbetween>();
    }
    void SetUpDeathListener()
    {
        damagable.death.AddListener(Respawn);
    }
    public void Initialize(PlayerData data, GameObject spawnPoint)
    {
        playerData = data;
        this.respawnPoint = spawnPoint;
    }

    void Respawn()
    {
        GameObject playerChar = Instantiate(playerPrefab, gameObject.transform);
        if (respawnPoint != null)
        {
            playerChar.transform.position = respawnPoint.transform.position;
        }
        if (playerData != null)
        {
            playerChar.gameObject.GetComponent<FactionMembership>().Faction = playerData.faction;
            playerChar.gameObject.GetComponent<CharacterSheet>().SetRace(playerData.race, playerData.face);
        }
        damagable = playerChar.GetComponent<Damageable>();
        SetUpDeathListener();
        cameraFollow.player = playerChar;
        playerInputInbetween.Initialize();

        GetComponentInChildren<GoldDisplay>().CoupleToPlayer(playerChar);
        GetComponentInChildren<HealthTracker>().CoupleToPlayer(playerChar);
        GetComponentInChildren<HotbarMenu>().CoupleToPlayer(playerChar);

    }
}
