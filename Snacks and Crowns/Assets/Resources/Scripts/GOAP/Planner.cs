using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    public class Planner
    {
        public Queue<Node> Plan(List<Action> actions, Goal goal, WorldState states)
        {            
            List<Node> leaves = new List<Node>();
            Node start = new Node(null, 0, states, null, null);

            bool success = BuildGraph(start, leaves, actions, goal,0);

            if (!success)
            {
                Debug.Log("No GOAP plan!");
                return null;
            }
            Node cheapest = null;
            foreach(Node leaf in leaves)
            {
                if (cheapest == null) cheapest = leaf;
                else if (leaf.cost < cheapest.cost) cheapest = leaf;
            }

            List<Node> result = new List<Node>();
            Node n = cheapest;
            while(n != null)
            {
                if(n.action != null)
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

            //Plan Debuging
            //Debug.Log("Leaves number: " + leaves.Count);
            /*
            Debug.Log("Last action: "+ cheapest.action.actionName);
            Debug.Log("Result Lenght: " + result.Count);

            Debug.Log("Plan Lenght: "+queue.Count);
            */
            string planDebug = "The plan is:";
            int actionNumber = 0;
            float costLast = 0;
            foreach (Node a in queue)
            {
                //planDebug += "\n"; 
                planDebug += " -> "+actionNumber + " - " + a.action.actionName + " (" + (a.cost - costLast)+")";
                actionNumber++;
                costLast = a.cost;
            }
            planDebug += " | Total Cost: " + cheapest.cost;
            Debug.Log(planDebug);
            return queue;
        }
        bool BuildGraph(Node parent, List<Node> leaves, List<Action> usableActions, Goal goal, int depth)
        {
            //if (depth > 5) return false;
            //Plans can use each action only ONCE!!!
            //Actions are valid as long as required KEY exists, it ignores goal values!
            bool foundPath = false;
            foreach(Action action in usableActions)
            {
                if (action.IsAchievableGiven(parent.state))
                {
                    List<Node> possibleNewStates = new List<Node>();
                    possibleNewStates = action.OnActionCompleteWorldStates(parent);
                    //currentState = action.OnActionCompleteWorldStates(currentState);
                    foreach (Node node in possibleNewStates)
                    {
                        //Node node = new Node(parent, parent.cost + newStateCostPair.cost, newStateCostPair.state, action);

                        if (goal.CompletedByState(node.state))
                        {
                            leaves.Add(node);
                            foundPath = true;
                        }
                        else
                        {
                            List<Action> subset = ActionSubset(usableActions, action); //REMOVES USED ACTIONS - PREVENTS LOOPS - ALSO PREVENTS REUSING ACTIONS
                            bool found = BuildGraph(node, leaves, subset, goal, depth+1);
                            if (found)
                                foundPath = true;
                        }
                    }
                }
            }
            return foundPath;
        }

        List<Action> ActionSubset(List<Action> actions, Action removeMe)//removes used actions
        {
            List<Action> subset = new List<Action>();
            foreach(Action a in actions)
            {
                if (!a.Equals(removeMe))
                    subset.Add(a);
            }
            return subset;
        }
    }
}