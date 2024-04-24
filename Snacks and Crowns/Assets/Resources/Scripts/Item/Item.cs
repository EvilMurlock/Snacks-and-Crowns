using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New_Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public Sprite icon;
    public string itemName;
    public string description;
    public int cost;
    [HideInInspector]
    public GameObject prefab;
    public float useDuration;
    public bool singleUse = false;
    [SerializeReference] private List<ComponentDataGeneric> componentDataUse;
    [SerializeReference] List<string> tags;

    [SerializeField]
    CraftingRecipe recipe;
    public CraftingRecipe Recipe { 
        get {
            if (recipe.ingredients == null || recipe.ingredients.Count == 0) return null;
            recipe.result = this;
            return recipe;       
        } }
    public List<string> Tags
    {
        get { return tags; }
    }
    public virtual void Use(GameObject player)
    {
        Debug.Log("Using " + itemName);
        GameObject corutiner = Instantiate((GameObject)Resources.Load("Prefabs/ScriptibleCorutiner"));
        corutiner.GetComponent<ScriptibleCorutiner>().StartCoroutine(UsingItem(player, corutiner));
        //ScriptibleCorutiner.instance.StartCoroutine(UsingItem(useDuration, player));
    }
    IEnumerator UsingItem(GameObject user, GameObject corutiner)
    {
        user.GetComponent<Movement>().Stun(useDuration);
        foreach (ComponentDataGeneric comData in componentDataUse)
        {
            if (comData.activateAtUse) comData.InicializeComponent(user, this);
        }
        yield return new WaitForSeconds(useDuration);
        foreach (ComponentDataGeneric comData in componentDataUse)
        {
            if(!comData.activateAtUse) comData.InicializeComponent(user, this);
        }
        Destroy(corutiner);
    }
    public void AddDataUse(ComponentDataGeneric data)
    {
        componentDataUse.Add(data);
    }
}