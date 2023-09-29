using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public enum Factions
{
    One, Two, Goblin, None
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
            }
        }
        ChangeFactionRelation(Factions.One, Factions.Two, Relations.War);
    }
    static void ChangeFactionRelation(Factions aF, Factions bF, Relations relation)
    {
        int a = (int)aF;
        int b = (int)bF;
        factionRelations[a, b] = relation;
        factionRelations[b, a] = relation;
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
