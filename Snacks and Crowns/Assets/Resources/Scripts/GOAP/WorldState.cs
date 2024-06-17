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
        public Dictionary<Chest, List<int>> chests; // chest reference, and its inventory
        public GameObject agent;
        public Vector3 myPosition;
        public List<int> myInventory;
        public List<int> myEquipment;
        //float myHealth;
        //float myGold;


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
        public void CopyChestInventory(Chest chest)
        {
            chests = new Dictionary<Chest, List<int>>(chests);
            chests[chest] = new List<int>(chests[chest]);
        }
        public WorldState()
        {
        }
        public WorldState( WorldState worldState)
        {
            // reference copy - shallow copy
            itemPickups = worldState.itemPickups;
            chests = worldState.chests;
            agent = worldState.agent;
            myPosition = worldState.myPosition;
            myInventory = worldState.myInventory;
            myEquipment = worldState.myEquipment;
            virtualItemPickups = worldState.virtualItemPickups;
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
            chests = generalBelieves.chests;
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
        public void PrintChestInventory(Chest chest)
        {
            Debug.Log("Chest inventory");
            foreach (int i in chests[chest])
            {
                Debug.Log("- - - - - " + World.GetItemFromId(i));
            }
        }
    }
}