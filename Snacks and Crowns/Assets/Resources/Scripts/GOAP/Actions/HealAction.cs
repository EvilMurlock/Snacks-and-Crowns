using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GOAP
{
    /// <summary>
    /// Heals self
    /// </summary>
    public class HealAction : NPCAction
    {

        HealGoal healGoal;
        List<ItemTags> healItemTags = new List<ItemTags>() { ItemTags.healing };
        public override void Awake()
        {
            speechBubbleType = SpeechBubbleTypes.Heal;
            base.Awake();
        }

        public override void Tick()
        {
            if (UseItem(healItemTags))
                Complete();
            else
                Deactivate();
        }
        public override void Activate(ActionData arg)
        {
            base.Activate(arg);
        }
        public override void Deactivate()
        {
            running = false;
        }
        public override void Complete()
        {
            running = false;
            completed = true;
        }
        public override bool IsAchievableGiven(WorldState worldState)//For the planner
        {
            healGoal = GetComponent<HealGoal>();
            if (healGoal == null) return false;
            return true;
        }

        public override List<Node> OnActionCompleteWorldStates(Node parentOriginal)//Tells the planer how the world state will change on completion
        {
            List<Node> possibleNodes = new List<Node>();
            Node parent = parentOriginal;
            if (!HasItem(parentOriginal.state, healItemTags))
            {
                //Debug.Log("Getting item");
                parent = GetRequiredItemWithTags(parentOriginal, healItemTags);
                if (parent == null)
                    return possibleNodes;
            }
            //else Debug.Log("Already have item");
            WorldState possibleWorldState = new WorldState(parent.state);
            possibleWorldState.CopyCompletedGoals();
            possibleWorldState.completedGoals.Add(healGoal);


            possibleNodes.Add(new Node(parent, 1 + parent.cost, possibleWorldState, this, null));
            return possibleNodes;
        }
    }
}