using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;
public class KillPlayer : Goal
{
    float priority = 10;
    private void Awake()
    {
        desiredState.AddState("DeadPlayer", true);
    }

    public override void Tick()
    {

    }
    public override void Activate()
    {

    }
    public override void Deactivate()
    {

    }

    public override void Complete()
    {

    }
    public override float CalculatePriority()
    {
        return priority;
    }
}
