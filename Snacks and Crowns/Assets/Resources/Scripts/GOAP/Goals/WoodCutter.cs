using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP {
    public class WoodCutter : Goal
    {
        float priority = 5;
        private void Awake()
        {
            desiredState.AddState("LogDrop", 1);
        }
        public override void Tick()
        {

        }
        public override void Activate()
        {

        }
        public override void Deactivate()
        {

        }

        public override void Complete()
        {

        }
        public override float CalculatePriority()
        {
            return priority;
        }
    }
}