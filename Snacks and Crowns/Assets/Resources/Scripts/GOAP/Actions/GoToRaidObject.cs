using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    /// <summary>
    /// Chooses a random enemy and travels towards them
    /// </summary>
    public class GoToRaidObject : NPCAction
    {
        LaunchRaid launchRaid;
        float lessThanFightGoalAggroRange = 7;
        public override void Awake()
        {
            speechBubbleType = SpeechBubbleTypes.Charge;
            base.Awake();
        }
        public override void Tick()
        {
            if (target == null || DistanceCalculator.CalculateDistance( target.transform.position, gameObject.transform.position ) < lessThanFightGoalAggroRange) 
                Deactivate();
            else if (launchRaid.IsCompleted())
            {
                Complete();
            }
        }
        public override void Activate(ActionData arg)
        {
            target = launchRaid.GetRandomEnemy();
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
            
            launchRaid = GetComponent<LaunchRaid>();
            if (launchRaid == null) return false;
            return true;
        }

        public override List<Node> OnActionCompleteWorldStates(Node parent)//Tells the planer how the world state will change on completion
        {
            List<Node> possibleNodes = new List<Node>();

            WorldState possibleWorldState = new WorldState(parent.state);
            possibleWorldState.CopyCompletedGoals();
            possibleWorldState.completedGoals.Add(launchRaid);

            possibleNodes.Add(new Node(parent, 1 + parent.cost, possibleWorldState, this, null));
            return possibleNodes;
        }
    }
}