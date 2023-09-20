using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    public class PickUpItem : Action
    {
        public override void Tick()
        {
            if (target == null) Deactivate();
            if (npcAi.reachedEndOfPath) Complete();
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
            if (items.Count >= GetComponent<Inventory>().capacity) achievable = false;//Full inventory = false

            List<(Item item, Vector3 position)> itemDrops = (List<(Item, Vector3)>)worldState.GetStates()["ItemDropList"];
            if (itemDrops.Count == 0) achievable = false;//any items to pick up

            return achievable;
        }
        public override void Activate(object arg)
        {
            (Item, Vector3) pair = ((Item, Vector3)) arg;


            Item item = pair.Item1;
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
        }
        public override void Deactivate()
        {
            running = false;
            npcAi.ChangeTarget(null);
        }
        public override void Complete()
        {
            if(GetComponent<Inventory>().AddItem(target.GetComponent<Item_Controler>().item))
                Destroy(target);

            running = false;
            completed = true;
        }
        public override List<Node> OnActionCompleteWorldStates(Node parent)//Tells the planer how the world state will change on completion
        {
            List<Node> possibleNodes = new List<Node>();

            List<Item> inventory = (List<Item>)parent.state.GetStates()["Inventory"];
            foreach((Item item, Vector3 position) iPpair in (List<(Item, Vector3)>)parent.state.GetStates()["ItemDropList"])
            {
                WorldState possibleWorldState = new WorldState(parent.state);

                List<Item> newInventory = new List<Item>(inventory);
                List<(Item item, Vector3 position)> newItemDropList = new List<(Item, Vector3)>(((List<(Item, Vector3)>) parent.state.GetStates()["ItemDropList"]));

                newInventory.Add(iPpair.item);
                newItemDropList.Remove(iPpair);

                possibleWorldState.ModifyState("Inventory", newInventory);
                possibleWorldState.ModifyState("ItemDropList", newItemDropList);
                possibleWorldState.ModifyState("MyPosition", iPpair.position);
                Vector3 myPosition = (Vector3)parent.state.GetStates()["MyPosition"];

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