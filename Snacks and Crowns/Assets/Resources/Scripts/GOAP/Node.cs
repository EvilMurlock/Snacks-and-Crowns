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

        public Node(Node parent, float cost, WorldState worldState, Action action)
        {
            this.parent = parent;
            this.cost = cost;
            this.state = new WorldState(worldState);
            this.action = action;
        }
    }
}