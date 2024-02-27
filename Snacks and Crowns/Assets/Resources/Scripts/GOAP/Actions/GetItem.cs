using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    public class GetItem : Action
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
            return false;
        }
        Node GetItemFromDrop(Node parent, Item requestedItem)
        {
            int item = World.GetIdFromItem(requestedItem);


            //SEARCHING FOR ITEM PICKUPS
            List<int> inventory = (List<int>)parent.state.GetStates()["Inventory"];
            List<(int itemId, Vector3 position)> itemDropList = (List<(int, Vector3)>)parent.state.GetStates()["ItemDropList"];
            Vector3 myPosition = (Vector3)parent.state.GetStates()["MyPosition"];

            (int item, Vector3 position) chosenPair = (-1, new Vector3());
            float distance = -1;
            foreach ((int item, Vector3 position) iPpair in itemDropList)
            {
                if (iPpair.item != item) continue;
                if (chosenPair.item == -1 || distance > GetDistanceBetween(myPosition, iPpair.position))
                {
                    chosenPair = iPpair;
                    distance = GetDistanceBetween(myPosition, iPpair.position);
                }
            }

            if (chosenPair.item != -1)
            {
                WorldState possibleWorldState = parent.state.MakeReferencialDuplicate();

                List<int> newInventory = new List<int>(inventory);
                List<(int itemId, Vector3 position)> newItemDropList = new List<(int, Vector3)>(itemDropList);

                newInventory.Add(chosenPair.item);
                newItemDropList.Remove(chosenPair);

                possibleWorldState.ModifyState("Inventory", newInventory);
                possibleWorldState.ModifyState("ItemDropList", newItemDropList);
                possibleWorldState.ModifyState("MyPosition", chosenPair.position);

                Node node = new Node(parent, 1 + parent.cost + distance, possibleWorldState, GetComponent<PickUpItem>(), chosenPair);
                return node;
            }
            return null;
        }
        Node GetItemFromChest(Node parent, Item requestedItem)
        {
            //SEARCHING FOR ITEMS IN CHESTS2
            int item = World.GetIdFromItem(requestedItem);
            List<int> inventory = (List<int>)parent.state.GetStates()["Inventory"];
            Vector3 myPosition = (Vector3)parent.state.GetStates()["MyPosition"];

            float distance = -1;
            List<(Chest, List<int>)> ChestList = (List<(Chest, List<int>)>)parent.state.GetStates()["ChestList"];

            (Chest chest, List<int> inventory) closestChest = (null, null);
            foreach ((Chest, List<int> chestInventory) chestInventoryPair in ChestList)
            {
                if (!chestInventoryPair.chestInventory.Contains(item)) continue;
                if (closestChest.chest == null || distance > GetDistanceBetween(myPosition, closestChest.chest.transform.position))
                {
                    closestChest = chestInventoryPair;
                    distance = GetDistanceBetween(myPosition, closestChest.chest.transform.position);
                }
            }
            if (closestChest.chest != null)
            {
                WorldState possibleWorldState = parent.state.MakeReferencialDuplicate();

                List<int> newInventory = new List<int>(inventory);

                List<(Chest, List<int>)> tempList = new List<(Chest, List<int>)>(ChestList);
                List<(Chest, List<int>)> chestInventoryPairList = new List<(Chest, List<int>)>();
                foreach ((Chest, List<int>) temp in tempList)
                {
                    List<int> itemList = new List<int>(temp.Item2);
                    chestInventoryPairList.Add((temp.Item1, itemList));
                }
                (Chest chest, List<int> chestInventory) pair = chestInventoryPairList.Find(x => x.Item1 == closestChest.Item1); //Perfectly coppied pair, ready to be modified into the next worldstate

                //World state modification
                newInventory.Add(item);
                pair.chestInventory.Remove(item);
                Vector3 myNewPosition = pair.Item1.transform.position;

                //World state aplication
                possibleWorldState.ModifyState("Inventory", newInventory);
                possibleWorldState.ModifyState("ChestList", chestInventoryPairList);
                possibleWorldState.ModifyState("MyPosition", myNewPosition);

                Node node = new Node(parent, 1 + parent.cost + GetDistanceBetween(pair.chest.transform.position, myPosition), possibleWorldState, GetComponent<PickItemFromChest>(), (closestChest.chest, item));
                return node;
            }

            return null;
        }
        public Node GetItemPlan(Node parent, Item requestedItem) //Gets a specific Item
        {
            
            Node node = GetItemFromDrop(parent, requestedItem);
            if (node != null) return node;
            node = GetItemFromChest(parent, requestedItem);
            if (node != null) return node;
            return null;
            
        }
        public Node GetItemPlanNoChest(Node parent, Item requestedItem) //Gets a specific Item
        {
            Node node = GetItemFromDrop(parent, requestedItem);
            if (node != null) return node;
            return null;
        }

        public override List<Node> OnActionCompleteWorldStates(Node parent)//Tells the planer how the world state will change on completion
        {
            List<Node> possibleNodes = new List<Node>();
            return possibleNodes;
        }
    }
}