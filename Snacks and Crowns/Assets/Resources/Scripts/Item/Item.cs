using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ItemTags
{
    meleeWeapon,
    rangedWeapon,
    armor,
    healing,
    attackBuff,
    armorBuff,
    attackSpell,
    defenceSpell
}
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
    [SerializeReference] List<ItemTags> tags;
    [SerializeField]
    CraftingRecipe recipe;
    public CraftingRecipe Recipe { 
        get {
            if (recipe.ingredients == null || recipe.ingredients.Count == 0) return null;
            recipe.result = this;
            return recipe;       
        } }
    [HideInInspector]
    public List<ItemTags> Tags
    {
        get { return tags; }
    }
    public virtual void Use(GameObject player)
    {
        //Debug.Log("Using " + itemName);
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
    public bool HasTags(List<ItemTags> otherTags)
    {
        foreach (ItemTags tag in otherTags)
        {
            if (!tags.Contains(tag))
                return false;
        }
        return true;
    }
    public static void DropItem(Item item, Vector3 position)
    {
        if (item == null) return;
        GameObject item_object = Instantiate((GameObject)Resources.Load("Prefabs/Items/Item"), position, Quaternion.Euler(0,0,Random.Range(0,360)));
        item_object.GetComponent<ItemPickup>().item = item;
        item_object.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-100, 100), Random.Range(-100, 100)).normalized * 500);
        //item_object.transform.rotation = this.transform.rotation;
    }
}