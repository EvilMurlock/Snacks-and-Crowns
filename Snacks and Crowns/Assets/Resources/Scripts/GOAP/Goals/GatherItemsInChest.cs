using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP {
    public class GatherItemsInChest : Goal
    {
        public GameObject chestOb;
        public Chest chest;
        public List<Item> desiredItems;
        float defaultPriority = 5;
        bool active = false;
        protected virtual void Start()
        {
            chest = chestOb.GetComponent<Chest>();

        }
        public override bool CompletedByState(WorldState state) //If more of desired item in chest then there is curently, then returns true
        {
            return CloserToGoalCheck(state);
        }
        public bool CloserToGoalCheck(WorldState state)
        {
            Dictionary<Chest, List<int>> chests = state.chests;

            List<int> items = chests[chest];
            if (chest == null) return false;

            int originalItemCount = 0;
            int newItemCount = 0;

            List<Item> tempDesiredItems1 = new List<Item>(desiredItems);
            foreach (int itemId in items)
            {
                Item item = World.GetItemFromId(itemId);
                if (tempDesiredItems1.Contains(item))
                {   
                    newItemCount++;
                    tempDesiredItems1.Remove(item);
                }
            }/*
            List<Item> tempDesiredItems2 = new List<Item>(desiredItems);
            foreach (ItemSlot item in chest.chest_inventory)
            {
                if (tempDesiredItems2.Contains(item.GetItem()))
                {
                    originalItemCount++;
                    tempDesiredItems2.Remove(item.GetItem());
                }
            }*/

            return originalItemCount < newItemCount;

        }
        public override void Tick()
        {

        }

        protected bool IsCompleted()
        {
            
            List<Item> chestItems = new List<Item>();
            /*
            foreach (ItemSlot itemSlot in chest.chest_inventory)
            {
                chestItems.Add(itemSlot.GetItem());
            }*/
            bool goalDone = true;
            foreach (Item item in desiredItems)
            {
                if (chestItems.Contains(item)) chestItems.Remove(item);
                else goalDone = false;
            }
            return goalDone;
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
            if (IsCompleted()) return -1;
            float priority = defaultPriority;


            if (chest == null) return -1;

            priority = HowCloseToFillingTheChest();


            if (active) priority *= 2;


            return priority;
        }

        protected int HowCloseToFillingTheChest()
        {
            int similarityCount = desiredItems.Count;
            /*
            List<Item> tempDesiredItems1 = new List<Item>(desiredItems);
            foreach (ItemSlot itemSlot in chest.chest_inventory)
            {
                Item item = itemSlot.GetItem();
                if (tempDesiredItems1.Contains(item))
                {
                    similarityCount--;
                    tempDesiredItems1.Remove(item);
                }
            }*/

            //Debug.Log("Priority is: "+similarityCount);
            return similarityCount;
        }
    }
}