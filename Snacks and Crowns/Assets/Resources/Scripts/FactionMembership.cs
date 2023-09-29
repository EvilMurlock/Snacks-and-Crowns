using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactionMembership : MonoBehaviour
{
    [SerializeField]
    Factions factionDefault = Factions.None;
    [SerializeField]
    Factions faction;
    private void Start()
    {
        faction = factionDefault;
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
