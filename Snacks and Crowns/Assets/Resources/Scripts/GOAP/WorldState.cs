using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
namespace GOAP
{
    /// <summary>
    /// Holds the word state used for planning
    /// </summary>
    [System.Serializable]
    public class WorldState
    {
        static float maxPickupRange = 30;

        public List<ItemPickup> itemPickups = new List<ItemPickup>();
        public List<int> virtualItemPickups = new List<int>(); // only used for planning, always starts empty
        public Dictionary<GameObject, List<int>> inventories = new Dictionary<GameObject, List<int>>(); // chest reference, and its inventory
        public GameObject agent;
        public Vector3 myPosition;
        public List<int> myInventory;
        public List<int> myEquipment;
        public HashSet<Goal> completedGoals = new HashSet<Goal>();

        /// <summary>
        /// DEEP COPIES individual fields---, "DEEP" means that we copy references inside of the lists, but don't duplicate the actual items
        /// </summary>
        public void CopyItemPickups()
        {
            itemPickups = new List<ItemPickup>(itemPickups);
        }
        /// <summary>
        /// DEEP COPIES individual fields---, "DEEP" means that we copy references inside of the lists, but don't duplicate the actual items
        /// </summary>
        public void CopyVirtualItemPickups()
        {
            virtualItemPickups = new List<int>(virtualItemPickups);
        }
        /// <summary>
        /// DEEP COPIES individual fields---, "DEEP" means that we copy references inside of the lists, but don't duplicate the actual items
        /// </summary>
        public void CopyInventory()
        {
            myInventory = new List<int>(myInventory);
        }
        /// <summary>
        /// DEEP COPIES individual fields---, "DEEP" means that we copy references inside of the lists, but don't duplicate the actual items
        /// </summary>
        public void CopyCompletedGoals()
        {
            completedGoals = new HashSet<Goal>(completedGoals);
        }
        /// <summary>
        /// DEEP COPIES individual fields---, "DEEP" means that we copy references inside of the lists, but don't duplicate the actual items
        /// </summary>
        public void CopyChestInventory(GameObject chest)
        {
            inventories = new Dictionary<GameObject, List<int>>(inventories);
            inventories[chest] = new List<int>(inventories[chest]);
        }
        public WorldState()
        {
        }
        /// <summary>
        /// Shallow copies all fields
        /// </summary>
        /// <param name="worldState"></param>
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
            RemoveInventoriesFromWrongFaction();
            RemoveItemFarAwayPickups();
        }
        void RemoveInventoriesFromWrongFaction()
        {
            Factions faction = agent.GetComponent<FactionMembership>().Faction;
            Dictionary<GameObject, List<int>> newInventories = new Dictionary<GameObject, List<int>>();
            foreach (GameObject inventoryObject in inventories.Keys)
            {
                FactionMembership factionMembership = inventoryObject.GetComponent<FactionMembership>();
                if (factionMembership == null)
                    continue;
                else if (faction == factionMembership.Faction
                    || FactionState.GetFactionRelations(faction, factionMembership.Faction) == Relations.Alliance)
                    { 
                        newInventories[inventoryObject] = inventories[inventoryObject];
                        CopyChestInventory(inventoryObject); 
                    }
            }
            inventories = newInventories;
        }
        void RemoveItemFarAwayPickups()
        {
            for(int i = itemPickups.Count -1; i >=0; i--)
            {
                if (itemPickups[i] == null)
                    itemPickups.RemoveAt(i);
                else if (DistanceCalculator.CalculateDistance(itemPickups[i].transform.position, agent.transform.position) > maxPickupRange)
                    itemPickups.RemoveAt(i);
            }
        }
        public void ClearNullElements()
        {
            ClearNullItemPickups();
            ClearNullInventories();
        }
        void ClearNullItemPickups()
        {
            for(int i = itemPickups.Count-1; i >= 0; i--)
            {
                if (itemPickups[i] == null)
                    itemPickups.RemoveAt(i);
            }
        }
        void ClearNullInventories()
        {
            // no need to do anything for now
        }
        void UpdateGeneralBelieves()
        {
            WorldState generalBelieves = World.Instance.GetWorld();
            itemPickups = generalBelieves.itemPickups;
            CopyItemPickups();
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