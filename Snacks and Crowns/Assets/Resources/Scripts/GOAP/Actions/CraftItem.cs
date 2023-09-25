using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    public class CraftItem : Action
    {
        List<Crafting_Recepy> craftingRecepyList;
        Crafting_Recepy currentRecepy;
        public override void Start()
        {
            craftingRecepyList = GameObject.Find("Crafting_Recepies").GetComponent<Crafting_Recepies>().crafting_recepies;
            base.Start();
        }
        public override void Tick()
        {
            if (target == null) Deactivate();
            if (npcAi.reachedEndOfPath) Complete();
        }
        public override void Activate(object arg)
        {
            (Crafting_Recepy recepy, GameObject craftingPiece) pair = ((Crafting_Recepy, GameObject)) arg;

            target = pair.craftingPiece;
            currentRecepy = pair.recepy;
            Debug.Log("Going to craft a: " +pair.recepy.result.item_name);

            running = true;
            completed = false;
            npcAi.ChangeTarget(target, 1f);
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

        public override List<Node> OnActionCompleteWorldStates(Node parent)//Tells the planer how the world state will change on completion
        {
            List<Node> possibleNodes = new List<Node>();
            Vector3 myPosition = (Vector3)parent.state.GetStates()["MyPosition"];
            List<int> inventory = (List<int>)parent.state.GetStates()["Inventory"];

            List<Item> inventoryItems = new List<Item>();
            foreach( int i in inventory)
            {
                inventoryItems.Add(World.GetItemFromId(i));
            }

            foreach(Crafting_Recepy craftingRecepy in craftingRecepyList)
            {
                if (GetClosestCraftingObject(craftingRecepy ,myPosition) == null) continue;

                if (craftingRecepy.CanCraftFrom(inventoryItems))
                {
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
            }            

            return possibleNodes;
        }
        GameObject GetClosestCraftingObject(Crafting_Recepy recepy, Vector3 myPosition)
        {
            GameObject closestForge = null;
            GameObject closestWorkshop = null;
            GameObject closestAnvil = null;

            foreach (TagSystem tagSys in GameObject.FindObjectsByType<TagSystem>(FindObjectsSortMode.None))
            {
                if (tagSys.HasTag("forge") && (closestForge == null || GetDistanceBetween(myPosition, closestForge.transform.position) > GetDistanceBetween(myPosition, tagSys.transform.position)))
                {
                    closestForge = tagSys.gameObject;
                }
                if (tagSys.HasTag("workshop") && (closestWorkshop == null || GetDistanceBetween(myPosition, closestWorkshop.transform.position) > GetDistanceBetween(myPosition, tagSys.transform.position)))
                {
                    closestWorkshop = tagSys.gameObject;
                }
                if (tagSys.HasTag("anvil") && (closestAnvil == null || GetDistanceBetween(myPosition, closestAnvil.transform.position) > GetDistanceBetween(myPosition, tagSys.transform.position)))
                {
                    closestAnvil = tagSys.gameObject;
                }
            }
            if (recepy.crafting_objekt == Crafting_Objekt.anvil) return closestAnvil;
            else if (recepy.crafting_objekt == Crafting_Objekt.workshop) return closestWorkshop;
            else return closestForge;

        }
    }
}