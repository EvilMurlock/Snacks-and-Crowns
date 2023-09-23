using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    public class MineIronOre : Action
    {
        Equipment axe;
        public override void Tick()
        {
            if (target == null) Complete();
            else if (npcAi.reachedEndOfPath)
            {
                axe.instance.GetComponent<Hand_Item_Controler>().Use();
                //Take a swing at it
            }
        }
        public override void Activate(object arg)
        {
            target = (GameObject)arg;

            //Equip axe
            List<Item> items = (List<Item>)GetComponent<Inventory>().GetInventory;
            foreach (Item item in items)
            {
                if (item.name == "Axe") { GetComponent<EquipmentManager>().EquipItem(item);axe =(Equipment) item ; break;}
                
            }


            running = true;
            completed = false;
            npcAi.ChangeTarget(target, 0.8f);
        }
        public override void Deactivate()
        {
            //Unequip axe
            GetComponent<EquipmentManager>().UnEquipItem(axe);
            running = false;
            npcAi.ChangeTarget(null);
        }
        public override void Complete()
        {
            GetComponent<EquipmentManager>().UnEquipItem(axe);

            running = false;
            completed = true;
        }
        public override bool IsAchievableGiven(WorldState worldState)//For the planner
        {
            bool achievable = true;
            bool haveAxe = false;
            List<int> items = (List<int>)worldState.GetStates()["Inventory"];
            foreach (int itemId in items)
            {

                if (World.GetItemFromId(itemId).name == "Axe") { haveAxe = true; break; }
            }
            if (!haveAxe) achievable = false;

            bool treeExists = false;
            foreach (TagSystem tagSys in GameObject.FindObjectsByType<TagSystem>(FindObjectsSortMode.None))
            {
                if (tagSys.HasTag("Iron Ore Mine")) { treeExists = true; break; }
            }
            if (!treeExists) achievable = false;

            return achievable;
        }

        public override List<Node> OnActionCompleteWorldStates(Node parent)//Tells the planer how the world state will change on completion
        {
            List<Node> possibleNodes = new List<Node>();
            Vector3 myPosition = (Vector3)parent.state.GetStates()["MyPosition"];

            List<TagSystem> trees = new List<TagSystem>(); 
            foreach (TagSystem tagSys in GameObject.FindObjectsByType<TagSystem>(FindObjectsSortMode.None))
            {
                if (tagSys.HasTag("Iron Ore Mine")) trees.Add(tagSys);
            }
            TagSystem closestTree = null;
            float distanceToTree = -1;
            foreach(TagSystem tree in trees)
            {
                if(closestTree == null || GetDistanceBetween(tree.transform.position, myPosition) < distanceToTree)
                {
                    closestTree = tree;
                    distanceToTree = GetDistanceBetween(closestTree.transform.position, myPosition);
                }
            }


            WorldState possibleWorldState = parent.state.MakeReferencialDuplicate();

            int ironOre = World.GetIdFromItem((Item)Resources.Load("Items/Iron Ore"));
            List<(int itemId, Vector3 position)> newItemDropList = new List<(int, Vector3)>(((List<(int, Vector3)>) parent.state.GetStates()["ItemDropList"]));
            
            foreach(string tag in closestTree.GetTags())
            {
                if(tag == "Iron OreDrop") newItemDropList.Add((ironOre, closestTree.transform.position));
            }
            

            possibleWorldState.ModifyState("ItemDropList", newItemDropList);
            possibleWorldState.ModifyState("MyPosition", closestTree.transform.position);

            possibleNodes.Add(new Node(parent, 1 + parent.cost + GetDistanceBetween(myPosition, closestTree.transform.position), possibleWorldState, this, closestTree.gameObject));
            

            return possibleNodes;
        }
    }
}