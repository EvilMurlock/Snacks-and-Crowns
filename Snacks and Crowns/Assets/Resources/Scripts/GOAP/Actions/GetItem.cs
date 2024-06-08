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
        public override void Activate(ActionData arg)
        {
            throw new System.Exception("This function should never be called, we only create nodes for other actions, so we should never activate");
        }

        Node GetItemFromDrop(Node parent, Item requestedItem)
        {
            // this will also try to pick up virtual items (future pick-ups)
            int item = World.GetIdFromItem(requestedItem);


            //SEARCHING FOR ITEM PICKUPS
            List<int> inventory = parent.state.myInventory;
            List<ItemPickup> itemPickups = parent.state.itemPickups;
            Vector3 myPosition = parent.state.myPosition;

            ItemPickup chosenPickup = null;
            float distance = -1;
            foreach (ItemPickup pickup in itemPickups)
            {
                if (World.GetIdFromItem(pickup.item) != item) continue;
                float newDistance = GetDistanceBetween(myPosition, pickup.transform.position);
                if (chosenPickup == null || distance > newDistance)
                {
                    chosenPickup = pickup;
                    distance = newDistance;
                }
            }

            if (chosenPickup != null)
            {
                WorldState possibleWorldState = new WorldState(parent.state);

                possibleWorldState.CopyInventory();
                possibleWorldState.CopyItemPickups();

                possibleWorldState.myInventory.Add(World.GetIdFromItem(chosenPickup.item));
                possibleWorldState.itemPickups.Remove(chosenPickup);

                Node node = new Node(parent, 1 + parent.cost + distance, possibleWorldState, GetComponent<PickUpItem>(), new ActionDataPickUpItem(chosenPickup));
                return node;
            }


            foreach (int virtualItem in parent.state.virtualItemPickups) // we try to pick up a virtual pickup (the game object doesnt exist, but it will in the real world after we execute the plan)
            {
                if (virtualItem == item)
                {
                    WorldState possibleWorldState = new WorldState(parent.state);

                    possibleWorldState.CopyInventory();
                    possibleWorldState.CopyVirtualItemPickups();

                    possibleWorldState.myInventory.Add(virtualItem);
                    possibleWorldState.virtualItemPickups.Remove(virtualItem);

                    // the null in node.actionData will cause a fail,
                    // but we will probably replan the same thing again, but this time the virtual item will already be instantiated,
                    // so we will be able to pick it up
                    Node node = new Node(parent, 1, possibleWorldState, GetComponent<PickUpItem>(), new ActionDataPickUpItem(null));
                    return node;
                }
            }
            return null;
        }
        Node GetItemFromChest(Node parent, Item requestedItem)
        {
            //SEARCHING FOR ITEMS IN CHESTS2
            int item = World.GetIdFromItem(requestedItem);
            List<int> inventory = parent.state.myInventory;
            Vector3 myPosition = parent.state.myPosition;

            float distance = -1;
            Dictionary<Chest, List<int>> chests = parent.state.chests;

            Chest closestChest = null;
            foreach (Chest chest in chests.Keys)
            {
                if (!chests[chest].Contains(item)) continue;
                float newDistance = GetDistanceBetween(myPosition, chest.transform.position);
                if (closestChest == null || distance > newDistance)
                {
                    closestChest = chest;
                    distance = newDistance;
                }
            }
            if (closestChest != null)
            {
                WorldState possibleWorldState = new WorldState(parent.state);

                possibleWorldState.CopyInventory();
                possibleWorldState.CopyChestInventory(closestChest);

                //World state modification
                possibleWorldState.myEquipment.Add(item);
                possibleWorldState.chests[closestChest].Remove(item);
                possibleWorldState.myPosition = closestChest.transform.position;

                Node node = new Node(parent, 1 + parent.cost + distance, possibleWorldState, GetComponent<PickItemFromChest>(), new ActionDataPickItemFromChest(closestChest, World.GetItemFromId(item)));
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