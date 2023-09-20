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
        public override void Activate(object newData)
        {
            planingData = ((Interactible_Chest,Item)) newData;
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
            foreach (Item item in (List<Item>)parent.state.GetStates()["Inventory"])
            {
                foreach ((Interactible_Chest, List<Item>) chestInventoryPair in (List<(Interactible_Chest, List<Item>) >)parent.state.GetStates()["ChestList"])
                {

                    WorldState possibleWorldState = new WorldState(parent.state);

                    List<Item> inventory = new List<Item>((List<Item>)parent.state.GetStates()["Inventory"]);
                    List<(Interactible_Chest, List<Item>)> tempList = new List<(Interactible_Chest, List<Item>)>((List<(Interactible_Chest, List<Item>)>)parent.state.GetStates()["ChestList"]);
                    List<(Interactible_Chest, List<Item>)> chestInventoryPairList = new List<(Interactible_Chest, List<Item>)>();
                    foreach((Interactible_Chest, List<Item>) temp in tempList)
                    {
                        List<Item> itemList = new List<Item>(temp.Item2);
                        chestInventoryPairList.Add((temp.Item1, itemList));
                    }

                    (Interactible_Chest, List<Item>) pair = chestInventoryPairList.Find(x => x.Item1 == chestInventoryPair.Item1);
                    /*
                    string oldListString = "";
                    foreach (Item i in pair.Item2)
                    {
                        oldListString += i.item_name + " | ";
                    }
                    pair.Item2 = new List<Item>(pair.Item2);
                    string newListString = "";
                    foreach (Item i in pair.Item2)
                    {
                        newListString += i.item_name + " | ";
                    }

                    Debug.Log("New List: " + newListString + " ||| Old List: "+ oldListString);
                    */
                    //DEBUG ITEMS
                    /*
                    List<Item> debugInventory = new List<Item>((List<Item>)parent.parent.state.GetStates()["Inventory"]);
                    List<(Interactible_Chest, List<Item>)> debugChestList = new List<(Interactible_Chest, List<Item>)>((List<(Interactible_Chest, List<Item>)>)parent.parent.state.GetStates()["ChestList"]);
                    (Interactible_Chest, List<Item>) debugPair = debugChestList.Find(x => x.Item1 == chestInventoryPair.Item1);
                    
                    string invStrPre = "";
                    foreach (Item i in pair.Item2)
                    {
                        invStrPre += i.item_name + " | ";
                    }
                    string npcStrPre = "";
                    foreach (Item i in inventory)
                    {
                        npcStrPre += i.item_name + " | ";
                    }
                    */
                    bool itemRemoved = inventory.Remove(item);
                    if (pair.Item2.Count < pair.Item1.chest_inventory.Length) pair.Item2.Add(item);
                    else continue;
                    /*
                    string invStr = "";
                    foreach (Item i in pair.Item2)
                    {
                        invStr += i.item_name + " | ";
                    }
                    string npcStr = "";
                    foreach (Item i in inventory)
                    {
                        npcStr += i.item_name + " | ";
                    }
                    string prevChest = "";
                    foreach (Item i in debugPair.Item2)
                    {
                        prevChest += i.item_name + " | ";
                    }
                    string prevInv = "";
                    foreach (Item i in debugInventory)
                    {
                        prevInv += i.item_name + " | ";
                    }

                    Debug.Log("PutInChest action, chest Inventory plan: " + invStr + " --- npc inventory: "+ npcStr + " --- Item removed?: "+ itemRemoved +"\n"
                        + "              chest Inventory plan before act: " + invStrPre + " --- npc inventory before act: " + npcStrPre + "\n"
                        + "              previous action chest inventory: " + prevChest + " --- Previous npc inventory: "+ prevInv + " --- Prev Action: "+parent.parent.action.actionName);

                    */

                    possibleWorldState.ModifyState("Inventory", inventory);
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