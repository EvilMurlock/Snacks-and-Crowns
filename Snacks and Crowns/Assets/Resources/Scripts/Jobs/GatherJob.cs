using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;

[CreateAssetMenu(fileName = "New_GatherJob", menuName = "Jobs/GatherJob")]
[System.Serializable]
public class GatherJob : Job
{
    [SerializeField]
    GameObject GatheringPlace;
    [SerializeField]
    List<Item> itemsToGather;
    public override void SetGoalsOfAnNPC(GameObject npc)
    {
        FillAnInventory fillAnInventory = npc.AddComponent<GOAP.FillAnInventory>();
        fillAnInventory.Initialize(GatheringPlace, itemsToGather);
        base.SetGoalsOfAnNPC(npc);
    }
}