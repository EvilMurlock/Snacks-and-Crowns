using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GOAP
{
    public class ActionDataPutItemInChest : ActionData
    {
        public Chest chest;
        public Item item;
        public ActionDataPutItemInChest(Chest chest, Item item)
        {
            this.chest = chest;
            this.item = item;
        }
    }
    public class PutItemInChest : Action
    {
        ActionDataPutItemInChest planingData;
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
            bool achievable = true;
            return achievable;
        }
        public override void Activate(ActionData newData)
        {
            planingData = (ActionDataPutItemInChest)newData;
            target = planingData.chest.gameObject;
        }
        public override void Deactivate()
        {
            running = false;
        }
        public override void Complete()
        {
            Inventory agentInventory = GetComponent<Inventory>();
            Inventory chestInventory = planingData.chest.GetComponent<Inventory>();
            if (!agentInventory.HasItem(planingData.item)) 
            { 
                Deactivate();
                return;
            }
            if (!chestInventory.HasEmptySpace(1))
            {
                Deactivate();
                return;
            }

            agentInventory.RemoveItem(planingData.item);
            chestInventory.AddItem(planingData.item);
            
            running = false;
            completed = true;
        }
        public override List<Node> OnActionCompleteWorldStates(Node parent_)//Tells the planer how the world state will change on completion
        {
            // IN THIS ACTION WE DONT JUST PUT AN ITEM FROM OUR INVENTORY, BUT ANY ITEM PICK UP INTO THE CHEST
            Node parent = parent_;
            List<Node> possibleNodes = new List<Node>();

            List<int> inventory = parent.state.myInventory;
            List<ItemPickup> itemDropList = parent.state.itemPickups;

            List<int> itemsToProcess = new List<int>();
            
            foreach(int item in inventory)//DOESNT WORK, WILL CHOOSE THE ITEM FROM A CHEST
            {
                if (!itemsToProcess.Contains(item)) itemsToProcess.Add(item);
            }
            foreach(ItemPickup itemPickup in itemDropList)
            {
                int itemId = World.GetIdFromItem(itemPickup.item);
                if (!itemsToProcess.Contains(itemId)) 
                    itemsToProcess.Add(itemId);
            }

            foreach (int itemId in itemsToProcess)
            {
                parent = parent_;
                Item item = World.GetItemFromId(itemId);

                if (!inventory.Contains(itemId))
                {
                    parent = GetRequiredItemNoChest(parent, item);
                }


            }

            
            return possibleNodes;
        }
    }
}