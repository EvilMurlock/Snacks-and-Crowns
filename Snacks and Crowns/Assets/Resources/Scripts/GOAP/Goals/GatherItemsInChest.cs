using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP {
    public class GatherItemsInChest : Goal
    {
        public Interactible_Chest chest;
        public Item item;
        float defaultPriority = 5;
        bool active = false;
        AgentBelieveState agentBelieves;
        private void Start()
        {
        }
        public override bool CompletedByState(WorldState state) //If more of desired item in chest then there is curently, then returns true
        {
            List<(Interactible_Chest, List<Item>)> chests = (List<(Interactible_Chest, List<Item>)>)state.GetStates()["ChestList"];

            (Interactible_Chest, List<Item>) pair = chests.Find(x => x.Item1 == chest);
            if (pair.Item1 == null) return false;

            int originalItemCount = 0;
            int newItemCount = 0;
            string planInv = "";

            foreach (Item item in pair.Item2)
            {
                if (item == this.item) {newItemCount++; planInv += item.item_name+"|"; }
            }
            foreach (Item_Slot item in chest.chest_inventory)
            {
                if (item.item == this.item) originalItemCount++;
            }
            
            Debug.Log("Complete Goal Check:    | Number of Logs in Chest: " + originalItemCount+"    | Number of Logs in Plan: " + newItemCount + " Plan inventory: "+planInv);
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
            if (chest != null)
            {
                bool full = true;
                foreach(Item_Slot slot in chest.chest_inventory)
                {
                    if (slot.item == null) full = false;
                }
                if (full) priority = 0;
            }
            else priority = 0;
            if (active) priority *= 2;
            return priority;
        }
    }
}