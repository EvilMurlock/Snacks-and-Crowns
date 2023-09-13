using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    public class KillPlayer : Action
    {
        public override void Awake()
        {
            preconditions.AddState("LogDrop", 1);

            effects.AddState("DeadPlayer", true);

            duration = 3;
            targetTags.Add("Player");
            base.Awake();
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
    } 
}
