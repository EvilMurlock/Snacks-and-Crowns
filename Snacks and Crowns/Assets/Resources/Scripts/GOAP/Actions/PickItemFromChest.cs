using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GOAP
{
    public class PickItemFromChest : Action
    {
        (Interactible_Chest chest, Item item) planingData;
        public override void Tick()
        {
            if (target == null)
            {
                Deactivate();
            }
            if (npcAi.reachedEndOfPath)
            {
                Complete();
            }
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
            return false;
            //Its a subaction

            bool achievable = true;
            List<int> items = (List<int>)worldState.GetStates()["Inventory"];
            if (items.Count == GetComponent<Inventory>().capacity) achievable = false;
            List<(Interactible_Chest, List<int>)> chests = (List<(Interactible_Chest, List<int>)>)worldState.GetStates()["ChestList"];
            if (chests.Count == 0) achievable = false;
            return achievable;
        }
        public override void Activate(object newData)
        {
            planingData = ((Interactible_Chest, Item))newData;
            target = planingData.chest.gameObject;
            running = true;
            completed = false;
            npcAi.ChangeTarget(target);

        }
        public override void Deactivate()
        {
            running = false;
        }
        public override void Complete()
        {
            if (!planingData.chest.RemoveItem(planingData.item))
            {
                Deactivate(); 
                return;
            }
            
            if (!GetComponent<Inventory>().AddItem(planingData.item))
            {
                planingData.chest.AddItem(planingData.item);
                Deactivate(); 
                return;
            }

            running = false;
            completed = true;
        }
        public override List<Node> OnActionCompleteWorldStates(Node parent)//Tells the planer how the world state will change on completion
        {
            List<Node> possibleNodes = new List<Node>();
            List<int> inventory = (List<int>)parent.state.GetStates()["Inventory"];
            List < (Interactible_Chest, List<int>)> ChestList = (List<(Interactible_Chest, List<int>)>)parent.state.GetStates()["ChestList"];
            Vector3 myPosition = (Vector3)parent.state.GetStates()["MyPosition"];

            List<int> chosenItems = new List<int>();


            foreach ((Interactible_Chest, List<int> chestInventory) chestInventoryPair in ChestList)
            {

                foreach(int item in chestInventoryPair.chestInventory)
                {
                    if (chosenItems.Contains(item)) continue;
                    chosenItems.Add(item);

                    WorldState possibleWorldState = parent.state.MakeReferencialDuplicate();

                    List<int> newInventory = new List<int>(inventory);

                    List<(Interactible_Chest, List<int>)> tempList = new List<(Interactible_Chest, List<int>)>(ChestList);
                    List<(Interactible_Chest, List<int>)> chestInventoryPairList = new List<(Interactible_Chest, List<int>)>();
                    foreach ((Interactible_Chest, List<int>) temp in tempList)
                    {
                        List<int> itemList = new List<int>(temp.Item2);
                        chestInventoryPairList.Add((temp.Item1, itemList));
                    }
                    (Interactible_Chest chest, List<int> chestInventory) pair = chestInventoryPairList.Find(x => x.Item1 == chestInventoryPair.Item1); //Perfectly coppied pair, ready to be modified into the next worldstate

                    //World state modification
                    newInventory.Add(item);
                    pair.chestInventory.Remove(item);
                    Vector3 myNewPosition = pair.Item1.transform.position;

                    //World state aplication
                    possibleWorldState.ModifyState("Inventory", newInventory);
                    possibleWorldState.ModifyState("ChestList", chestInventoryPairList);
                    possibleWorldState.ModifyState("MyPosition", myNewPosition);

                    Node node = new Node(parent, 1 + parent.cost + GetDistanceBetween(pair.chest.transform.position, myPosition), possibleWorldState, this, (chestInventoryPair.Item1, item));
                    possibleNodes.Add(node);
                }
            }                    
            return possibleNodes;
        }
    }
}