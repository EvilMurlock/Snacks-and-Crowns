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

            targetTags.Add("Log");
            base.Awake();
        }
    } 
}
