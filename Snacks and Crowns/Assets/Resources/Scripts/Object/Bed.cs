using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : Interactible_Object
{
    GameObject sleepPoint;
    GameObject sleeper;
    Vector3 lastSleeperPosition;
    // Start is called before the first frame update
    void Start()
    {
        sleepPoint = transform.GetChild(1).gameObject;
    }
    public override void Interact(GameObject newSleeper)
    {
        if(sleeper == null)
        {
            sleeper = newSleeper;
            lastSleeperPosition = sleeper.transform.position;
            sleeper.transform.position = sleepPoint.transform.position;
            sleeper.transform.rotation = sleepPoint.transform.rotation;
            sleeper.layer = LayerMask.NameToLayer("Sleeper");
            sleeper.GetComponent<Player_State_Manager>().Change_State(Player_State.in_ui_menu);
        }
    }
    public override void Un_Interact(GameObject newSleeper)
    {
        sleeper.transform.position = lastSleeperPosition;
        sleeper.layer = LayerMask.NameToLayer("Default");
        sleeper.GetComponent<Player_State_Manager>().Change_State(Player_State.normal);
        sleeper = null;
    }

}
