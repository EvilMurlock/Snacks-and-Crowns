using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;
public class SleepJob : Job
{
    [SerializeField]
    GameObject bed;

    public override void SetGoalsOfAnNPC(GameObject npc)
    {
        SleepGoal sleep = npc.AddComponent<SleepGoal>();
        sleep.Initialize(bed);
        base.SetGoalsOfAnNPC(npc);
    }
}
