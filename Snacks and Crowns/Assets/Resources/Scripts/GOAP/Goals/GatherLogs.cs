using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP 
{
    public class GatherLogs : GatherItemsInChest
    {
        Item log;
        // Start is called before the first frame update
        protected override void Start()
        {
            /*
            base.Start();
            log = (Item)Resources.Load("Items/Log");
            for (int i = 0; i<= chest.chest_inventory.Length; i++)
            {
                desiredItems.Add(log);
            }*/
        }
        public override bool CanRun()
        {
            WorldState state = World.Instance.GetWorld();
            List<ItemPickup> itemPickups = state.itemPickups;
            foreach (ItemPickup pickUp in itemPickups)
            {
                if (pickUp.item == log) return true;
            }

            TagSystem[] tagSystems = FindObjectsOfType<TagSystem>();
            foreach (TagSystem tagSys in tagSystems)
            {
                if (tagSys.HasTag("Tree")) return true;
            }
            return false;
        }
    }
}