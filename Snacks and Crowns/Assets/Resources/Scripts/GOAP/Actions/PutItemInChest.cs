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
            List<Item> items = (List<Item>)worldState.GetStates()["Inventory"];
            if (items.Count == 0) achievable = false;
            List<(Interactible_Chest, List<Item>)> chests = (List<(Interactible_Chest, List<Item>)>)worldState.GetStates()["ChestList"];
            if (chests.Count == 0) achievable = false;
            return achievable;
        }
        public override bool IsAchievable()//Checs curent condition + world state
        {
            return true;
        }
        public override void Activate(object newData)
        {
            planingData = ((Interactible_Chest,Item)) newData;
            target = planingData.chest.gameObject;
            Debug.Log("Target is now: " + target.name);
            running = true;
            completed = false;
            Debug.Log("Switching path to " + target.name);
            gameObject.GetComponent<NpcAi>().ChangeTarget(target);

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
            foreach (Item item in (List<Item>)parent.state.GetStates()["Inventory"])
            {
                foreach ((Interactible_Chest, List<Item>) chestInventoryPair in (List<(Interactible_Chest, List<Item>) >)parent.state.GetStates()["ChestList"])
                {

                    WorldState possibleWorldState = new WorldState(parent.state);

                    List<Item> inventory = new List<Item>((List<Item>)parent.state.GetStates()["Inventory"]);
                    List<(Interactible_Chest, List<Item>)> chestInventoryPairList = new List<(Interactible_Chest, List<Item>)>((List<(Interactible_Chest, List<Item>)>)parent.state.GetStates()["ChestList"]);

                    inventory.Remove(item);
                    (Interactible_Chest, List<Item>) pair = chestInventoryPairList.Find(x => x == chestInventoryPair);

                    /*
                    Debug.Log("Chest Items: " + pair.Item2.Count );
                    Debug.Log("Chest space: " +pair.Item1.chest_inventory.Length);

                    Debug.Log("Enough room in chest: "+ (pair.Item2.Count < pair.Item1.chest_inventory.Length));
                    Debug.Log("Item planing to put in chest: " + item);
                    */
                    if (pair.Item2.Count < pair.Item1.chest_inventory.Length) pair.Item2.Add(item);
                    else break;

                    possibleWorldState.ModifyState("Inventory", inventory);
                    possibleWorldState.ModifyState("ChestList", chestInventoryPairList);
                    GameObject tempTarget = chestInventoryPair.Item1.gameObject;
                    possibleNodes.Add(new Node(parent, parent.cost + GetDistanceFromObject(tempTarget), possibleWorldState, this, (chestInventoryPair.Item1,item)));
                }
            }


            return possibleNodes;
        }
    }
}