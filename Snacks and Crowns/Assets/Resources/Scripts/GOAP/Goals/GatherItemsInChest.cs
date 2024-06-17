using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP {
    public class GatherItemsInChest : Goal
    {
        public GameObject chestObject;
        Chest chest;
        public List<Item> desiredItems;
        float defaultPriority = 5;
        bool active = false;
        protected virtual void Start()
        {
            chest = chestObject.GetComponent<Chest>();

        }
        public void SetDesiredChest(Chest newChest)
        {
            chest = newChest;
        }
        public void SetDesiredItems(List<Item> newDesiredItems)
        {
            desiredItems = newDesiredItems;
        }
        public override bool CompletedByState(WorldState state) //If more of desired item in chest then there is curently, then returns true
        {
            // bug here i think
            return CloserToGoalCheck(state);
        }
        public bool CloserToGoalCheck(WorldState state)
        {
            Dictionary<Chest, List<int>> chests = state.chests;

            if (chest == null) return false;
            List<int> chestItems = chests[chest];

            int originalItemCount = 0;
            int newItemCount = 0;

            List<Item> tempDesiredItems1 = new List<Item>(desiredItems);
            foreach (int itemId in chestItems)
            {
                Item item = World.GetItemFromId(itemId);
                if (tempDesiredItems1.Contains(item))
                {   
                    newItemCount++;
                    tempDesiredItems1.Remove(item);
                    //Debug.Log("---- item in plan chest: " + item.name);
                }
            }




            List<Item> tempDesiredItems2 = new List<Item>(desiredItems);
            Inventory currentChestInventory = chest.GetComponent<Inventory>();
            foreach (Item item in currentChestInventory.Items)
            {
                if (tempDesiredItems2.Contains(item))
                {
                    originalItemCount++;
                    tempDesiredItems2.Remove(item);
                }
            }
            //Debug.Log("Goal closer to completion: " + (originalItemCount < newItemCount));
            return originalItemCount < newItemCount;

        }
        public override void Tick()
        {

        }

        protected bool IsCompleted()
        {


            // we get a copy of chest items
            Inventory currentChestInventory = chest.GetComponent<Inventory>();
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
            
            List<Item> tempDesiredItems1 = new List<Item>(desiredItems);
            Inventory currentChestInventory = chest.GetComponent<Inventory>();
            foreach (Item item in currentChestInventory.Items)
            {
                if (tempDesiredItems1.Contains(item))
                {
                    similarityCount--;
                    tempDesiredItems1.Remove(item);
                }
            }

            //Debug.Log("Priority is: "+similarityCount);
            return similarityCount;
        }
        /*
        public override bool CanRun()
        {
            WorldState state = World.Instance.GetWorld();
            //List<(int, Vector3)> itemDrops = (List<(int, Vector3)>)state.GetStates()["ItemDropList"];
            //foreach((int, Vector3) pair in itemDrops)
            //{
            //    if (pair.Item1 == World.GetIdFromItem(iron)) return true;
            //}

            TagSystem[] tagSystems = FindObjectsOfType<TagSystem>();
            foreach (TagSystem tagSys in tagSystems)
            {
                if (tagSys.HasTag("Iron Ore Mine")) return true;
            }
            return false;
        }*/
    }
}