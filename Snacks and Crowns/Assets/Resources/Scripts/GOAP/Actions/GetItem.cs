using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    public class GetItem : Action
    {
        CraftingRecipes craftingRecipes;
        public override void Start()
        {
            craftingRecipes = GameObject.Find("Crafting Recipes").GetComponent<CraftingRecipes>();
            base.Start();
        }
        public override bool IsUsableBy(GameObject g)
        {
            return true;
        }
        public override bool IsAchievableGiven(WorldState worldState)//For the planner
        {
            return true;
        }
        public override void Activate(ActionData arg)
        {
            throw new System.Exception("This function should never be called, we only create nodes for other actions, so we should never activate");
        }

        Node GetItemFromDrop(Node parent, Item requestedItem)
        {
            // this will also try to pick up virtual items (future pick-ups)
            int item = World.GetIdFromItem(requestedItem);


            //SEARCHING FOR ITEM PICKUPS
            List<int> inventory = parent.state.myInventory;
            List<ItemPickup> itemPickups = parent.state.itemPickups;
            Vector3 myPosition = parent.state.myPosition;

            ItemPickup chosenPickup = null;
            float distance = -1;
            foreach (ItemPickup pickup in itemPickups)
            {
                if (World.GetIdFromItem(pickup.item) != item) continue;
                float newDistance = GetDistanceBetween(myPosition, pickup.transform.position);
                if (chosenPickup == null || distance > newDistance)
                {
                    chosenPickup = pickup;
                    distance = newDistance;
                }
            }

            if (chosenPickup != null)
            {
                WorldState possibleWorldState = new WorldState(parent.state);

                possibleWorldState.CopyInventory();
                possibleWorldState.CopyItemPickups();

                possibleWorldState.myInventory.Add(World.GetIdFromItem(chosenPickup.item));
                possibleWorldState.itemPickups.Remove(chosenPickup);

                Node node = new Node(parent, 1 + parent.cost + distance, possibleWorldState, GetComponent<PickUpItem>(), new ActionDataPickUpItem(chosenPickup));
                return node;
            }


            foreach (int virtualItem in parent.state.virtualItemPickups) // we try to pick up a virtual pickup (the game object doesnt exist, but it will in the real world after we execute the plan)
            {
                if (virtualItem == item)
                {
                    WorldState possibleWorldState = new WorldState(parent.state);

                    possibleWorldState.CopyInventory();
                    possibleWorldState.CopyVirtualItemPickups();

                    possibleWorldState.myInventory.Add(virtualItem);
                    possibleWorldState.virtualItemPickups.Remove(virtualItem);

                    // the null in node.actionData will cause a fail,
                    // but we will probably replan the same thing again, but this time the virtual item will already be instantiated,
                    // so we will be able to pick it up
                    Node node = new Node(parent, 1, possibleWorldState, GetComponent<PickUpItem>(), new ActionDataPickUpItem(virtualItem));
                    return node;
                }
            }
            return null;
        }
        Node GetItemFromChest(Node parent, Item requestedItem)
        {
            //SEARCHING FOR ITEMS IN CHESTS2
            int item = World.GetIdFromItem(requestedItem);
            Vector3 myPosition = parent.state.myPosition;

            float distance = -1;
            Dictionary<GameObject, List<int>> chests = parent.state.inventories;

            GameObject closestChest = null;
            foreach (GameObject chest in chests.Keys)
            {
                if (!chests[chest].Contains(item)) continue;
                float newDistance = GetDistanceBetween(myPosition, chest.transform.position);
                if (closestChest == null || distance > newDistance)
                {
                    closestChest = chest;
                    distance = newDistance;
                }
            }
            if (closestChest != null)
            {
                WorldState possibleWorldState = new WorldState(parent.state);

                possibleWorldState.CopyInventory();
                possibleWorldState.CopyChestInventory(closestChest);

                //World state modification
                possibleWorldState.myInventory.Add(item);
                possibleWorldState.inventories[closestChest].Remove(item);
                possibleWorldState.myPosition = closestChest.transform.position;

                Node node = new Node(parent, 1 + parent.cost + distance, possibleWorldState, GetComponent<PickItemFromChest>(), new ActionDataPickItemFromChest(closestChest, World.GetItemFromId(item)));
                return node;
            }

            return null;
        }
        Node GetItemFromCrafting(Node parentOriginal, Item requestedItem)
        {
            Node parent = parentOriginal;
            // does a recipe exist?
            CraftingRecipe recipe = null;
            foreach(CraftingRecipe thisRecipe in craftingRecipes.craftingRecipes)
            {
                if(thisRecipe.result == requestedItem)
                {
                    recipe = thisRecipe;
                }
            }
            if (recipe == null) return null;

            GameObject craftingObject = GetClosestCraftingObject(recipe, parent.state.myPosition);
            if (craftingObject == null) return null;

            List<int> inventory = new List<int>(parent.state.myInventory);
            List<Item> missingItems = new List<Item>();
            foreach(Item ingredient in recipe.ingredients)
            {
                int ingredientId = World.GetIdFromItem(ingredient);
                if (inventory.Contains(ingredientId))
                    inventory.Remove(ingredientId);
                else
                {
                    missingItems.Add(ingredient);
                }
            }
            parent = GetRequiredItems(parent, missingItems);
            if (parent == null) return null;



            WorldState possibleWorldState = new WorldState(parent.state);

            possibleWorldState.CopyInventory();
            foreach (Item ingredient in recipe.ingredients)
            {
                int ingredientId = World.GetIdFromItem(ingredient);
                possibleWorldState.myInventory.Remove(ingredientId);
            }
            possibleWorldState.myInventory.Add(World.GetIdFromItem(recipe.result));
            //World state modification
            possibleWorldState.myPosition = craftingObject.transform.position;
            Node node = new Node(parent, 1 + parent.cost, possibleWorldState, GetComponent<CraftItem>(), new ActionDataCraftItem(recipe, craftingObject));
            return node;
            // use GetRequiredItems(Node parent, List<Item> requiredItems) to get missing items, returns null if cant get all items
            //
        }
        public Node GetItemPlan(Node parent, Item requestedItem) //Gets a specific Item
        {
            Node node = GetItemFromDrop(parent, requestedItem);
            if (node != null) return node;
            node = GetItemFromChest(parent, requestedItem);
            if (node != null) return node;
            node = GetItemFromCrafting(parent, requestedItem);
            if (node != null) return node;
            return null;
        }
        public Node GetItemPlanNoChest(Node parent, Item requestedItem) //Gets a specific Item
        {
            Node node = GetItemFromDrop(parent, requestedItem);
            if (node != null) return node;
            node = GetItemFromCrafting(parent, requestedItem);
            if (node != null) return node;
            return null;
        }

        public override List<Node> OnActionCompleteWorldStates(Node parent)//Tells the planer how the world state will change on completion
        {
            List<Node> possibleNodes = new List<Node>();
            return possibleNodes;
        }
        GameObject GetClosestCraftingObject(CraftingRecipe recipe, Vector3 myPosition)
        {
            GameObject closestForge = null;
            GameObject closestWorkshop = null;
            GameObject closestAnvil = null;

            foreach (Crafter crafter in GameObject.FindObjectsByType<Crafter>(FindObjectsSortMode.None))
            {
                if (crafter.CraftingObjekt == CraftingObjekt.forge && (closestForge == null || GetDistanceBetween(myPosition, closestForge.transform.position) > GetDistanceBetween(myPosition, crafter.transform.position)))
                {
                    closestForge = crafter.gameObject;
                }
                if (crafter.CraftingObjekt == CraftingObjekt.workshop && (closestWorkshop == null || GetDistanceBetween(myPosition, closestWorkshop.transform.position) > GetDistanceBetween(myPosition, crafter.transform.position)))
                {
                    closestWorkshop = crafter.gameObject;
                }
                if (crafter.CraftingObjekt == CraftingObjekt.anvil && (closestAnvil == null || GetDistanceBetween(myPosition, closestAnvil.transform.position) > GetDistanceBetween(myPosition, crafter.transform.position)))
                {
                    closestAnvil = crafter.gameObject;
                }
            }

            if (recipe.craftingObjekt == CraftingObjekt.anvil) return closestAnvil;
            else if (recipe.craftingObjekt == CraftingObjekt.workshop) return closestWorkshop;
            else return closestForge;

        }
    }
}