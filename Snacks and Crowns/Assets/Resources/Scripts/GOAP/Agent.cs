using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System;

namespace GOAP
{
    public class Agent : MonoBehaviour
    {
        public List<Action> actions = new List<Action>();
        public List<Goal> goals = new List<Goal>();

        Planner planner;
        Queue<Node> nodeQueue;
        Node currentNode;
        public Action currentAction;
        Goal currentGoal;
        WorldState worldState;

        NpcAi npcAi;
        
        float planingDelayAfterFail = 1;
        float planingDelayNow = 1;

        // Start is called before the first frame update
        protected virtual void Start()
        {

            planner = new Planner();
            worldState = new WorldState(this.gameObject);
            npcAi = GetComponent<NpcAi>();
            AddAllActions();
            /*
            object[] action_scripts = Resources.LoadAll("Scripts/GOAP/Actions"); // adds all actions avalible to this agent
            foreach(object o in action_scripts)
            {
                MonoScript a = (MonoScript)o;
                //Debug.Log("Action loaded: "+ a.GetClass().ToString());
                
                gameObject.AddComponent(a.GetClass());
            }*/
            LoadActionsAndGoals();
        }
        private void AddAllActions()
        {
            // uses reflection to add all action monobehaviours to our gameObject
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var types = assemblies.SelectMany(assembly => assembly.GetTypes());
            var filteredTypes = types.Where(
            type => IsSubclassOfRawGeneric(typeof(GOAP.Action), type)
            && !type.ContainsGenericParameters
            && type.IsClass);
            List<Type> actionTypes = filteredTypes.ToList();
            //Debug.Log("Instantiating this many actions: " + actionTypes.Count);
            foreach(Type actionType in actionTypes)
            {
                //Debug.Log(actionType.ToString());
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

                /*
                if (toCheck.Namespace == "GOAP")
                {
                    Debug.Log("Checking: " + cur.Name.ToString());
                    Debug.Log("Generic: " + generic.Name.ToString());
                }*/

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
            Action[] acts = this.GetComponents<Action>();
            foreach (Action a in acts)
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
            Debug.Log("GOALS:\n"+goals.ToString());
        }
        // Update is called once per frame
        void LateUpdate()
        {
            if (goals.Count == 0)
            {
                //Debug.Log("No goals");
                return;
            }
            if(currentGoal != null)
            {
                //Debug.Log("Current goal: " + currentGoal.GetType().ToString());
            }
            if (nodeQueue == null)
            {
                npcAi.ChangeTarget(null); // we have no active action, so we arent moving anywhere
                // we try to find a plan that fulfils one of our goals, in order of priority
                var sortedGoals = from goal in goals orderby goal.CalculatePriority() descending select goal;
                Goal newGoal = null;
                Queue<Node> newNodeQueue = null;
                if (planingDelayNow < planingDelayAfterFail)
                    planingDelayNow += Time.deltaTime;
                else
                {
                    worldState.UpdateBelieves();
                    foreach (Goal g in sortedGoals)
                    {
                        if (g.CalculatePriority() <= 0) continue;
                        Queue<Node> queue = null;
                        //Debug.Log("Current goal can run: " + g.CanRun());
                        if (g.CanRun())
                            queue = planner.CreatePlan(actions, g, worldState);
                        if (queue == null)
                        {
                            //Debug.Log("Queue is null!!!!");
                            planner.DebugPrintPlan(queue);
                        }
                        if (queue != null)
                        {
                            newGoal = g;
                            newNodeQueue = queue;
                            break;
                        }
                    }
                    planingDelayNow = 0;
                }

                // we swap into the new plan if we found a plan and our old goal has a lesser priority
                if (newGoal != null && newGoal.CalculatePriority() > 0 
                    && (currentGoal == null || newGoal.CalculatePriority() > currentGoal.CalculatePriority()))//Switch plans when plan with a higher priority goal is found
                {
                    //Debug.Log("Working on a new goal: " + newGoal.GetType().ToString());
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
            if (currentAction != null && currentAction.running)
            {
                //Debug.Log("Actionin runing");
                currentAction.Tick(); // actions step
                if (currentAction.completed)//if action done
                {
                    if(nodeQueue.Count == 0)//if end of plan
                    {
                        //Debug.Log("Completed plan: " +currentGoal.ToString());
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
                    Debug.Log("Plan failed - ACTION");
                    currentGoal.Deactivate();
                    currentAction = null;
                    currentGoal = null;
                    nodeQueue = null;
                }
                else if(currentGoal.Active == false || currentGoal.enabledGoal == false)
                {
                    Debug.Log("Plan failed - GOAL");
                    currentAction.Deactivate();
                    currentAction = null;
                    currentGoal = null;
                    nodeQueue = null;
                }
            }
        }
    }
}