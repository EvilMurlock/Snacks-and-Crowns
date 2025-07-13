using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// class used to store the faction of the GameObject
public class FactionMembership : MonoBehaviour
{
    [SerializeField]
    Factions faction;
    public Factions Faction
    {
        get { return faction; }
        set
        {
            faction = value;
        }
    }
}
