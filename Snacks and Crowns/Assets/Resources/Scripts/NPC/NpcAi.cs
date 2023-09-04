using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class NpcAi : MonoBehaviour
{
    // Start is called before the first frame update
    List<Item> inventory;
    int invetorySize = 9;

    public Transform target;
    public float nextWaypointDistance = 3f;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rb;
    Player_Movement movementScript;
    void Start()
    {
        movementScript = GetComponent<Player_Movement>();
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        //target = GameObject.Find("Player1(Clone)").transform;
        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }
    void UpdatePath()
    {
        if(seeker.IsDone())
        seeker.StartPath(rb.position, target.position, OnPathComplete);

    }
    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (path == null) return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            movementScript.ChangeMovementDirection(Vector2.zero);
            return;
        }
        else reachedEndOfPath = false;

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;

        movementScript.ChangeMovementDirection(direction);

        float distanceFromWaypoint = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if(distanceFromWaypoint < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }
    public void ChangeTarget(GameObject newTarget)
    {
        target = newTarget.transform;
    }
}