using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
namespace GOAP
{
    public class ActionDataPutItemInChest : ActionData
    {
        public GameObject targetObject;
        public Item item;
        public ActionDataPutItemInChest(GameObject targetObject, Item item)
        {
            this.targetObject = targetObject;
            this.item = item;
        }
    }
    public class PutItemInChest : Action
    {
        CraftingRecipes craftingRecipes;
        ActionDataPutItemInChest planingData;
        public override void Awake()
        {
            speachBubbleType = SpeachBubbleTypes.GetItem;
            base.Awake();
        }
        public override void Start()
        {
            craftingRecipes = GameObject.Find("Crafting Recipes").GetComponent<CraftingRecipes>();
            base.Start();
        }
        public override void Tick()
        {
            if (target == null)
            {
                Deactivate();
            }
            if (npcAi.reachedEndOfPath)
            {
                Complete();
            }
        }
        public override float GetCost(WorldState worldState)
        {

            return GetDistanceFromTarget();
        }
        public override bool IsUsableBy(GameObject g)
        {
            return true;
        }
        public override bool IsAchievableGiven(WorldState worldState)//For the planner
        {
            bool achievable = true;
            return achievable;
        }
        public override void Activate(ActionData newData)
        {
            planingData = (ActionDataPutItemInChest)newData;

            target = planingData.targetObject.gameObject;


            npcAi.ChangeTarget(target);
            base.Activate(newData);
        }
        public override void Deactivate()
        {
            running = false;
        }
        public override void Complete()
        {
            Inventory agentInventory = GetComponent<Inventory>();
            Inventory chestInventory = planingData.targetObject.GetComponent<Inventory>();
            if (!agentInventory.HasItem(planingData.item)) 
            { 
                Deactivate();
                return;
            }
            if (!chestInventory.HasEmptySpace(1))
            {
                Deactivate();
                return;
            }

            agentInventory.RemoveItem(planingData.item);
            chestInventory.AddItem(planingData.item);
            
            running = false;
            completed = true;
        }
        public override List<Node> OnActionCompleteWorldStates(Node parent)//Tells the planer how the world state will change on completion
        {

            // IN THIS ACTION WE DONT JUST PUT AN ITEM FROM OUR INVENTORY, BUT ANY ITEM PICK UP INTO THE CHEST
            List<Node> possibleNodes = new List<Node>();

            List<int> inventory = parent.state.myInventory;

            List<int> itemsToProcess = new List<int>();


            foreach (int item in inventory)//DOESNT WORK, WILL CHOOSE THE ITEM FROM A CHEST
            {
                //if (!itemsToProcess.Contains(item)) 
                    itemsToProcess.Add(item);
            }
            foreach (ItemPickup itemPickup in parent.state.itemPickups)
            {
                int itemId = World.GetIdFromItem(itemPickup.item);
                //if (!itemsToProcess.Contains(itemId))
                    itemsToProcess.Add(itemId);
                
            }
            foreach (int itemId in parent.state.virtualItemPickups)
            {
                //if (!itemsToProcess.Contains(itemId))
                    itemsToProcess.Add(itemId);
            }

            foreach (CraftingRecipe recipe in craftingRecipes.craftingRecipes)
            {
                itemsToProcess.Add(World.GetIdFromItem(recipe.result));
            }

            itemsToProcess = itemsToProcess.Distinct().ToList();

            foreach (int itemId in itemsToProcess)
            {

                Item item = World.GetItemFromId(itemId);
                Node nodeParent = parent;
                if (!inventory.Contains(itemId))
                {
                    nodeParent = GetRequiredItemNoChest(parent, item); // this will also try to pick up virtual items (future pick-ups)
                    if (nodeParent == null)
                        continue;
                }
                List<GameObject> keyList = new List<GameObject>(nodeParent.state.inventories.Keys);
                for (int ch = 0; ch < keyList.Count; ch++) 
                {
                    GameObject chest = keyList[ch];
                //foreach (Chest chest in nodeParent.state.chests.Keys)
                //{
                    if (chest.GetComponent<Inventory>().GetCapacity() < nodeParent.state.inventories.Count)
                        continue;

                    WorldState possibleWorldState = new WorldState(nodeParent.state);
                    possibleWorldState.CopyChestInventory(chest);
                    possibleWorldState.CopyInventory();

                    possibleWorldState.myInventory.Remove(itemId);
                    possibleWorldState.inventories[chest].Add(itemId);

                    Node node = new Node(nodeParent, 1, possibleWorldState, GetComponent<PutItemInChest>(), new ActionDataPutItemInChest(chest, item));
                    possibleNodes.Add(node);
                }
            }
            return possibleNodes;
        }
    }
}