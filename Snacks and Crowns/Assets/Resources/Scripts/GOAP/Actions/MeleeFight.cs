using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    /// <summary>
    /// Gets a melee weapon and fights enemies in its agro range with it
    /// </summary>
    public class MeleeFight : NPCAction
    {
        MeleeFightGoal meleeFightGoal;
        List<ItemTags> meleeItemTags = new List<ItemTags>() { ItemTags.meleeWeapon };
        float attackRange = 0.3f;
        Equipment meleeItem;

        EquipmentManager equipmentManager;
        public override void Awake()
        {
            speechBubbleType = SpeechBubbleTypes.Fight;
            base.Awake();
        }
        public override void Start()
        {
            equipmentManager = GetComponent<EquipmentManager>();
            base.Start();
        }
        public override void Tick()
        {
            if (!meleeFightGoal.EnemyInRange(target))
                Deactivate();
            else if (target == null) 
                Complete();
            else if (meleeFightGoal.IsCompleted())
                Complete();
            else if(npcAi.reachedEndOfPath || DistanceCalculator.CalculateDistance(transform.position, target.transform.position) <= attackRange)
            {
                meleeItem.GetInstance(gameObject).GetComponent<HandItemControler>().Use();
            }
        }
        public override void Activate(ActionData arg)
        {
            target = meleeFightGoal.GetClosestEnemy();
            if (equipmentManager.HasEquippedItem(meleeItemTags))
                meleeItem = GetEquippedItem(meleeItemTags);
            else
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
            npcAi.ChangeTarget(null);
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
            if (!HasItem(parentOriginal.state, meleeItemTags) && !equipmentManager.HasEquippedItem(meleeItemTags))
            {
                parent = GetRequiredItemWithTags(parentOriginal, meleeItemTags);
                if (parent == null)
                    return possibleNodes; // we cant fight, we don't have a weapon
            }
            WorldState possibleWorldState = new WorldState(parent.state);
            possibleWorldState.CopyCompletedGoals();
            possibleWorldState.completedGoals.Add(meleeFightGoal);


            possibleNodes.Add(new Node(parent, 1 + parent.cost, possibleWorldState, this, null));
            return possibleNodes;
        }
    }
}
