using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GOAP
{
    /// <summary>
    /// Parent class for all SubActions, those actions are not being chosen during planning by the planner, but can be inserted into the plan by the actions themselves, for example to get certain required items
    /// </summary>
    public abstract class SubAction : NPCAction
    {
        public override bool IsUsableBy(GameObject g)
        {
            return true;
        }
        public override bool IsAchievableGiven(WorldState worldState)//For the planner
        {
            return false;
        }
        public override List<Node> OnActionCompleteWorldStates(Node parent)
        {
            //This code will never fire
            List<Node> possibleNodes = new List<Node>();
            return possibleNodes;
        }
    }
}
