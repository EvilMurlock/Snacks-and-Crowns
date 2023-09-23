using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP {
    public class GatherItemsInChest : Goal
    {
        public GameObject chestOb;
        public Interactible_Chest chest;
        public List<Item> desiredItems;
        float defaultPriority = 5;
        bool active = false;
        AgentBelieveState agentBelieves;
        protected virtual void Start()
        {
            chest = chestOb.GetComponent<Interactible_Chest>();

        }
        public override bool CompletedByState(WorldState state) //If more of desired item in chest then there is curently, then returns true
        {
            return CloserToGoalCheck(state);
        }
        public bool CloserToGoalCheck(WorldState state)
        {
            List<(Interactible_Chest, List<int>)> chests = (List<(Interactible_Chest, List<int>)>)state.GetStates()["ChestList"];

            (Interactible_Chest, List<int>) pair = chests.Find(x => x.Item1 == chest);
            if (pair.Item1 == null) return false;

            int originalItemCount = 0;
            int newItemCount = 0;

            List<Item> tempDesiredItems1 = new List<Item>(desiredItems);
            foreach (int itemId in pair.Item2)
            {
                Item item = World.GetItemFromId(itemId);
                if (tempDesiredItems1.Contains(item))
                {   
                    newItemCount++;
                    tempDesiredItems1.Remove(item);
                }
            }
            List<Item> tempDesiredItems2 = new List<Item>(desiredItems);
            foreach (Item_Slot item in chest.chest_inventory)
            {
                if (tempDesiredItems2.Contains(item.item))
                {
                    originalItemCount++;
                    tempDesiredItems2.Remove(item.item);
                }
            }

            return originalItemCount < newItemCount;

        }
        public override void Tick()
        {

        }
        public override void Activate()
        {
            active = true;
        }
        public override void Deactivate()
        {
            active = false;
        }

        public override void Complete()
        {
            Debug.Log("Plan " + this.GetType().ToString() + " Completed");
            active = false;
        }
        public override float CalculatePriority()
        {
            float priority = defaultPriority;


            if (chest == null) return -1;

            priority = HowCloseToFillingTheChest();


            if (active) priority *= 2;


            return priority;
        }

        protected int HowCloseToFillingTheChest()
        {
            int similarityCount = desiredItems.Count;

            List<Item> tempDesiredItems1 = new List<Item>(desiredItems);
            foreach (Item_Slot itemSlot in chest.chest_inventory)
            {
                Item item = itemSlot.item;
                if (tempDesiredItems1.Contains(item))
                {
                    similarityCount--;
                    tempDesiredItems1.Remove(item);
                }
            }

            //Debug.Log("Priority is: "+similarityCount);
            return similarityCount;
        }
    }
}