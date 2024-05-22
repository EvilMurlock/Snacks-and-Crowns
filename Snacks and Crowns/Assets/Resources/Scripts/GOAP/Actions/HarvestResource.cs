using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    class ActionDataHarvestResource : ActionData
    {
        public ActionDataHarvestResource(GameObject target, HarvestData harvestData)
        {
            this.target = target;
            this.harvestData = harvestData;
        }
        public GameObject target;
        public HarvestData harvestData;
    }
    class HarvestData
    {
        public HarvestData(Item requiredTool, string targetTag, Item resourceItem)
        {
            if (requiredTool == null || resourceItem == null) throw new System.Exception("Harvest data does not have correct item data - in HarvesResource.cs");
            this.requiredTool = requiredTool;
            this.targetTag = targetTag;
            this.resourceItem = resourceItem;
        }
        public Item requiredTool;
        public string targetTag;
        public Item resourceItem;
    }
    public class HarvestResource : Action
    {
        List<HarvestData> harvestDatas = new List<HarvestData>();
        ActionDataHarvestResource planingData;
        Equipment tool;
        public override void Start()
        {
            // we initialize harvesting data
            Item requiredItemAxe = World.GetItemFromName("Axe");
            Item resourceItemLog = World.GetItemFromName("Log");
            harvestDatas.Add(new HarvestData(requiredItemAxe, "Tree", resourceItemLog));


            Item requiredItemPickaxe = World.GetItemFromName("Pickaxe");
            Item resourceItemIronOre = World.GetItemFromName("Iron Ore");
            harvestDatas.Add(new HarvestData(requiredItemAxe, "Iron Ore", resourceItemLog));
            base.Start();
        }
        public override void Tick()
        {
            if (target == null) Complete();
            else if (npcAi.reachedEndOfPath)
            {
                tool.instance.GetComponent<Hand_Item_Controler>().Use();
                //Take a swing at it
            }
        }
        public override void Activate(ActionData arg)
        {
            planingData = (ActionDataHarvestResource)arg;
            target = planingData.target;


            Inventory agentInventory = GetComponent<Inventory>();
            EquipmentManager agentEquipmentManager = GetComponent<EquipmentManager>();
            if (!agentInventory.HasItem(planingData.harvestData.requiredTool))
            {
                Deactivate();
                return;
            }


            // equipts the required tool
            if(!EquipItem(agentInventory, agentEquipmentManager, planingData.harvestData.requiredTool))
            {
                Deactivate();
                return;
            }

            running = true;
            completed = false;
            npcAi.ChangeTarget(target);
        }
        public override void Deactivate()
        {
            //Unequip axe
            Inventory agentInventory = GetComponent<Inventory>();
            EquipmentManager agentEquipmentManager = GetComponent<EquipmentManager>();

            Unequip(agentInventory, agentEquipmentManager, planingData.harvestData.requiredTool);

            running = false;
            npcAi.ChangeTarget(null);
        }
        public override void Complete()
        {
            Inventory agentInventory = GetComponent<Inventory>();
            EquipmentManager agentEquipmentManager = GetComponent<EquipmentManager>();

            Unequip(agentInventory, agentEquipmentManager, planingData.harvestData.requiredTool);

            running = false;
            completed = true;
        }
        public override bool IsAchievableGiven(WorldState worldState)//For the planner
        {
            bool achievable = true;
            bool treeExists = false;
            foreach (TagSystem tagSys in GameObject.FindObjectsByType<TagSystem>(FindObjectsSortMode.None))
            {
                if (tagSys.HasTag("Tree")) { treeExists = true; break; }
            }
            if (!treeExists) achievable = false;

            return achievable;
        }

        public override List<Node> OnActionCompleteWorldStates(Node parent_)//Tells the planer how the world state will change on completion
        {
            
            Node parent = parent_;

            List<Node> possibleNodes = new List<Node>();
            /*
            bool haveAxe = false;
            List<int> items = (List<int>)parent.state.GetStates()["Inventory"];
            foreach (int itemId in items)
            {

                if (World.GetItemFromId(itemId) == requiredItem) { haveAxe = true; break; }
            }
            if (!haveAxe)
            {
                Node newNode = GetRequiredItem(parent, requiredItem);
                if (newNode == null) return possibleNodes;
                parent = newNode;
            }



            Vector3 myPosition = (Vector3)parent.state.GetStates()["MyPosition"];


            List<TagSystem> trees = new List<TagSystem>(); 
            foreach (TagSystem tagSys in GameObject.FindObjectsByType<TagSystem>(FindObjectsSortMode.None))
            {
                if (tagSys.HasTag("Tree")) trees.Add(tagSys);
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

            int log = World.GetIdFromItem((Item)Resources.Load("Items/Log"));
            List<(int itemId, Vector3 position)> newItemDropList = new List<(int, Vector3)>(((List<(int, Vector3)>) parent.state.GetStates()["ItemDropList"]));
            
            foreach(string tag in closestTree.GetTags())
            {
                if(tag == "LogDrop") newItemDropList.Add((log, closestTree.transform.position));
            }
            

            possibleWorldState.ModifyState("ItemDropList", newItemDropList);
            possibleWorldState.ModifyState("MyPosition", closestTree.transform.position);

            possibleNodes.Add(new Node(parent, 1 + parent.cost + GetDistanceBetween(myPosition, closestTree.transform.position), possibleWorldState, this, closestTree.gameObject));
            
            */
            return possibleNodes;
        }
    }
}