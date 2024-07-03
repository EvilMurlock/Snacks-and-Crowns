using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    public enum SpeachBubbleTypes
    {
        None,
        Charge,
        Fight,
        Gather,
        Sleep,
        Walk,
        GetItem
    }
    public abstract class Action : MonoBehaviour
    {
        public bool reusable = false; //can this action be used multiple times in the planner?, often set true for subactions

        float speachBubbleDuration = 3;
        public string actionName = "Action";
        public GameObject target;
        public List<string> targetTags = new List<string>();

        public bool running = false;
        public bool completed = false;
        protected NpcAi npcAi;
        Sprite speachBubbleSprite = null;
        protected SpeachBubbleTypes speachBubbleType = SpeachBubbleTypes.None;
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
            switch (speachBubbleType)
            {
                case SpeachBubbleTypes.None:
                    return;
                    break;
                case SpeachBubbleTypes.Charge:
                    spriteName = "bubble_charge";
                    break;
                case SpeachBubbleTypes.Fight:
                    spriteName = "bubble_fight";
                    break;
                case SpeachBubbleTypes.Gather:
                    spriteName = "bubble_gather";
                    break;
                case SpeachBubbleTypes.Sleep:
                    spriteName = "bubble_sleep";
                    break;
                case SpeachBubbleTypes.Walk:
                    spriteName = "bubble_walk";
                    break;
                case SpeachBubbleTypes.GetItem:
                    spriteName = "bubble_get_item";
                    break;
            }
            speachBubbleSprite = Resources.Load<Sprite>("Sprites/Speach Bubbles/" + spriteName);
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
        /*
        public virtual void Activate()
        {
            Activate(FindTarget());
        }*/
        public virtual void Activate(ActionData data)
        {
            StartCoroutine(CreateTimedSpeachBubble());
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
                    //Debug.Log("Target found");
                }
            }


            //DEBUG SECTION
            /*
            string allTgs = "";
            foreach (string t in targetTags)
            {
                allTgs += (t + ", ");
            }
            Debug.Log("Target with tags: "+ allTgs);
            Debug.Log("\n Found: " + foundTarget.name);
            */
            return foundTarget;
        }

        public abstract bool IsAchievableGiven(WorldState worldState);//For the planner

        protected Node GetRequiredItemNoChest(Node parent, Item requiredItem) //Returns a plan that will colect the required items, returns null if no such plan exists
        {
            // this will also try to pick up virtual items (future pick-ups)

            GetItem getItem = GetComponent<GetItem>();
            Node currentNode = parent;

            Node newNode = getItem.GetItemPlanNoChest(currentNode, requiredItem);
            if (newNode == null) return null;

            return newNode;
        }

        protected Node GetRequiredItem(Node parent, Item requiredItem) //Returns a plan that will colect the required items, returns null if no such plan exists
        {
            GetItem getItem = GetComponent<GetItem>();
            Node currentNode = parent;

            Node newNode = getItem.GetItemPlan(currentNode, requiredItem);
            if (newNode == null) return null;
            currentNode = newNode;

            return currentNode;
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
        protected Node GetRequiredItemWithTags(Node parent, List<ItemTags> tags) //Returns a plan that will colect the required items, returns null if no such plan exists
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

        protected Node GetRequiredItems(Node parent, List<Item> requiredItems) //Returns a plan that will colect the required items, returns null if no such plan exists
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
        protected Equipment EquipItem(List<ItemTags> tags)
        {
            Inventory inventory = GetComponent<Inventory>();
            EquipmentManager equipmentManager = GetComponent<EquipmentManager>();


            Item test = FindItemWithTags(tags, inventory.Items);
            Debug.Log("Item is: " + test.itemName);
            Equipment item = (Equipment)test;//FindItemWithTags(tags, inventory.Items);
            if (item == null) return null;
            EquipItem(inventory, equipmentManager, item);
            return item;
        }
        protected bool EquipItem(Inventory inventory, EquipmentManager equipmentManager, Item item)
        {
            if (item == null)
                return false;
            if (!inventory.HasItem(item))
            {
                Debug.Log("Ai: npc doesnt have the item it wants to equip");
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
            EquipItem(inventory, equipmentManager, item);
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

        IEnumerator CreateTimedSpeachBubble()
        {
            SpriteRenderer renderer = transform.Find("SpeachBubble").GetComponent<SpriteRenderer>();


            renderer.sprite = speachBubbleSprite;

            float tick = 0.03f;
            float alphaChange = 0.5f;
            yield return new WaitForSeconds(speachBubbleDuration - tick/alphaChange);
            while(renderer.color.a != 0)
            {
                float newAlpha = renderer.color.a - alphaChange;
                renderer.color = new Color(1, 1, 1, Mathf.Max(newAlpha, 0));
                yield return new WaitForSeconds(tick);
            }
            renderer.sprite = null;
            renderer.color = new Color(1, 1, 1, 1);

        }
    }
}
