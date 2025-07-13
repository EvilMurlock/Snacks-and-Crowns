using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GOAP
{
    /// <summary>
    /// Searches for a plan that fulfils the given plan
    /// </summary>
    public class Planner
    {
        public Queue<Node> CreatePlan(List<NPCAction> actions, Goal goal, WorldState states)
        {
            Node lastPlanNode = FindPlanBreathFirst(actions, goal, states);

            // if we found no plan
            if (lastPlanNode == null)
            {
                return null;
            }
            Queue<Node> planQueue = CreatePlanQueue(lastPlanNode);
            //DebugPrintPlan(planQueue);
            return planQueue;
        }
        public void DebugPrintPlan(Queue<Node> planQueue)
        {
            string planDebug = "The plan is:";
            int actionNumber = 0;
            float costLast = 0;
            float totalCost = 0;
            if (planQueue == null) return;
            foreach (Node a in planQueue)
            {
                planDebug += " -> " + actionNumber + " - " + a.action.GetInfo(a.data);
                actionNumber++;
                costLast = a.cost;
                totalCost += a.cost;
            }
            planDebug += " | Total Cost: " + totalCost;
            Debug.Log(planDebug);
        }
        /// <summary>
        /// Creates a Queue of Nodes representing the plan 
        /// </summary>
        /// <param name="lastPlanNode"></param>
        /// <returns></returns>
        Queue<Node> CreatePlanQueue(Node lastPlanNode)
        {
            List<Node> result = new List<Node>();
            Node n = lastPlanNode;
            while (n != null)
            {
                if (n.action != null)
                {
                    result.Insert(0, n);
                }
                n = n.parent;
            }
            Queue<Node> queue = new Queue<Node>();
            foreach (Node a in result)
            {
                queue.Enqueue(a);
            }
            return queue;
        }
        /// <summary>
        /// Checks if we already used the given action in a previous node
        /// </summary>
        bool ActionAlreadyUsed(Node node, NPCAction action)
        {
            Node currentNode = node;
            while(currentNode != null)
            {
                if (currentNode.action == action) return true;
                currentNode = currentNode.parent;
            }
            return false;
        }
        /// <summary>
        /// Inicializes plan search
        /// </summary>
        /// <param name="actions"></param>
        /// <param name="goal"></param>
        /// <param name="states"></param>
        /// <returns></returns>
        Node FindPlanBreathFirst(List<NPCAction> actions, Goal goal, WorldState states)
        {
            // we initialize our search graph with  starting leaf (root of the graph)
            Node start = new Node(null, 0, states, null, null);
            List<Node> leaves = new List<Node>();
            leaves.Add(start);
            return FindPlanBreathFirstRecursion(leaves, actions, goal, 1);
        }
        /// <summary>
        /// Searches using BFS with a max depth determined by the plan
        /// </summary>
        /// <param name="leaves"></param>
        /// <param name="usableActions"></param>
        /// <param name="goal"></param>
        /// <param name="depth"></param>
        /// <returns></returns>
        Node FindPlanBreathFirstRecursion(List<Node> leaves, List<NPCAction> usableActions, Goal goal, int depth)
        {
            if (depth > goal.MaxPlanDepth) return null;
            if (leaves.Count == 0) return null;

            List<Node> newLeaves = new List<Node>();
            foreach(Node parent in leaves)
            {
                foreach (NPCAction action in usableActions)
                {
                    if (!action.reusable && ActionAlreadyUsed(parent, action)) continue;
                    if (action.IsAchievableGiven(parent.state))
                    {
                        List<Node> possibleNewStates = action.OnActionCompleteWorldStates(parent);
                        
                        foreach (Node node in possibleNewStates)
                        {

                            if (goal.CompletedByState(node.state))
                            {
                                return node;
                            }
                            newLeaves.Add(node);
                        }

                    }
                }
            }
            //This code only fires when no action could fulfil the goal at this depth
            List<NPCAction> subset = new List<NPCAction>(usableActions);


            return FindPlanBreathFirstRecursion(newLeaves, subset, goal, depth + 1);
        }
    }
}