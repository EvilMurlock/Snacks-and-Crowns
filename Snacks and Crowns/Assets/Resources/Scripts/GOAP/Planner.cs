using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GOAP
{
    public class Planner
    {
        //int maxDepth = 3;
        float maxCost = 20;
        public Queue<Node> CreatePlan(List<Action> actions, Goal goal, WorldState states)
        {
            Node lastPlanNode = FindPlanBreathFirst(actions, goal, states);

            // if we found no plan
            if (lastPlanNode == null)
            {
                //Debug.Log("No GOAP plan!");
                return null;
            }
            //Debug.Log(lastPlanNode.action.actionName);
            Queue<Node> planQueue = CreatePlanQueue(lastPlanNode);
            
            DebugPrintPlan(planQueue);
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
                //planDebug += "\n"; 
                planDebug += " -> " + actionNumber + " - " + a.action.GetInfo(a.data);//+ a.action.actionName + " (" + (a.cost - costLast) + ")";
                actionNumber++;
                costLast = a.cost;
                totalCost += a.cost;
            }
            planDebug += " | Total Cost: " + totalCost;
            Debug.Log(planDebug);
        }
        Queue<Node> CreatePlanQueue(Node lastPlanNode)
        {
            List<Node> result = new List<Node>();
            Node n = lastPlanNode;
            while (n != null)
            {
                if (n.action != null)
                {
                    //result.Add(n);
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
        bool ActionAlreadyUsed(Node node, Action action)
        {
            Node currentNode = node;
            while(currentNode != null)
            {
                if (currentNode.action == action) return true;
                currentNode = currentNode.parent;
            }
            return false;
        }
        Node FindPlanBreathFirst(List<Action> actions, Goal goal, WorldState states)
        {
            // we initialize our search graph with  starting leaf (root of the graph)
            Node start = new Node(null, 0, states, null, null);
            List<Node> leaves = new List<Node>();
            leaves.Add(start);
            return FindPlanBreathFirstRecursion(leaves, actions, goal, 1);
        }
        Node FindPlanBreathFirstRecursion(List<Node> leaves, List<Action> usableActions, Goal goal, int depth)
        {
            //if (depth > maxDepth) return null;
            //Debug.Log("Max depth of goal: " + goal.GetType().ToString() + " => " + goal.MaxPlanDepth);
            if (depth > goal.MaxPlanDepth) return null;
            if (leaves.Count == 0) return null;

            List<Node> newLeaves = new List<Node>();
            foreach(Node parent in leaves)
            {
                foreach (Action action in usableActions)
                {
                    if (!action.reusable && ActionAlreadyUsed(parent, action)) continue;
                    if (action.IsAchievableGiven(parent.state))
                    {
                        //if(depth == 1)Debug.Log("Action name: " + action.GetType().ToString());
                        
                        List<Node> possibleNewStates = action.OnActionCompleteWorldStates(parent);
                        
                        // currentState = action.OnActionCompleteWorldStates(currentState);
                        //Debug.Log("New Node count: " + possibleNewStates.Count);
                        foreach (Node node in possibleNewStates)
                        {
                            /*
                            if (depth == 2 && parent.action.actionName == "HarvestResource")
                            {
                                //Debug.Log("Action name: " + action.GetType().ToString());
                                if (action.GetType().ToString() == "GOAP.PutItemInChest" && ((ActionDataPutItemInChest)node.data).item.itemName == "Log" )
                                {
                                    Debug.Log( "Putting this item into a chest: "+((ActionDataPutItemInChest)node.data).item + "  BUT goal completed: "+ goal.CompletedByState(node.state));
                                }
                            }*/
                            

                            if (goal.CompletedByState(node.state))
                            {
                                //Debug.Log("Action name in plan: " + node.parent.action.GetType().ToString());
                                //Debug.Log("- - - - Data: " + ((ActionDataPickUpItem)node.parent.data).itemPickup.item.itemName);
                                
                                //Debug.Log("Plan Found");
                                return node;
                            }

                            // if (node.cost <= maxCost)// && UniqueState(achievedStates, node.state))
                            //{
                            //achievedStates.Add(node.state); // for skipping duplicate states - not doing it, its not worth it
                            newLeaves.Add(node);
                            //}
                        }

                    }
                }
            }
            //This code only fires when no action could fulfil the goal at this depth
            List<Action> subset = new List<Action>(usableActions);

            //if (!action.reusable)
            //    subset = ActionSubset(usableActions, action); //REMOVES USED ACTIONS - PREVENTS LOOPS - ALSO PREVENTS REUSING ACTIONS

            return FindPlanBreathFirstRecursion(newLeaves, subset, goal, depth + 1);
        }
    }
}