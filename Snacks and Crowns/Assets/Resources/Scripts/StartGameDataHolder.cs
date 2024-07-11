using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerData
{
    public string controlScheme;
    public InputDevice deviceType;
    public Race race;
    public int face;
    public Factions faction;
    public PlayerData(string controlScheme, InputDevice deviceType, Race race, Factions faction, int face)
    {
        this.controlScheme = controlScheme;
        this.deviceType = deviceType;
        this.race = race;
        this.faction = faction;
        this.face = face;
    }
}
public static class StartGameDataHolder
{
    static string chosenLevel = "Level1";
    public static string ChosenLevel { get { return chosenLevel; } set { chosenLevel = value; } }
    static List<PlayerData> players = new List<PlayerData>();
    public static List<PlayerData> Players => players;
    public static void AddPlayer(string controlScheme, InputDevice deviceType, Race race, Factions faction, int face)
    {
        players.Add(new PlayerData(controlScheme, deviceType, race, faction, face));
    }

 
}
