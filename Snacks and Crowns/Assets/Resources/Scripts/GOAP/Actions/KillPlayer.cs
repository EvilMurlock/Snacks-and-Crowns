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

            targetTags.Add("Player");
            base.Awake();
        }
    } 
}
