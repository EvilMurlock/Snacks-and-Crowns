using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GOAP
{
    public abstract class SubAction : Action
    {
        public override bool IsUsableBy(GameObject g)
        {
            return true;
        }
        public override bool IsAchievableGiven(WorldState worldState)//For the planner
        {
            return false;
            //Its a subaction
        }
        public override List<Node> OnActionCompleteWorldStates(Node parent)
        {
            //This code will never fire
            List<Node> possibleNodes = new List<Node>();
            return possibleNodes;
        }
    }
}
