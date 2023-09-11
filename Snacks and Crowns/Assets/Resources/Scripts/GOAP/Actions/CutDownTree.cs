using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GOAP
{
    public class CutDownTree : Action
    {
        public override void Awake()
        {
            duration = 3;
            targetTags.Add("Log");

            this.effects.Add("LogDrop", 0);
            base.Awake();
        }
        public override bool PrePerform()
        {
            bool hasTarget = FindTarget();
            return hasTarget;
        }

        public override bool PostPreform()
        {
            return true;
        }
    } 
}
