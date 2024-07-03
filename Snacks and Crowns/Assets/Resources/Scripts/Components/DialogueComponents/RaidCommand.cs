using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;
public class RaidCommand : DialogueComponentData<ActivateGolem>
{
    public override void InicializeComponent(GameObject player, GameObject listener)
    {
        if (player.GetComponent<FactionMembership>().Faction != listener.GetComponent<FactionMembership>().Faction)
            return;

        listener.GetComponent<GoToLocation>().enabledGoal = false;
        listener.GetComponent<LaunchRaid>().enabledGoal = true;
        listener.GetComponent<IdleAroundAPoint>().enabledGoal = false;
    }
}
