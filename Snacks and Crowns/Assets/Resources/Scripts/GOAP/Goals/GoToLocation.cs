using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP {
    public class GoToLocation : Goal
    {
        public GameObject targetObject;
        float minDistance;

        float defaultPriority = 5;
        
        bool active = false;
        protected virtual void Start()
        {
        }
        public void Initialize(GameObject targetObject, float minDistance)
        {
            this.targetObject = targetObject;
            this.minDistance = minDistance;
        }
        public void SetDesiredTarget(GameObject newTarget)
        {
            targetObject = newTarget;
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
            return DistanceCalculator.CalculateDistance(transform.position, targetObject.transform.position) <= minDistance;
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

            return defaultPriority;
        }

        
        public override bool CanRun()
        {
            if (targetObject == null) return false;
            return true;
        }
    }
}