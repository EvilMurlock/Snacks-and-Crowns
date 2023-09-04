using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    public class Agent : MonoBehaviour
    {
        public List<Action> actions = new List<Action>();
        public Dictionary<SubGoal, int> goals = new Dictionary<SubGoal, int>();

        Planner planner;
        Queue<Action> actionQueue;
        public Action currentAction;
        SubGoal currentGoal;
        // Start is called before the first frame update
        void Start()
        {
            Action[] acts = this.GetComponents<Action>();
            foreach (Action a in acts)
                actions.Add(a);
        }

        // Update is called once per frame
        void LateUpdate()
        {

        }
    }
}