using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class RespawnPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    GameObject playerPrefab;

    Damagable damagable;
    CameraFollowPlayer cameraFollow;
    
    PlayerData playerData;
    GameObject respawnPoint;
    PlayerInputInbetween playerInputInbetween;
    void Start()
    {
        damagable = GetComponentInChildren<Damagable>();
        damagable.death.AddListener(Respawn);
        cameraFollow = GetComponentInChildren<CameraFollowPlayer>();
        playerInputInbetween = GetComponent<PlayerInputInbetween>();
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
            playerChar.gameObject.GetComponent<CharakterSheet>().SetRace(playerData.race, playerData.face);
        }
        cameraFollow.player = playerChar;
        playerInputInbetween.Initialize();

        GetComponentInChildren<GoldDisplay>().CoupleToPlayer(playerChar);
        GetComponentInChildren<HealthTracker>().CoupleToPlayer(playerChar);
        GetComponentInChildren<HotbarMenu>().CoupleToPlayer(playerChar);

    }
}
