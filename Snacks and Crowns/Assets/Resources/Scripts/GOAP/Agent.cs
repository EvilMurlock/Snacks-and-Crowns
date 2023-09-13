using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
namespace GOAP
{
    public class Agent : MonoBehaviour
    {
        NpcAi npcAi;

        public List<Action> actions = new List<Action>();
        public List<Goal> goals = new List<Goal>();

        Planner planner;
        Queue<Action> actionQueue;
        public Action currentAction;
        Goal currentGoal;

        bool invoked = false;

        // Start is called before the first frame update
        protected virtual void Start()
        {
            npcAi = gameObject.GetComponent<NpcAi>();


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
        void CompleteAction()
        {
            currentAction.running = false;
            //currentAction.OnActionComplete();
            invoked = false;
        }
        void PrintWorldGoals()
        {
            Debug.Log("GOALS:\n"+goals.ToString());
        }
        // Update is called once per frame
        void LateUpdate()
        {
            if (goals.Count() == 0) 
            {
                Debug.Log("No goals"); 
                return; 
            }
            if(currentAction != null && currentAction.running)
            {
                if (npcAi.reachedEndOfPath)
                {
                    if (!invoked)
                    {
                        Invoke("CompleteAction", currentAction.duration);
                        invoked = true;
                    }
                }
                return;
            }
            if(planner == null || actionQueue == null) //Returns a plan for the most important goal posible
            {
                planner = new Planner();

                var sortedGoals = from entry in goals orderby entry.CalculatePriority() descending select entry;

                foreach(Goal g in sortedGoals)
                {
                    Debug.Log("Planing for: "+g.name);
                    actionQueue = planner.Plan(actions, g.DesiredWorldState(), null);
                    if(actionQueue != null)
                    {
                        currentGoal = g;
                        break;
                    }
                }
            }
            if(actionQueue != null && actionQueue.Count == 0) //Goal quee finished - Goal no nececarily completed
            {
                Debug.Log("Acomplished goal: " + currentGoal.GetType().ToString());
                currentGoal.Complete();
                currentGoal = null;
                planner = null;
            }
            if(actionQueue != null && actionQueue.Count > 0)
            {
                currentAction = actionQueue.Dequeue();
                if (currentAction.IsAchievable())
                {
                    /*
                    if(currentAction.target == null && currentAction.targetTags.cou != "")
                    {
                        //!!! REPLACE THIS WITH CUSTOM TAG SYSTEM!!!!
                        //ALSO MAKE THE TAGS IN ACTIONS A LIST OF TAGS - EG: (Pickupable, Weapon)
                        currentAction.target = GameObject.FindWithTag(currentAction.targetTag);
                    }
                    */
                    //if(currentAction.targetTags.Count != 0)
                    //{
                        currentAction.running = true;
                        npcAi.ChangeTarget(currentAction.target);
                    //}
                }
                else
                {
                    actionQueue = null;
                }
            }
        }
    }
}