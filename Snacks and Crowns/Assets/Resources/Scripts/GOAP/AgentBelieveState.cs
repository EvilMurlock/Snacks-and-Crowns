using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;
public class AgentBelieveState : MonoBehaviour
{
    WorldState agentBelieves = new WorldState();
    private void Awake()
    {
        GetComponent<Inventory>().onChangeInventory.AddListener(InventoryUpdate);
    }
    public WorldState AgentBelieves 
    {
        
        get {
            UpdateGeneralBelieves();
            return agentBelieves; }
    }
    public void ModifyAgentBelieves(string key, object value)
    {
        agentBelieves.ModifyState(key, value);
    }
    void UpdateGeneralBelieves()
    {
        WorldState generalBelieves = World.Instance.GetWorld();
        foreach(KeyValuePair<string, object> pair in generalBelieves.GetStates())
        {
            //Debug.Log("TEST: Key: " + pair.Key + " Value: " + pair.Value);
            agentBelieves.ModifyState(pair.Key, pair.Value);
        }
        agentBelieves.ModifyState("MyPosition", transform.position);
    }
    void InventoryUpdate(Inventory inventory)
    {
        List<int> inventoryId = new List<int>();
        foreach(Item item in inventory.Items)
        {
            inventoryId.Add(World.GetIdFromItem(item));
        }
        ModifyAgentBelieves("Inventory", inventoryId);
    }
}
