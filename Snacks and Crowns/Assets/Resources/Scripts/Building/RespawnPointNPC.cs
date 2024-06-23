using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    [SerializeField]
    GameObject NPCPrefab;    
    [SerializeField]
    GatherJob job;
    [SerializeField]
    float respawnDuration;

    float timeUnitlRespawn = 0;
    GameObject npc;
    // Start is called before the first frame update
    private void Start()
    {
        timeUnitlRespawn = respawnDuration;
        Spawn();
    }
    void Spawn()
    {
        npc = GameObject.Instantiate(NPCPrefab);
        npc.GetComponent<FactionMembership>().Faction = GetComponent<FactionMembership>().Faction;
        job.SetGoalsOfAnNPC(npc);
    }
    // Update is called once per frame
    void Update()
    {
        if(npc == null)
        {
            timeUnitlRespawn -= Time.deltaTime;
            if (timeUnitlRespawn <= 0)
            {
                Spawn();
                timeUnitlRespawn = respawnDuration;
            }
        }
    }
}
