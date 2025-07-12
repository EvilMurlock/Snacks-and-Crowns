using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadLevelWhenReady : MonoBehaviour
{
    int players = 0;
    int readyPlayers = 0;
    public void AddPlayer()
    {
        players++;
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
