using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{

    class ActionDataCraftItem : ActionData
    {
        public CraftingRecipe recipe;
        public GameObject craftingPiece;

        public ActionDataCraftItem(CraftingRecipe recipe, GameObject craftingPiece)
        {
            this.recipe = recipe;
            this.craftingPiece = craftingPiece;
        }
    }
    /// <summary>
    /// Crafts an Item
    /// </summary>
    public class CraftItem : SubAction
    {
        CraftingRecipe currentRecipy;
        public override void Awake()
        {
            speechBubbleType = SpeechBubbleTypes.GetItem;
            base.Awake();
        }
        public override void Start()
        {
            base.Start();
        }
        public override void Tick()
        {
            if (target == null) Deactivate();
            if (npcAi.reachedEndOfPath)
            {
                // Do the crafting
                if (HasItems(currentRecipy.ingredients))
                {
                    Inventory inventory = GetComponent<Inventory>();
                    foreach (Item item in currentRecipy.ingredients)
                    {
                        inventory.RemoveItem(item);
                    }
                    inventory.AddItem(currentRecipy.result);
                    Complete();
                }
                else
                {
                    Deactivate();
                }
            }
        }
        public override void Activate(ActionData arg)
        {
            ActionDataCraftItem data = (ActionDataCraftItem)arg;

            target = data.craftingPiece;
            currentRecipy = data.recipe;

            npcAi.ChangeTarget(target, 1f);
            base.Activate(arg);
        }
        public override void Deactivate()
        {
            //Unequip axe
            running = false;
            npcAi.ChangeTarget(null);
        }
        public override void Complete()
        {

            running = false;
            completed = true;
        }
        public override bool IsAchievableGiven(WorldState worldState)//For the planner
        {
            bool achievable = false;
            foreach (TagSystem tagSys in GameObject.FindObjectsByType<TagSystem>(FindObjectsSortMode.None))
            {
                if (tagSys.HasTag("forge") || tagSys.HasTag("workshop") || tagSys.HasTag("anvil")) achievable = true;
            }

            return achievable;
        }

        public override List<Node> OnActionCompleteWorldStates(Node parent_)//Tells the planer how the world state will change on completion
        {
            List<Node> possibleNodes = new List<Node>();

            return possibleNodes;
        }

    }
}