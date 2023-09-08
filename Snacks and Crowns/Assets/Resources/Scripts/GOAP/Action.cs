using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    public abstract class Action : MonoBehaviour
    {
        public string actionName = "Action";
        public float cost = 1f;
        public GameObject target;
        public string targetTag;
        public float duration = 0;
        public WorldState[] preConditions;
        public WorldState[] afterEffects;
        public NpcAi agent;

        public Dictionary<string, int> preconditions;
        public Dictionary<string, int> effects;

        public WorldStates agentBeliefs;

        public bool running = false;

        public Action()
        {
            preconditions = new Dictionary<string, int>();
            effects = new Dictionary<string, int>();
        }
        public void Awake()
        {
            actionName = this.GetType().Name;
            agent = this.gameObject.GetComponent<NpcAi>();
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
            return true;
        }
        public abstract bool PrePerform();
        public abstract bool PostPreform();
    }
}
