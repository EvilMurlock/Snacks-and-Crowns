using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GOAP
{
    public class EquipItemAction : Action
    {

        EquipItemGoal equipItemGoal;
        public override void Awake()
        {
            speachBubbleType = SpeachBubbleTypes.Heal;
            base.Awake();
        }

        public override void Tick()
        {
            if(EquipItem(equipItemGoal.tags) == null){
                Deactivate();
            }
            else
            {
                Complete();
            }
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
            equipItemGoal = GetComponent<EquipItemGoal>();
            if (equipItemGoal == null) return false;
            return true;
        }

        public override List<Node> OnActionCompleteWorldStates(Node parentOriginal)//Tells the planer how the world state will change on completion
        {
            List<Node> possibleNodes = new List<Node>();
            Node parent = parentOriginal;
            if (!HasItem(parentOriginal.state, equipItemGoal.tags))
            {
                //Debug.Log("Getting item");
                parent = GetRequiredItemWithTags(parentOriginal, equipItemGoal.tags);
                if (parent == null)
                    return possibleNodes; // we cant fight, we dont have a weapon
            }
            //else Debug.Log("Already have item");
            WorldState possibleWorldState = new WorldState(parent.state);
            possibleWorldState.CopyCompletedGoals();
            possibleWorldState.completedGoals.Add(equipItemGoal);


            possibleNodes.Add(new Node(parent, 1 + parent.cost, possibleWorldState, this, null));
            return possibleNodes;
        }
    }
}