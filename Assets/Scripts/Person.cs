using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PerState //person state
{
    Default, //starting state of person. person will behave according to their specific type (kid, police, ice cream guy, etc).
    Attracted, //attracted to something, either player seducing them or at a world detail (ice cream truck, dance party, etc). will move towards source of attraction.
    Afraid, //scared of something, either player petitioning or at world detail (scary picture, loud noise, etc). will move away from source of scary.
    Suspicious //has started to see alien. timer begins and a visual indicator ("!?") appear above them. if timer runs out before distracted, GAME OVER.
}

public class Person : MonoBehaviour
{
    //configuration attributes: these attributes are meant to be fiddled with to make the person act differently, such as moving faster or spotting the alien quicker.
    [SerializeField] private float speedDefault = 1.0f; //speed at which person moves while in Default state
    [SerializeField] private float speedAttracted = 1.0f; //speed at which person moves while in Attracted state
    [SerializeField] private float speedAfraid = 1.0f; //speed at which person moves while in Afaid state
    [SerializeField] private float spottingTime = 10.0f; //time it takes person to fully notice alien. when spotTimer runs out, person has spotted alien and GAME OVER.

    [SerializeField] private float turnRate = 3.0f; 

    //state variables: these attributes are used and changed during the execution of the game.
    private PerState state = PerState.Default; //state of the person. see enum declaration for more.
    private Vector2 targetDefault = new Vector2(0, 0); //default target that the person will seek towards unless distracted, either by player, world detail, or alien.
    private Vector2 targetAttract = new Vector2(0, 0); //target which is 

    private Rigidbody2D rb;
    private GameObject player;

    //bookkeeping attributes: holder attributes and such. used mostly for convience.
    Vector3 vectorToTarget;
    float turnAngle;
    Quaternion turnQuat;

    //debug attributes: stuff used to make debugging easier.

    // Start is called before the first frame update
    void Start()
    {
        rb = transform.GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");


        Vector2 test = new Vector2(10, 1);
        Debug.Log(test.normalized);


    }

    // Update is called once per frame
    void Update()
    {
        TurnTowardsPoint(player.transform.position);

        //move forward
        rb.AddForce(Vector2.right * speedDefault * Time.deltaTime, ForceMode2D.Impulse); //move forwards* (*rightwards) at speedDefault times deltaTime

        #region Old Test Stuff

        //Debug.DrawLine(transform.position, player.transform.position);
        //Debug.DrawLine(transform.position, transform.position + transform.right, Color.green);

        //Vector2 toPlayer =  player.transform.position - transform.position;
        //Debug.DrawRay(transform.position, toPlayer.normalized, Color.blue);

        //forward vector
        //(a - b) vector
        //Debug.Log(Vector2.Angle(player.transform.position, toPlayer));

        //Vector3 newDirection = Vector3.RotateTowards(transform.right, toPlayer, 1.0f * Time.deltaTime, 0.0f);
        //Debug.Log("RotateTowards " + Vector3.RotateTowards(transform.right, toPlayer, 1.0f * Time.deltaTime, 0.0f));
        #endregion
    }

    //turn to "face" a point. more specifically, rotate the gameobject around the z-axis by the difference in angle between its position and the arbitrary point
    private void TurnTowardsPoint(Vector3 _point)
    {
        //turn to face target
        vectorToTarget = _point - transform.position;
        turnAngle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        turnQuat = Quaternion.AngleAxis(turnAngle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, turnQuat, Time.deltaTime * turnRate);
    }

    //property for state
    public PerState State
    {
        get
        {
            return state;
        }

        set
        {
            state = value;
        }
    }
}
