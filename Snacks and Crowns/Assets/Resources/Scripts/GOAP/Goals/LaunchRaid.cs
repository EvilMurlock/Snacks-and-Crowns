using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
namespace GOAP
{
    public class LaunchRaid : Goal
    {
        float priority = 4;
        float raidTimeout = 60;
        float timeOfLastRaid = 0;
        public override void Start()
        {
            MaxPlanDepth = 1;
        }
        public bool IsCompleted()
        {
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
            
            foreach (CharakterSheet character in Object.FindObjectsOfType<CharakterSheet>())
            {
                Factions myFaction = GetComponent<FactionMembership>().Faction;
                Factions theirFaction = character.GetComponent<FactionMembership>().Faction;
                if (FactionState.GetFactionRelations(myFaction, theirFaction) == Relations.War)
                {
                    return priority;
                }
            }
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