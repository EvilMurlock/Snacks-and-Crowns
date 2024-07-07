using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    public class RangedFight : Action
    {
        RangedFightGoal rangedFightGoal;
        List<ItemTags> rangedItemTags = new List<ItemTags>() { ItemTags.rangedWeapon };
        float attackRange = 7f;
        Equipment rangedItem;
        public override void Awake()
        {
            speachBubbleType = SpeachBubbleTypes.Fight;
            base.Awake();
        }

        public override void Tick()
        {
            if (!rangedFightGoal.EnemyInRange(target))
                Deactivate();
            else if (target == null) 
                Complete();
            else if (rangedFightGoal.IsCompleted())
                Complete();
            else if(npcAi.reachedEndOfPath || DistanceCalculator.CalculateDistance(transform.position, target.transform.position) <= attackRange)
            {
                rangedItem.instance.GetComponent<Hand_Item_Controler>().Use();
            }
        }
        public override void Activate(ActionData arg)
        {
            target = rangedFightGoal.GetClosestEnemy();
            if (HasEquipedItem(rangedItemTags))
                rangedItem = GetEquipedItem(rangedItemTags);
            else
                rangedItem = EquipItem(rangedItemTags);
            npcAi.ChangeTarget(target);
            base.Activate(arg);
        }
        public override void Deactivate()
        {
            running = false;
            UnequipItem(rangedItemTags);
            npcAi.ChangeTarget(null);
        }
        public override void Complete()
        {
            running = false;
            completed = true;
            npcAi.ChangeTarget(null);
            UnequipItem(rangedItemTags);
        }
        public override bool IsAchievableGiven(WorldState worldState)//For the planner
        {
            rangedFightGoal = GetComponent<RangedFightGoal>();
            if (rangedFightGoal == null) return false;
            return true;
        }

        public override List<Node> OnActionCompleteWorldStates(Node parentOriginal)//Tells the planer how the world state will change on completion
        {
            List<Node> possibleNodes = new List<Node>();
            Node parent = parentOriginal;
            if (!HasItem(parentOriginal.state, rangedItemTags) && !HasEquipedItem(rangedItemTags))
            {
                //Debug.Log("Getting item");
                parent = GetRequiredItemWithTags(parentOriginal, rangedItemTags);
                if (parent == null)
                    return possibleNodes; // we cant fight, we dont have a weapon
            }
            //else Debug.Log("Already have item");
            WorldState possibleWorldState = new WorldState(parent.state);
            possibleWorldState.CopyCompletedGoals();
            possibleWorldState.completedGoals.Add(rangedFightGoal);


            possibleNodes.Add(new Node(parent, 1 + parent.cost, possibleWorldState, this, null));
            return possibleNodes;
        }
    }
}
