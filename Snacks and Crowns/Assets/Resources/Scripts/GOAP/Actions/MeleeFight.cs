using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    public class MeleeFight : Action
    {
        MeleeFightGoal meleeFightGoal;
        List<ItemTags> meleeItemTags = new List<ItemTags>() { ItemTags.meleeWeapon };
        float attackRange = 0.3f;
        Equipment meleeItem;
        public override void Awake()
        {
            speachBubbleType = SpeachBubbleTypes.Fight;
            base.Awake();
        }

        public override void Tick()
        {
            if (!meleeFightGoal.EnemyInRange(target))
                Deactivate();
            if (target == null) 
                Complete();
            else if (meleeFightGoal.IsCompleted())
                Complete();
            if(npcAi.reachedEndOfPath || DistanceCalculator.CalculateDistance(transform.position, target.transform.position) <= attackRange)
            {
                meleeItem.instance.GetComponent<Hand_Item_Controler>().Use();
            }
        }
        public override void Activate(ActionData arg)
        {
            target = meleeFightGoal.GetClosestEnemy();
            meleeItem = EquipItem(meleeItemTags);
            npcAi.ChangeTarget(target);
            base.Activate(arg);
        }
        public override void Deactivate()
        {
            running = false;
            UnequipItem(meleeItemTags);
            npcAi.ChangeTarget(null);
        }
        public override void Complete()
        {
            running = false;
            completed = true;
            UnequipItem(meleeItemTags);
        }
        public override bool IsAchievableGiven(WorldState worldState)//For the planner
        {
            meleeFightGoal = GetComponent<MeleeFightGoal>();
            if (meleeFightGoal == null) return false;
            return true;
        }

        public override List<Node> OnActionCompleteWorldStates(Node parentOriginal)//Tells the planer how the world state will change on completion
        {
            List<Node> possibleNodes = new List<Node>();
            Node parent = parentOriginal;
            if (!HasItem(parentOriginal.state, meleeItemTags))
            {
                Debug.Log("Getting item");
                parent = GetRequiredItemWithTags(parentOriginal, meleeItemTags);
                if (parent == null)
                    return possibleNodes; // we cant fight, we dont have a weapon
            }
            else Debug.Log("Already have item");
            WorldState possibleWorldState = new WorldState(parent.state);
            possibleWorldState.CopyCompletedGoals();
            possibleWorldState.completedGoals.Add(meleeFightGoal);


            possibleNodes.Add(new Node(parent, 1 + parent.cost, possibleWorldState, this, null));
            return possibleNodes;
        }
    }
}
