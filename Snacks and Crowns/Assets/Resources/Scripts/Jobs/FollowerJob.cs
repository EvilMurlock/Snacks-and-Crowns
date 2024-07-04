using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;

[CreateAssetMenu(fileName = "New_GatherJob", menuName = "Jobs/GatherJob")]
[System.Serializable]
public class FollowerJob : Job
{
    
    GameObject whatToFollow;
    float minDistance;
    public override void SetGoalsOfAnNPC(GameObject npc)
    {
        GoToLocation goToLocation = npc.AddComponent<GOAP.GoToLocation>();
        goToLocation.Initialize(whatToFollow, minDistance);
        base.SetGoalsOfAnNPC(npc);
    }
}
