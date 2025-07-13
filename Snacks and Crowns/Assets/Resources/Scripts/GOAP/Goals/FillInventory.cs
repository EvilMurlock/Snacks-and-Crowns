using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP {
    public class FillAnInventory : Goal
    {
        public GameObject targetObject;
        public List<Item> desiredItems;
        float defaultPriority = 5;
        public override void Start()
        {
            MaxPlanDepth = 3;
        }
        public void Initialize(GameObject targetObject, List<Item> desiredItems)
        {
            this.targetObject = targetObject;
            this.desiredItems = desiredItems;
            
        }
        public void SetDesiredChest(GameObject newTarget)
        {
            targetObject = newTarget;
        }
        public GameObject GetDesiredChest()
        {
            return targetObject;
        }
        public void SetDesiredItems(List<Item> newDesiredItems)
        {
            desiredItems = newDesiredItems;
        }
        public override bool CompletedByState(WorldState state) //If more of desired item in chest then there is currently, then returns true
        {
            // bug here i think
            return CloserToGoalCheck(state);
        }
        public bool CloserToGoalCheck(WorldState state)
        {
            Dictionary<GameObject, List<int>> inventories = state.inventories;

            if (targetObject == null) return false;
            List<int> chestItems = inventories[targetObject];

            int originalItemCount = 0; // number of items in the chest currently
            int newItemCount = 0; // number of items in the chest at this state of the plan

            List<Item> tempDesiredItems1 = new List<Item>(desiredItems);
            foreach (int itemId in chestItems)
            {
                Item item = World.GetItemFromId(itemId);
                if (tempDesiredItems1.Contains(item))
                {   
                    newItemCount++;
                    tempDesiredItems1.Remove(item);
                }
            }




            List<Item> tempDesiredItems2 = new List<Item>(desiredItems);
            Inventory currentChestInventory = targetObject.GetComponent<Inventory>();
            foreach (Item item in currentChestInventory.Items)
            {
                if (tempDesiredItems2.Contains(item))
                {
                    originalItemCount++;
                    tempDesiredItems2.Remove(item);
                }
            }

            return originalItemCount < newItemCount;

        }
        public override void Tick()
        {

        }

        protected bool IsCompleted()
        {


            // we get a copy of chest items
            Inventory currentChestInventory = targetObject.GetComponent<Inventory>();
            List<Item> chestItems = new List<Item>(currentChestInventory.Items);

            // we check if we have all required items
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
            active = false;
        }
        public override float CalculatePriority()
        {
            if (targetObject == null) return -1;
            if (IsCompleted()) return -1;
            float priority = defaultPriority;



            priority = HowCloseToFillingTheChest();

            if (active) priority *= 2;

            return priority;
        }

        protected int HowCloseToFillingTheChest()
        {
            int similarityCount = desiredItems.Count;
            
            List<Item> tempDesiredItems1 = new List<Item>(desiredItems);
            Inventory currentChestInventory = targetObject.GetComponent<Inventory>();
            foreach (Item item in currentChestInventory.Items)
            {
                if (tempDesiredItems1.Contains(item))
                {
                    similarityCount--;
                    tempDesiredItems1.Remove(item);
                }
            }

            return similarityCount;
        }
    }
}