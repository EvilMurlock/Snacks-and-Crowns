using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP 
{
    public class GatherWeapons : GatherItemsInChest
    {
        Item sword;
        Item hammer;
        Item spear;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            sword = (Item)Resources.Load("Items/Equipment/Sword");
            hammer = (Item)Resources.Load("Items/Equipment/Hammer");
            spear = (Item)Resources.Load("Items/Equipment/Spear");

            desiredItems.Add(sword);
            desiredItems.Add(hammer);
            desiredItems.Add(spear);
        }
        public override bool CanRun()
        {
            /*
            WorldState state = World.Instance.GetWorld();
            List<(int, Vector3)> itemDrops = (List<(int, Vector3)>)state.GetStates()["ItemDropList"];
            foreach((int, Vector3) pair in itemDrops)
            {
                if (pair.Item1 == World.GetIdFromItem(iron)) return true;
            }

            TagSystem[] tagSystems = FindObjectsOfType<TagSystem>();
            foreach (TagSystem tagSys in tagSystems)
            {
                if (tagSys.HasTag("Iron Ore Mine")) return true;
            }
            */
            return true;
        }
    }
}