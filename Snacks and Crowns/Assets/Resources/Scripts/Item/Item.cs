using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New_Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public Sprite icon;
    public string item_name;
    public string description;
    public int cost;
    [HideInInspector]
    public GameObject prefab;
    public float useDuration;
    public bool singleUse = false;
    [SerializeReference] private List<ComponentDataGeneric> componentDataUse;
    [SerializeReference] List<string> tags;

    public List<string> Tags
    {
        get { return tags; }
    }
    public virtual void Use(GameObject player)
    {
        Debug.Log("Using " + item_name);
        GameObject corutiner = Instantiate((GameObject)Resources.Load("Prefabs/ScriptibleCorutiner"));
        corutiner.GetComponent<ScriptibleCorutiner>().StartCoroutine(UsingItem(player, corutiner));
        //ScriptibleCorutiner.instance.StartCoroutine(UsingItem(useDuration, player));
    }
    IEnumerator UsingItem(GameObject player, GameObject corutiner)
    {
        player.GetComponent<Player_Movement>().Stun(useDuration);
        foreach (ComponentDataGeneric comData in componentDataUse)
        {
            if (comData.activateAtUse) comData.InicializeComponent(player, this);
        }
        yield return new WaitForSeconds(useDuration);
        foreach (ComponentDataGeneric comData in componentDataUse)
        {
            if(!comData.activateAtUse) comData.InicializeComponent(player, this);
        }
        Destroy(corutiner);
    }
    public void AddDataUse(ComponentDataGeneric data)
    {
        componentDataUse.Add(data);
    }
}