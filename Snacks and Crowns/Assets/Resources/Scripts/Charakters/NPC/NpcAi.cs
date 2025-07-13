using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;


/// <summary>
/// Pathfinding management for an NPC, determines walk direction
/// and detects when the target was reached
/// </summary>
public class NpcAi : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform target;
    public float nextWaypointDistance = 0.3f;
    public float lastWaypointDistanceDefault = 0.8f; // used to be 0.8f

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
    /// <summary>
    /// Here we check which node we should travel to and determine our direction of travel
    /// </summary>
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
            movement.RotateTowards(direction);//End of path not necessarily inside of the actual object
        }
        else target = null;
        reachedEndOfPath = true;
        movement.ChangeMovementDirection(Vector2.zero);
    }
    public void ChangeTarget(GameObject newTarget, float distanceFromTarget)
    {
        lastWaypointDistance = distanceFromTarget;
        if (newTarget == null)
            target = null;
        else
            target = newTarget.transform;

        reachedEndOfPath = false;
        path = null;
        UpdatePath();
    }

    public void ChangeTarget(GameObject newTarget)
    {
        if (newTarget != null &&
            target == newTarget.transform)
        {
            return;
        }
        ChangeTarget(newTarget, lastWaypointDistanceDefault);
        
    }
}