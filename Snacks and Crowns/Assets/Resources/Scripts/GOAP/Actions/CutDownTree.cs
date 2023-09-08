using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP 
{
    public class CutDownTree : Action
    {
        public override bool PrePerform()
        {
            return true;
        }
        public override bool PostPreform()
        {
            return true;
        }
    } 
}
