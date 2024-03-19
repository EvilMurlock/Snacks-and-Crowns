using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : Interactible
{
    public override bool LockMove { get { return true; } }
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
        }
    }
    public override void UnInteract(GameObject newSleeper)
    {
        sleeper.transform.position = lastSleeperPosition;
        sleeper.layer = LayerMask.NameToLayer("Default");
        sleeper = null;
    }

}
