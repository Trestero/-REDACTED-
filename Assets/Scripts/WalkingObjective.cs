using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D))]
public class WalkingObjective : MonoBehaviour
{
    [Header("Path-following Properties")]
    [SerializeField, Tooltip("The maximum radius at which the character will begin seeking the next point")]
    float goalThreshold = 1.0f;

    [SerializeField]
    float walkSpeed = 2.0f;

    [SerializeField] List<Transform> startingPath;

    Queue<Vector2> path;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        path = new Queue<Vector2>();

        UpdatePath(startingPath);
    }

    // Update is called once per frame
    void Update()
    {
        FollowPath();
    }

    void FollowPath()
    {
        if (path.Count > 0)
        {
            Seek(path.Peek());

            if ((path.Peek() - (Vector2)transform.position).sqrMagnitude <= (goalThreshold * goalThreshold))
            {
                path.Enqueue(path.Dequeue()); //move point from end of queue to start of queue
            }
        }
    }

    void Seek(Vector2 goal)
    {
        Vector2 delta = goal - (Vector2)transform.position;
        Vector2 direction = delta - rb.velocity;
        rb.AddForce(direction * walkSpeed, ForceMode2D.Impulse);

        LimitSpeed();
    }

    // Limits RB velocity to walkspeed
    void LimitSpeed()
    {
        float speed = rb.velocity.magnitude;
        if (speed > walkSpeed)
        {
            // Applies an impulse to slow the object down
            Vector2 counterForce = -rb.velocity.normalized;

            counterForce *= (rb.mass * (speed - walkSpeed));
            rb.AddForce(counterForce, ForceMode2D.Impulse);
        }
    }

    void UpdatePath(List<Transform> points)
    {
        foreach (var point in points)
        {
            path.Enqueue(point.position);
        }
    }
}
