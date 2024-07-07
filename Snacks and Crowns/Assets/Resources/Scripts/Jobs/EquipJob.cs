using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;
public class EquipJob : Job
{
    [SerializeField]
    List<ItemTags> tags = new List<ItemTags>();
    public override void SetGoalsOfAnNPC(GameObject npc)
    {
        EquipItemGoal equipItemGoal = npc.AddComponent<EquipItemGoal>();
        equipItemGoal.Initialize(tags);
        base.SetGoalsOfAnNPC(npc);
    }
}
