using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactionMembership : MonoBehaviour
{
    [SerializeField]
    Factions faction;
    private void Start()
    {
    }
    public Factions Faction
    {
        get { return faction; }
        set
        {
            faction = value;
        }
    }
}
