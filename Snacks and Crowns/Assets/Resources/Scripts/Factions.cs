using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public enum Factions
{
    One, Two, Goblin, None, Monster, Cat
}
public enum Relations
{
    Alliance, Neutral, War
}


public sealed class FactionState
{
    public static Relations[,] factionRelations;
    static FactionState()
    {
        string[] factionsNames = Enum.GetNames(typeof(Factions));
        factionRelations = new Relations[factionsNames.Length, factionsNames.Length];

        for(int i = 0; i < factionsNames.Length; i++)
        {
            for (int j = 0; j < factionsNames.Length; j++)
            {
                factionRelations[i, j] = Relations.Neutral;
                if(i != j &&
                    factionsNames[i] == "Monster")
                {
                    factionRelations[i, j] = Relations.War;
                }
            }
        }
        ChangeFactionRelation(Factions.One, Factions.Two, Relations.War);
    }
    public static void ChangeFactionRelation(Factions aF, Factions bF, Relations relation)
    {
        int a = (int)aF;
        int b = (int)bF;
        factionRelations[a, b] = relation;
        factionRelations[b, a] = relation;
        if (aF == Factions.One || aF == Factions.Two)
            UpdateRelatedFactionRelations(aF);
        if (bF == Factions.One || bF == Factions.Two)
            UpdateRelatedFactionRelations(bF);

        PrintRelationsMatrix();
    }

    /// <summary>
    /// Changes factions allied or at war with players, to be allied and at war with the player factions allies
    /// </summary>
    static void UpdateRelatedFactionRelations(Factions changedFaction)
    {
        if (factionRelations[(int)changedFaction, (int)Factions.One] == Relations.War)
        {
            ChangeRelationsOnaAlliesOf(changedFaction, Factions.One, Relations.War);
        }
        if (factionRelations[(int)changedFaction, (int)Factions.One] == Relations.Alliance)
        {
            ChangeRelationsOnaAlliesOf(changedFaction, Factions.One, Relations.Alliance);
        }
        if (factionRelations[(int)changedFaction, (int)Factions.Two] == Relations.War)
        {
            ChangeRelationsOnaAlliesOf(changedFaction, Factions.Two, Relations.War);
        }
        if (factionRelations[(int)changedFaction, (int)Factions.Two] == Relations.Alliance)
        {
            ChangeRelationsOnaAlliesOf(changedFaction, Factions.Two, Relations.Alliance);
        }
    }
    static void ChangeRelationsOnaAlliesOf(Factions myFaction, Factions enemyFaction, Relations newRelation)
    {
        for (int i = 0; i < Enum.GetValues(typeof(Factions)).Length; i++)
        {
            if (factionRelations[i, (int)enemyFaction] == Relations.Alliance)
            {
                ChangeFactionRelation(myFaction, (Factions)i, newRelation);
            }
        }
    }
    public static Relations GetFactionRelations(Factions aF, Factions bF)
    {
        int a = (int)aF;
        int b = (int)bF;
        return factionRelations[a, b];
    }

    public static void PrintRelationsMatrix()
    {
        string[] factionsNames = Enum.GetNames(typeof(Factions));

        string matrix = "";

        for (int i = 0; i < factionsNames.Length; i++)
        {
            matrix += Enum.GetName(typeof(Factions), i) + " => ";
            for (int j = 0; j < factionsNames.Length; j++)
            {
                matrix += Enum.GetName(typeof(Factions), j) +": "+ factionRelations[i,j].ToString() + " | "; ;
            }
            matrix += "\n";
        }
        Debug.Log(matrix);
    }
}
