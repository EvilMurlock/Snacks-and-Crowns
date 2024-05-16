using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GOAP
{
    public class Node
    {
        public Node parent;
        public float cost;
        public WorldState state;
        public Action action;
        public ActionData data;
        public Node(Node parent, float cost, WorldState worldState, Action action, ActionData data)
        {
            this.parent = parent;
            this.cost = cost;
            this.state = worldState;
            this.action = action;
            this.data = data;
        }
    }
}