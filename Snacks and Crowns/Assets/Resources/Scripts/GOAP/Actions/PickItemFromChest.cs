using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GOAP
{
    public class PickItemFromChest : Action
    {
        (Chest chest, Item item) planingData;
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
        public override void Activate(object newData)
        {
        }
        public override void Deactivate()
        {
            running = false;
        }
        public override void Complete()
        {
            if (!planingData.chest.RemoveItem(planingData.item))
            {
                Deactivate(); 
                return;
            }
            /*
            if (!GetComponent<Inventory>().AddItem(planingData.item))
            {
                planingData.chest.AddItem(planingData.item);
                Deactivate(); 
                return;
            }*/

            running = false;
            completed = true;
        }
        public override List<Node> OnActionCompleteWorldStates(Node parent)//Tells the planer how the world state will change on completion
        {
            List<Node> possibleNodes = new List<Node>();
            List<int> inventory = (List<int>)parent.state.GetStates()["Inventory"];
            Vector3 myPosition = (Vector3)parent.state.GetStates()["MyPosition"];

            List<int> chosenItems = new List<int>();


            return possibleNodes;
        }
    }
}