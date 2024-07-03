using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptibleCorutiner : MonoBehaviour
{

    // corutiner serves as intermedietary for corutines, we use it when the object that called a corutine will get destroyed before the corutine finishes,
    // which allows the corutine to finish without causing errors
    // to start a corutine we just need a monobehaviour
    public static ScriptibleCorutiner instance;

    void Start()
    {
        instance = this;
    }
}
