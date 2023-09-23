using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP 
{
    public class GatherIronOre : GatherItemsInChest
    {
        Item iron;
        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            iron = (Item)Resources.Load("Items/Iron Ore");
            Debug.Log("Chest size: "+chest.chest_inventory.Length);
            for (int i = 0; i<= chest.chest_inventory.Length; i++)
            {
                Debug.Log("Fired");
                desiredItems.Add(iron);
            }
        }
        public override bool CanRun()
        {
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
            return false;
        }
    }
}