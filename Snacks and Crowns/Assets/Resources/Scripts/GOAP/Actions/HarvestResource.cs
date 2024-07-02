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
            // use argument null exception
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
        public override void Awake()
        {
            speachBubbleType = SpeachBubbleTypes.Gather;
            base.Awake();
        }
        public override void Start()
        {
            // we initialize harvesting data - use scriptable objects here
            Item requiredItemAxe = World.GetItemFromName("Axe");
            Item resourceItemLog = World.GetItemFromName("Log");
            harvestDatas.Add(new HarvestData(requiredItemAxe, "Tree", resourceItemLog));


            Item requiredItemPickaxe = World.GetItemFromName("Pickaxe");
            Item resourceItemIronOre = World.GetItemFromName("Iron Ore");
            harvestDatas.Add(new HarvestData(requiredItemPickaxe, "Iron Ore Mine", resourceItemIronOre));
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
            
            // we find the item in our inventory to equipt it
            foreach(Item item in agentInventory.Items)
            {
                if (item == planingData.harvestData.requiredTool)
                {
                    tool = (Equipment)item;
                    break;
                }
            }

            // equipts the required tool
            if(!EquipItem(agentInventory, agentEquipmentManager, planingData.harvestData.requiredTool))
            {
                Deactivate();
                return;
            }


            npcAi.ChangeTarget(target);
            base.Activate(arg);
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
            bool harvestableExists = false;
            foreach (TagSystem tagSys in GameObject.FindObjectsByType<TagSystem>(FindObjectsSortMode.None))
            {
                foreach(HarvestData harvestData in harvestDatas)
                {
                    if (tagSys.HasTag(harvestData.targetTag)) 
                    { 
                        harvestableExists = true; 
                        break; 
                    }
                }
                if (harvestableExists) 
                    break;
            }
            if (!harvestableExists) achievable = false;

            return achievable;
        }

        public override List<Node> OnActionCompleteWorldStates(Node parentOriginal)//Tells the planer how the world state will change on completion
        {
            
            

            List<Node> possibleNodes = new List<Node>();
            foreach (HarvestData harvestData in harvestDatas)
            {
                Node parent = parentOriginal;
                bool haveTool = false;
                List<int> items = parent.state.myInventory;
                foreach (int itemId in items)
                {
                    if (World.GetItemFromId(itemId) == harvestData.requiredTool) { haveTool = true; break; }
                }
                if (!haveTool)
                {
                    Node newNode = GetRequiredItem(parent, harvestData.requiredTool);
                    if (newNode == null) continue; // we cant find the needed tool, so we stop planing to harvest this resource
                    parent = newNode;
                }



                Vector3 myPosition = parent.state.myPosition;

                // dát do funkce
                List<TagSystem> resourceDeposits = new List<TagSystem>(); 
                foreach (TagSystem tagSys in GameObject.FindObjectsByType<TagSystem>(FindObjectsSortMode.None))
                {
                    if (tagSys.HasTag(harvestData.targetTag)) resourceDeposits.Add(tagSys);
                }
                TagSystem closestDeposit = null;
                float distanceToDeposit = -1;
                foreach(TagSystem deposit in resourceDeposits)
                {
                    if(closestDeposit == null || GetDistanceBetween(deposit.transform.position, myPosition) < distanceToDeposit)
                    {
                        closestDeposit = deposit;
                        distanceToDeposit = GetDistanceBetween(closestDeposit.transform.position, myPosition);
                    }
                }


                WorldState possibleWorldState = new WorldState(parent.state);
                possibleWorldState.CopyVirtualItemPickups();
                    
                int resource = World.GetIdFromItem(harvestData.resourceItem);
                



                string itemName = harvestData.resourceItem.name;
                if (closestDeposit == null)
                    continue;
                foreach(string tag in closestDeposit.GetTags()) // we add item pickups droped from the resource deposit to the plan world state
                {
                    //Debug.Log("PreCount: " + possibleWorldState.virtualItemPickups.Count);
                    //Debug.Log("Item name: "+itemName);
                    if(tag == itemName+"Drop") possibleWorldState.virtualItemPickups.Add(resource);
                    //Debug.Log("PostCount: " + possibleWorldState.virtualItemPickups.Count);
                }

                possibleWorldState.myPosition = closestDeposit.transform.position;
                ActionDataHarvestResource actionData = new ActionDataHarvestResource(closestDeposit.gameObject, harvestData);
                possibleNodes.Add(new Node(parent, 1 + parent.cost + GetDistanceBetween(myPosition, closestDeposit.transform.position), possibleWorldState, this, actionData));

                
            }
            return possibleNodes;
        }
    }
}