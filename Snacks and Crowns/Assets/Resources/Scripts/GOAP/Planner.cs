using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    public class Planner
    {
        public Queue<Action> Plan(List<Action> actions, Dictionary<string, int> goal, WorldStates states)
        {
            List<Action> usableActions = new List<Action>();
            foreach(Action a in actions)
            {
                if (a.IsAchievable()) usableActions.Add(a);
            }
            List<Node> leaves = new List<Node>();
            Node start = new Node(null, 0, World.Instance.GetWorld().GetStates(),null);

            bool success = BuildGraph(start, leaves, usableActions, goal);
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
            Debug.Log("The plan is:");
            int actionNumber = 0;
            foreach (Action a in queue)
            {
                Debug.Log(actionNumber +" - "+a.actionName);
                actionNumber++;
            }

            return queue;
        }
        bool BuildGraph(Node parent, List<Node> leaves, List<Action> usableActions, Dictionary<string,int> goal)
        {
            //Plans can use each action only ONCE!!!
            //Actions are valid as long as required KEY exists, it ignores goal values!
            bool foundPath = false;
            foreach(Action action in usableActions)
            {
                if (action.IsAchievableGiven(parent.state))
                {
                    Dictionary<string, int> currentState = new Dictionary<string, int>(parent.state);
                    foreach(KeyValuePair<string, int> eff in action.effects)
                    {
                        if (!currentState.ContainsKey(eff.Key))
                            currentState.Add(eff.Key, eff.Value);
                    }
                    Node node = new Node(parent, parent.cost + action.cost, currentState, action);
                    if(GoalAchieved(goal, currentState))
                    {
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

        bool GoalAchieved(Dictionary<string, int> goal, Dictionary<string, int> state)
        {
            foreach(KeyValuePair<string, int> g in goal)
            {
                if (!state.ContainsKey(g.Key))
                {
                    return false;
                }
            }
            return true;
        }
        List<Action> ActionSubset(List<Action> actions, Action removeMe)
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