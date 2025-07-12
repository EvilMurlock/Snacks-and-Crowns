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
        GameObject trees;
        GameObject ironOres;
        GameObject magicOres;
        List<HarvestData> harvestDatas = new List<HarvestData>();
        ActionDataHarvestResource planingData;
        public Equipment tool;
        EquipmentManager equipmentManager;
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

            Item resourceItemCrystalShard = World.GetItemFromName("Crystal Shard");
            harvestDatas.Add(new HarvestData(requiredItemPickaxe, "Crystal Mine", resourceItemCrystalShard));

            trees = GameObject.Find("Trees");
            ironOres = GameObject.Find("Iron Ore Deposits");
            magicOres = GameObject.Find("Crystal Deposits");
            equipmentManager = GetComponent<EquipmentManager>();
            base.Start();
        }
        public override void Tick()
        {
            if (target == null) Complete();
            else if (npcAi.reachedEndOfPath)
            {
                //Debug.Log("Using tool <" + tool +"> to harvest");
                if(tool == null || tool.GetInstance(gameObject) == null)
                {
                    Deactivate();
                    return;
                }
                Hand_Item_Controler controler = tool.GetInstance(gameObject).GetComponent<Hand_Item_Controler>();
                controler.Use();
                //Take a swing at it
            }
        }
        public override void Activate(ActionData arg)
        {
            planingData = (ActionDataHarvestResource)arg;
            target = planingData.target;
            
            if (!equipmentManager.HasEquipedItem(planingData.harvestData.requiredTool))
                EquipItem(planingData.harvestData.requiredTool);
            tool = GetEquipedItem(planingData.harvestData.requiredTool);

            // equipts the required tool
            if (tool == null)
            {
                Deactivate();
                return;
            }

            //Debug.Log("NPC: "+this.transform.position+" Found the correct tool: "+tool.itemName);

            npcAi.ChangeTarget(target);
            base.Activate(arg);
        }
        public override void Deactivate()
        {
            //Unequip axe
            Inventory agentInventory = GetComponent<Inventory>();
            EquipmentManager agentEquipmentManager = GetComponent<EquipmentManager>();

            Unequip(agentInventory, agentEquipmentManager, planingData.harvestData.requiredTool);
            tool = null;
            running = false;
            npcAi.ChangeTarget(null);
        }
        public override void Complete()
        {
            Inventory agentInventory = GetComponent<Inventory>();
            EquipmentManager agentEquipmentManager = GetComponent<EquipmentManager>();

            Unequip(agentInventory, agentEquipmentManager, planingData.harvestData.requiredTool);
            tool = null;
            running = false;
            completed = true;
        }
        List<TagSystem> GetListOfResources()
        {
            List<TagSystem> resources = new List<TagSystem>();
            foreach(Transform t in trees.transform)
            {
                resources.Add(t.gameObject.GetComponent<TagSystem>());
            }
            foreach (Transform t in ironOres.transform)
            {
                resources.Add(t.gameObject.GetComponent<TagSystem>());
            }
            foreach (Transform t in magicOres.transform)
            {
                resources.Add(t.gameObject.GetComponent<TagSystem>());
            }
            return resources;
        }
        public override bool IsAchievableGiven(WorldState worldState)//For the planner
        {
            bool achievable = true;
            bool harvestableExists = false;
            foreach (TagSystem tagSys in GetListOfResources())//GameObject.FindObjectsByType<TagSystem>(FindObjectsSortMode.None))
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
                parent = GetTool(parent, harvestData.requiredTool);
                if (parent == null)
                    continue;

                Vector3 myPosition = parent.state.myPosition;

                TagSystem closestDeposit = FindClosestDeposit(harvestData, myPosition);

                WorldState possibleWorldState = new WorldState(parent.state);
                if (closestDeposit == null)
                    continue;

                possibleWorldState.CopyVirtualItemPickups();

                AddVirtualItems(harvestData, closestDeposit, possibleWorldState);
                

                possibleWorldState.myPosition = closestDeposit.transform.position;
                ActionDataHarvestResource actionData = new ActionDataHarvestResource(closestDeposit.gameObject, harvestData);
                possibleNodes.Add(new Node(parent, 1 + parent.cost + GetDistanceBetween(myPosition, closestDeposit.transform.position), possibleWorldState, this, actionData));

                
            }
            //Debug.Log("Number of harvest resource plans: " +possibleNodes.Count);
            //Debug.Log("Resource: " + ((ActionDataHarvestResource)possibleNodes[0].data).target.ToString());
            return possibleNodes;
        }
        void AddVirtualItems(HarvestData harvestData, TagSystem closestDeposit, WorldState possibleWorldState)
        {
            int resource = World.GetIdFromItem(harvestData.resourceItem);
            string itemName = harvestData.resourceItem.name;
            foreach (string tag in closestDeposit.GetTags()) // we add item pickups droped from the resource deposit to the plan world state
            {
                //Debug.Log("PreCount: " + possibleWorldState.virtualItemPickups.Count);
                //Debug.Log("Item name: "+itemName);
                if (tag == itemName + "Drop") possibleWorldState.virtualItemPickups.Add(resource);
                //Debug.Log("PostCount: " + possibleWorldState.virtualItemPickups.Count);
            }
        }
        TagSystem FindClosestDeposit(HarvestData harvestData, Vector3 myPosition)
        {
            List<TagSystem> resourceDeposits = new List<TagSystem>();
            foreach (TagSystem tagSys in GetListOfResources())//GameObject.FindObjectsByType<TagSystem>(FindObjectsSortMode.None))
            {
                if (tagSys.HasTag(harvestData.targetTag)) resourceDeposits.Add(tagSys);
            }
            TagSystem closestDeposit = null;
            float distanceToDeposit = -1;
            foreach (TagSystem deposit in resourceDeposits)
            {
                if (closestDeposit == null || GetDistanceBetween(deposit.transform.position, myPosition) < distanceToDeposit)
                {
                    closestDeposit = deposit;
                    distanceToDeposit = GetDistanceBetween(closestDeposit.transform.position, myPosition);
                }
            }
            return closestDeposit;
        }
    }
}