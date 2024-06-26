using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    class ActionDataGoToObject : ActionData
    {
        public ActionDataGoToObject()
        {
        }
    }
    public class GoToObject : Action
    {
        GoToLocation goToLocation;
        public override void Start()
        {
            base.Start();
        }
        public override void Tick()
        {
            if (target == null) Deactivate();
            else if (goToLocation.IsCompleted())
            {
                Complete();
            }
        }
        public override void Activate(ActionData arg)
        {
            target = goToLocation.targetObject;

            running = true;
            completed = false;
            npcAi.ChangeTarget(target);
        }
        public override void Deactivate()
        {
            running = false;
            npcAi.ChangeTarget(null);
        }
        public override void Complete()
        {
            running = false;
            completed = true;
        }
        public override bool IsAchievableGiven(WorldState worldState)//For the planner
        {
            goToLocation = GetComponent<GoToLocation>();
            if (goToLocation == null) return false;

            return true;
        }

        public override List<Node> OnActionCompleteWorldStates(Node parent)//Tells the planer how the world state will change on completion
        {
            List<Node> possibleNodes = new List<Node>();

            WorldState possibleWorldState = new WorldState(parent.state);
            possibleWorldState.CopyCompletedGoals();
            possibleWorldState.completedGoals.Add(goToLocation);


            ActionDataGoToObject actionData = new ActionDataGoToObject();
            possibleNodes.Add(new Node(parent, 1 + parent.cost + GetDistanceBetween(this.transform.position, goToLocation.targetObject.transform.position ), possibleWorldState, this, actionData));
            return possibleNodes;
        }
    }
}