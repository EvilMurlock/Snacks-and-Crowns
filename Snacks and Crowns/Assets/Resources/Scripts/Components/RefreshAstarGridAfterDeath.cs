using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
public class RefreshAstarGridAfterDeath : MonoBehaviour
{
    void Start()
    {
        GetComponent<Damageable>().death.AddListener(CallUpdateAstarGrid);
    }
    void CallUpdateAstarGrid()
    {
        GameObject corutiner = Instantiate((GameObject)Resources.Load("Prefabs/ScriptibleCorutiner"));
        corutiner.GetComponent<ScriptibleCorutiner>().StartCoroutine(UpdateAstarGrid(GetComponentInChildren<CircleCollider2D>().bounds, corutiner));
    }
    IEnumerator UpdateAstarGrid(Bounds bounds, GameObject corutiner)
    {
        yield return new WaitForSeconds(1);
        var guo = new GraphUpdateObject(bounds);
        guo.updatePhysics = true;
        AstarPath.active.UpdateGraphs(guo);
        Destroy(corutiner);
    }
}
