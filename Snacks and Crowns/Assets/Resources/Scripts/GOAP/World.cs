using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GOAP
{

    /// <summary>
    /// Keeps track of some important data that are used in the WorldState
    /// </summary>
    public sealed class World
    {
        
        private static readonly World instance = new World();
        private static WorldState worldState;
        static List<Item> ItemIdList = new List<Item>();
        public static List<Item> ItemList => ItemIdList;
        float lastUpdateTime = 0f;
        float updateCooldown = 1f;

        static World()
        {
            Item[] itemArray = Resources.LoadAll<Item>("Items");
            foreach(Item item in itemArray)
            {
                ItemIdList.Add(item);
            }
            worldState = new WorldState();
        }
        public static void AddInventory(Inventory inventory)
        {
            GameObject gameObject = inventory.gameObject;
            List<int> chestInventory = new List<int>();
            foreach (Item item in inventory.GetComponent<Inventory>().Items)
            {
                if (item != null) chestInventory.Add(World.GetIdFromItem(item));
            }
            worldState.inventories[gameObject] = chestInventory;
        }
        public static void AddPickup(ItemPickup pickup)
        {
            worldState.itemPickups.Add(pickup);
        }
        private World()
        {

        }
        public static World Instance
        {
            get { return instance; }
        }
        public WorldState GetWorld()
        {
            UpdateWorld();
            return worldState;
        }
        public static Item GetItemFromId(int id)
        {
            return ItemIdList[id];
        }
        public static int GetIdFromItem(Item item)
        {
            return ItemIdList.IndexOf(item);
        }
        public static Item GetItemFromName(string itemName)
        {
            foreach(Item item in ItemIdList)
            {
                if (item.itemName == itemName) return item;
            }
            return null;
        }

        List<ItemPickup> LoadItemPickups()
        {
            List<ItemPickup> itemPickups = new List<ItemPickup>();
            
            foreach (ItemPickup itemPickup in GameObject.FindObjectsByType<ItemPickup>(FindObjectsSortMode.None))
            {
                itemPickups.Add(itemPickup);
            }
            return itemPickups;
        }


        // was used to load all chests and shops, later we decided it would be better if each such inventory subsribed itself to this singleton, as seen in the method AddInventory
        Dictionary<GameObject, List<int>> LoadInventories()
        {
            
            Dictionary<GameObject, List<int>> inventories = new Dictionary<GameObject, List<int>>();
            
            foreach (Chest chest in GameObject.FindObjectsByType<Chest>(FindObjectsSortMode.None))
            {
                GameObject gameObject = chest.gameObject;
                List<int> chestInventory = new List<int>();
                foreach (Item item in chest.GetComponent<Inventory>().Items)
                {
                    if (item != null) chestInventory.Add(World.GetIdFromItem(item));
                }
                inventories[gameObject] = chestInventory;
            }
            foreach (Shop shop in GameObject.FindObjectsByType<Shop>(FindObjectsSortMode.None))
            {
                GameObject gameObject = shop.gameObject;
                List<int> chestInventory = new List<int>();
                foreach (Item item in shop.GetComponent<Inventory>().Items)
                {
                    if (item != null) chestInventory.Add(World.GetIdFromItem(item));
                }
                inventories[gameObject] = chestInventory;
            }
            return inventories;
        }
        void UpdateInventories()
        {
            Dictionary<GameObject, List<int>> updatedInventories = new Dictionary<GameObject, List<int>>();
            foreach ( GameObject key in worldState.inventories.Keys)
            {
                List<int> chestInventory = new List<int>();
                foreach (Item item in key.GetComponent<Inventory>().Items)
                {
                    if (item != null) chestInventory.Add(World.GetIdFromItem(item));
                }
                updatedInventories[key] = chestInventory;
            }
            worldState.inventories = updatedInventories;
        }
        void UpdateWorld()
        {            
            if (Time.timeSinceLevelLoad > lastUpdateTime + updateCooldown)
            {
                UpdateInventories();
                lastUpdateTime = Time.timeSinceLevelLoad;
                worldState.ClearNullElements();
                worldState.itemPickups = LoadItemPickups();
            }
        }
    }
}
