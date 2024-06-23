using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
public class AstarGridRefresher : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        var guo = new GraphUpdateObject(GetComponentInChildren<BoxCollider2D>().bounds);
        guo.updatePhysics = true;
        AstarPath.active.UpdateGraphs(guo);
    }
}
