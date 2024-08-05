using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    public class RangedFight : Action
    {
        RangedFightGoal rangedFightGoal;
        List<ItemTags> rangedItemTags = new List<ItemTags>() { ItemTags.rangedWeapon };
        float attackRange = 6f;
        Equipment rangedItem;
        Movement movement;
        public override void Awake()
        {
            speachBubbleType = SpeachBubbleTypes.Fight;
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
                movement.RotateTowars(target.transform.position - transform.position);
                rangedItem.GetInstance(gameObject).GetComponent<Hand_Item_Controler>().Use();
                npcAi.ChangeTarget(null);
            }
            else
            {
                Debug.Log("Going after target! : " + target.name);
                npcAi.ChangeTarget(target);
            }
        }
        public override void Activate(ActionData arg)
        {
            target = rangedFightGoal.GetClosestEnemy();
            
            if (GetComponent<EquipmentManager>().HasEquipedItem(rangedItemTags))
            {
                //Debug.Log("Reading ranged weapon");
                rangedItem = GetEquipedItem(rangedItemTags);
            }
            else
            {
                //Debug.Log("Equiped ranged weapon");
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
            if (!HasItem(parentOriginal.state, rangedItemTags) && !GetComponent<EquipmentManager>().HasEquipedItem(rangedItemTags))
            {
                Debug.Log("Getting item");
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
