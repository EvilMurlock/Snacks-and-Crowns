using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    /// <summary>
    /// sleeps in a bed
    /// </summary>
    public class Sleep : NPCAction
    {
        SleepGoal sleepGoal;
        bool sleeping;
        public override void Awake()
        {
            speechBubbleType = SpeechBubbleTypes.Sleep;
            base.Awake();
        }
        public override void Start()
        {
            base.Start();
        }
        public override void Tick()
        {
            
            if (target == null) Deactivate();
            else if (sleeping)
            {
                if (sleepGoal.IsCompleted())
                    Complete();
            }
            else if (npcAi.reachedEndOfPath)
            {
                Bed bed = target.GetComponent<Bed>();
                if (bed.IsFree())
                {
                    bed.Interact(gameObject);
                    sleeping = true;
                }
            }
        }
        public override void Activate(ActionData arg)
        {
            target = sleepGoal.bed;
            sleeping = false;

            npcAi.ChangeTarget(target);
            base.Activate(arg);
        }
        public override void Deactivate()
        {
            if(sleeping)
                target.GetComponent<Bed>().UnInteract(gameObject);
            running = false;
            npcAi.ChangeTarget(null);
        }
        public override void Complete()
        {
            if (sleeping)
                target.GetComponent<Bed>().UnInteract(gameObject);
            running = false;
            completed = true;
        }
        public override bool IsAchievableGiven(WorldState worldState)//For the planner
        {
            sleepGoal = GetComponent<SleepGoal>();
            if (sleepGoal == null) return false;

            return true;
        }

        public override List<Node> OnActionCompleteWorldStates(Node parent)//Tells the planer how the world state will change on completion
        {
            List<Node> possibleNodes = new List<Node>();

            WorldState possibleWorldState = new WorldState(parent.state);
            possibleWorldState.CopyCompletedGoals();
            possibleWorldState.completedGoals.Add(sleepGoal);


            possibleNodes.Add(new Node(parent, 1 + parent.cost + GetDistanceBetween(this.transform.position, sleepGoal.transform.position ), possibleWorldState, this, null));
            return possibleNodes;
        }
    }
}