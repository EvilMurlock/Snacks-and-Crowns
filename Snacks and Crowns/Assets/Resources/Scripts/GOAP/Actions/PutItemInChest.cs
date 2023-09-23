using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GOAP
{
    public class PutItemInChest : Action
    {
        (Interactible_Chest chest, Item item) planingData;
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
            List<int> items = (List<int>)worldState.GetStates()["Inventory"];
            if (items.Count == 0) achievable = false;
            List<(Interactible_Chest, List<int>)> chests = (List<(Interactible_Chest, List<int>)>)worldState.GetStates()["ChestList"];
            if (chests.Count == 0) achievable = false;
            return achievable;
        }
        public override void Activate(object newData)
        {
            planingData = ((Interactible_Chest, Item))newData;
            target = planingData.chest.gameObject;
            running = true;
            completed = false;
            npcAi.ChangeTarget(target);

        }
        public override void Deactivate()
        {
            running = false;
        }
        public override void Complete()
        {
            if (!GetComponent<Inventory>().RemoveItem(planingData.item)) 
            { 
                Deactivate();
                return;
            }
            if (!planingData.chest.AddItem(planingData.item))
            { 
                GetComponent<Inventory>().AddItem(planingData.item); //re-adding the previously removed item into our inventory
                Deactivate(); 
                return;
            }

            running = false;
            completed = true;
        }
        public override List<Node> OnActionCompleteWorldStates(Node parent)//Tells the planer how the world state will change on completion

        {
            List<Node> possibleNodes = new List<Node>();
            List<int> inventory = (List<int>)parent.state.GetStates()["Inventory"];
            List < (Interactible_Chest, List<int>)> ChestList = (List<(Interactible_Chest, List<int>)>)parent.state.GetStates()["ChestList"];
            foreach (int itemId in inventory)
            {
                Item item = World.GetItemFromId(itemId);
                foreach ((Interactible_Chest, List<int>) chestInventoryPair in ChestList)
                {

                    WorldState possibleWorldState = parent.state.MakeReferencialDuplicate();

                    List<int> newInventory = new List<int>(inventory);
                    List<(Interactible_Chest, List<int>)> tempList = new List<(Interactible_Chest, List<int>)>(ChestList);
                    List<(Interactible_Chest, List<int>)> chestInventoryPairList = new List<(Interactible_Chest, List<int>)>();
                    foreach((Interactible_Chest, List<int>) temp in tempList)
                    {
                        List<int> itemList = new List<int>(temp.Item2);
                        chestInventoryPairList.Add((temp.Item1, itemList));
                    }

                    (Interactible_Chest, List<int>) pair = chestInventoryPairList.Find(x => x.Item1 == chestInventoryPair.Item1);

                    bool itemRemoved = newInventory.Remove(itemId);
                    if (pair.Item2.Count < pair.Item1.chest_inventory.Length) pair.Item2.Add(itemId);
                    else continue;
                

                    possibleWorldState.ModifyState("Inventory", newInventory);
                    possibleWorldState.ModifyState("ChestList", chestInventoryPairList);
                    possibleWorldState.ModifyState("MyPosition", pair.Item1.transform.position);
                    GameObject tempTarget = chestInventoryPair.Item1.gameObject;

                    Vector3 myPosition = (Vector3)parent.state.GetStates()["MyPosition"];

                    possibleNodes.Add(new Node(parent, 1 + parent.cost + GetDistanceBetween(tempTarget.transform.position, myPosition), possibleWorldState, this, (chestInventoryPair.Item1,item)));
                }
            }


            return possibleNodes;
        }
    }
}