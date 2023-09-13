using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    public class Planner
    {
        public Queue<Action> Plan(List<Action> actions, WorldState goal, WorldState states)
        {            
            List<Node> leaves = new List<Node>();
            Node start = new Node(null, 0, World.Instance.GetWorld(), null);

            bool success = BuildGraph(start, leaves, actions, goal);

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

            List<Action> result = new List<Action>();
            Node n = cheapest;
            while(n != null)
            {
                if(n.action != null)
                {
                    result.Insert(0, n.action);
                }
                n = n.parent;
            }
            Queue<Action> queue = new Queue<Action>();
            foreach (Action a in result)
            {
                queue.Enqueue(a);
            }
            string planDebug = "The plan is:";
            int actionNumber = 0;
            foreach (Action a in queue)
            {
                //planDebug += "\n"; 
                planDebug += " -> "+actionNumber + " - " + a.actionName;
                actionNumber++;

            }
            planDebug += " | Cost: " + cheapest.cost;
            Debug.Log(planDebug);
            return queue;
        }
        bool BuildGraph(Node parent, List<Node> leaves, List<Action> usableActions, WorldState goal)
        {
            //Plans can use each action only ONCE!!!
            //Actions are valid as long as required KEY exists, it ignores goal values!
            bool foundPath = false;
            foreach(Action action in usableActions)
            {
                /*
                Debug.Log("Curent State: \n" + parent.state.ToString());
                Debug.Log("Action "+action.actionName+" predonditions: \n" + action.preconditions.ToString());
                bool testBool = true;
                foreach (KeyValuePair<string, object> g in action.preconditions.GetStates())
                {
                    if (!parent.state.GetStates().ContainsKey(g.Key)) testBool = false;
                    else if (parent.state.GetStates()[g.Key] != (g.Value))
                        {
                        Debug.Log("Bool Result: " + !parent.state.GetStates()[g.Key].Equals(g.Value));
                        Debug.Log("Goal Value: " + parent.state.GetStates()[g.Key]); 
                        Debug.Log("State Value: " + g.Value); testBool = false; }
                }

                Debug.Log("Is achievable: " + testBool);
                */
                if (action.IsAchievableGiven(parent.state))
                {
                    WorldState currentState = new WorldState(parent.state);
                    currentState = action.OnActionCompleteWorldStates(currentState);

                    Node node = new Node(parent, parent.cost + action.GetCost(currentState), currentState, action);

                    if(currentState.CompletesGoal(goal))
                    {
                        Debug.Log("Leaf Found");
                        leaves.Add(node);
                        foundPath = true;
                    }
                    else
                    {
                        
                        List<Action> subset = ActionSubset(usableActions, action); //REMOVES USED ACTIONS - PREVENTS LOOPS - ALSO PREVENTS REUSING ACTIONS
                        bool found = BuildGraph(node, leaves, subset, goal);
                        if (found)
                            foundPath = true;
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