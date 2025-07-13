using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System;

namespace GOAP
{
    /// <summary>
    /// Agent class, keeps track of action and goals and the current plan
    /// 
    /// </summary>
    public class Agent : MonoBehaviour
    {
        public List<NPCAction> actions = new List<NPCAction>();
        public List<Goal> goals = new List<Goal>();

        Planner planner;
        Queue<Node> nodeQueue;
        Node currentNode;
        public NPCAction currentAction;
        Goal currentGoal;
        WorldState worldState;

        NpcAi npcAi;

        float planningDelayAfterFail = 1;
        float planningDelayNow = 1;
        float rePlanTimer = 0;
        float rePlanCooldown = 3;
        float rePlanCooldownRandomSpread = 1;
        // Start is called before the first frame update
        protected virtual void Start()
        {

            planner = new Planner();
            worldState = new WorldState(this.gameObject);
            npcAi = GetComponent<NpcAi>();
            AddAllActions();
            LoadActionsAndGoals();
        }
        void ReplanCheck()
        {
            rePlanTimer -= Time.deltaTime;
            if (rePlanTimer <= 0)
            {
                nodeQueue = null;
                float rVal = UnityEngine.Random.Range(-rePlanCooldownRandomSpread, rePlanCooldownRandomSpread);
                rePlanTimer = rePlanCooldown + rVal;
            }

        }

        private void AddAllActions()
        {
            // uses reflection to add all action monobehaviours to our gameObject
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var types = assemblies.SelectMany(assembly => assembly.GetTypes());
            var filteredTypes = types.Where(
            type => IsSubclassOfRawGeneric(typeof(GOAP.NPCAction), type)
            && !type.ContainsGenericParameters
            && type.IsClass);
            List<Type> actionTypes = filteredTypes.ToList();
            foreach (Type actionType in actionTypes)
            {
                if (actionType.IsAbstract)
                    continue;
                gameObject.AddComponent(actionType);
            }
        }
        static bool IsSubclassOfRawGeneric(Type generic, Type toCheck)
        {
            while (toCheck != null && toCheck != typeof(object))
            {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (generic == cur)
                {
                    return true;
                }
                toCheck = toCheck.BaseType;
            }
            return false;
        }
        void LoadActionsAndGoals()
        {
            LoadActions();
            LoadGoals();
        }
        public void RefreshActions()
        {
            actions.Clear();
            LoadActions();
        }
        void LoadActions()
        {
            NPCAction[] acts = this.GetComponents<NPCAction>();
            foreach (NPCAction a in acts)
                if (a.IsUsableBy(gameObject))
                    actions.Add(a);
        }
        void LoadGoals()
        {
            Goal[] gs = this.GetComponents<Goal>();
            foreach (Goal g in gs)
            {
                //Debug.Log("Adding goal: " + g.GetType().ToString());
                goals.Add(g);
            }
        }
        void PrintWorldGoals()
        {
            Debug.Log("GOALS:\n" + goals.ToString());
        }
        /// <summary>
        /// We try to find a new plan, we search for a plan that completes a goal in order of goal priority, we choose the first plan we find
        /// </summary>
        void GetNewPlan()
        {
            npcAi.ChangeTarget(null); // we have no active action, so we aren't moving anywhere
                                      // we try to find a plan that fulfils one of our goals, in order of priority

            var sortedGoals = from goal in goals orderby goal.CalculatePriority() descending select goal;
            Goal newGoal = null;
            Queue<Node> newNodeQueue = null;
            if (planningDelayNow < planningDelayAfterFail)
                planningDelayNow += Time.deltaTime;
            else
            {
                worldState.UpdateBelieves();

                foreach (Goal g in sortedGoals)
                {
                    if (g.CalculatePriority() <= 0) continue;
                    Queue<Node> queue = null;

                    if (g.CanRun())
                        queue = planner.CreatePlan(actions, g, worldState);
                    if (queue == null)
                    {
                        //planner.DebugPrintPlan(queue);
                    }
                    if (queue != null)
                    {
                        newGoal = g;
                        newNodeQueue = queue;
                        break;
                    }
                }
                planningDelayNow = 0;
            }


            // we swap into the new plan if we found a plan and our old goal has a lesser priority
            if (newGoal != null && newGoal.CalculatePriority() > 0
                && (currentGoal == null || newGoal.CalculatePriority() > currentGoal.CalculatePriority()))//Switch plans when plan with a higher priority goal is found
            {
                // we cancel what we were doing
                if (currentGoal != null) currentGoal.Deactivate();
                if (currentAction != null) currentAction.Deactivate();

                // we initialize our new plan
                currentGoal = newGoal;
                nodeQueue = newNodeQueue;
                currentNode = nodeQueue.Dequeue();
                currentAction = currentNode.action;
                currentAction.Activate(currentNode.data);
                currentGoal.Activate();
            }
        }
        void LateUpdate()
        {
            //ReplanCheck();
            if (goals.Count == 0)
            {
                return;
            }
            if (nodeQueue == null)
            {
                GetNewPlan();
            }
            
            if (currentAction != null && currentAction.running)
            {
                currentAction.Tick(); // actions step
                if (currentAction.completed)//if action done
                {
                    if(nodeQueue.Count == 0)//if end of plan
                    {
                        currentGoal.Complete();
                        currentAction = null;
                        currentGoal = null;
                        nodeQueue = null;
                    }
                    else//if not end of plan
                    {
                        // we load the next node from our plan queue
                        currentNode = nodeQueue.Dequeue();
                        currentAction = currentNode.action;
                        currentAction.Activate(currentNode.data);
                    }
                }
                else if(currentAction.running == false)//action or goal failed -> purge current action,goal,plan
                {
                    currentGoal.Deactivate();
                    currentAction = null;
                    currentGoal = null;
                    nodeQueue = null;
                }
                else if(currentGoal.Active == false || currentGoal.enabledGoal == false)
                {
                    currentAction.Deactivate();
                    currentAction = null;
                    currentGoal = null;
                    nodeQueue = null;
                }
            }
        }
    }
}