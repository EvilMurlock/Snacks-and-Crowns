using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    public enum SpeechBubbleTypes
    {
        None,
        Charge,
        Fight,
        Gather,
        Sleep,
        Walk,
        GetItem,
        Heal
    }

    /// <summary>
    /// Base action class inlcuding some helpfull methods
    /// </summary>
    public abstract class NPCAction : MonoBehaviour
    {
        public bool reusable = false; //can this action be used multiple times in the planner?, often set true for sub actions

        float speechBubbleDuration = 3;
        public string actionName = "Action";
        public GameObject target;
        public List<string> targetTags = new List<string>();

        public bool running = false;
        public bool completed = false;
        protected NpcAi npcAi;
        Sprite speechBubbleSprite = null;
        protected SpeechBubbleTypes speechBubbleType = SpeechBubbleTypes.None;
        int equipmentSlots = 5;
        public virtual string GetInfo(ActionData data)
        {
            return this.GetType().ToString();
        }
        public virtual void Awake()
        {
            LoadSpeachBubbleSprite();
            actionName = this.GetType().Name;
        }
        void LoadSpeachBubbleSprite()
        {
            string spriteName = "";
            switch (speechBubbleType)
            {
                case SpeechBubbleTypes.None:
                    return;
                case SpeechBubbleTypes.Charge:
                    spriteName = "bubble_charge";
                    break;
                case SpeechBubbleTypes.Fight:
                    spriteName = "bubble_fight";
                    break;
                case SpeechBubbleTypes.Gather:
                    spriteName = "bubble_gather";
                    break;
                case SpeechBubbleTypes.Sleep:
                    spriteName = "bubble_sleep";
                    break;
                case SpeechBubbleTypes.Walk:
                    spriteName = "bubble_walk";
                    break;
                case SpeechBubbleTypes.GetItem:
                    spriteName = "bubble_get_item";
                    break;
            }
            speechBubbleSprite = Resources.Load<Sprite>("Sprites/Speach Bubbles/" + spriteName);
        }
        public virtual void Start()
        {
            npcAi = GetComponent<NpcAi>();
        }
        public virtual void Tick()
        {
            if (target == null) Deactivate();
            if (npcAi.reachedEndOfPath) Complete();
        }
        public virtual float GetCost(WorldState worldState)
        {
            
            return GetDistanceFromTarget();
        }
        public virtual bool IsUsableBy(GameObject g)
        {
            return true;
        }
        public virtual void Activate(ActionData data)
        {
            StartCoroutine(CreateTimedSpeechBubble());
            running = true;
            completed = false;
        }
        public virtual void Deactivate()
        {
            running = false;
        }
        public virtual void Complete()
        {
            running = false;
            completed = true;
        }
        public abstract List<Node> OnActionCompleteWorldStates(Node parent);
        protected float GetDistanceFromTarget()
        {
            if (target != null) return (gameObject.transform.position - target.transform.position).magnitude;
            return 0;
        }
        protected float GetDistanceFromVector(Vector2 vector)
        {
            return (gameObject.transform.position - (Vector3)vector).magnitude;
        }

        protected float GetDistanceFromObject(GameObject distanceTarget)
        {
            if (distanceTarget != null) return (gameObject.transform.position - distanceTarget.transform.position).magnitude;
            return 0;
        }
        protected float GetDistanceBetween(Vector3 a, Vector3 b)
        {
            return (a - b).magnitude;
        }
        protected GameObject FindTarget()
        {
            GameObject foundTarget = null;
            if (targetTags.Count == 0) //If no target tags then target is considered self
            {
                foundTarget = this.gameObject; 
            }
            foreach (TagSystem tagSys in FindObjectsByType<TagSystem>(FindObjectsSortMode.None))
            {
                if (tagSys.HasTags(targetTags))
                {
                    if (foundTarget == null || (gameObject.transform.position - target.transform.position).magnitude < (gameObject.transform.position - tagSys.gameObject.transform.position).magnitude)
                    {
                        foundTarget = tagSys.gameObject;
                    }
                }
            }
            return foundTarget;
        }

        public abstract bool IsAchievableGiven(WorldState worldState);//For the planner

        /// <summary>
        /// Inserts actions into the plan that find the given Item, the Item will not be taken from a chest
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="requiredItem"></param>
        /// <returns></returns>
        protected Node GetRequiredItemNoChest(Node parent, Item requiredItem) //Returns a plan that will collect the required items, returns null if no such plan exists
        {
            // this will also try to pick up virtual items (future pick-ups)

            GetItem getItem = GetComponent<GetItem>();
            Node currentNode = parent;

            Node newNode = getItem.GetItemPlanNoChest(currentNode, requiredItem);
            if (newNode == null) return null;

            return newNode;
        }
        protected Bed FindClosestBed()
        {
            Bed[] beds = FindObjectsByType<Bed>(FindObjectsSortMode.None);
            Bed closestBed = null;
            float smallestDistnace = -1f;
            foreach(Bed bed in beds)
            {
                if (bed.GetComponent<FactionMembership>() == null) continue;
                if (bed.GetComponent<FactionMembership>().Faction != GetComponent<FactionMembership>().Faction) continue;

                float distance = (float)DistanceCalculator.CalculateDistance(transform.position, bed.transform.position);
                if (smallestDistnace < 0)
                {
                    closestBed = bed;
                    smallestDistnace = distance;
                }
                if(distance < smallestDistnace)
                {
                    closestBed = bed;
                    smallestDistnace = distance;
                }
            }
            return closestBed;
        }
        /// <summary>
        /// Inserts actions into the plan that find the given Item, the Item can be crated, picked up from a pick up, or taken from a chest
        /// </summary>
        protected Node GetRequiredItem(Node parent, Item requiredItem) //Returns a plan that will collect the required items, returns null if no such plan exists
        {
            GetItem getItem = GetComponent<GetItem>();
            Node currentNode = parent;

            Node newNode = getItem.GetItemPlan(currentNode, requiredItem);
            if (newNode == null) return null;
            currentNode = newNode;

            return currentNode;
        }
        protected bool HasItems(List<Item> items)
        {
            List<Item> inventoryItems = new List<Item>(GetComponent<Inventory>().Items);
            foreach(Item item in items)
            {
                if (!inventoryItems.Remove(item))
                    return false;
            }
            return true;
        } 
        protected bool HasItems(WorldState state, List<List<ItemTags>> tagsOfItems)
        {
            List<int> shuffledItems = state.myInventory;
            shuffledItems.Shuffle();
            foreach(List<ItemTags> tags in tagsOfItems)
            {
                for(int i = shuffledItems.Count-1; i < 0; i--)
                {
                    Item item = World.GetItemFromId(shuffledItems[i]);
                    if (item.HasTags(tags))
                    {
                        shuffledItems.RemoveAt(i);
                    }
                    else
                        return false;
                }
            }
            return true;
        }
        protected bool HasItem(WorldState state, List<ItemTags> tags)
        {
            List<int> shuffledItems = new List<int>(state.myInventory);
            shuffledItems.Shuffle();
            for (int i = shuffledItems.Count - 1; i >= 0; i--)
            {
                Item item = World.GetItemFromId(shuffledItems[i]);
                if (item.HasTags(tags))
                    return true;
            }
            return false;
        }
        
        protected Equipment GetEquippedItem(List<ItemTags> tags)
        {
            foreach (Equipment equipment in GetComponent<EquipmentManager>().Equipments)
            {
                if (equipment == null) continue;

                if (equipment.HasTags(tags))
                    return equipment;
            }
            return null;
        }
        protected Equipment GetEquippedItem(Item item)
        {
            foreach (Equipment equipment in GetComponent<EquipmentManager>().Equipments)
            {
                if (equipment == null) continue;

                if (equipment == item)
                    return equipment;
            }

            Debug.Log("ITEM <" + item.itemName + "> NOT FOUND!!!!");
            return null;
        }
        /// <summary>
        /// Inserts actions into the plan that find an Item that has given tags, the Item can be crated, picked up from a pick up, or taken from a chest
        /// </summary>
        protected Node GetRequiredItemWithTags(Node parent, List<ItemTags> tags) //Returns a plan that will collect the required items, returns null if no such plan exists
        {
            GetItem getItem = GetComponent<GetItem>();
            Node currentNode = parent;

            Node newNode = null;
            List<Item> shufledItems = new List<Item>(World.ItemList);
            shufledItems.Shuffle();
            foreach (Item item in shufledItems) 
            {
                if (!item.HasTags(tags))
                    continue;
                newNode = getItem.GetItemPlan(currentNode, item);
                if (newNode != null) 
                    break;
            }

            if (newNode == null) return null;
            currentNode = newNode;

            return currentNode;
        }
        /// <summary>
        /// Inserts actions into the plan that find all given Items, the Items can be crated, picked up from a pick up, or taken from a chest
        /// </summary>
        protected Node GetRequiredItems(Node parent, List<Item> requiredItems) //Returns a plan that will collect the required items, returns null if no such plan exists
        {
            GetItem getItem = GetComponent<GetItem>();
            Node currentNode = parent;

            foreach (Item item in requiredItems)
            {
                Node newNode = getItem.GetItemPlan(currentNode, item);
                if (newNode == null) return null;
                currentNode = newNode;
            }
            return currentNode;
        }
        protected void TakeItem(Inventory inventory, Item item)
        {
            if (!inventory.HasEmptySpace(1))
            {
                DropRandomItemFromInventory(inventory);
            }
            inventory.AddItem(item);
        }
        protected void DropRandomItemFromInventory(Inventory inventory)
        {
            int index = Random.Range(0, inventory.GetCapacity());
            GameObject itemPickUpPrefab = Resources.Load<GameObject>("Prefabs/Items/Item");
            GameObject itemDrop = Instantiate(itemPickUpPrefab);
            itemDrop.GetComponent<ItemPickup>().item = inventory.Items[index];
            inventory.RemoveItem(index);
            itemDrop.transform.position = this.transform.position;
        }
        protected Item FindItemWithTags(List<ItemTags> tags, IEnumerable<Item> items)
        {
            foreach(Item item in items)
            {
                if (item == null) continue;
                if (item.HasTags(tags))
                    return item;
            }
            return null;
        }

        protected bool UseItem(List<ItemTags> tags)
        {
            Inventory myInventory = GetComponent<Inventory>();
            for(int i = 0; i < myInventory.Items.Length; i++)
            {
                Item item = myInventory.Items[i];
                if (item.HasTags(tags))
                {
                    item.Use(gameObject);
                    if (item.singleUse)
                        myInventory.RemoveItem(item);
                    return true;
                }
            }
            return false;
        }
        protected Equipment EquipItem(List<ItemTags> tags)
        {
            Inventory inventory = GetComponent<Inventory>();
            EquipmentManager equipmentManager = GetComponent<EquipmentManager>();

            Equipment item = (Equipment)FindItemWithTags(tags, inventory.Items);
            //Debug.Log("Item is: " + test.itemName);
            if (item == null) return null;
            EquipItem(inventory, equipmentManager, item);
            return item;
        }
        protected Equipment EquipItem(Item item)
        {
            Inventory inventory = GetComponent<Inventory>();
            EquipmentManager equipmentManager = GetComponent<EquipmentManager>();
            //Debug.Log("Item is: " + test.itemName);
            if (item == null) return null;
            EquipItem(inventory, equipmentManager, item);
            return (Equipment)item;
        }
        protected bool EquipItem(Inventory inventory, EquipmentManager equipmentManager, Item item)
        {
            if (item == null)
                return false;
            if (!inventory.HasItem(item))
            {
                Debug.Log("Ai: npc doesn't have the item it wants to equip");
                return false;
            }
            for(int i = 0; i < equipmentSlots; i++)
            {
                if(equipmentManager.CanEquipItem(item, i))
                {
                    equipmentManager.EquipItem(item, i);
                    inventory.RemoveItem(item);
                    return true;
                }
            }
            return false;
        }
        protected void UnequipItem(List<ItemTags> tags)
        {
            Inventory inventory = GetComponent<Inventory>();
            EquipmentManager equipmentManager = GetComponent<EquipmentManager>();

            Item item = FindItemWithTags(tags, equipmentManager.Equipments);
            if (item == null) return;
            Unequip(inventory, equipmentManager, item);
        }
        protected void Unequip(Inventory inventory, EquipmentManager equipmentManager, Item item)
        {
            if (item == null) return;
            Equipment[] equipments = equipmentManager.Equipments;

            int index = 0;
            foreach(Item equipment in equipments)
            {
                if(item == equipment)
                {
                    TakeItem(inventory, item);
                    equipmentManager.UnEquipItem(index);
                    return;
                }
                index++;
            }
        }

        IEnumerator CreateTimedSpeechBubble()
        {
            SpriteRenderer renderer = transform.Find("SpeachBubble").GetComponent<SpriteRenderer>();


            renderer.sprite = speechBubbleSprite;

            float tick = 0.03f;
            float alphaChange = 0.5f;
            yield return new WaitForSeconds(speechBubbleDuration - tick/alphaChange);
            while(renderer.color.a != 0)
            {
                float newAlpha = renderer.color.a - alphaChange;
                renderer.color = new Color(1, 1, 1, Mathf.Max(newAlpha, 0));
                yield return new WaitForSeconds(tick);
            }
            renderer.sprite = null;
            renderer.color = new Color(1, 1, 1, 1);

        }

        /// <summary>
        /// Inserts actions into the plan that find the given Item
        /// </summary>
        protected Node GetTool(Node parent, Item requiredTool)
        {

            bool haveTool = false;
            List<int> items = parent.state.myInventory;
            // we get required tool
            foreach (int itemId in items)
            {
                if (World.GetItemFromId(itemId) == requiredTool) { haveTool = true; break; }
            }
            if (!haveTool)
            {
                Node newNode = GetRequiredItem(parent, requiredTool);
                return newNode;
            }
            return parent;
        }
    }
}
