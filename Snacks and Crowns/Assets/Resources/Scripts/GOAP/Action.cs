using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    public abstract class Action<DataType> : MonoBehaviour
    {
        public bool reusable = false; //can this action be used multiple times in the planner?, often set true for subactions
        

        public string actionName = "Action";
        public GameObject target;
        public List<string> targetTags = new List<string>();

        public bool running = false;
        public bool completed = false;
        protected NpcAi npcAi;
        public virtual void Awake()
        {
            actionName = this.GetType().Name;

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
        public abstract void Activate(DataType data);
            /*
        {
            
            if (newTarget == null) target = FindTarget();
            else this.target = (GameObject)newTarget;
            running = true;
            completed = false;
            npcAi.ChangeTarget(target);

        }*/
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
            GetItem getItem = GetComponent<GetItem>();
            Node currentNode = parent;

            Node newNode = getItem.GetItemPlanNoChest(currentNode, requiredItem);
            if (newNode == null) return null;
            currentNode = newNode;

            return currentNode;
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
    }
}
