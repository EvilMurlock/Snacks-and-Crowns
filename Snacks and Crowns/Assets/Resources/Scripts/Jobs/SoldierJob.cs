using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;

public class SoldierJob : Job
{
    [SerializeField]
    Dialogue ordersDialogue;
    public override void SetGoalsOfAnNPC(GameObject npc)
    {
        npc.GetComponent<DialogueManager>().startDialogue = ordersDialogue;

        var goalGoTo = npc.AddComponent<GoToLocation>();
        goalGoTo.enabledGoal = false;

        var goalRaid = npc.AddComponent<LaunchRaid>();
        goalRaid.enabledGoal = false;

        base.SetGoalsOfAnNPC(npc);
    }
}
