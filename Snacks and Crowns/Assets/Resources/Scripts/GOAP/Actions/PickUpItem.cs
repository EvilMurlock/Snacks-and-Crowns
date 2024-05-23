using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    public class ActionDataPickUpItem : ActionData
    {
        public ActionDataPickUpItem(ItemPickup itemPickup)
        {
            this.itemPickup = itemPickup;
        }
        public ItemPickup itemPickup;
    }
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
            //Is subaction, we call it explicitly
            return false;
            /*
            bool achievable = true;
            List<int> items = worldState.myInvetory
            if (items.Count >= GetComponent<Inventory>().Items.Length) achievable = false;//Full inventory = false

            List<(int itemID, Vector3 position)> itemDrops = (List<(int, Vector3)>)worldState.GetStates()["ItemDropList"];
            if (itemDrops.Count == 0) achievable = false;//any items to pick up

            return achievable;*/
        }
        public override void Activate(ActionData dataArg)
        {
            
            ActionDataPickUpItem data = (ActionDataPickUpItem)dataArg;
            if (data.itemPickup == null) Deactivate();
            ItemPickup itemPickup = data.itemPickup;


            Item item = itemPickup.item;
            Vector3 targetPosition = itemPickup.transform.position;

            //Debug.Log("Going to pick up " + item+" at position: "+position);


            ItemPickup[] itemControlers = GameObject.FindObjectsByType<ItemPickup>(FindObjectsSortMode.None);
            float distance = -1;
            ItemPickup chosenItem = null;
            foreach (ItemPickup itemControler in itemControlers)
            {
                if((distance == -1 ||  GetDistanceBetween(itemControler.transform.position, targetPosition) < distance) && itemControler.item == item)
                {
                    distance = GetDistanceBetween(itemControler.transform.position, targetPosition);
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
            if (GetComponent<Inventory>().HasEmptySpace(1))
            {
                GetComponent<Inventory>().AddItem(target.GetComponent<ItemPickup>().item);
                Destroy(target);
            }

            running = false;
            completed = true;
        }
        public override List<Node> OnActionCompleteWorldStates(Node parent)//Tells the planer how the world state will change on completion
        {
            List<Node> possibleNodes = new List<Node>();

            List<ItemPickup> closestItems = new List<ItemPickup>();

            Vector3 myPosition = parent.state.myPosition;

            List<int> inventory = parent.state.myInventory;
            List<ItemPickup> itemPickups = parent.state.itemPickups;

            // we find the closest item for each item type
            foreach (ItemPickup itemPickup in itemPickups)
            {
                int itemId = World.GetIdFromItem(itemPickup.item);

                ItemPickup fartherItemPickup = closestItems.Find(x => World.GetIdFromItem(x.item) == itemId);
                if (fartherItemPickup != null)
                {
                    
                    if (GetDistanceBetween(myPosition, itemPickup.transform.position) < GetDistanceBetween(myPosition, fartherItemPickup.transform.position))
                    {
                        closestItems.Remove(fartherItemPickup);
                    }
                    else continue;
                }
                closestItems.Add(itemPickup);
            }

            foreach (ItemPickup itemPickup in closestItems)
            {
                //WorldState possibleWorldState = parent.state;//DEBUG LINE
                //Vector3 myPosition = (Vector3)parent.state.GetStates()["MyPosition"];//DEBUG LINE

                WorldState possibleWorldState = new WorldState(parent.state);
                possibleWorldState.CopyInventory();
                possibleWorldState.CopyItemPickups();

                possibleWorldState.myInventory.Add(World.GetIdFromItem(itemPickup.item));
                possibleWorldState.itemPickups.Remove(itemPickup);
                possibleWorldState.myPosition = itemPickup.transform.position;

                /*Debuging
                string invStr = "";
                foreach(Item i in newInventory)
                {
                    invStr += i.item_name+" | ";
                }
                Debug.Log("Pick up item Inventory plan: "+invStr);
                */
                float cost = 1 + parent.cost + GetDistanceBetween(myPosition, itemPickup.transform.position);
                possibleNodes.Add(new Node(parent, cost, possibleWorldState, this, new ActionDataPickUpItem(itemPickup)));
            }
            

            return possibleNodes;
        }
    }
}