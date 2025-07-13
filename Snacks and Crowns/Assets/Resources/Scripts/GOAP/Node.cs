using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GOAP
{
    /// <summary>
    /// Holds data in the Node of the plan
    /// </summary>
    public class Node
    {
        public Node parent;
        /// <summary>
        /// Cost is not really used in the current version, but would be used to find the cheapest plan
        /// </summary>
        public float cost;
        public WorldState state;
        public NPCAction action;
        /// <summary>
        /// Data that some action have pre-planned
        /// </summary>
        public ActionData data;
        public Node(Node parent, float cost, WorldState worldState, NPCAction action, ActionData data)
        {
            this.parent = parent;
            this.cost = cost;
            this.state = worldState;
            this.action = action;
            this.data = data;
        }
    }
}