using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP {
    public class IdleAroundAPoint : Goal
    {
        public Vector3 basePoint;
        [SerializeField]
        float wanderDistance = 3f;
        [SerializeField]
        float priorityIncreasePerSecond = 0.1f;
        [SerializeField]
        float maxPriority = 3;
        [SerializeField]
        float minPriority = 0.95f;
        [SerializeField]
        float wanderPriority = 0.95f;
        [SerializeField]
        float distanceFromPoint = 0.002f;
        Vector3 randomIdlePoint = Vector3.zero;
        public Vector3 RandomIdlePoint => randomIdlePoint;
        float startTime = 0;
        float resetTime = 10;
        
        protected virtual void Start()
        {
            basePoint = this.transform.position;
            ChooseRandomPoint();
        }
        public void Initialize(Vector3 basePoint, float wanderDistance, float priority)
        {
            this.basePoint = basePoint;
            this.wanderDistance = wanderDistance;
            this.maxPriority = priority;
        }
        public void SetDesiredTarget(Vector3 basePoint)
        {
            this.basePoint = basePoint;
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
            wanderPriority += priorityIncreasePerSecond * Time.deltaTime;
            if (active)
            {
                //Debug.Log("Time passed: " + (Time.realtimeSinceStartup - startTime));
                if (Time.realtimeSinceStartup - startTime > resetTime)
                {
                    ChooseRandomPoint();
                    Deactivate();
                }
            }
        }

        public bool IsCompleted()
        {
            return DistanceCalculator.CalculateDistance(transform.position, randomIdlePoint) <= distanceFromPoint;
        }
        public override void Activate()
        {
            startTime = Time.realtimeSinceStartup;
            active = true;
        }
        public override void Deactivate()
        {
            active = false;
        }

        public override void Complete()
        {
            //Debug.Log("Plan " + this.GetType().ToString() + " Completed");
            ChooseRandomPoint();
            wanderPriority = minPriority;
            active = false;
        }
        void ChooseRandomPoint()
        {
            Vector3 randomPoint = Random.insideUnitCircle * wanderDistance;
            randomIdlePoint = randomPoint + basePoint;
            //Debug.Log("New random point chosen: " + "("+randomIdlePoint.x + ", "+randomIdlePoint.y+")");
        }
        public override float CalculatePriority()
        {

            return Mathf.Min(wanderPriority, maxPriority);
        }

        
        public override bool CanRun()
        {
            return true;
        }

    }
}