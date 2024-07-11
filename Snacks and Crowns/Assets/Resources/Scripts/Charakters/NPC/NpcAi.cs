using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class NpcAi : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform target;
    public float nextWaypointDistance = 0.3f;
    public float lastWaypointDistanceDefault = 0.8f;

    public float lastWaypointDistance;
    Path path;
    int currentWaypoint = 0;
    public bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rb;
    Movement movement;
    void Start()
    {
        lastWaypointDistance = lastWaypointDistanceDefault;
        movement = GetComponent<Movement>();
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        //target = GameObject.Find("Player1(Clone)").transform;
        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }
    void UpdatePath()
    {
        if (seeker.IsDone() && target != null)
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        else
        {
            movement.ChangeMovementDirection(Vector2.zero);
            path = null;
        }
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
            ReachedEnd();
            return;
        }
        else reachedEndOfPath = false;

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;

        movement.ChangeMovementDirection(direction);

        float distanceFromWaypoint = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        float distanceFromEnd = Vector2.Distance(rb.position, path.vectorPath[path.vectorPath.Count-1]);//HERE READ LAST WAIPOINT!!!!

        //Debug.Log("A*: Distance from waypoint = "+ distanceFromWaypoint);
        if (distanceFromWaypoint < nextWaypointDistance)
        {
            currentWaypoint++;
        }
        else if(distanceFromEnd < lastWaypointDistance)
        {
            ReachedEnd();
        }
    }
    void ReachedEnd()
    {
        if (target != null) {
            Vector2 direction = ((Vector2)target.position - rb.position).normalized;
            movement.RotateTowars(direction);//End of path not nececeraly insode of the actual object, use diferent end point
        }
        else target = null;
        reachedEndOfPath = true;
        movement.ChangeMovementDirection(Vector2.zero);
        //Debug.Log("reached End of path");
    }
    public void ChangeTarget(GameObject newTarget, float distanceFromTarget)
    {
        lastWaypointDistance = distanceFromTarget;
        if (newTarget == null)
            target = null;
        else
            target = newTarget.transform;
        //Debug.Log("Pathing target is now: " + target.name);

        reachedEndOfPath = false;
        path = null;
        UpdatePath();
    }

    public void ChangeTarget(GameObject newTarget)
    {

        //Debug.Log("New target is: " + newTarget);
        if (newTarget != null &&
            target == newTarget.transform)
        {
            //Debug.Log("New target is same as old");
            return;
        }
        ChangeTarget(newTarget, lastWaypointDistanceDefault);
        
    }
}