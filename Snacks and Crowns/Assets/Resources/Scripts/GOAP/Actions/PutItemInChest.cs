using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GOAP
{
    public class PutItemInChest : Action
    {
        (Chest chest, Item item) planingData;
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
        public override void Activate(object newData)
        {
        }
        public override void Deactivate()
        {
            running = false;
        }
        public override void Complete()
        {
            if (!GetComponent<Inventory>().RemoveItem(planingData.item)) 
            { 
                Deactivate();
                return;
            }
            if (!planingData.chest.AddItem(planingData.item))
            { 
                GetComponent<Inventory>().AddItem(planingData.item); //re-adding the previously removed item into our inventory
                Deactivate(); 
                return;
            }

            running = false;
            completed = true;
        }
        public override List<Node> OnActionCompleteWorldStates(Node parent_)//Tells the planer how the world state will change on completion
        {
            Node parent = parent_;
            List<Node> possibleNodes = new List<Node>();
            List<int> inventory = (List<int>)parent.state.GetStates()["Inventory"];
            List<(int item, Vector3 position)> itemDropList = (List < (int item, Vector3 position) >)parent.state.GetStates()["ItemDropList"];

            List<int> itemsToProcess = new List<int>();
            
            foreach(int item in inventory)//DOESNT WORK, WILL CHOOSE THE ITEM FROM A CHEST
            {
                if (!itemsToProcess.Contains(item)) itemsToProcess.Add(item);
            }
            foreach((int item, Vector3 position) pair in itemDropList)
            {
                if (!itemsToProcess.Contains(pair.item)) itemsToProcess.Add(pair.item);
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