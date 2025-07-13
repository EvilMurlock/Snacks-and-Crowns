using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP {
    public class EquipItemGoal : Goal
    {
        float defaultPriority = 4;
        public List<ItemTags> tags;
        EquipmentManager equipmentManager;
        public override void Start()
        {
            equipmentManager = GetComponent<EquipmentManager>();
            MaxPlanDepth = 1;
        }
        public void Initialize(List<ItemTags> tags)
        {
            this.tags = tags;
        }
        public override bool CompletedByState(WorldState state) //If more of desired item in chest then there is currently, then returns true
        {
            // used in the planning step
            return CloserToGoalCheck(state);
        }
        public bool CloserToGoalCheck(WorldState state)
        {

            return state.completedGoals.Contains(this);
        }
        public override void Tick()
        {

        }

        public bool IsCompleted()
        {
            return HasEquipedItem();
        }
        bool HasEquipedItem()
        {
            foreach(Equipment equipment in GetComponent<EquipmentManager>().Equipments)
            {
                if (equipment == null)
                    continue;
                if (equipment.HasTags(tags))
                    return true;
            }
            return false;
        }
        public override void Activate()
        {
            active = true;
        }
        public override void Deactivate()
        {
            active = false;
        }

        public override void Complete()
        {
            active = false;
        }
        public override float CalculatePriority()
        {
            if (GetComponent<EquipmentManager>().HasEquippedItem(tags))
                return 0;
            else
                return defaultPriority;
        }

        public override bool CanRun()
        {
            return base.CanRun();
        }
    }
}