using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
namespace GOAP
{
    /// <summary>
    /// Puts an item inside of a chest
    /// </summary>
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
    public class PutItemInChest : NPCAction
    {
        CraftingRecipes craftingRecipes;
        ActionDataPutItemInChest planingData;
        FillAnInventory fillInventoryGoal;
        public override void Awake()
        {
            speechBubbleType = SpeechBubbleTypes.GetItem;
            base.Awake();
        }
        public override void Start()
        {
            craftingRecipes = GameObject.Find("Crafting Recipes").GetComponent<CraftingRecipes>();
            fillInventoryGoal = GetComponent<FillAnInventory>();
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
            if (GetComponent<FillAnInventory>() == null) return false;
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
            foreach (int item in inventory)
            {
                itemsToProcess.Add(item);
            }
            foreach (ItemPickup itemPickup in parent.state.itemPickups)
            {
                int itemId = World.GetIdFromItem(itemPickup.item);
                itemsToProcess.Add(itemId);
                
            }
            foreach (int itemId in parent.state.virtualItemPickups)
            {
                itemsToProcess.Add(itemId);
            }

            foreach (CraftingRecipe recipe in craftingRecipes.craftingRecipes)
            {
                itemsToProcess.Add(World.GetIdFromItem(recipe.result));
            }

            itemsToProcess = itemsToProcess.Distinct().ToList();

            foreach (int itemId in itemsToProcess)
            {
                if (!fillInventoryGoal.desiredItems.Contains(World.GetItemFromId(itemId)))
                    continue;

                Item item = World.GetItemFromId(itemId);
                Node nodeParent = parent;
                if (!inventory.Contains(itemId))
                {
                    // Without chest is used to prevent item bouncing between resource harvesters
                    nodeParent = GetRequiredItem(parent, item);
                    if (nodeParent == null)
                        continue;
                }
                List<GameObject> keyList = new List<GameObject>(nodeParent.state.inventories.Keys);
                GameObject targetChest = fillInventoryGoal.GetDesiredChest();

                // Code replacing the code bellow, this code only checks our target chest, not all chests
                if (targetChest.GetComponent<Inventory>().GetCapacity() < nodeParent.state.inventories.Count)
                    continue;

                WorldState possibleWorldState = new WorldState(nodeParent.state);
                possibleWorldState.CopyChestInventory(targetChest);
                possibleWorldState.CopyInventory();


                possibleWorldState.myInventory.Remove(itemId);
                possibleWorldState.inventories[targetChest].Add(itemId);

                Node node = new Node(nodeParent, 1, possibleWorldState, GetComponent<PutItemInChest>(), new ActionDataPutItemInChest(targetChest, item));
                possibleNodes.Add(node);


            }
            return possibleNodes;
        }
    }
}