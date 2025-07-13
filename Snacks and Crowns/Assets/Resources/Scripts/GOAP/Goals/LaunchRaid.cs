using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
namespace GOAP
{
    public class LaunchRaid : Goal
    {
        float priority = 4;
        //float raidCooldown = 5;
        float raidTimeout = 120;
        float timeOfLastRaid = 0;
        public override void Start()
        {
            MaxPlanDepth = 1;
        }
        public bool IsCompleted()
        {
            //Debug.Log("Raid start time: " + timeOfLastRaid + " timeout time: "+raidTimeout + " Curent time: "+ Time.timeSinceLevelLoad);
            if (timeOfLastRaid < Time.timeSinceLevelLoad - raidTimeout)
                return true;
            return false;
        }
        public override void Activate()
        {
            timeOfLastRaid = Time.timeSinceLevelLoad;
            base.Activate();
        }
        public override float CalculatePriority()
        {
            
            foreach (CharakterSheet charakter in Object.FindObjectsOfType<CharakterSheet>())
            {
                Factions myFaction = GetComponent<FactionMembership>().Faction;
                Factions theirFaction = charakter.GetComponent<FactionMembership>().Faction;
                if (FactionState.GetFactionRelations(myFaction,theirFaction) == Relations.War)
                {
                    //Debug.Log("Enemies in range!!!");
                    return priority;
                }
            }
            //Debug.Log("No enemies in range!!!");
            return 0;
        }
        public GameObject GetRandomEnemy()
        {
            CharakterSheet[] characters = FindObjectsOfType<CharakterSheet>();
            List<CharakterSheet> enemies = new List<CharakterSheet> ();
            for (int i = characters.Length-1;  i >= 0; i--)
            {
                Factions myFaction = GetComponent<FactionMembership>().Faction;
                Factions theirFaction = characters[i].GetComponent<FactionMembership>().Faction;
                if (FactionState.GetFactionRelations(myFaction, theirFaction) == Relations.War)
                {
                    enemies.Add(characters[i]);       
                }
            }
            CharakterSheet chosenEnemy = enemies[Random.Range(0, enemies.Count)];


            return chosenEnemy.gameObject;
        }
        public override bool CompletedByState(WorldState state)
        {
            return state.completedGoals.Contains(this);
        }
    }
}