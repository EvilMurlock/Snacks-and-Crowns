using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GOAP
{
    public class Goal : MonoBehaviour
    {
        protected WorldState desiredState = new WorldState();
        private void Update()
        {
            Tick();
        }
        public virtual void Tick()
        {

        }
        public virtual bool CanRun()
        {
            return true;
        }
        public virtual void Activate()
        {

        }
        public virtual void Deactivate()
        {

        }

        public virtual void Complete()
        {

        }
        public virtual float CalculatePriority()
        {
            return -1;
        }
        public virtual WorldState DesiredWorldState()
        {
            return desiredState;
        }
    }

    /*
    public class SubGoal
    {
        public Dictionary<string, object> sgoals;
        public bool remove;

        public SubGoal(string key, object value, bool remove)
        {
            sgoals = new Dictionary<string, object>();
            sgoals.Add(key, value);
            this.remove = remove;
        }
    }
    */
}
