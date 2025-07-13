using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    /// <summary>
    /// Travels to a location
    /// </summary>
    public class GoToPoint : NPCAction
    {
        IdleAroundAPoint idleAroundAPoint;
        public override void Start()
        {
            base.Start();
        }
        public override void Tick()
        {
            if (target == null) Deactivate();
            else if (npcAi.reachedEndOfPath)
            {
                Complete();
            }
        }
        public override void Activate(ActionData arg)
        {
            target = new GameObject("WanderPoint");
            target.transform.position = idleAroundAPoint.RandomIdlePoint;
            npcAi = GetComponent<NpcAi>();
            npcAi.ChangeTarget(target);
            base.Activate(arg);
        }
        public override void Deactivate()
        {
            running = false;
            npcAi.ChangeTarget(null);
            Destroy(target);
        }
        public override void Complete()
        {
            running = false;
            completed = true;
            Destroy(target);
        }
        public override bool IsAchievableGiven(WorldState worldState)//For the planner
        {
            idleAroundAPoint = GetComponent<IdleAroundAPoint>();
            if (idleAroundAPoint == null) return false;

            return true;
        }

        public override List<Node> OnActionCompleteWorldStates(Node parent)//Tells the planer how the world state will change on completion
        {
            List<Node> possibleNodes = new List<Node>();

            WorldState possibleWorldState = new WorldState(parent.state);
            possibleWorldState.CopyCompletedGoals();
            possibleWorldState.completedGoals.Add(idleAroundAPoint);


            possibleNodes.Add(new Node(parent, 1 + parent.cost + GetDistanceBetween(this.transform.position, idleAroundAPoint.RandomIdlePoint ), possibleWorldState, this, null));
            return possibleNodes;
        }
    }
}