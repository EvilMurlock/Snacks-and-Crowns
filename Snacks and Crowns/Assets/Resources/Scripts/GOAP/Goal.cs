using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GOAP
{
    public abstract class Goal : MonoBehaviour
    {
        protected WorldState desiredState = new WorldState();
        [SerializeField]
        protected bool active = false;
        public bool Active => active;
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
            Debug.Log("Acomplished goal: " + this.GetType().ToString());
        }
        public virtual float CalculatePriority()
        {
            return -1;
        }
        public virtual WorldState DesiredWorldState()
        {
            return desiredState;
        }
        public abstract bool CompletedByState(WorldState state);
    }
}
