using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GOAP
{
    public sealed class World
    {
        private static readonly World instance = new World();
        private static WorldState worldState;
        static World()
        {
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
        void UpdateWorld()
        {

            List<Item_Controler> itemDrops = new List<Item_Controler>();
            foreach (Item_Controler itemControler in GameObject.FindObjectsByType<Item_Controler>(FindObjectsSortMode.None))
            {
                itemDrops.Add(itemControler);
            }
            worldState.ModifyState("ItemDropList",itemDrops);

            List<(Interactible_Chest, List<Item>)> chests = new List<(Interactible_Chest, List<Item>)>();
            foreach (Interactible_Chest chest in GameObject.FindObjectsByType<Interactible_Chest>(FindObjectsSortMode.None))
            {
                List<Item> chestInventory = new List<Item>();
                foreach(Item_Slot item in chest.chest_inventory)
                {
                    if(item.Is_Not_Empty())chestInventory.Add(item.item);
                }
                chests.Add((chest, chestInventory));
            }
            worldState.ModifyState("ChestList", chests);

        }
    }
}
