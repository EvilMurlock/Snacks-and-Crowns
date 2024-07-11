using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP {
    public class EquipItemGoal : Goal
    {
        float defaultPriority = 4;
        public List<ItemTags> tags;
        EquipmentManager equipmentManager;
        protected virtual void Start()
        {
            equipmentManager = GetComponent<EquipmentManager>();
        }
        public void Initialize(List<ItemTags> tags)
        {
            this.tags = tags;
        }
        public override bool CompletedByState(WorldState state) //If more of desired item in chest then there is curently, then returns true
        {
            // used in the planing step
            return CloserToGoalCheck(state);
        }
        public bool CloserToGoalCheck(WorldState state)
        {

            return state.completedGoals.Contains(this);
            //return DistanceCalculator.CalculateDistance(state.myPosition, targetObject.transform.position) <= minDistance;
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
            //Debug.Log("Plan " + this.GetType().ToString() + " Completed");
            active = false;
        }
        public override float CalculatePriority()
        {
            if (GetComponent<EquipmentManager>().HasEquipedItem(tags))
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