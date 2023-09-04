using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GOAP
{

    public class SubGoal
    {
        public Dictionary<string, int> sgoals;
        public bool remove;

        public SubGoal(string s, int i, bool r)
        {
            sgoals = new Dictionary<string, int>();
            sgoals.Add(s, i);
            remove = r;
        }
    }
}
