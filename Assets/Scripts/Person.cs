using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum PerState //person state
{
    Default, //starting state of person. person will behave according to their specific type (kid, police, ice cream guy, etc).
    Attracted, //attracted to something, either player seducing them or at a world detail (ice cream truck, dance party, etc). will move towards source of attraction.
    Afraid, //scared of something, either player petitioning or at world detail (scary picture, loud noise, etc). will move away from source of scary.
    Suspicious, //has started to see alien. will begin slowly walking towards alien. if reaches alien, GAME OVER.
    Leaving
}

public class Person : MonoBehaviour
{
    //configuration attributes: these attributes are meant to be fiddled with to make the person act differently, such as moving faster or spotting the alien quicker.
    [SerializeField] private float speedDefault = 10.0f; //speed at which person moves while in Default state
    [SerializeField] private float speedAfraid = 10.0f; //speed at which person moves while in Afaid state
    [SerializeField] private float speedSus = 2.0f; //speed at which person moves while in Afaid state
    [SerializeField] private float turnRate = 5.0f; //rate at which person turns around to face target

    [SerializeField] private float closeDistanceSqrRt = 0.7f; //**the square root of** how close gameobject has to be to a node to "reach" it
    [SerializeField] private float alienReachDistanceSqrRt = 0.5f; //**the square root of** how close gameobject has to be to alien to "reach" it

    [SerializeField] private int nodeVisits = 10; //how many nodes to visit before leaving


    //state variables: these attributes are used and changed during the execution of the game.
    private PerState state = PerState.Default; //state of the person. see enum declaration for more.
    private Vector2 targetCurrent = new Vector2(0, 0); //default target that the person will seek towards unless distracted, either by player, world detail, or alien.
    private Vector2 targetNext = new Vector2(0, 0); //target that gets overwritten by other scripts
    private int xIndex;
    private int yIndex;

    private Rigidbody2D rb; //rigidbody for this gameobject
    private GameObject player; //reference to player gameobject
    private NodeMesh nodeMesh; //reference to node mesh script attached to Node_Mesh gameobject

    private int nodesVisisted = 0; //how many nodes has this person visited

    System.Random randy = new System.Random();


    //bookkeeping attributes: holder attributes and such. used mostly for convience.
    private Vector3 vectorToTarget;
    private float turnAngle;
    private Quaternion turnQuat;

    private List<Vector2> possibleNext;
    private Vector2 nearestParkNode;
    private Vector2 nearestExitNode;

    //debug attributes: stuff used to make debugging easier.

    // Start is called before the first frame update
    void Start()
    {
        rb = transform.GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        nodeMesh = GameObject.FindGameObjectWithTag("NodeMesh").GetComponent<NodeMesh>();

        EnterPark(); //find nearest park node and go to it
    }

