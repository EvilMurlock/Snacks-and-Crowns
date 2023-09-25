using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    public class Planner
    {
        int maxDepth = 4;
        float maxCost = 20;
        bool breathFirst = true;
        public Queue<Node> Plan(List<Action> actions, Goal goal, WorldState states)
        {            
            List<Node> leaves = new List<Node>();
            Node start = new Node(null, 0, states, null, null);

            Node lastPlanNode = null;
            if (breathFirst)
            {
                List<Node> leavesPlan = new List<Node>();
                leavesPlan.Add(start);

                lastPlanNode = BuildGraphBreathFirst(leavesPlan, actions, goal, 1);
                if (lastPlanNode == null)
                {
                    Debug.Log("No GOAP plan!");
                    return null;
                }
                Debug.Log(lastPlanNode.action.actionName);
            }
            else
            {
                bool success = BuildGraph(start, leaves, actions, goal, 1);

                if (!success)
                {
                    Debug.Log("No GOAP plan!");
                    return null;
                }
                Node cheapest = null;
                foreach (Node leaf in leaves)
                {
                    if (cheapest == null) cheapest = leaf;
                    else if (leaf.cost < cheapest.cost) cheapest = leaf;
                }
                lastPlanNode = cheapest;
            }
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
            planDebug += " | Total Cost: " + lastPlanNode.cost;
            Debug.Log(planDebug);
            return queue;
        }
        Node BuildGraphBreathFirst(List<Node> leaves, List<Action> usableActions, Goal goal, int depth)
        {
            if (depth > maxDepth) return null;
            if (leaves.Count == 0) return null;

            List<Node> newLeaves = new List<Node>();
            foreach(Node parent in leaves)
            {
                foreach (Action action in usableActions)
                {
                    if (action.IsAchievableGiven(parent.state))
                    {
                        List<Node> possibleNewStates = new List<Node>();
                        possibleNewStates = action.OnActionCompleteWorldStates(parent);
                        //currentState = action.OnActionCompleteWorldStates(currentState);
                        foreach (Node node in possibleNewStates)
                        {
                            if (node.cost <= maxCost) newLeaves.Add(node);
                            //Node node = new Node(parent, parent.cost + newStateCostPair.cost, newStateCostPair.state, action);

                            if (goal.CompletedByState(node.state))
                            {
                                Debug.Log("Plan Found");
                                return node;
                            }
                        }

                    }
                }
            }
            //This code only fires when no action could furfill the goal
            List<Action> subset = new List<Action>(usableActions);

            //if (!action.reusable)
            //    subset = ActionSubset(usableActions, action); //REMOVES USED ACTIONS - PREVENTS LOOPS - ALSO PREVENTS REUSING ACTIONS

            return BuildGraphBreathFirst(newLeaves, subset, goal, depth + 1);
        }

        bool EqualStates(WorldState a, WorldState b)
        {

            //Invnetory similarity
            List<int> aInventory = (List<int>)a.GetStates()["Inventory"];
            List<int> bInventory = (List<int>)b.GetStates()["Inventory"];
            foreach(int i in aInventory)
            {
                if (bInventory.Contains(i)) bInventory.Remove(i);
                else return false;
            }
            //Item Drop similarity
            List<(int itemId, Vector3 position)> aItemDrops = new List<(int itemId, Vector3 position)>( (List<(int itemId, Vector3 position)>)a.GetStates()["ItemDropList"]);
            List<(int itemId, Vector3 position)> bItemDrops = new List<(int itemId, Vector3 position)>( (List<(int itemId, Vector3 position)>)b.GetStates()["ItemDropList"]);
            foreach ((int itemId, Vector3 position) aPair in aItemDrops)
            {
                if (bItemDrops.Contains(aPair)) bItemDrops.Remove(aPair);
                else return false;
            }

            //Chest Inventory similarity
            List<(Interactible_Chest, List<int>)> aChestList = new List<(Interactible_Chest, List<int>)>((List<(Interactible_Chest, List<int>)>)a.GetStates()["ChestList"]);
            List<(Interactible_Chest, List<int>)> bChestList = new List<(Interactible_Chest, List<int>)>((List<(Interactible_Chest, List<int>)>)b.GetStates()["ChestList"]);
            List<List<int>> aChestInventoryList = new List<List<int>>();
            List<List<int>> bChestInventoryList = new List<List<int>>();

            foreach ((Interactible_Chest, List<int>) pair in aChestList)
            {
                List<int> tempList = new List<int>();
                foreach(int i in pair.Item2)
                {
                    tempList.Add(i);
                }
                aChestInventoryList.Add(tempList);
            }
            foreach ((Interactible_Chest, List<int>) pair in aChestList)
            {
                List<int> tempList = new List<int>();
                foreach (int i in pair.Item2)
                {
                    tempList.Add(i);
                }
                bChestInventoryList.Add(tempList);
            }

            //COMPARE EACH INVENTORY IN EACH CHEST
            foreach (List<> i in aInventory)
            {
                if (bInventory.Contains(i)) bInventory.Remove(i);
                else return false;
            }


            return true;

        }
        bool BuildGraph(Node parent, List<Node> leavesCompleteGoal, List<Action> usableActions, Goal goal, int depth)
        {
            if (depth > maxDepth) return false;
            if (parent.cost > maxCost) return false; 
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
                            leavesCompleteGoal.Add(node);
                            foundPath = true;
                        }
                        else
                        {
                            List<Action> subset = new List<Action>(usableActions);

                            if(!action.reusable)
                                subset = ActionSubset(usableActions, action); //REMOVES USED ACTIONS - PREVENTS LOOPS - ALSO PREVENTS REUSING ACTIONS
                            bool found = BuildGraph(node, leavesCompleteGoal, subset, goal, depth+1);
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