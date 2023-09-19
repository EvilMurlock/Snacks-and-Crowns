using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;
public class KillPlayer : Goal
{
    float defaultPriority = 10;
    private void Start()
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
        return defaultPriority;
    }
}
