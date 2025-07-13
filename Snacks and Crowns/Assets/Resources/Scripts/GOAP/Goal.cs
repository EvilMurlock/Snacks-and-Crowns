using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GOAP
{
    /// <summary>
    /// Base goal class
    /// </summary>
    public abstract class Goal : MonoBehaviour
    {
        public int MaxPlanDepth { get; protected set; }
        protected WorldState desiredState = new WorldState();

        [SerializeField]
        public bool enabledGoal = true;

        protected bool active = false;
        public bool Active => active;
        public virtual void Start()
        {
            MaxPlanDepth = 3;
        }
        private void Update()
        {
            Tick();
        }
        public virtual void Tick()
        {

        }
        public virtual bool CanRun()
        {
            return enabledGoal;
        }
        public virtual void Activate()
        {
            active = true;
        }
        public virtual void Deactivate()
        {
            active = false;
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
