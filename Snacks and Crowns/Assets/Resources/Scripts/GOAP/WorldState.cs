using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GOAP
{
    [System.Serializable]
    public class WorldState
    {
        public List<ItemPickup> itemPickups;
        public Dictionary<Chest, List<int>> chests;
        public WorldState()
        {
        }
    }
}