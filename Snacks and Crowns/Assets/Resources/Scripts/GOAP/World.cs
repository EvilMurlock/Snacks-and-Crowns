using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GOAP
{
    public sealed class World
    {
        private static readonly World instance = new World();
        private static WorldState worldState;
        static List<Item> ItemIdList = new List<Item>();
        static World()
        {
            Item[] itemArray = Resources.LoadAll<Item>("Items");
            foreach(Item item in itemArray)
            {
                ItemIdList.Add(item);
            }
            worldState = new WorldState();
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


        void UpdateWorld()
        {
            /*
            List<(int itemId, Vector3 position)> itemDrops = new List<(int, Vector3)>();
            foreach (ItemPickup itemControler in GameObject.FindObjectsByType<ItemPickup>(FindObjectsSortMode.None))
            {
                itemDrops.Add((GetIdFromItem(itemControler.item), itemControler.transform.position));
            }
            worldState.ModifyState("ItemDropList",itemDrops);

            List<(Chest, List<int>)> chests = new List<(Chest, List<int>)>();
            foreach (Chest chest in GameObject.FindObjectsByType<Chest>(FindObjectsSortMode.None))
            {
                List<int> chestInventory = new List<int>();
                foreach(ItemSlot item in chest.chest_inventory)
                {
                    if(item.IsNotEmpty())chestInventory.Add(World.GetIdFromItem(item.GetItem()));
                }
                chests.Add((chest, chestInventory));
            }
            worldState.ModifyState("ChestList", chests);*/

        }
    }
}
