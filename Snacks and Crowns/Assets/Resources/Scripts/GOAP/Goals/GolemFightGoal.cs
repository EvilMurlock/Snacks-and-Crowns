using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GOAP
{
    public class GolemFightGoal : FightGoal
    {
        public override bool CompletedByState(WorldState state)
        {
            return state.completedGoals.Contains(this);
        }
    }
}