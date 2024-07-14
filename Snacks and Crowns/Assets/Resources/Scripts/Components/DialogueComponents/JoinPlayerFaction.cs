using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;
public class JoinPlayerFaction : DialogueComponentData<ActivateGolem>
{
    public override void InicializeComponent(GameObject player, GameObject listener)
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
