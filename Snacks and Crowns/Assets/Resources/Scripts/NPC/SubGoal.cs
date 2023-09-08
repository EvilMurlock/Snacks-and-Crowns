using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GOAP
{

    public class SubGoal
    {
        public Dictionary<string, int> sgoals;
        public bool remove;

        public SubGoal(string key, int value, bool remove)
        {
            sgoals = new Dictionary<string, int>();
            sgoals.Add(key, value);
            this.remove = remove;
        }
    }
}
