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
    public float speed;
    public float nextWaypointDistance = 3f;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rb;
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        target = GameObject.Find("Player1(Clone)").transform;
        Debug.Log(target);

        InvokeRepeating("UpdatePath", 0f, 1f);
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
            return;
        }
        else reachedEndOfPath = false;

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 distance = direction * speed * Time.fixedDeltaTime;

        //rb.AddForce(force);
        rb.MovePosition((distance) + (Vector2)transform.position);


        if (direction != Vector2.zero) //rotates object in direction of movement
        {
            Quaternion rotate_to = Quaternion.LookRotation(Vector3.forward, direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotate_to, 10);
        }



        float distanceFromWaypoint = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if(distanceFromWaypoint < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }
}