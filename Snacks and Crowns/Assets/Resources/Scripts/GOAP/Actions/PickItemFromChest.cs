using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GOAP
{
    /// <summary>
    /// Action data for which item and from which chest it is taken
    /// </summary>
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
    /// <summary>
    /// Takes an item from a chest
    /// </summary>
    public class PickItemFromChest : SubAction
    {
        ActionDataPickItemFromChest planningData;
        public override void Awake()
        {
            speechBubbleType = SpeechBubbleTypes.GetItem;
            base.Awake();
        }
        public override void Start()
        {
            reusable = true; //This is a sub-action
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

        public override void Activate(ActionData arg)
        {
            planningData = (ActionDataPickItemFromChest)arg;
            target = planningData.targetObject.gameObject;
            npcAi.ChangeTarget(target);
            base.Activate(arg);
        }
        public override void Deactivate()
        {
            running = false;
        }
        public override void Complete()
        {

            Inventory chestInventory = planningData.targetObject.GetComponent<Inventory>();
            if (!chestInventory.HasItem(planningData.item))
            {
                Deactivate();
                return;
            }
            chestInventory.RemoveItem(planningData.item);


            Inventory agentInventory = GetComponent<Inventory>();
            if (!agentInventory.HasEmptySpace(1))
            {
                DropRandomItemFromInventory(agentInventory);
            }
            agentInventory.AddItem(planningData.item);
            running = false;
            completed = true;
        }
    }
}