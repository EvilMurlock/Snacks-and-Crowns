using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    public class KillPlayer : Action
    {
        public override void Awake()
        {
            duration = 3;
            targetTags.Add("Player");
            this.preconditions.Add("LogDrop", 0);

            this.effects.Add("DeadPlayer", 0);
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
