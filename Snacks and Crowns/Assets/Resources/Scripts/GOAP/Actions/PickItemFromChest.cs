using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GOAP
{

    public class ActionDataPickItemFromChest : ActionData
    {
        public GameObject targetObject;
        public Item item;
        public ActionDataPickItemFromChest(GameObject targetObject, Item item)
        {
            this.targetObject = targetObject;
            this.item = item;
        }
    }
    public class PickItemFromChest : SubAction
    {
        ActionDataPickItemFromChest planingData;
        public override void Start()
        {
            reusable = true; //This is a subaction
            base.Start();
        }
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

        public override void Activate(ActionData newData)
        {
            planingData = (ActionDataPickItemFromChest)newData;
            target = planingData.targetObject.gameObject;
        }
        public override void Deactivate()
        {
            running = false;
        }
        public override void Complete()
        {

            Inventory chestInventory = planingData.targetObject.GetComponent<Inventory>();
            if (!chestInventory.HasItem(planingData.item))
            {
                Deactivate();
                return;
            }
            chestInventory.RemoveItem(planingData.item);


            Inventory agentInventory = GetComponent<Inventory>();
            if (!agentInventory.HasEmptySpace(1))
            {
                DropRandomItemFromInventory(agentInventory);
            }
            agentInventory.AddItem(planingData.item);
            running = false;
            completed = true;
        }
    }
}