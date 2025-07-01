using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP {
    public class HealGoal : Goal
    {
        float currentPriority = 0;
        public override void Start()
        {
            MaxPlanDepth = 1;
            GetComponent<Damagable>().healthChange.AddListener(UpdatePriority);
        }
        void UpdatePriority(float hp, float maxHp)
        {
            currentPriority = 10 * (1 - hp / maxHp);
        }
        public override bool CompletedByState(WorldState state)
        {
            return CloserToGoalCheck(state);
        }
        public bool CloserToGoalCheck(WorldState state)
        {
            return state.completedGoals.Contains(this);
        }
        public override void Complete()
        {
            active = false;
        }
        public override float CalculatePriority()
        {
            return currentPriority;
        }
    }
}