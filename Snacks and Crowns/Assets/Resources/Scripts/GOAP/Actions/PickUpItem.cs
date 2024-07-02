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
        public ActionDataPickUpItem(int virtualItemId)
        {
            this.itemPickup = null;
            this.virtualItemId = virtualItemId;
        }
        public ItemPickup itemPickup;
        public int virtualItemId;
    }
    public class PickUpItem : SubAction
    {
        public override void Awake()
        {
            speachBubbleType = SpeachBubbleTypes.GetItem;
            base.Awake();
        }
        public override void Start()
        {
            reusable = true; //this is a subaction
            base.Start();
        }
        public override string GetInfo(ActionData data)
        {
            ActionDataPickUpItem myData = (ActionDataPickUpItem)data;
            if (myData.itemPickup == null) 
                return this.GetType().ToString() + " ( virtual item )";
            return this.GetType().ToString() + " ( item: " + myData.itemPickup.item.name + ")";
        }
        public override void Tick()
        {
            if (target == null) Deactivate();
            if (npcAi.reachedEndOfPath) {  Complete(); }
        }
        public override void Activate(ActionData dataArg)
        {
            
            ActionDataPickUpItem data = (ActionDataPickUpItem)dataArg;
            ItemPickup itemPickup = null;
            if (data.itemPickup == null)
            {
                if(data.virtualItemId >= 0)
                {
                    itemPickup = ItemPickup.GetItemPickupWithItem(World.GetItemFromId(data.virtualItemId));
                    // find closest item pickup with that item
                }
                if(itemPickup == null)
                {
                    Deactivate();
                    return;
                }
            }
            else
                itemPickup = data.itemPickup;


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

            npcAi.ChangeTarget(target);
            //Debug.Log("NPC reached end of path: " + npcAi.reachedEndOfPath);
            base.Activate(dataArg);
        }
        public override void Deactivate()
        {
            running = false;
            Debug.Log("npcAI is: " + npcAi);
            npcAi.ChangeTarget(null);
        }
        public override void Complete()
        {
            //Debug.Log("Distance from target: " + GetDistanceFromTarget());
            //Debug.Log("NPC AI Target is: " + npcAi.target.name + " | Action target: " + target.name);
            if (GetComponent<Inventory>().HasEmptySpace(1))
            {
                GetComponent<Inventory>().AddItem(target.GetComponent<ItemPickup>().item);
                Destroy(target);
            }

            running = false;
            completed = true;
        }

    }
}