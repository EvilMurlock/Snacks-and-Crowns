using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GOAP
{
    public abstract class FightGoal : Goal
    {
        float agroDistance = 6;
        float priority = 10;
        List<GameObject> charactersInRange = new List<GameObject>();
        private void Start()
        {
            string name = "FightDetector";
            if (GameObject.Find(name) == null)
            {
                GameObject detector = new GameObject(name);
                detector.transform.SetParent(transform);
                detector.transform.position = transform.position;
                CircleCollider2D collider = detector.AddComponent<CircleCollider2D>();
                collider.radius = agroDistance;
                collider.isTrigger = true;
                Collision2D_Proxy proxy = detector.AddComponent<Collision2D_Proxy>();
                proxy.OnTriggerEnter2D_Action += TriggerEnter;
                proxy.OnTriggerExit2D_Action += TriggerExit;
            }
        }
        void TriggerEnter(Collider2D collision)
        {
            if(collision.gameObject.GetComponent<CharakterSheet>() != null)
            {
                Debug.Log("Adding this character: " + collision.gameObject.name);
                charactersInRange.Add(collision.gameObject);
            }
        }
        void TriggerExit(Collider2D collision)
        {
            charactersInRange.Remove(collision.gameObject);
        }
        public override float CalculatePriority()
        {
            foreach(GameObject charakter in charactersInRange)
            {
                Factions myFaction = GetComponent<FactionMembership>().Faction;
                Factions theirFaction = charakter.GetComponent<FactionMembership>().Faction;
                if (FactionState.GetFactionRelations(myFaction,theirFaction) == Relations.War)
                {
                    Debug.Log("Enemies in range!!!");
                    return priority;
                }
            }
            Debug.Log("No enemies in range!!!");
            return 0;
        }
        public override bool CanRun()
        {
            return EnemiesInRange();
        }
        bool EnemiesInRange()
        {
            return false;
        }
    }
}