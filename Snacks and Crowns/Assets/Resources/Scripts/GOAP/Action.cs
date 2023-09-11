using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    public abstract class Action : MonoBehaviour
    {
        public string actionName = "Action";
        [SerializeField]
        public float cost = 1f;
        public GameObject target;
        public List<string> targetTags = new List<string>();
        public float duration = 0;
        public WorldState[] preConditions;
        public WorldState[] afterEffects;
        //public NpcAi agent;

        public Dictionary<string, int> preconditions;
        public Dictionary<string, int> effects;

        public WorldStates agentBeliefs;

        public bool running = false;

        public Action()
        {
            preconditions = new Dictionary<string, int>();
            effects = new Dictionary<string, int>();
        }
        public virtual void Awake()
        {
            actionName = this.GetType().Name;
            //agent = this.gameObject.GetComponent<NpcAi>();
            /*
            if (preConditions != null)
                foreach(WorldState w in preConditions)
                {
                    preconditions.Add(w.key, w.value);
                }
            if (afterEffects != null)
                foreach (WorldState w in afterEffects)
                {
                    effects.Add(w.key, w.value);
                }
            */
        }
        public bool IsAchievableBy(GameObject actor)
        {
            return true;
        }

        public bool IsAchievable()
        {
            return true;
        }
        public bool IsAchievableGiven(Dictionary<string, int> conditions)
        {
            foreach(KeyValuePair<string,int> p in preconditions)
            {
                if (!conditions.ContainsKey(p.Key)) return false;
            }
            return FindTarget();
        }
        public abstract bool PrePerform();
        public abstract bool PostPreform();

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
                    cost = (gameObject.transform.position - target.transform.position).magnitude;
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