    // Update is called once per frame
    void Update()
    {
        //turn to target
        TurnTowardsPoint(targetCurrent); //rotate the gameobject's towards an arbitrary point

        switch(state)
        {
            case PerState.Default:
                //move forward
                rb.AddForce(transform.right.normalized * speedDefault * Time.deltaTime, ForceMode2D.Impulse); //move forwards* (*rightwards) at speedDefault times deltaTime

                //if gameobject is within a certain distance (closeDistanceSqrRt) of an arbitrary point, return true 
                if ((targetCurrent - (Vector2)transform.position).sqrMagnitude <= (closeDistanceSqrRt * closeDistanceSqrRt))
                {
                    nodesVisisted++;

                    if (nodesVisisted >= nodeVisits) //if visited enough nodes in the park, time to leave
                    {
                        ExitPark();
                    }
                    else
                    {
                        WanderPark();
                    }

                }

                break;

            case PerState.Attracted:
                //if gameobject is within a certain distance (closeDistanceSqrRt) of an arbitrary point, return true 
                if ((targetCurrent - (Vector2)transform.position).sqrMagnitude <= (closeDistanceSqrRt * closeDistanceSqrRt))
                {

                }
                else
                {
                    rb.AddForce(transform.right.normalized * speedDefault * Time.deltaTime, ForceMode2D.Impulse); //move forwards* (*rightwards) at speedDefault times deltaTime
                }


                break;

            case PerState.Afraid:
                rb.AddForce(transform.right.normalized * speedAfraid * Time.deltaTime, ForceMode2D.Impulse); //move forwards* (*rightwards) at speedDefault times deltaTime
                break;

            case PerState.Leaving:
                rb.AddForce(transform.right.normalized * speedDefault * Time.deltaTime, ForceMode2D.Impulse); //move forwards* (*rightwards) at speedDefault times deltaTime

                if ((targetCurrent - (Vector2)transform.position).sqrMagnitude <= (closeDistanceSqrRt * closeDistanceSqrRt))
                {
                    Destroy(this.gameObject);
                }
                break;

            case PerState.Suspicious:
                rb.AddForce(transform.right.normalized * speedSus * Time.deltaTime, ForceMode2D.Impulse); //move forwards* (*rightwards) at speedDefault times deltaTime

                if ((targetCurrent - (Vector2)transform.position).sqrMagnitude <= (closeDistanceSqrRt * closeDistanceSqrRt))
                {
                    Application.Quit();
                }
                break;
        }
        
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

    public void Scare()
    {
        state = PerState.Afraid;

        ExitPark();
        //set target to nearest exit node
    }

    public void Attract(Vector2 attractPoint)
    {
        //if in afraid state, ignore attract 
        if(state == PerState.Afraid)
        {
            return;
        }

        state = PerState.Attracted;
        targetCurrent = attractPoint;

        //set target to attractPoint

    }

    //go to nearest park node and start wandering
    public void EnterPark()
    {
        nearestParkNode = new Vector2(100, 100);
        for(int y = 0; y < 5; y++)
        {
            for(int x = 0; x < 5; x++)
            {
                if ((nodeMesh.ParkMesh[x,y] - (Vector2)transform.position).sqrMagnitude < (nearestParkNode - (Vector2)transform.position).sqrMagnitude)
                {
                    nearestParkNode = nodeMesh.ParkMesh[x, y];
                    xIndex = x;
                    yIndex = y;
                }
            }
        }

        state = PerState.Default;
        targetCurrent = nearestParkNode;
    }

    //find next node to walk to
    public void WanderPark()
    {

        //make list of valid nodes to visit
        //randomly select a node to visit
        //if node selected is (2,2): alien spotted
        possibleNext = new List<Vector2>();

        bool validPick = false;
        do
        {
            int randomChoice = randy.Next(0, 4);

            switch (randomChoice)
            {
                case 0:
                    //add north node if it exists
                    //(nodeMesh.ParkMesh[xIndex, yIndex - 1] != null)
                    try
                    {
                        targetCurrent = nodeMesh.ParkMesh[xIndex, yIndex - 1];
                        validPick = true;
                        yIndex -= 1;
                    }
                    catch
                    {

                    }
                    break;

                case 1:
                    //add east node if it exists
                    //if (nodeMesh.ParkMesh[xIndex + 1, yIndex] != null)
                    try
                    {
                        targetCurrent = nodeMesh.ParkMesh[xIndex + 1, yIndex];
                        validPick = true;
                        xIndex += 1;
                    }
                    catch
                    {

                    }
                    break;

                case 2:
                    //add south node if it exists
                    //if (nodeMesh.ParkMesh[xIndex, yIndex + 1] != null)
                    try
                    {
                        targetCurrent = nodeMesh.ParkMesh[xIndex, yIndex + 1];
                        validPick = true;
                        yIndex += 1;
                    }
                    catch
                    {

                    }
                    break;

                case 3:
                    //add west node if it exists
                    //if (nodeMesh.ParkMesh[xIndex - 1, yIndex] != null)
                    try
                    {
                        targetCurrent = nodeMesh.ParkMesh[xIndex - 1, yIndex];
                        validPick = true;
                        xIndex -= 1;
                    }
                    catch
                    {

                    }
                    break;
            }
        } while (validPick == false);


        if(xIndex == 2 && yIndex == 2)
        {
            state = PerState.Suspicious;
        }

    }

    public void ExitPark()
    {
        //seek nearest exit node
        nearestExitNode = new Vector2(100, 100);
        for(int i = 0; i < 8; i++)
        {
            //find the nearest exit node among all the exit nodes
            if((nodeMesh.ExitMesh[i] - (Vector2)transform.position).sqrMagnitude < (nearestExitNode - (Vector2)transform.position).sqrMagnitude)
            {
                nearestExitNode = nodeMesh.ExitMesh[i];
            }
        }

        if(state != PerState.Afraid) //if not Afraid, set state to Leaving
        {
            state = PerState.Leaving;
        }
        
        targetCurrent = nearestExitNode;
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
