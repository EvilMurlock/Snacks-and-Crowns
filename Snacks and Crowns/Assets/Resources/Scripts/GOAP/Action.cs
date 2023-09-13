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
        public float duration = 0;

        public WorldState preconditions = new WorldState();
        public WorldState effects = new WorldState();

        public bool running = false;

        public virtual void Awake()
        {
            actionName = this.GetType().Name;
        }
        public virtual void Tick()
        {

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
            return worldState.CompletesGoal(preconditions);
        }
        public virtual bool IsAchievable()//Checs curent condition + world state
        {
            return true;
        }
        public virtual void Activate()
        {
            
        }
        public virtual void Deactivate()
        {

        }
        public virtual WorldState OnActionCompleteWorldStates(WorldState worldstate)
        {
            //Tells the planer how the world state will change on completion
            WorldState newWorldstate = new WorldState(worldstate);
            newWorldstate.AddStates(effects);
            return newWorldstate;
        }
        float GetDistanceFromTarget()
        {
            if (target != null) return (gameObject.transform.position - target.transform.position).magnitude;
            return 0;
        }
        protected bool FindTarget()
        {
            if (targetTags.Count == 0) //If no target tags then target is considered self
            {
                target = gameObject;
                return true; 
            }
            bool found = false;
            foreach (TagSystem tagSys in FindObjectsByType<TagSystem>(FindObjectsSortMode.None))
            {
                if (tagSys.HasTags(targetTags))
                {
                    if (found == false) 
                    {
                        target = tagSys.gameObject; found = true;
                    }
                    else if ((gameObject.transform.position - target.transform.position).magnitude < (gameObject.transform.position - tagSys.gameObject.transform.position).magnitude)
                    {
                        target = tagSys.gameObject;

                    }
                }
            }


            //DEBUG SECTION
            string allTgs = "";
            foreach (string t in targetTags)
            {
                allTgs += (t + ", ");
            }
            Debug.Log("Target with tags: "+ allTgs);
            Debug.Log("\n Found: " +found);
            
            return found;
        }
    }
}
