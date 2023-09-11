using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
namespace GOAP
{
    public class Agent : MonoBehaviour
    {
        NpcAi npcAi;

        public List<Action> actions = new List<Action>();
        public Dictionary<SubGoal, int> goals = new Dictionary<SubGoal, int>();

        Planner planner;
        Queue<Action> actionQueue;
        public Action currentAction;
        SubGoal currentGoal;

        bool invoked = false;

        // Start is called before the first frame update
        protected virtual void Start()
        {
            npcAi = gameObject.GetComponent<NpcAi>();
            var all_actions = Resources.LoadAll("Scripts/GOAP/Actions");
            Debug.Log("Action count: "+all_actions.Count());
            Debug.Log("Loaded actions: ");
            foreach (Object o in all_actions)   
            {
                Action a = (Action)o;
                Debug.Log(o.GetType().ToString());
                //if (a.IsAchievableBy(gameObject)) gameObject.AddComponent( a.GetType());
            }
            Action[] acts = this.GetComponents<Action>();
            foreach (Action a in acts)
                actions.Add(a);
        }
        void CompleteAction()
        {
            currentAction.running = false;
            currentAction.PostPreform();
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
            if(planner == null || actionQueue == null)
            {
                planner = new Planner();

                var sortedGoals = from entry in goals orderby entry.Value descending select entry;

                foreach(KeyValuePair<SubGoal, int> sg in sortedGoals)
                {
                    actionQueue = planner.Plan(actions, sg.Key.sgoals, null);
                    if(actionQueue != null)
                    {
                        currentGoal = sg.Key;
                        break;
                    }
                }
            }
            if(actionQueue != null && actionQueue.Count == 0)
            {
                if (currentGoal.remove)
                {
                    Debug.Log("Acomplished goal: " + currentGoal.sgoals.Keys.First<string>());
                    goals.Remove(currentGoal);
                }
                planner = null;
            }
            if(actionQueue != null && actionQueue.Count > 0)
            {
                currentAction = actionQueue.Dequeue();
                if (currentAction.PrePerform())
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