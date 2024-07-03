using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{


    public class GolemFight : Action
    {
        GolemFightGoal golemFightGoal;
        float attackRange = 0.3f;
        Ability golemFire;
        float timeOfLastFireUse;
        public override void Awake()
        {
            speachBubbleType = SpeachBubbleTypes.Fight;
            foreach(Ability ability in GetComponent<Abilities>().abilities)
            {
                if (ability.name == "GolemFireBreath")
                {
                    golemFire = ability;
                    break;
                }
            }
            if (golemFire == null)
                Destroy(this);
            base.Awake();
            
        }

        public override void Tick()
        {
            if (!golemFightGoal.EnemyInRange(target))
                Deactivate();
            if (target == null) 
                Complete();
            else if (golemFightGoal.IsCompleted())
                Complete();
            if(npcAi.reachedEndOfPath || DistanceCalculator.CalculateDistance(transform.position, target.transform.position) <= attackRange)
            {
                if (timeOfLastFireUse + golemFire.cooldown < Time.timeSinceLevelLoad)
                {
                    golemFire.Use(this.gameObject);
                    timeOfLastFireUse = Time.timeSinceLevelLoad;
                }
            }
        }
        public override void Activate(ActionData arg)
        {
            target = golemFightGoal.GetClosestEnemy();
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
            golemFightGoal = GetComponent<GolemFightGoal>();
            if (golemFightGoal == null) return false;
            return true;
        }

        public override List<Node> OnActionCompleteWorldStates(Node parentOriginal)//Tells the planer how the world state will change on completion
        {
            List<Node> possibleNodes = new List<Node>();
            Node parent = parentOriginal;

            WorldState possibleWorldState = new WorldState(parent.state);
            possibleWorldState.CopyCompletedGoals();
            possibleWorldState.completedGoals.Add(golemFightGoal);


            possibleNodes.Add(new Node(parent, 1 + parent.cost, possibleWorldState, this, null));
            return possibleNodes;
        }
    }
}
