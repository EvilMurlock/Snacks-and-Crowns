using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GOAP
{
    public class CutDownTree : Action
    {
        public override void Awake()
        {
            effects.AddState("LogDrop", 1);

            duration = 3;
            targetTags.Add("Log");
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
