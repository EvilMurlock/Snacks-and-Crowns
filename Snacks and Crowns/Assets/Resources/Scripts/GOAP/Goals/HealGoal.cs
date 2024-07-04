using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP {
    public class HealGoal : Goal
    {
        float currentPriority = 0;
        protected virtual void Start()
        {
            GetComponent<Damagable>().healthChange.AddListener(UpdatePriority);
        }
        void UpdatePriority(float hp, float maxHp)
        {
            currentPriority = 10 * (1 - hp / maxHp);
        }
        public void Initialize()
        {
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
            return currentPriority == 0;
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
            return currentPriority;
        }

        public override bool CanRun()
        {
            return base.CanRun();
        }
    }
}