using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


/// <summary>
/// remembers player data, both some character data and meta data, like the controls 
/// </summary>
public class PlayerData
{
    public string controlScheme;
    public InputDevice deviceType;
    public Race race;
    public int face;
    public Factions faction;
    public int index;
    public PlayerData(string controlScheme, InputDevice deviceType, Race race, Factions faction, int face, int index)
    {
        this.controlScheme = controlScheme;
        this.deviceType = deviceType;
        this.race = race;
        this.faction = faction;
        this.face = face;
        this.index = index;
    }
}

/// <summary>
/// manages player data, that are used to initialize the players and the chosen level
/// </summary>
public static class StartGameDataHolder
{
    static string chosenLevel = "Level1";
    public static string ChosenLevel { get { return chosenLevel; } set { chosenLevel = value; } }
    static List<PlayerData> players = new List<PlayerData>();
    public static List<PlayerData> Players => players;
    public static void AddPlayer(string controlScheme, InputDevice deviceType, Race race, Factions faction, int face, int index)
    {
        players.Add(new PlayerData(controlScheme, deviceType, race, faction, face, index));
    }
    public static void ResetValues()
    {
        players = new List<PlayerData>();
    }

}
