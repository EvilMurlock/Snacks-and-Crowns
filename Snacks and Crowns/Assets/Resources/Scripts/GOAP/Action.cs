using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    public abstract class Action : MonoBehaviour
    {
        public string actionName = "Action";
        public GameObject target;
        public List<string> targetTags = new List<string>();

        public WorldState preconditions = new WorldState();
        public WorldState effects = new WorldState();

        public bool running = false;
        public bool completed = false;
        protected NpcAi npcAi;
        public virtual void Awake()
        {
            actionName = this.GetType().Name;
        }
        protected void Start()
        {
            npcAi = GetComponent<NpcAi>();
        }
        public virtual void Tick()
        {
            if (target == null) Deactivate();
            if (npcAi.reachedEndOfPath) Complete();
        }
        public virtual float GetCost(WorldState worldState)
        {
            
            return GetDistanceFromTarget();
        }
        public virtual bool IsUsableBy(GameObject g)
        {
            return true;
        }
        public virtual bool IsAchievableGiven(WorldState worldState)//For the planer
        {
            if (!worldState.CompletesGoal(preconditions) || FindTarget() == null) return false;
            return true;
        }
        public virtual bool IsAchievable()//Checs curent condition + world state
        {
            return true;
        }
        public virtual void Activate()
        {
            Activate(FindTarget());
        }
        public virtual void Activate(object newTarget)
        {
            if (newTarget == null) target = FindTarget();
            else this.target = (GameObject)newTarget;
            Debug.Log("Target is now: " + target.name);
            running = true;
            completed = false;
            Debug.Log("Switching path to " + target.name);
            gameObject.GetComponent<NpcAi>().ChangeTarget(target);

        }
        public virtual void Deactivate()
        {
            running = false;
        }
        public virtual void Complete()
        {
            running = false;
            completed = true;
        }
        public virtual List<Node> OnActionCompleteWorldStates(Node parent)
        {
            //Tells the planer how the world state will change on completion
            WorldState newWorldstate = new WorldState(parent.state);
            newWorldstate.AddStates(effects);
            List<Node> possibleNodes = new List<Node>();

            possibleNodes.Add(new Node(parent, parent.cost + GetDistanceFromTarget(), newWorldstate, this, null));
            return possibleNodes;
        }
        protected float GetDistanceFromTarget()
        {
            if (target != null) return (gameObject.transform.position - target.transform.position).magnitude;
            return 0;
        }
        protected float GetDistanceFromObject(GameObject distanceTarget)
        {
            if (distanceTarget != null) return (gameObject.transform.position - distanceTarget.transform.position).magnitude;
            return 0;
        }
        protected GameObject FindTarget()
        {
            GameObject foundTarget = null;
            if (targetTags.Count == 0) //If no target tags then target is considered self
            {
                foundTarget = this.gameObject; 
            }
            foreach (TagSystem tagSys in FindObjectsByType<TagSystem>(FindObjectsSortMode.None))
            {
                if (tagSys.HasTags(targetTags))
                {
                    if (foundTarget == null || (gameObject.transform.position - target.transform.position).magnitude < (gameObject.transform.position - tagSys.gameObject.transform.position).magnitude)
                    {
                        foundTarget = tagSys.gameObject;
                    }
                    //Debug.Log("Target found");
                }
            }


            //DEBUG SECTION
            /*
            string allTgs = "";
            foreach (string t in targetTags)
            {
                allTgs += (t + ", ");
            }
            Debug.Log("Target with tags: "+ allTgs);
            Debug.Log("\n Found: " + foundTarget.name);
            */
            return foundTarget;
        }
    }
}
