using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    /// <summary>
    /// Travels to an Object
    /// </summary>
    public class GoToObject : NPCAction
    {
        GoToLocation goToLocation;
        public override void Awake()
        {
            speechBubbleType = SpeechBubbleTypes.Walk;
            base.Awake();
        }
        public override void Tick()
        {
            FightGoal fightGoal = GetComponent<FightGoal>();
            if (target == null) Deactivate();
            else if (fightGoal != null)
            {
                if (fightGoal.GetClosestEnemy() != null)
                    Deactivate();
            }
            else if (goToLocation.IsCompleted())
            {
                Complete();
            }
        }
        public override void Activate(ActionData arg)
        {
            target = goToLocation.targetObject;

            npcAi.ChangeTarget(target);
            base.Activate(arg);
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
            if (goToLocation.targetObject == null) return false;
            return true;
        }

        public override List<Node> OnActionCompleteWorldStates(Node parent)//Tells the planer how the world state will change on completion
        {
            List<Node> possibleNodes = new List<Node>();

            WorldState possibleWorldState = new WorldState(parent.state);
            possibleWorldState.CopyCompletedGoals();
            possibleWorldState.completedGoals.Add(goToLocation);


            possibleNodes.Add(new Node(parent, 1 + parent.cost + GetDistanceBetween(this.transform.position, goToLocation.targetObject.transform.position ), possibleWorldState, this, null));
            return possibleNodes;
        }
    }
}