using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] float walkSpeed = 1.0f;
    [SerializeField] float accelerationRate = 0.5f;
    [SerializeField] float yellRange = 1f;
    [SerializeField] float megaphoneRange = 2f;
    public Person[] people { get; set; }

    enum CurrentTool
    {
        Petition,
        Megaphone
    }

    private CurrentTool currentTool;

    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentTool = CurrentTool.Petition;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 movementInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (movementInput.sqrMagnitude > 0) Move(movementInput);

        if (Input.GetKey(KeyCode.E))
        {
            switch (currentTool)
            {
                case CurrentTool.Petition:
                    Petition();
                    break;

                case CurrentTool.Megaphone:
                    Megaphone();
                    break;

                default:
                    break;
            }
        }
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

    void Megaphone()
    {
        //get people withn megaphone range
        Person[] closeEnoughPeople = FindPeopleWithinRange(megaphoneRange);
        //scare them
        foreach (Person p in closeEnoughPeople)
            p.Scare();
        
    }

    void Petition()
    {
        //get people withn megaphone range
        Person[] closeEnoughPeople = FindPeopleWithinRange(yellRange);
        //scare them
        foreach (Person p in closeEnoughPeople)
            p.Scare();

    }

    /// <summary>
    /// returns an array of people within the given range
    /// </summary>
    /// <param name="range"></param>
    /// <returns></returns>
    Person[] FindPeopleWithinRange(float range)
    {
        var closeEnoughPeople = (from p in people
                                where Vector2.Distance(p.transform.position, transform.position) <= megaphoneRange
                                select p).ToArray();
        return closeEnoughPeople;
    }
}