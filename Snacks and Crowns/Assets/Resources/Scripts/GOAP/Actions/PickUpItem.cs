using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    public class PickUpItem : Action
    {
        public override void Tick()
        {

        }
        public override float GetCost(WorldState worldState)
        {

            return GetDistanceFromTarget();
        }
        public override bool IsUsableBy(GameObject g)
        {
            return true;
        }
        public override bool IsAchievableGiven(WorldState worldState)//For the planner
        {
            bool achievable = true;
            List<Item> controlers = (List<Item>)worldState.GetStates()["Inventory"];
            if (controlers.Count == 0) achievable = false;
            List<(Interactible_Chest, List<Item>)> chests = (List<(Interactible_Chest, List<Item>)>)worldState.GetStates()["ChestList"];
            if (controlers.Count == 0) achievable = false;

            return achievable;
        }
        public override bool IsAchievable()//Checs curent condition + world state
        {
            return true;
        }
        public override void Activate()
        {
            target = FindTarget();
            Debug.Log("Target is now: " + target.name);
            running = true;
            completed = false;
            Debug.Log("Switching path to " + target.name);
            gameObject.GetComponent<NpcAi>().ChangeTarget(target);
        }
        public override void Deactivate()
        {
            running = false;
        }
        public override void Complete()
        {
            running = false;
            completed = true;
        }
        public override List<Node> OnActionCompleteWorldStates(Node parent)//Tells the planer how the world state will change on completion
        {



            List<Node> possibleNodes = new List<Node>();
            /*
            foreach (Item item in (List<Item>)parent.state.GetStates()["Inventory"])
            {
                foreach ((Interactible_Chest, List<Item>) chestInventoryPair in (List<(Interactible_Chest, List<Item>)>)parent.state.GetStates()["ChestList"])
                {
                    WorldState possibleWorldState = new WorldState(parent.state);

                    List<Item> inventory = new List<Item>((List<Item>)parent.state.GetStates()["Inventory"]);
                    List<(Interactible_Chest, List<Item>)> chestInventoryPairList = new List<(Interactible_Chest, List<Item>)>((List<(Interactible_Chest, List<Item>)>)parent.state.GetStates()["ChestList"]);

                    inventory.Remove(item);
                    (Interactible_Chest, List<Item>) pair = chestInventoryPairList.Find(x => x == chestInventoryPair);
                    if (pair.Item2.Count < pair.Item1.chest_inventory.Length) pair.Item2.Add(item);
                    else break;

                    possibleWorldState.ModifyState("Inventory", inventory);
                    possibleWorldState.ModifyState("ChestList", chestInventoryPairList);
                    GameObject tempTarget = chestInventoryPair.Item1.gameObject;
                    possibleNodes.Add(new Node(parent, parent.cost + GetDistanceFromObject(tempTarget), possibleWorldState, this, tempTarget));
                }
            }
            */

            return possibleNodes;
        }
    }
}