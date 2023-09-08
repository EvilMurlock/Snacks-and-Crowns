using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GOAP
{
    public class Node
    {
        public Node parent;
        public float cost;
        public Dictionary<string, int> state;
        public Action action;

        public Node(Node parent, float cost, Dictionary<string,int> allstates, Action action)
        {
            this.parent = parent;
            this.cost = cost;
            this.state = new Dictionary<string, int>(allstates);
            this.action = action;
        }
    }
}