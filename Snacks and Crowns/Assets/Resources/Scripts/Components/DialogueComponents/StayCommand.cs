using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;
public class StayCommand : DialogueComponentData<ActivateGolem>
{
    public override void InicializeComponent(GameObject player, GameObject listener)
    {
        Factions playerFaction = player.GetComponent<FactionMembership>().Faction;
        Factions npcFaction = listener.GetComponent<FactionMembership>().Faction;
        if (playerFaction != npcFaction &&
            FactionState.GetFactionRelations(playerFaction, npcFaction) != Relations.Alliance)
            return;

        listener.GetComponent<GoToLocation>().enabledGoal = false;
        listener.GetComponent<LaunchRaid>().enabledGoal = false;
        listener.GetComponent<IdleAroundAPoint>().enabledGoal = true;
        listener.GetComponent<IdleAroundAPoint>().SetDesiredTarget(listener.transform.position);
    }
}
