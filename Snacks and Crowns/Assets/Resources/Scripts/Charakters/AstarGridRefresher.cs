using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

/// <summary>
/// Updates the AstarGrid in the area of the collider, 
/// was used to during experiments trying to make NPCs walk around each other
/// is unsed as this aproach was not effective.
/// </summary>
public class AstarGridRefresher : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        var guo = new GraphUpdateObject(GetComponentInChildren<BoxCollider2D>().bounds);
        guo.updatePhysics = true;
        AstarPath.active.UpdateGraphs(guo);
    }
}
