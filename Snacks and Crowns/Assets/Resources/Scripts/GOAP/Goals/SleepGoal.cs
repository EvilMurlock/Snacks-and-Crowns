using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP {
    public class SleepGoal : Goal
    {
        public GameObject bed;

        float sleepDuration = 10;
        float sleepStartTime = 0;
        float defaultPriority = 1;
        float currentPriority;
        float sleepPriorityGainPerSecond = 0.002f;
        float randomDurationSpread = 0.3f;
        float durationModifier = 1;
        public override void Start()
        {
            currentPriority = defaultPriority;
            MaxPlanDepth = 1;
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
            currentPriority += sleepPriorityGainPerSecond * Time.deltaTime;
        }

        public bool IsCompleted()
        {
            currentPriority = defaultPriority;
            if (sleepDuration * durationModifier + sleepStartTime < Time.timeSinceLevelLoad)
                return true;
            return false;
        }
        public override void Activate()
        {
            durationModifier = Random.Range(1 - randomDurationSpread, 1 + randomDurationSpread);
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

            return currentPriority;
        }

        
        public override bool CanRun()
        {
            if (bed == null) return false;
            return base.CanRun();
        }
    }
}