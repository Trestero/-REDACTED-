using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] float walkSpeed = 1.0f;
    [SerializeField] float accelerationRate = 0.5f;

    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 movementInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (movementInput.sqrMagnitude > 0) Move(movementInput);
    }

    void Move(Vector2 input)
    {
        float impulse = rb.mass * (walkSpeed * accelerationRate) * Time.deltaTime;
        rb.AddForce(input.normalized * impulse, ForceMode2D.Impulse);
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
}