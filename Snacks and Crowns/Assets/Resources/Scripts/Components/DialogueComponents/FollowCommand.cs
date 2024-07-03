using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;
public class FollowCommand : DialogueComponentData<ActivateGolem>
{
    public override void InicializeComponent(GameObject player, GameObject listener)
    {
        if (player.GetComponent<FactionMembership>().Faction != listener.GetComponent<FactionMembership>().Faction)
            return;

        listener.GetComponent<GoToLocation>().enabledGoal = true;
        listener.GetComponent<GoToLocation>().SetDesiredTarget(player);
        listener.GetComponent<LaunchRaid>().enabledGoal = false;
        listener.GetComponent<IdleAroundAPoint>().enabledGoal = false;
    }
}
