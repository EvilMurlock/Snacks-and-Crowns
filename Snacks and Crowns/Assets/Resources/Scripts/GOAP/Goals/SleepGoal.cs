using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP {
    public class SleepGoal : Goal
    {
        public GameObject bed;

        float sleepDuration = 10;
        float sleepStartTime = 0;
        float defaultPriority = 0;
        float currentPriority;
        float sleepPriorityGainPerSecond = 0.001f;
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
        public override bool CompletedByState(WorldState state) //If more of desired item in chest then there is currently, then returns true
        {
            // used in the planing step
            return CloserToGoalCheck(state);
        }
        public bool CloserToGoalCheck(WorldState state)
        {

            return state.completedGoals.Contains(this);
        }
        public override void Tick()
        {
            currentPriority += sleepPriorityGainPerSecond * Time.deltaTime;
            if(currentPriority > 4)
                currentPriority = 4;
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
            currentPriority = defaultPriority;
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
            currentPriority = defaultPriority;
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