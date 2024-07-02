using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    class ActionDataCraftItem : ActionData
    {
        public CraftingRecipe recipe;
        public GameObject craftingPiece;

        public ActionDataCraftItem(CraftingRecipe recipe, GameObject craftingPiece)
        {
            this.recipe = recipe;
            this.craftingPiece = craftingPiece;
        }
    }
    public class CraftItem : SubAction
    {
        CraftingRecipe currentRecepy;
        public override void Awake()
        {
            speachBubbleType = SpeachBubbleTypes.GetItem;
            base.Awake();
        }
        public override void Start()
        {
            base.Start();
        }
        public override void Tick()
        {
            if (target == null) Deactivate();
            if (npcAi.reachedEndOfPath) Complete();
        }
        public override void Activate(ActionData arg)
        {
            ActionDataCraftItem data = (ActionDataCraftItem)arg;

            target = data.craftingPiece;
            currentRecepy = data.recipe;
            //Debug.Log("Going to craft a: " + data.recipe.result.itemName);

            npcAi.ChangeTarget(target, 1f);
            base.Activate(arg);
        }
        public override void Deactivate()
        {
            //Unequip axe
            running = false;
            npcAi.ChangeTarget(null);
        }
        public override void Complete()
        {
            // Do the crafting
            Inventory inventory = GetComponent<Inventory>();
            foreach(Item item in currentRecepy.ingredients)
            {
                inventory.RemoveItem(item);
            }
            inventory.AddItem(currentRecepy.result);

            running = false;
            completed = true;
        }
        public override bool IsAchievableGiven(WorldState worldState)//For the planner
        {
            bool achievable = false;
            foreach (TagSystem tagSys in GameObject.FindObjectsByType<TagSystem>(FindObjectsSortMode.None))
            {
                if (tagSys.HasTag("forge") || tagSys.HasTag("workshop") || tagSys.HasTag("anvil")) achievable = true;
            }

            return achievable;
        }

        public override List<Node> OnActionCompleteWorldStates(Node parent_)//Tells the planer how the world state will change on completion
        {
            List<Node> possibleNodes = new List<Node>();
            /*
            Vector3 myPosition = (Vector3)parent_.state.GetStates()["MyPosition"];
            List<int> inventory = (List<int>)parent_.state.GetStates()["Inventory"];

            List<Item> inventoryItems = new List<Item>();
            foreach( int i in inventory)
            {
                inventoryItems.Add(World.GetItemFromId(i));
            }

            foreach(Crafting_Recepy craftingRecepy in craftingRecepyList)
            {
                Node parent = parent_;
                if (GetClosestCraftingObject(craftingRecepy ,myPosition) == null) continue;

                if (!craftingRecepy.CanCraftFrom(inventoryItems))
                {
                    List<Item> requiredItems = new List<Item>(craftingRecepy.ingredients);
                    foreach(int i in inventory)
                    {
                        Item item = World.GetItemFromId(i);
                        if (requiredItems.Contains(item)) requiredItems.Remove(item);
                    }
                    Node newParent = GetRequiredItems(parent, requiredItems);
                    if (newParent == null) continue;
                    parent = newParent;
                }

                WorldState possibleWorldState = parent.state.MakeReferencialDuplicate();
                List<int> newInventory = new List<int>(inventory);
                foreach(Item item in craftingRecepy.ingredients)
                {
                    newInventory.Remove(World.GetIdFromItem(item));
                }
                newInventory.Add(World.GetIdFromItem(craftingRecepy.result));

                GameObject craftingPiece = GetClosestCraftingObject(craftingRecepy, myPosition);
                Vector3 craftingPosition = craftingPiece.transform.position;
                float distance = GetDistanceBetween(craftingPosition, myPosition);

                possibleWorldState.ModifyState("Inventory", newInventory);
                possibleWorldState.ModifyState("MyPosition", craftingPosition);
                Node newNode = new Node(parent, 15 + parent.cost + distance, possibleWorldState, this, (craftingRecepy, craftingPiece));
                possibleNodes.Add(newNode);
            }
            */
            return possibleNodes;
        }

    }
}