using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GOAP
{

    public class ActionDataPickItemFromChest : ActionData
    {
        public Chest chest;
        public Item item;
        public ActionDataPickItemFromChest(Chest chest, Item item)
        {
            this.chest = chest;
            this.item = item;
        }
    }
    public class PickItemFromChest : Action
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
            return false;
            //Its a subaction
        }
        public override void Activate(ActionData newData)
        {
            planingData = (ActionDataPickItemFromChest)newData;
            target = planingData.chest.gameObject;
        }
        public override void Deactivate()
        {
            running = false;
        }
        public override void Complete()
        {

            Inventory chestInventory = planingData.chest.GetComponent<Inventory>();
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
        public override List<Node> OnActionCompleteWorldStates(Node parent)//Tells the planer how the world state will change on completion
        {
            // WE DONT DO THIS - PLANNS GENERATED IN DIFFERENT ACTION - THIS ACTION IS HERE ONLY FOR ITS EXECUTION NOT PLANNING PART
            
            List<Node> possibleNodes = new List<Node>();
            /*
            //List<int> inventory = (List<int>)parent.state.GetStates()["Inventory"];
            //Vector3 myPosition = (Vector3)parent.state.GetStates()["MyPosition"];

            List<int> chosenItems = new List<int>();

            */
            return possibleNodes;
        }
    }
}