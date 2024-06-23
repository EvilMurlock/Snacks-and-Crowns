using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;


[System.Serializable]

public class Job
{
    [SerializeField]
    Bed bed;
    // job loads a set of goals to a newly spawned NPC

    public virtual void SetGoalsOfAnNPC(GameObject npc)
    {
        //npc.AddComponent<>
    }
}
