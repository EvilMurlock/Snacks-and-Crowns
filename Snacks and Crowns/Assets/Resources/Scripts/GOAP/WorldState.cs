using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GOAP
{
    [System.Serializable]
    public class WorldState
    {
        public List<ItemPickup> itemPickups;
        public List<int> virtualItemPickups = new List<int>(); // only used for planning, always starts empty
        public Dictionary<GameObject, List<int>> inventories; // chest reference, and its inventory
        public GameObject agent;
        public Vector3 myPosition;
        public List<int> myInventory;
        public List<int> myEquipment;
        //float myHealth;
        //float myGold;
        public HashSet<Goal> completedGoals = new HashSet<Goal>();

        // DEEP COPIES individual items---
        public void CopyItemPickups()
        {
            itemPickups = new List<ItemPickup>(itemPickups);
        }
        public void CopyVirtualItemPickups()
        {
            virtualItemPickups = new List<int>(virtualItemPickups);
        }
        public void CopyInventory()
        {
            myInventory = new List<int>(myInventory);
        }
        public void CopyCompletedGoals()
        {
            completedGoals = new HashSet<Goal>(completedGoals);
        }
        public void CopyChestInventory(GameObject chest)
        {
            inventories = new Dictionary<GameObject, List<int>>(inventories);
            inventories[chest] = new List<int>(inventories[chest]);
        }
        public WorldState()
        {
        }
        public WorldState( WorldState worldState)
        {
            // reference copy - shallow copy
            itemPickups = worldState.itemPickups;
            inventories = worldState.inventories;
            agent = worldState.agent;
            myPosition = worldState.myPosition;
            myInventory = worldState.myInventory;
            myEquipment = worldState.myEquipment;
            virtualItemPickups = worldState.virtualItemPickups;
            completedGoals = worldState.completedGoals;
        }
        public WorldState(GameObject agent)
        {
            this.agent = agent;
        }

        public void UpdateBelieves() 
        {
            UpdateGeneralBelieves();
            InventoryUpdate(agent.GetComponent<Inventory>());
        }
        void UpdateGeneralBelieves()
        {
            WorldState generalBelieves = World.Instance.GetWorld();
            itemPickups = generalBelieves.itemPickups;
            inventories = generalBelieves.inventories;
            myPosition = agent.transform.position;
        }
        void InventoryUpdate(Inventory inventory)
        {
            List<int> inventoryItems = new List<int>();
            foreach (Item item in inventory.Items)
            {
                if(item!=null)
                    inventoryItems.Add(World.GetIdFromItem(item));
            }
            myInventory = inventoryItems;
        }

        public void PrintMyInventory()
        {
            Debug.Log("Agent inventory");
            foreach (int i in myInventory)
            {
                Debug.Log("- - - - - " + World.GetItemFromId(i));
            }
        }
        public void PrintChestInventory(GameObject chest)
        {
            Debug.Log("Chest inventory");
            foreach (int i in inventories[chest])
            {
                Debug.Log("- - - - - " + World.GetItemFromId(i));
            }
        }
    }
}