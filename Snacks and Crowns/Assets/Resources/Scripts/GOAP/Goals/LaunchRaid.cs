using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GOAP
{
    public class LaunchRaid : Goal
    {
        float priority = 4;
        float raidCooldown = 5;
        float raidTimeout = 10;
        float timeOfLastRaid = 0;
        public bool IsCompleted()
        {
            if (timeOfLastRaid + raidTimeout >= Time.timeSinceLevelLoad)
                return true;
            return false;
        }
        public override float CalculatePriority()
        {/*
            if (timeOfLastRaid + raidCooldown >= Time.timeSinceLevelLoad)
                return 0;*/

            timeOfLastRaid = Time.timeSinceLevelLoad;
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
            var characters = Object.FindObjectsOfType<CharakterSheet>();
            CharakterSheet chosenEnemy = characters[Random.Range(0, characters.Length - 1)];
            Debug.Log("Random enemy is: "+ chosenEnemy.gameObject.name);

            return chosenEnemy.gameObject;
        }
        public override bool CompletedByState(WorldState state)
        {
            return state.completedGoals.Contains(this);
        }
    }
}