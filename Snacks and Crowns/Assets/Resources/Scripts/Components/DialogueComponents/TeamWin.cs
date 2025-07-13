using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;
using UnityEngine.InputSystem;
using Pathfinding;
public class TeamWin : DialogueComponentData<ActivateGolem>
{
    [SerializeField]
    GameObject prefabGameEndMenu;
    public override void InicializeComponent(GameObject player, GameObject listener)
    {
        Factions winningFaction = listener.GetComponent<FactionMembership>().Faction;//player.GetComponent<FactionMembership>().Faction;
        MyJoinPlayerFromData myJoinPlayers = GameObject.Find("PlayerManager").GetComponent<MyJoinPlayerFromData>();

        var players = myJoinPlayers.GetPlayers();
        foreach(PlayerInput input in players)
        {
            GameObject GameEndMenu = GameObject.Instantiate(prefabGameEndMenu, input.gameObject.GetComponentInChildren<Canvas>().gameObject.transform);

            bool sameTeam;
            Factions otherFaction = input.gameObject.GetComponentInChildren<FactionMembership>().Faction;
            if (winningFaction == otherFaction)
                sameTeam = true;
            else
                sameTeam = false;

            GameEndMenu.GetComponent<GameEndMenu>().Inicialize(sameTeam);

            GameEndMenu.GetComponent<GameEndMenu>().StartCoroutine(SetPlaytInMenuState(input));
        }
    }
    
    IEnumerator SetPlaytInMenuState(PlayerInput input)
    {
        yield return new WaitForSeconds(0.05f);
        input.gameObject.GetComponentInChildren<PlayerStateManager>().ChangeState(CharakterState.inMenu);
    }
}
