using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP {
    public class WoodCutter : Agent
    {
        protected override void Start()
        {
            base.Start();
            SubGoal sl = new SubGoal("LogDrop", 1, true);
            SubGoal s2 = new SubGoal("DeadPlayer", 1, true);
            goals.Add(sl, 3);
            goals.Add(s2, 4);
        }
    }
}