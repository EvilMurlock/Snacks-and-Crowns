using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    /// <summary>
    /// Gets a ranged weapon and fights with it
    /// </summary>
    public class RangedFight : NPCAction
    {
        RangedFightGoal rangedFightGoal;
        List<ItemTags> rangedItemTags = new List<ItemTags>() { ItemTags.rangedWeapon };
        float attackRange = 6f;
        Equipment rangedItem;
        Movement movement;
        public override void Awake()
        {
            speechBubbleType = SpeechBubbleTypes.Fight;
            base.Awake();
        }
        public override void Start()
        {
            movement = GetComponent<Movement>();
            base.Start();
        }

        public override void Tick()
        {
            if (!rangedFightGoal.EnemyInRange(target))
                Deactivate();
            else if (target == null) 
                Complete();
            else if (rangedFightGoal.IsCompleted())
                Complete();
            else if(DistanceCalculator.CalculateDistance(transform.position, target.transform.position) <= attackRange)
            {
                movement.RotateTowards(target.transform.position - transform.position);
                rangedItem.GetInstance(gameObject).GetComponent<HandItemControler>().Use();
                npcAi.ChangeTarget(null);
            }
            else
            {
                npcAi.ChangeTarget(target);
            }
        }
        public override void Activate(ActionData arg)
        {
            target = rangedFightGoal.GetClosestEnemy();
            
            if (GetComponent<EquipmentManager>().HasEquippedItem(rangedItemTags))
            {
                rangedItem = GetEquippedItem(rangedItemTags);
            }
            else
            {
                rangedItem = EquipItem(rangedItemTags);
            }
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
            if (!HasItem(parentOriginal.state, rangedItemTags) && !GetComponent<EquipmentManager>().HasEquippedItem(rangedItemTags))
            {
                parent = GetRequiredItemWithTags(parentOriginal, rangedItemTags);
                if (parent == null)
                    return possibleNodes; // we cant fight, we don't have a weapon
            }
            WorldState possibleWorldState = new WorldState(parent.state);
            possibleWorldState.CopyCompletedGoals();
            possibleWorldState.completedGoals.Add(rangedFightGoal);


            possibleNodes.Add(new Node(parent, 1 + parent.cost, possibleWorldState, this, null));
            return possibleNodes;
        }
    }
}
