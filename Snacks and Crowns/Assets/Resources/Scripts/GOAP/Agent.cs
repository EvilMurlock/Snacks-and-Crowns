using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
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
        AgentBelieveState agentBelieveState;

        // Start is called before the first frame update
        protected virtual void Start()
        {
            planner = new Planner();
            agentBelieveState = gameObject.GetComponent<AgentBelieveState>();

            object[] action_scripts = Resources.LoadAll("Scripts/GOAP/Actions");
            foreach(object o in action_scripts)
            {
                MonoScript a = (MonoScript)o;
                //Debug.Log("Action loaded: "+ a.GetClass().ToString());
                
                gameObject.AddComponent(a.GetClass());
            }
            Action[] acts = this.GetComponents<Action>();
            foreach (Action a in acts)
                if(a.IsUsableBy(gameObject))actions.Add(a);

            Goal[] gs = this.GetComponents<Goal>();
            foreach (Goal g in gs)
                goals.Add(g);
        }
        void PrintWorldGoals()
        {
            Debug.Log("GOALS:\n"+goals.ToString());
        }
        WorldState GetAgentBelieveState()
        {
            return agentBelieveState.AgentBelieves;
        }
        (Queue<Node>, Goal) FindBestPlan() //Returns a plan for the most important goal posible
        {
            var sortedGoals = from entry in goals orderby entry.CalculatePriority() descending select entry;

            foreach (Goal g in sortedGoals)
            {
                //Debug.Log("Planing for: "+g.name);
                /*
                Debug.Log(g.name);
                Debug.Log(actions);
                Debug.Log(GetAgentBelieveState());
                Debug.Log(planner);
                */
                Queue<Node> queue = planner.Plan(actions, g, GetAgentBelieveState());
                if (queue != null)
                {
                    return (queue, g);
                }
            }
            return (null, null);
        }
        // Update is called once per frame
        void LateUpdate()
        {
            if (goals.Count() == 0) 
            {
                Debug.Log("No goals"); 
                return; 
            }

            (Queue<Node> nodeQueue, Goal goal) planGoal = FindBestPlan();

            if (planGoal.goal != null && planGoal.goal.CalculatePriority() > 0 && (currentGoal == null 
                || planGoal.goal.CalculatePriority() > currentGoal.CalculatePriority()))//Switch plans when plan with a higher priority goal is found
            {
                if(currentGoal != null) currentGoal.Deactivate();
                if(currentAction != null) currentAction.Deactivate();

                currentGoal = planGoal.goal;
                nodeQueue = planGoal.nodeQueue;
                currentNode = nodeQueue.Dequeue();
                currentAction = currentNode.action;
                currentAction.Activate(currentNode.data);
                currentGoal.Activate();
            }

            if (currentAction != null && currentAction.running)
            {
                currentAction.Tick();
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
                        currentNode = nodeQueue.Dequeue();
                        currentAction = currentNode.action;
                        currentAction.Activate(currentNode.data);
                    }
                }
                else if(currentAction.running == false)//action failed -> purge current action,goal,plan
                {
                    currentAction = null;
                    currentGoal = null;
                    nodeQueue = null;
                }
            }
        }
    }
}