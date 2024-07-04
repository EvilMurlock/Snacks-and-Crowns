using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;
public class EquipArmorJob : Job
{
    List<ItemTags> tags = new List<ItemTags>() { ItemTags.armor };
    public override void SetGoalsOfAnNPC(GameObject npc)
    {
        EquipItemGoal equipItemGoal = npc.AddComponent<EquipItemGoal>();
        equipItemGoal.Initialize(tags);
        base.SetGoalsOfAnNPC(npc);
    }
}
