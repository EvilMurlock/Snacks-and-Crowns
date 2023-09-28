using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    public class PickUpItem : Action
    {
        public override void Start()
        {
            reusable = true; //this is a subaction
            base.Start();
        }
        public override void Tick()
        {
            if (target == null) Deactivate();
            if (npcAi.reachedEndOfPath) {  Complete(); }
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
            //Is subaction
            return false;

            bool achievable = true;
            List<int> items = (List<int>)worldState.GetStates()["Inventory"];
            if (items.Count >= GetComponent<Inventory>().capacity) achievable = false;//Full inventory = false

            List<(int itemID, Vector3 position)> itemDrops = (List<(int, Vector3)>)worldState.GetStates()["ItemDropList"];
            if (itemDrops.Count == 0) achievable = false;//any items to pick up

            return achievable;
        }
        public override void Activate(object arg)
        {
            (int, Vector3) pair = ((int, Vector3)) arg;


            Item item = World.GetItemFromId(pair.Item1);
            Vector2 position = pair.Item2;

            //Debug.Log("Going to pick up " + item+" at position: "+position);


            Item_Controler[] itemControlers = GameObject.FindObjectsByType<Item_Controler>(FindObjectsSortMode.None);
            float distance = -1;
            Item_Controler chosenItem = null;
            foreach (Item_Controler itemControler in itemControlers)
            {
                if((distance == -1 ||  GetDistanceBetween(itemControler.transform.position, position) < distance) && itemControler.item == item)
                {
                    distance = GetDistanceBetween(itemControler.transform.position, position);
                    chosenItem = itemControler;
                }
            }
            target = chosenItem.gameObject;
            running = true;
            completed = false;
            npcAi.ChangeTarget(target);
            Debug.Log("NPC at target value: " + npcAi.reachedEndOfPath);
        }
        public override void Deactivate()
        {
            running = false;
            npcAi.ChangeTarget(null);
        }
        public override void Complete()
        {
            Debug.Log("Distance from target: " + GetDistanceFromTarget());
            Debug.Log("NPC AI Target is: " + npcAi.target.name + " | Action target: " + target.name);
            if(GetComponent<Inventory>().AddItem(target.GetComponent<Item_Controler>().item))
                Destroy(target);

            running = false;
            completed = true;
        }
        public override List<Node> OnActionCompleteWorldStates(Node parent)//Tells the planer how the world state will change on completion
        {
            List<Node> possibleNodes = new List<Node>();

            List<(int itemId, Vector3 position)> processedItems = new List<(int itemId, Vector3 position)>();

            Vector3 myPosition = (Vector3)parent.state.GetStates()["MyPosition"];

            List<int> inventory = (List<int>)parent.state.GetStates()["Inventory"];
            List<(int itemId, Vector3 position)> itemDropList = (List<(int, Vector3)>)parent.state.GetStates()["ItemDropList"];

            foreach ((int item, Vector3 position) iPpair in itemDropList)
            {
                if (processedItems.Exists(x => x.itemId == iPpair.item))
                {
                    //Debug.Log("Duplicate item found: " + iPpair.item);
                    if (GetDistanceBetween(myPosition, iPpair.position) < GetDistanceBetween(myPosition, processedItems.Find(x => x.itemId == iPpair.item).position))
                    {
                        processedItems.Remove(processedItems.Find(x => x.itemId == iPpair.item));
                    }
                    else continue;
                }
                processedItems.Add(iPpair);
            }

            foreach ((int item, Vector3 position) iPpair in processedItems)
            {
                //WorldState possibleWorldState = parent.state;//DEBUG LINE
                //Vector3 myPosition = (Vector3)parent.state.GetStates()["MyPosition"];//DEBUG LINE

                WorldState possibleWorldState = parent.state.MakeReferencialDuplicate();

                List<int> newInventory = new List<int>(inventory);
                List<(int itemId, Vector3 position)> newItemDropList = new List<(int, Vector3)>(itemDropList);

                newInventory.Add(iPpair.item);
                newItemDropList.Remove(iPpair);

                possibleWorldState.ModifyState("Inventory", newInventory);
                possibleWorldState.ModifyState("ItemDropList", newItemDropList);
                possibleWorldState.ModifyState("MyPosition", iPpair.position);

                /*Debuging
                string invStr = "";
                foreach(Item i in newInventory)
                {
                    invStr += i.item_name+" | ";
                }
                Debug.Log("Pick up item Inventory plan: "+invStr);
                */
                possibleNodes.Add(new Node(parent, 1 + parent.cost + GetDistanceBetween(myPosition, iPpair.position), possibleWorldState, this, iPpair));
            }
            

            return possibleNodes;
        }
    }
}