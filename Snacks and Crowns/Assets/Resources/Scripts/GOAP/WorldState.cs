using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GOAP
{
    [System.Serializable]
    public class WorldState
    {
        protected Dictionary <string, object> states;
        public WorldState()
        {
            states = new Dictionary<string, object>();
        }
        public WorldState(WorldState copyStates)
        {
            states = new Dictionary<string, object>(copyStates.states);
        }
        public override string ToString()
        {
            string worldStateString = "";
            worldStateString += "World State:\n";
            foreach(KeyValuePair<string, object> pair in states)
            {
                worldStateString += pair.Key + "\t-> " + pair.Value.ToString();
            }
            return worldStateString;
        }
        public bool HasState(string key)
        {
            return states.ContainsKey(key);
        }
        public void AddState(string key, object value)
        {
            states.Add(key, value);
        }
        public void AddStates(WorldState aditionalStates)
        {
            foreach(KeyValuePair<string, object> pair in aditionalStates.GetStates())
            {
                ModifyState(pair.Key, pair.Value);
            }
        }

        public void ModifyState(string key, object value)
        {
            if (states.ContainsKey(key))
            {
                states[key] = value;
            }
            else
            {
                states.Add(key, value);
            }
        }
        public void RemoveState(string key)
        {
            if (states.ContainsKey(key))
                states.Remove(key);
        }
        public void SetState(string key, object value)
        {
            if (states.ContainsKey(key))
                states[key] = value;
            else states.Add(key, value);
        }
        public Dictionary<string, object> GetStates()
        {
            return states;
        }
        public bool CompletesGoal(WorldState goal) 
        {
            foreach(KeyValuePair<string, object> g in goal.GetStates())
            {
                if (!states.ContainsKey(g.Key) 
                    || !states[g.Key].Equals(g.Value))
                    //|| states[g.Key] != g.Value) 
                    return false;
            }
            return true;
        }

    }
}