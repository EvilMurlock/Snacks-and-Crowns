using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptibleCorutiner : MonoBehaviour
{
    public static ScriptibleCorutiner instance;

    void Start()
    {
        instance = this;
    }
}
