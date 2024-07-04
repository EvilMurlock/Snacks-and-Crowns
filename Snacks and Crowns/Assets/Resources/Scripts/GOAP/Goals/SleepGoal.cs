using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP {
    public class SleepGoal : Goal
    {
        public GameObject bed;

        float sleepDuration = 30;
        float sleepStartTime = 0;
        float defaultPriority = 1;
        protected virtual void Start()
        {
        }
        public void Initialize(GameObject bed)
        {
            this.bed = bed;
        }
        public void SetDesiredTarget(GameObject bed)
        {
            this.bed = bed;
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
            if (sleepDuration + sleepStartTime < Time.timeSinceLevelLoad)
                return true;
            return false;
        }
        public override void Activate()
        {
            sleepStartTime = Time.timeSinceLevelLoad;
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

            return defaultPriority;
        }

        
        public override bool CanRun()
        {
            if (bed == null) return false;
            return base.CanRun();
        }
    }
}