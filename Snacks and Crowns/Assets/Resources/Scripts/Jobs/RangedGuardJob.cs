using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;

public class RangedGuardJob : Job
{

    [SerializeField]
    GameObject patrolPoint;
    [SerializeField]
    float patrolRadius;
    public override void SetGoalsOfAnNPC(GameObject npc)
    {
        IdleAroundAPoint idleAroundAPoint = npc.AddComponent<GOAP.IdleAroundAPoint>();
        idleAroundAPoint.Initialize(patrolPoint.transform.position, patrolRadius, 4);

        RangedFightGoal rangedFightGoal = npc.AddComponent<GOAP.RangedFightGoal>();

        base.SetGoalsOfAnNPC(npc);
    }
}
