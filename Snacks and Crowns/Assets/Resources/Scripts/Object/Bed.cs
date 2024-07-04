using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : Interactible
{

    GameObject sleepPoint;
    GameObject sleeper;
    Vector3 lastSleeperPosition;
    int sleeperLayer;
    // Start is called before the first frame update
    void Start()
    {
        lockMove = true;
        sleepPoint = transform.GetChild(1).gameObject;
    }

    public bool IsFree()
    {
        return sleeper == null;
    }
    public override void Interact(GameObject newSleeper)
    {
        if (sleeper == null)
        {
            lockMove = true;
            sleeper = newSleeper;
            lastSleeperPosition = sleeper.transform.position;
            sleeper.transform.position = sleepPoint.transform.position;
            sleeper.transform.rotation = sleepPoint.transform.rotation;
            sleeperLayer = sleeper.layer;
            sleeper.layer = LayerMask.NameToLayer("Sleeper");
            sleeper.AddComponent<Regeneration>();
        }
        else
            lockMove = false;
    }
    public override void UnInteract(GameObject newSleeper)
    {
        if (sleeper != newSleeper)
            return;
        sleeper.transform.position = lastSleeperPosition;
        Destroy(sleeper.GetComponent<Regeneration>());
        sleeper.layer = sleeperLayer;
        sleeper = null;
    }

}
