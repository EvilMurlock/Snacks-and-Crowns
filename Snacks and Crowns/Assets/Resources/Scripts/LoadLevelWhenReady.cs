using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Loads the level after all players ready
/// </summary>
public class LoadLevelWhenReady : MonoBehaviour
{
    int players = 0;
    int readyPlayers = 0;
    public void AddPlayer()
    {
        players++;
    }
    public void ResetValues()
    {
        players = 0;
        readyPlayers = 0;
        StartGameDataHolder.ResetValues();
    }
    public void PlayerReady()
    {
        readyPlayers++;
        if(players == readyPlayers)
        {
            LoadLevel();
        }
    }
    void LoadLevel()
    {
        SceneManager.LoadScene(StartGameDataHolder.ChosenLevel);
    }
}
