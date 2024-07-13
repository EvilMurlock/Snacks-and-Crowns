using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;
using UnityEngine.InputSystem;
public class TeamWin : DialogueComponentData<ActivateGolem>
{
    [SerializeField]
    GameObject prefabGameEndMenu;
    public override void InicializeComponent(GameObject player, GameObject listener)
    {
        Factions playerFaction = player.GetComponent<FactionMembership>().Faction;
        MyJoinPlayerFromData myJoinPlayers = GameObject.Find("PlayerManager").GetComponent<MyJoinPlayerFromData>();

        var players = myJoinPlayers.GetPlayers();
        foreach(PlayerInput input in players)
        {
            GameObject GameEndMenu = GameObject.Instantiate(prefabGameEndMenu, input.gameObject.GetComponentInChildren<Canvas>().gameObject.transform);

            bool sameTeam;
            Factions otherFaction = input.gameObject.GetComponentInChildren<FactionMembership>().Faction;
            if (playerFaction == otherFaction)
                sameTeam = true;
            else
                sameTeam = false;
            
            GameEndMenu.GetComponent<GameEndMenu>().Inicialize(sameTeam);
        }
    }
}
