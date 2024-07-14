using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;
public class RaidCommand : DialogueComponentData<ActivateGolem>
{
    public override void InicializeComponent(GameObject player, GameObject listener)
    {
        Factions playerFaction = player.GetComponent<FactionMembership>().Faction;
        Factions npcFaction = listener.GetComponent<FactionMembership>().Faction;
        if (playerFaction != npcFaction &&
            FactionState.GetFactionRelations(playerFaction, npcFaction) != Relations.Alliance)
            return;

        listener.GetComponent<GoToLocation>().enabledGoal = false;
        listener.GetComponent<LaunchRaid>().enabledGoal = true;
        listener.GetComponent<IdleAroundAPoint>().enabledGoal = false;
    }
}
