using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;


/// <summary>
/// Changes relationship between NPC and playr factions to Alliance
/// </summary>
public class JoinPlayerFaction : DialogueComponentData<ActivateGolem>
{
    public override void InitializeComponent(GameObject player, GameObject listener)
    {
        Factions playerFaction = player.GetComponent<FactionMembership>().Faction;
        Factions npcFaction = listener.GetComponent<FactionMembership>().Faction;

        FactionState.ChangeFactionRelation(playerFaction, npcFaction, Relations.Alliance);

        if (playerFaction == Factions.One)
            FactionState.ChangeFactionRelation(Factions.Two, npcFaction, Relations.War);
        else 
            FactionState.ChangeFactionRelation(Factions.One, npcFaction, Relations.War);

    }
}
