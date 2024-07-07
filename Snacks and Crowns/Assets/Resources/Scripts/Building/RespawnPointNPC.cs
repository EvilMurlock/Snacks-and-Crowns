using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    [SerializeField]
    GameObject NPCPrefab;    
    [SerializeField]
    float respawnDuration;

    float timeUnitlRespawn = 60;
    GameObject npc;
    // Start is called before the first frame update
    private void Start()
    {
        timeUnitlRespawn = respawnDuration;
        Spawn();
    }
    void Spawn()
    {
        npc = Instantiate(NPCPrefab, transform.position, transform.rotation);
        npc.GetComponent<FactionMembership>().Faction = GetComponent<FactionMembership>().Faction;

        foreach(Job job in GetComponents<Job>())
        {
            job.SetGoalsOfAnNPC(npc);
        }
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
