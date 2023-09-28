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
                if (item.item_name == itemName) return item;
            }
            return null;
        }


        void UpdateWorld()
        {

            List<(int itemId, Vector3 position)> itemDrops = new List<(int, Vector3)>();
            foreach (Item_Controler itemControler in GameObject.FindObjectsByType<Item_Controler>(FindObjectsSortMode.None))
            {
                itemDrops.Add((GetIdFromItem(itemControler.item), itemControler.transform.position));
            }
            worldState.ModifyState("ItemDropList",itemDrops);

            List<(Interactible_Chest, List<int>)> chests = new List<(Interactible_Chest, List<int>)>();
            foreach (Interactible_Chest chest in GameObject.FindObjectsByType<Interactible_Chest>(FindObjectsSortMode.None))
            {
                List<int> chestInventory = new List<int>();
                foreach(Item_Slot item in chest.chest_inventory)
                {
                    if(item.Is_Not_Empty())chestInventory.Add(World.GetIdFromItem(item.item));
                }
                chests.Add((chest, chestInventory));
            }
            worldState.ModifyState("ChestList", chests);

        }
    }
}
